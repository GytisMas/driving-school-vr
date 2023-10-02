using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingInputTest : MonoBehaviour
{
    private float gasInput;
    private float steeringInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gasInput = DrivingInput.GetAxis("Gas V");
        steeringInput = DrivingInput.GetAxis("Steering H");
        // Debug.Log($"| {gasInput:10.2f} | {steeringInput:10.2f} |");
    }
}
