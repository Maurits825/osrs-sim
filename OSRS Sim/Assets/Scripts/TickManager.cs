using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static TickManager Instance { get; private set; }

    [SerializeField] private float tickLength = 0.6f;

    [SerializeField] private GameObject gameControllerObject;
    private GameController gameController;

    private float timeSinceTick = 0.0f;

    private void Awake()
    {
        if (TickManager.Instance == null)
        {
            TickManager.Instance = this;
        }
    }

    private void Start()
    {
        gameController = gameControllerObject.GetComponent<GameController>();
    }

    private void Update()
    {
        timeSinceTick += Time.deltaTime;
        if (timeSinceTick >= tickLength)
        {
            timeSinceTick = 0;
            gameController.OnGameTick();
        }
    }
}
