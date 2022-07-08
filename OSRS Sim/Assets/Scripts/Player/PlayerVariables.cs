using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVariables", menuName = "PlayerVariables")]
public class PlayerVariables : ScriptableObject
{
    public float runEnergy;
    public float maxRunEnergy = 100;
    public bool isRunning = false;
}
