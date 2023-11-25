using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    private TrafficLightState currentState;
    [SerializeField]private List<GameObject> lights;
    [SerializeField] private float yellowLightInterval = 2f;
    [SerializeField] private float initialBlinkDuration = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetState(TrafficLightState state)
    {
        SetLights(state);
    }

    public void SwitchNextState()
    {

        if(currentState == TrafficLightState.Red)
        {
            StartCoroutine(ChangeToGreen( ));
        }
        else
        {
            StartCoroutine(ChangeToRed());
        }

    }

    private void SetLights(TrafficLightState state)
    {

        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].SetActive(i == (int)state);
        }
        currentState = state;
    }
    private void SetLightsOff()
    {

        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].SetActive(false);
        }
        
    }

    private IEnumerator ChangeToGreen()
    {
        yield return new WaitForSeconds(yellowLightInterval+2);
        SetLights(TrafficLightState.Yellow);
        yield return new WaitForSeconds(yellowLightInterval);
        SetLights(TrafficLightState.Green);

        // Update the current state to the new state.
        currentState = TrafficLightState.Green;
    }
    private IEnumerator ChangeToRed()
    {
        float blinkInterval = 0.5f; // Time interval for the lights to blink.
        float blinkDuration = initialBlinkDuration;
        while (blinkDuration > 0)
        {
            SetLights(TrafficLightState.Green);

            yield return new WaitForSeconds(blinkInterval);

            SetLightsOff();
            yield return new WaitForSeconds(blinkInterval);

            blinkDuration -= 2 * blinkInterval;
        }
        SetLights(TrafficLightState.Yellow);
        yield return new WaitForSeconds(yellowLightInterval);
        // Restore the traffic light to its actual state.
        SetLights(TrafficLightState.Red);

        // Update the current state to the new state.
        currentState = TrafficLightState.Red;
    }






}

public enum TrafficLightState
{
    Red,
    Yellow,
    Green
}