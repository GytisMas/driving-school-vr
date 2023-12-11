using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem tireSmokeParticle;
    private CarControllerV2 carController;

    private void Start() 
    {
        carController = GetComponent<CarControllerV2>();
        carController.tireSmoke += TireSmoke;
        tireSmokeParticle.gameObject.SetActive(false);
    }



    private void TireSmoke(bool enable, float ratio)
    {
        tireSmokeParticle.gameObject.SetActive(enable);
        var emission = tireSmokeParticle.emission;
        // emission.rateOverDistance = ratio;
        emission.rateOverTimeMultiplier = ratio * 100f;
        Debug.Log(emission.rateOverTimeMultiplier);
    }

    
}
