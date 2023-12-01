using System.Collections;
using UnityEngine;

public class AIDriver : PassiveTaskObject 
{
    [HideInInspector] public bool hasToStopCar = false;
    [SerializeField] private bool debug = false;
    [SerializeField] private CarControllerV2 carController;
    private bool forwardGear;
    private float maxSpeed;
    private float accelerationInput;
    private float oldAccelerationInput;
    private float horizontalInput;
    private bool nextActionIsReady = true;

    public bool NextActionIsReady {
        get {
            return nextActionIsReady;
        }
    }
    void Awake() 
    {
        carController.aiDriving = true;
        forwardGear = true;
        maxSpeed = 0f;
        accelerationInput = 0f;
        horizontalInput = 0f;
    }

    public void SetAcceleration(float acceleration) 
    {
        LogInConsole($"Setting car to drive");
        forwardGear = true;
        accelerationInput = acceleration;
        oldAccelerationInput = acceleration;
    }

    public void SetTurnRate(float turnRate)
    {
        if (turnRate > 1f)
            turnRate = 1f;
        else if (turnRate < -1f)
            turnRate = -1f;
        LogInConsole($"Setting car turn rate: {turnRate}");
        horizontalInput = turnRate;
    }

    public void SetSpeed(float speed)
    {
        LogInConsole($"Setting car speed: {speed}");
        maxSpeed = speed;
    }

    public void SlowDownAndWait(float time, float targetSpeed) 
    {
        StartCoroutine(SlowenAndWait(time, targetSpeed));
    }

    IEnumerator SlowenAndWait(float time, float targetSpeed) 
    {
        nextActionIsReady = false;
        accelerationInput = -1f;
        LogInConsole($"Stopping car if necessary");
        while (carController.CarSpeed > targetSpeed) {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            time -= Time.fixedDeltaTime;
        }
        maxSpeed = targetSpeed;
        LogInConsole($"Car Slowed");
        accelerationInput = 0f;
        yield return new WaitForSeconds(time);
        LogInConsole($"Waiting over");
        nextActionIsReady = true;
    }

    public void DriveToPoint(Vector3 target, float targetRot, float delta = 2f) 
    {
        StartCoroutine(DrivingToPoint(target, targetRot, delta));
    }

    IEnumerator DrivingToPoint(Vector3 target, float targetRot, float delta) 
    {
        nextActionIsReady = false;
        float rotSign = Mathf.Sign(targetRot - transform.eulerAngles.y);
        while (!nextActionIsReady) {
            if (hasToStopCar) {
                if (accelerationInput != -1f)
                    oldAccelerationInput = accelerationInput;
                accelerationInput = -1f;
                yield return null;
            } else {
                accelerationInput = oldAccelerationInput;
                float rotDiff = targetRot - transform.eulerAngles.y;
                if (rotDiff * rotSign < 0f) {
                    rotSign = Mathf.Sign(rotDiff);
                    float newTurnRate = rotDiff;
                    SetTurnRate(newTurnRate);
                }
                yield return null;
                if (Vector3.Distance(transform.position, target) < delta)
                    nextActionIsReady = true;
            }
        }
        SetTurnRate(0f);
    }

    public void Despawn() 
    {
        Destroy(gameObject);
    }

    void LogInConsole(string str) 
    {
        if (debug)
            Debug.Log(str);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Car") {
            onFailState?.Invoke(this, "Collision with another vehicle");
        }
    }

    private void Update() 
    {
        carController.SetCarInputAI(forwardGear, maxSpeed, accelerationInput, horizontalInput);
    }
}