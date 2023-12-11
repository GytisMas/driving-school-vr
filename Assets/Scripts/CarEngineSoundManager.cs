using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngineSoundManager : MonoBehaviour
{
    public float maxEnginePitch = 1.5f;
    public float accelerationThreshold = 0.5f;
    public float accelerationRate = 0.1f;
    public float decelerationRate = 0.2f;
    public float normalPitch = 1.0f;

    public AudioSource audioSource;
    private bool isAccelerating = false;
    public CarControllerV2 carController;

    void Start()
    {
        audioSource.pitch = normalPitch;
    }

    void Update()
    {
        float potentialPitch = normalPitch;
        if (carController.Carspeed > 0.1f) {
            potentialPitch += Mathf.Sqrt(carController.Carspeed / carController.maxSpeed * 0.25f);
        }
        audioSource.pitch = Mathf.Min(potentialPitch, maxEnginePitch);

    }
}
