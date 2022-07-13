using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickController : MonoBehaviour
{
    public static TickController Instance { get; private set; }

    [SerializeField] private GameStates gameState;
    [SerializeField] private GameController gameController;

    private float timeSinceTick = 0.0f;

    private void Awake()
    {
        if (TickController.Instance == null)
        {
            TickController.Instance = this;
        }
    }

    private void Update()
    {
        timeSinceTick += Time.deltaTime;
        if (timeSinceTick >= gameState.tickLength)
        {
            timeSinceTick = 0;
            gameState.currentTick++;

            gameController.OnGameTick();
        }
    }
}
