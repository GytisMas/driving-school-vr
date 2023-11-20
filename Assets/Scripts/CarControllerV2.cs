using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// TODO: padaryt braking force (irgi velChange maybe)
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
    [SerializeField] float decelerationRate;
    [SerializeField] DriveTrain driveTrainType;
    [SerializeField] AnimationCurve powerCurve;
    [SerializeField] float reverseAccelerationMultiplier;
    [Space]
    [Header("Steering")]
    [SerializeField] float steeringSpeed;
    [SerializeField] float maxSteeringAngle;
    [SerializeField] float maxGrip = 5f;
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
    private bool forwardGear = true;
    private float currentMaxSpeed;
    private float accelerationInput = 0f;
    private float horizontalInput = 0f;
    private float carSpeed;
    private float wheelLength;
    private bool skipDecel = false;

    private float[] steeringStrs = new float[4];
    private bool[] wheelsTouching = new bool[4];

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
        currentMaxSpeed = _maxSpeed;
        wheelLength = wheelDiameter * Mathf.PI;
        wheelRadius = wheelDiameter / 2f;
        onAirOffset = (wheelDiameter - offsetRange * 2f) / 2f;
        rBody.centerOfMass = rigidBodyCenterOfMass;
    }

    private void SetCarInput()
    {
        SetCarGear();
        SetAccelerationInput();
        SetHorizontalInput();
    }

    private void SetCarGear() 
    {
        InputLabel pressedGearKey = CustomInput.GetInputDown(InputLabel.GEARUP | InputLabel.GEARDOWN);
        // dont change gear if none 
        // or both buttons are pressed
        if (pressedGearKey == 0 || (pressedGearKey == (InputLabel.GEARUP | InputLabel.GEARDOWN)))
            return;
        forwardGear = (pressedGearKey & InputLabel.GEARUP) != 0;
        currentMaxSpeed = forwardGear ? _maxSpeed : _maxReverseSpeed;
    }

    public void SetAccelerationInput()
    {
        if (LogitechSteeringWheel.wheelConnected) {
            float gasInput = 1 - CustomInput.GetAxisNormalised01("Gas");
            float brakeInput = 1 - CustomInput.GetAxisNormalised01("Brake");
            accelerationInput = gasInput - brakeInput;
        }
        accelerationInput = Input.GetAxis("Vertical");
    }

    public void SetHorizontalInput()
    {
        if (LogitechSteeringWheel.wheelConnected)
            horizontalInput = CustomInput.GetAxisNormalised("Steering");
        horizontalInput = Input.GetAxis("Horizontal");
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

    void ChangeWheelDirection() {
        float currentSteerAngle = maxSteeringAngle * horizontalInput;
        Quaternion target = Quaternion.Euler(0, currentSteerAngle, 0f);
        // Debug.Log($"{target.eulerAngles} {allWheelsTarget[0].rotation.eulerAngles}");
        Wheels[0].localRotation = Quaternion.Slerp(Wheels[0].localRotation, target,  Time.fixedDeltaTime * steeringSpeed);
        Wheels[1].localRotation = Quaternion.Slerp(Wheels[1].localRotation, target,  Time.fixedDeltaTime * steeringSpeed);
    }

    private void SetForces()
    {
        skipDecel = true;
        // Debug.Log($"Speed: {Vector3.Dot(transform.forward, rBody.velocity)}");
        // Debug.Log($"{steeringStrs[0]:0.00f} {steeringStrs[1]:0.00f} {steeringStrs[2]:0.00f} {steeringStrs[3]:0.00f} ");
        SteeringF();
        for (int i = 0; i < Wheels.Count; i++)
        {
            SuspensionF(Wheels[i], i);
            AccelerationF(Wheels[i], i);
        }
    }
    private void SetWheelMeshes()
    {
        for (int i = 0; i < WheelMeshes.Count; i++)
            SetWheelMesh(WheelMeshes[i], Offsets[i]);
    }

    private void RotateWheelMeshes() 
    {        
        float rps = carSpeed / wheelLength;
        float rpfu = rps * Time.fixedDeltaTime;
        for (int i = 0; i < WheelMeshes.Count; i++) {
            float mult = i == 0 || i == 3 ? -1f : 1f;
            WheelMeshes[i].Rotate(
                new Vector3(rpfu * mult * 360f, 0f, 0f)
            );
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
    private void SteeringF()
    {
        // suskaiciuot velocity sum ant visu ratu
        // pasidaryt force ratios
        // applyint pagal force ratio, turet max reiksme maxGrip
        float totalVel = 0f;
        int wheelsTouchingCount = 0;
        for (int i = 0; i < Wheels.Count; i++) {
            var wheel = Wheels[i];
            wheelsTouching[i] = false;
            if (Physics.Raycast(wheel.position, -wheel.up, wheelRadius + 0.1f, colLayerMask)) {
                wheelsTouchingCount++;
                wheelsTouching[i] = true;
                Vector3 steeringDirection = wheel.right;
                Vector3 worldVel = rBody.GetPointVelocity(wheel.position);
                steeringStrs[i] = -Vector3.Dot(steeringDirection, worldVel);
                totalVel += Mathf.Abs(steeringStrs[i]);
            }
        }
        
        float localMaxGrip = (float)(maxGrip * (wheelsTouchingCount / 4f));
        float averageVel = totalVel / 4f;
        // Debug.Log($"maxgrip {maxGrip} {(float)(maxGrip * (3 / 4f))}");
        
        float velUsed = averageVel < localMaxGrip ? averageVel : localMaxGrip;
        Debug.Log($"velused {velUsed}");
        for (int i = 0; i < Wheels.Count; i++) {
            if (!wheelsTouching[i])
                continue;
            var wheel = Wheels[i];
            Vector3 steeringDirection = wheel.right;
            float forceStr = velUsed * (steeringStrs[i] / totalVel);
            rBody.AddForceAtPosition(steeringDirection * forceStr, wheel.position, ForceMode.VelocityChange);
        }
    }
    private void DecelerationF(Transform wheel, int i) 
    {

        if (Physics.Raycast(wheel.position, -wheel.up, wheelRadius + 0.1f, colLayerMask))
        {
            Vector3 forwardDirection = wheel.forward;
            Vector3 worldVel = rBody.GetPointVelocity(wheel.position);
            float accelVel = Vector3.Dot(forwardDirection, worldVel);
            float accelVelAbs = Mathf.Abs(accelVel);
            float reverseMult = accelVel < 0f ? -1f : 1f;
            float desiredVelChange = decelerationRate;
            if (accelVelAbs <= .2f)
                 desiredVelChange = accelVelAbs / 4f;
            // Debug.Log($"Force: {i} {forwardDirection} {worldVel} {steeringVel} {desiredAccel} {forwardDirection * desiredAccel}");
            // Debug.Log($"Force: {i} {steeringVel} {forwardDirection * tireMass * desiredAccel}");
            rBody.AddForceAtPosition(-forwardDirection * desiredVelChange * reverseMult, wheel.position, ForceMode.VelocityChange);
        }
    }
    private void AccelerationF(Transform wheel, int i) 
    {
        carSpeed = Vector3.Dot(transform.forward, rBody.velocity);
        if (accelerationInput <= 0f) {
            DecelerationF(Wheels[i], i);
            return;
        }
        float carSpeedAbs = Mathf.Abs(carSpeed);
        bool predAWD = driveTrainType == DriveTrain.AWD;
        int countDriveWheels = predAWD ? 4 : 2;
        bool applyTorque = 
            (accelerationInput > 0f && carSpeed < currentMaxSpeed)
            || (accelerationInput < 0f && carSpeed > 0f && forwardGear)
            || (accelerationInput < 0f && carSpeed < 0f && !forwardGear);
        if (IsDriveWheel(i) && applyTorque &&
            Physics.Raycast(wheel.position, -wheel.up, wheelRadius + 0.1f, colLayerMask)) 
        {
            Vector3 forwardDirection = forwardGear ? wheel.forward : -wheel.forward;
            float gearMult = forwardGear 
                ? powerCurve.Evaluate(carSpeedAbs / currentMaxSpeed)
                : reverseAccelerationMultiplier;
            float torque = 
                  gearMult
                * accelerationInput 
                * accelerationMultiplier
                / countDriveWheels;
            rBody.AddForceAtPosition(forwardDirection * torque, wheel.position, ForceMode.VelocityChange);
        }
    }    

    void FixedUpdate()
    {
        // Time.timeScale = timeScale;
        SetForces();
        ChangeWheelDirection();
        SetWheelMeshes();
        RotateWheelMeshes();
    }

    private void Update() {
        SetCarInput();
    }

    private void OnDrawGizmos() {
        // foreach (var wheel in allWheelsTarget) {
            // Debug.DrawRay(transform.position, transform.forward.normalized * 5f, Color.red);
        // Debug.DrawRay(transform.position, Quaternion.Euler(-30, 0, 0) * transform.forward.normalized * 4f, Color.blue);
        // }
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
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
