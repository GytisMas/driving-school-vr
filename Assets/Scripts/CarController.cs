
using UnityEngine;
public abstract class CarController : MonoBehaviour {
    // public abstract void SetCarStatsByLevelAndDamage(int sL, int hL, float dmg = 1f);
    public abstract float maxSpeed { get; set; }
    public abstract float accelerationMultiplier { get; set; }
    public abstract Vector3 centerOfMass { get; }
    public abstract float GetAccelerationInput();
    public abstract float GetHorizontalInput();
}