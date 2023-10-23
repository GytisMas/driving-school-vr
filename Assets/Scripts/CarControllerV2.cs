using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControllerV2 : CarController
{
    enum DriveTrain {
        AWD,
        FWD,
        RWD
    }
    [Header("Speed")]
    [SerializeField] float _maxSpeed;
    public override float maxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } } 
    [SerializeField] float _maxReverseSpeed;
    [SerializeField] float _accelerationMultiplier;
    public override float accelerationMultiplier { get { return _accelerationMultiplier; } set { _accelerationMultiplier = value; } }
    [SerializeField] DriveTrain driveTrainType;
    [SerializeField] AnimationCurve powerCurve;
    [Space]
    [Header("Steering")]
    [SerializeField] float steeringSpeed;
    [SerializeField] float maxSteeringAngle;
    [SerializeField] float fTireGripFactor = 1f;
    [SerializeField] float rTireGripFactor = 1f;

    [Space]
    [Header("Suspension")]
    // 
    [SerializeField] float frontSpringStrengthKNm;
    [SerializeField] float rearSpringStrengthKNm;
    [SerializeField] float wheelDiameter = .25f;
    private float wheelRadius;
    [SerializeField] float offsetRange = .25f;
    [SerializeField] float angleForFront = 0f;
    [Header("Dampers [be careful with those :))))]")]
    [SerializeField] float frontSpringDamperK;
    [SerializeField] float rearSpringDamperK;

    [Space]
    [Header("Misc")]
    [SerializeField] private LayerMask colLayerMask;
    // [SerializeField] float timeScale = 1f;
    [SerializeField] float wheelMeshUpdateRate = 10f;
    [SerializeField] Rigidbody rBody;
    [SerializeField] Vector3 rigidBodyCenterOfMass;
    public override Vector3 centerOfMass { get { return rigidBodyCenterOfMass; } }
    [SerializeField] List<Transform> Wheels;
    [SerializeField] List<Transform> WheelMeshes;
    [SerializeField] List<Transform> SuspensionTops;
    public Text carSpeedText;

    List<float> Offsets = new List<float> {
        0f, 0f, 0f, 0f
    };
    List<Vector3> SpringVectors = new List<Vector3> {
        Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero
    };

    private bool isNextPush = false;
    private float steeringAxis = 0f;
    private float onAirOffset;

    private float frontSpringStrength {
        get {
            return frontSpringStrengthKNm * 1000f;
        }
    }

    private float rearSpringStrength {
        get {
            return rearSpringStrengthKNm * 1000f;
        }
    }

    private float frontSpringDamper {
        get {
            return frontSpringDamperK * 1000f;
        }
    }

    private float rearSpringDamper {
        get {
            return rearSpringDamperK * 1000f;
        }
    }

    private void Awake() {
        wheelRadius = wheelDiameter / 2f;
        onAirOffset = (wheelDiameter - offsetRange * 2f) / 2f;
        rBody.centerOfMass = rigidBodyCenterOfMass;
    }

    // public override void SetCarStatsByLevelAndDamage(
    //     int speedLevel, 
    //     int handlingLevel, 
    //     float damageRatio = 1f
    //     ) 
    // { 
    //     ParticleSystem smokePS = GetComponent<CarBehaviour>().smokePS;
    //     var emission = smokePS.emission;
    //     var reverseRatio = 1 - damageRatio;
    //     var psMain = smokePS.main; 
    //     if (reverseRatio <= 0.1f) {
    //         emission.rateOverTime = 0f;
    //         psMain.startColor = Color.HSVToRGB(0, 0, 1);
    //     }
    //     else {
    //         emission.rateOverTime = reverseRatio*reverseRatio*100;
    //         psMain.startColor = Color.HSVToRGB(0, 0, Mathf.Sqrt(damageRatio));
    //     }
    // }

    public override float GetAccelerationInput()
    {
        if (LogitechSteeringWheel.wheelConnected) {
            float gasInput = 1 - CustomInput.GetAxisNormalised01("Gas");
            float brakeInput = 1 - CustomInput.GetAxisNormalised01("Brake");
            return gasInput - brakeInput;
        }
        return Input.GetAxis("Vertical");
    }

    public override float GetHorizontalInput()
    {
        if (LogitechSteeringWheel.wheelConnected)
            return CustomInput.GetAxisNormalised("Steering");
        return Input.GetAxis("Horizontal");
    }

    public void TurnNextPushOn() {
        isNextPush = true;
    }

    bool IsDriveWheel(int wheelIndex) {
        if (driveTrainType == DriveTrain.AWD)
            return wheelIndex >= 0;
        else if (driveTrainType == DriveTrain.FWD)
            return wheelIndex <= 1;
        else if (driveTrainType == DriveTrain.RWD)
            return wheelIndex >= 2;
        Debug.LogWarning("DriveTrain not one of detectable types");
        return false;
    }

    void SetWheelPos(Transform wheelT, float offset) {
        wheelT.localPosition = new Vector3(
            wheelT.localPosition.x,
            offsetRange - offset + wheelRadius,
            wheelT.localPosition.z
        );
    }

    void SetWheelMesh(Transform wheelMesh, float offset) {
        float localPosY = wheelMesh.localPosition.y;
        float diff = offset - localPosY;
        if (diff < 0.01f && diff > -0.01f) // too close to zero
            return;
        float diffAbs = Mathf.Abs(diff);
        float diffSign = diff / diffAbs;
        float change = wheelMeshUpdateRate * Time.fixedDeltaTime;
        if (diffAbs < change) {
            change = diffAbs;
        }
        change = localPosY + diffSign * change;
        wheelMesh.localPosition = new Vector3(
            wheelMesh.localPosition.x,
            change,
            wheelMesh.localPosition.z
        );
    }

    void ChangeSteeringDirection() {
        float currentSteerAngle = maxSteeringAngle * GetHorizontalInput();
        Quaternion target = Quaternion.Euler(0, currentSteerAngle, 0f);
        // Debug.Log($"{target.eulerAngles} {allWheelsTarget[0].rotation.eulerAngles}");
        Wheels[0].localRotation = Quaternion.Slerp(Wheels[0].localRotation, target,  Time.fixedDeltaTime * steeringSpeed);
        Wheels[1].localRotation = Quaternion.Slerp(Wheels[1].localRotation, target,  Time.fixedDeltaTime * steeringSpeed);
    }

    private void ManageForces()
    {
        int wheelsTouching = 0;
        // Debug.Log($"Speed: {Vector3.Dot(transform.forward, rBody.velocity)}");
        for (int i = 0; i < Wheels.Count; i++)
        {
            bool thisWheelTouching = SteeringF(Wheels[i], i);
            SuspensionF(Wheels[i], i);
            AccelerationF(Wheels[i], i);            

            wheelsTouching = thisWheelTouching ? wheelsTouching + 1 : wheelsTouching;
        }
    }

    private bool SuspensionF(Transform wheel, int i)
    {
        bool res = false;
        Vector3 springDirection = wheel.up;
        SpringVectors[i] = springDirection;
        RaycastHit hit;
        Offsets[i] = onAirOffset;
        if (Physics.Raycast(SuspensionTops[i].position, -springDirection, out hit, offsetRange * 2, colLayerMask))
        {
            // Debug.Log("AA");
            float springStrength;
            float springDamper;
            // rear wheel
            if (i > 1) {
                springStrength = rearSpringStrength;
                springDamper = rearSpringDamper;
            } else {            
                springStrength = frontSpringStrength;
                springDamper = frontSpringDamper;
            }
            Offsets[i] = onAirOffset + offsetRange * 2 - hit.distance;
            // if (i == 0)
                // Debug.Log($"Sus {i} | {Offsets[i]}");
            Vector3 worldVel = rBody.GetPointVelocity(wheel.position);
            float vel = Vector3.Dot(springDirection, worldVel);
            float force = (Offsets[i] * springStrength) - (vel * springDamper);
            rBody.AddForceAtPosition(Vector3.up * force * Mathf.Cos(Mathf.Deg2Rad * wheel.rotation.eulerAngles.x), wheel.position);
            res = true;
            // if (i == 0)
                // Debug.Log($"force {i} force: {Vector3.up * force * Mathf.Cos(Mathf.Deg2Rad * wheel.rotation.eulerAngles.x)}");
                // Debug.Log($"force {i} | spring {Offsets[i]} {springStrength} {Offsets[i] * springStrength} | damper {vel} {springDamper} {vel * springDamper} | res {force} | force: {Vector3.up * force} {Vector3.up * force * Mathf.Cos(Mathf.Deg2Rad * wheel.rotation.eulerAngles.x)}");
        }
        // res = true;
        return res;
    }
    private bool SteeringF(Transform wheel, int i)
    {
        bool res = false;
        if (Physics.Raycast(wheel.position, -wheel.up, wheelRadius + 0.1f, colLayerMask))
        {
            res = true;
            Vector3 steeringDirection = wheel.right;
            Vector3 worldVel = rBody.GetPointVelocity(wheel.position);
            // float intervalVal = 1f;
            float steeringVel = Vector3.Dot(steeringDirection, worldVel);
            float desiredVelChange = -steeringVel;
            // desiredVelChange = desiredVelChange > intervalVal ? intervalVal
            // : desiredVelChange < -intervalVal ? -intervalVal
            // : desiredVelChange;

            if (i >= 2)
                desiredVelChange *= rTireGripFactor;
            else
                desiredVelChange *= fTireGripFactor;
            float desiredAccel = desiredVelChange;
            // if (i == 0)
            //     Debug.Log($"Force: {i} {worldVel} {steeringVel} {steeringDirection * desiredAccel}");
            // Debug.Log($"Force: {i} {steeringDirection} {worldVel} {steeringVel} {desiredAccel} {steeringDirection * desiredAccel}");
            rBody.AddForceAtPosition(steeringDirection * desiredAccel, wheel.position, ForceMode.Acceleration);
        }
        return res;
    }
    private void DecelerationF(Transform wheel, int i) 
    {
        if (Physics.Raycast(wheel.position, -wheel.up, wheelRadius + 0.1f, colLayerMask))
        {
            Vector3 forwardDirection = wheel.forward;
            Vector3 worldVel = rBody.GetPointVelocity(wheel.position);
            float steeringVel = Vector3.Dot(forwardDirection, worldVel);
            float desiredVelChange = -steeringVel * fTireGripFactor;
            float desiredAccel = desiredVelChange * 0.3f;
            // Debug.Log($"Force: {i} {forwardDirection} {worldVel} {steeringVel} {desiredAccel} {forwardDirection * desiredAccel}");
            // Debug.Log($"Force: {i} {steeringVel} {forwardDirection * tireMass * desiredAccel}");
            rBody.AddForceAtPosition(forwardDirection * desiredAccel, wheel.position, ForceMode.Acceleration);
        }
    }
    private void AccelerationF(Transform wheel, int i) 
    {
        float accelerationInput = GetAccelerationInput();
        float carSpeed = Vector3.Dot(transform.forward, rBody.velocity);
        // carSpeedText.text = Mathf.RoundToInt(carSpeed).ToString();
        bool predFWD = driveTrainType == DriveTrain.FWD;
        bool predAWD = driveTrainType == DriveTrain.AWD;
        bool predRWD = driveTrainType == DriveTrain.RWD;
        int countDriveWheels = predFWD || predRWD ? 2 : 4;
        // bool one = true;
        // c < m, posA -- true
        // c > m, posA -- false
        // c > m, negA -- true
        // c > m, negA -- false
        bool applyTorque = 
            carSpeed >= 0 && carSpeed < maxSpeed
            || carSpeed >= 0 && carSpeed >= maxSpeed && accelerationInput < 0f
            || carSpeed < 0 && carSpeed > -_maxReverseSpeed
            || carSpeed < 0 && carSpeed <= -_maxReverseSpeed && accelerationInput > 0f;
        // if (IsDriveWheel(i)) {
        //     Debug.Log($"{maxSpeed} | {applyTorque} | {Physics.Raycast(wheel.position, -wheel.up, wheelRadius + 0.1f, colLayerMask)}");

        // }
        if (accelerationInput == 0f) {
            DecelerationF(Wheels[i], i);
            return;
        }
        else if (IsDriveWheel(i) && applyTorque &&
            Physics.Raycast(wheel.position, -wheel.up, wheelRadius + 0.1f, colLayerMask)) 
        {
            Vector3 forwardDirection = wheel.forward;
            float torque = 
                powerCurve.Evaluate(carSpeed / maxSpeed) 
                * accelerationInput 
                * accelerationMultiplier
                / countDriveWheels;
            // Debug.Log($"Force: {i} {forwardDirection} {worldVel} {steeringVel} {desiredAccel} {forwardDirection * desiredAccel}");
            // if (one)
            //     Debug.Log($"Force: {i} {carSpeed} {torque} {forwardDirection * torque} ");
            // one = false;
            // Debug.Log($"Should add accel force ({accelerationInput})");
            rBody.AddForceAtPosition(forwardDirection * torque, wheel.position, ForceMode.Acceleration);
        }
    }    

    void FixedUpdate()
    {        
        // Time.timeScale = timeScale;
        ManageForces();
        ChangeSteeringDirection();

        for (int i = 0; i < WheelMeshes.Count; i++)
            SetWheelMesh(WheelMeshes[i], Offsets[i]);
    }

    private void Update() {
        // Debug.Log(rBody.centerOfMass);
    }

    private void OnDrawGizmos() {
        // foreach (var wheel in allWheelsTarget) {
            // Debug.DrawRay(transform.position, transform.forward.normalized * 5f, Color.red);
        // Debug.DrawRay(transform.position, Quaternion.Euler(-30, 0, 0) * transform.forward.normalized * 4f, Color.blue);
        // }
        for (int i = 0; i < Wheels.Count; i++) {
            // Debug.DrawRay(Wheels[i].position, Wheels[i].up.normalized * 2f, Color.red);
            // Debug.DrawRay(Wheels[i].position, SpringVectors[i].normalized * .2f, Color.yellow);

            // tilted spring hardcode
            // Debug.DrawRay(Wheels[i].position, Quaternion.AngleAxis( angleForFront, Wheels[i].right) * Wheels[i].up * 2f, Color.blue);
            // Debug.DrawRay(Wheels[i].position, Quaternion.AngleAxis(-angleForFront, Wheels[i].right) * Wheels[i].up * 2f, Color.green);

            // up, right, forward
            Debug.DrawRay(Wheels[i].position, Wheels[i].up * 2f, Color.red);
            Debug.DrawRay(Wheels[i].position, Wheels[i].right * 2f, Color.green);
            Debug.DrawRay(Wheels[i].position, Wheels[i].forward * 2f, Color.blue);
        }
    }
}
