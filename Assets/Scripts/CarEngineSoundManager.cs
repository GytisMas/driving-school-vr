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

       

        if(carController.AccelerationInput == 1)
        {
            isAccelerating = true;
        }
        else
        {
            isAccelerating = false;
        }
        


        if (isAccelerating)
        {
            audioSource.pitch = Mathf.Min(audioSource.pitch + accelerationRate * Time.deltaTime, maxEnginePitch);
        }
        else
        { 
            audioSource.pitch = Mathf.Max(audioSource.pitch - decelerationRate * Time.deltaTime, normalPitch);
        }
    }
}
