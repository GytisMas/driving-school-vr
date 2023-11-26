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
    [SerializeField] float decelerationRate;
    [SerializeField] DriveTrain driveTrainType;
    [SerializeField] AnimationCurve powerCurve;
    [SerializeField] float reverseAccelerationMultiplier;
    [SerializeField] float brakingRate = 0.5f;
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

    public bool aiDriving = false;

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

    private float[] steeringStrs = new float[4];
    private bool[] wheelsTouching = new bool[4];

    public float CarSpeed {
        get {
            return carSpeed;
        }
    }

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

    private void Awake() 
    {
        currentMaxSpeed = _maxSpeed;
        wheelLength = wheelDiameter * Mathf.PI;
        wheelRadius = wheelDiameter / 2f;
        onAirOffset = (wheelDiameter - offsetRange * 2f) / 2f;
        rBody.centerOfMass = rigidBodyCenterOfMass;
    }

    public void SetCarInputAI(bool p_forwardGear, float p_maxSpeed, float p_accelerationInput, float p_horizontalInput) 
    {
        forwardGear = p_forwardGear;
        currentMaxSpeed = p_maxSpeed;
        accelerationInput = p_accelerationInput;
        horizontalInput = p_horizontalInput;
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
        } else
            accelerationInput = Input.GetAxis("Vertical");
    }

    public void SetHorizontalInput()
    {
        if (LogitechSteeringWheel.wheelConnected)
            horizontalInput = CustomInput.GetAxisNormalised("Steering");
        else
            horizontalInput = Input.GetAxis("Horizontal");
    }

    void SetWheelMesh(Transform wheelMesh, float offset) 
    {
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

    void ChangeWheelDirection() 
    {
        float currentSteerAngle = maxSteeringAngle * horizontalInput;
        Quaternion target = Quaternion.Euler(0, currentSteerAngle, 0f);
        // Debug.Log($"{target.eulerAngles} {allWheelsTarget[0].rotation.eulerAngles}");
        Wheels[0].localRotation = Quaternion.Slerp(Wheels[0].localRotation, target,  Time.fixedDeltaTime * steeringSpeed);
        Wheels[1].localRotation = Quaternion.Slerp(Wheels[1].localRotation, target,  Time.fixedDeltaTime * steeringSpeed);
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

    private void ApplyForces()
    {
        SteeringF();
        for (int i = 0; i < Wheels.Count; i++)
        {
            SuspensionF(Wheels[i], i);
            AccelerationF(Wheels[i], i);
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
            Vector3 worldVel = rBody.GetPointVelocity(wheel.position);
            float vel = Vector3.Dot(springDirection, worldVel);
            float force = (Offsets[i] * springStrength) - (vel * springDamper);
            force *= Mathf.Cos(Mathf.Deg2Rad * wheel.rotation.eulerAngles.x);
            rBody.AddForceAtPosition(Vector3.up * force, wheel.position);
            res = true;
        }
        return res;
    }
    
    private void SteeringF()
    {
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
        if (totalVel == 0f)
            return;
        
        float localMaxGrip = (float)(maxGrip * (wheelsTouchingCount / 4f));
        float averageVel = totalVel / 4f;        
        float cappedVel = averageVel < localMaxGrip ? averageVel : localMaxGrip;

        // Debug.Log($"{wheelsTouching[0]} {cappedVel} {steeringStrs[0]} {totalVel} {Wheels[0].right}");

        for (int i = 0; i < Wheels.Count; i++) {
            if (!wheelsTouching[i])
                continue;
            var wheel = Wheels[i];
            Vector3 steeringDirection = wheel.right;
            float forceStr = cappedVel * (steeringStrs[i] / totalVel);
            // Debug.Log($"{cappedVel} * ({steeringStrs[i]} / {totalVel}) = {forceStr} | {steeringDirection * forceStr}");
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
            rBody.AddForceAtPosition(-forwardDirection * desiredVelChange * reverseMult, wheel.position, ForceMode.VelocityChange);
        }
    }

    private void AccelerationF(Transform wheel, int i) 
    {
        carSpeed = Vector3.Dot(transform.forward, rBody.velocity);
        if (accelerationInput == 0f) {
            DecelerationF(Wheels[i], i);
        }
        float carSpeedAbs = Mathf.Abs(carSpeed);
        bool predAWD = driveTrainType == DriveTrain.AWD;
        int countDriveWheels = predAWD ? 4 : 2;
        bool applyTorque = 
            accelerationInput < 0f 
            || (accelerationInput > 0f && (
                    (forwardGear && carSpeed < currentMaxSpeed)
                    || (!forwardGear && carSpeed > currentMaxSpeed)
                )
            );
        if (IsDriveWheel(i) && applyTorque &&
            Physics.Raycast(wheel.position, -wheel.up, wheelRadius + 0.1f, colLayerMask)) 
        {
            // Vector3 forwardDirection = forwardGear ? wheel.forward : -wheel.forward;
            Vector3 forwardDirection = wheel.forward;
            float gearMult = forwardGear 
                ? powerCurve.Evaluate(carSpeedAbs / currentMaxSpeed)
                : -reverseAccelerationMultiplier;            

            float torque = 
                  gearMult
                * accelerationInput 
                * accelerationMultiplier
                / countDriveWheels;

            if (accelerationInput < 0f) {
                if (carSpeed == 0f)
                    return;
                torque = brakingRate;

                // Possible undefined behaviour if not all 
                // driving wheels are touching ground
                if (torque > carSpeedAbs / countDriveWheels)
                    torque = carSpeedAbs / countDriveWheels;
                if (carSpeed > 0f)
                    torque *= -1f;
            }
            rBody.AddForceAtPosition(forwardDirection * torque, wheel.position, ForceMode.VelocityChange);
        }
    }    

    bool IsDriveWheel(int wheelIndex) 
    {
        if (driveTrainType == DriveTrain.AWD)
            return wheelIndex >= 0;
        else if (driveTrainType == DriveTrain.FWD)
            return wheelIndex <= 1;
        else if (driveTrainType == DriveTrain.RWD)
            return wheelIndex >= 2;
        Debug.LogWarning("DriveTrain not one of detectable types");
        return false;
    }
    
    void FixedUpdate() 
    {
        ApplyForces();
        ChangeWheelDirection();
        SetWheelMeshes();
        RotateWheelMeshes();
    }

    private void Update() 
    {
        if (!aiDriving)
            SetCarInput();
    }

    private void OnDrawGizmos() 
    {
        // foreach (var wheel in allWheelsTarget) {
            // Debug.DrawRay(transform.position, transform.forward.normalized * 5f, Color.red);
        // Debug.DrawRay(transform.position, Quaternion.Euler(-30, 0, 0) * transform.forward.normalized * 4f, Color.blue);
        // }
        // Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
        // for (int i = 0; i < Wheels.Count; i++) {
            // Debug.DrawRay(Wheels[i].position, Wheels[i].up.normalized * 2f, Color.red);
            // Debug.DrawRay(Wheels[i].position, SpringVectors[i].normalized * .2f, Color.yellow);

            // tilted spring hardcode
            // Debug.DrawRay(Wheels[i].position, Quaternion.AngleAxis( angleForFront, Wheels[i].right) * Wheels[i].up * 2f, Color.blue);
            // Debug.DrawRay(Wheels[i].position, Quaternion.AngleAxis(-angleForFront, Wheels[i].right) * Wheels[i].up * 2f, Color.green);

            // up, right, forward
        //     Debug.DrawRay(Wheels[i].position, Wheels[i].up * 2f, Color.red);
        //     Debug.DrawRay(Wheels[i].position, Wheels[i].right * 2f, Color.green);
        //     Debug.DrawRay(Wheels[i].position, Wheels[i].forward * 2f, Color.blue);
        // }
    }
}
