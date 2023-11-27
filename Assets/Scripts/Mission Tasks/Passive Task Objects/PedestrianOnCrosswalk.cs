using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianOnCrosswalk : PassiveTaskObject
{
    public int pedestriansOnCrosswalk = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            if (pedestriansOnCrosswalk > 0)
            {
                onFailState?.Invoke(this);
            }
        }

        if (other.tag == "Human")
        {
            pedestriansOnCrosswalk += 1;
        }

    }
   

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Human")
        {
            pedestriansOnCrosswalk -= 1;
        }

    }
}
