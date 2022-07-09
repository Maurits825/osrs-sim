using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour
{
    [Tooltip("Choose which GameSettings asset to use")]
    public GameSettings settings;

    [SerializeField] public static GameSettings s;
    public static Settings instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Settings.instance == null)
        {
            Settings.instance = this;
        }
        if (Settings.s == null)
        {
            Settings.s = settings;
        }
    }
}