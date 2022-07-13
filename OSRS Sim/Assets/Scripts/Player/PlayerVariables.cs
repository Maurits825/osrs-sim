using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVariables", menuName = "PlayerVariables")]
public class PlayerVariables : ScriptableObject
{
    [SerializeField]
    private float runEnergy;

    public float RunEnergy
    { 
        get { return runEnergy; }
        set
        {
            runEnergy = Mathf.Clamp(value, 0f, maxRunEnergy);
        }
    }
    public float maxRunEnergy = 100;
    public bool isRunning = false;
}
