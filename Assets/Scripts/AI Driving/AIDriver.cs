using System.Collections;
using UnityEngine;

public class AIDriver : PassiveTaskObject 
{
    [SerializeField] private bool debug = false;
    [SerializeField] private CarControllerV2 carController;
    private bool forwardGear;
    private float maxSpeed;
    private float accelerationInput;
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
    }

    public void SetTurnRate(float turnRate)
    {
        LogInConsole($"Setting car turn rate: {turnRate}");
        horizontalInput = turnRate;
    }

    public void SetSpeed(float speed)
    {
        LogInConsole($"Setting car speed: {speed}");
        maxSpeed = speed;
    }

    public void StopForSeconds(float time) 
    {
        StartCoroutine(StopAndWait(time));
    }

    IEnumerator StopAndWait(float time) 
    {
        nextActionIsReady = false;
        accelerationInput = -1f;
        LogInConsole($"Stopping car if necessary");
        while (carController.CarSpeed > 0f) {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            time -= Time.fixedDeltaTime;
        }
        LogInConsole($"Car Stopped");
        accelerationInput = 0f;
        yield return new WaitForSeconds(time);
        LogInConsole($"Waiting over");
        nextActionIsReady = true;
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
            onFailState?.Invoke(this);
        }
    }

    private void Update() 
    {
        carController.SetCarInputAI(forwardGear, maxSpeed, accelerationInput, horizontalInput);
    }
}