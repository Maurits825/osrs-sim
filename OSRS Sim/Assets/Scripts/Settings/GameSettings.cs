using UnityEngine;

[CreateAssetMenu(fileName = "NewGameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public bool drawPlayerPath;
    public bool drawPlayerTile;
}
