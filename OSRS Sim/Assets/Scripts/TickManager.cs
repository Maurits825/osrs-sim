using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static TickManager Instance { get; private set; }

    [SerializeField] private float tickLength = 0.6f;

    [SerializeField] private GameObject playerObject;
    private PlayerController playerController;

    private float timeSinceTick = 0.0f;

    private Vector3Int tileClicked;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        timeSinceTick += Time.deltaTime;
        if (timeSinceTick >= tickLength)
        {
            timeSinceTick = 0;
            OnGameTick();
        }
    }

    private void OnGameTick()
    {
        playerController.SetTileClicked(tileClicked);

        playerController.OnGameTick();
    }

    public void RegisterMovementClick(Vector3Int tile)
    {
        tileClicked = tile;
    }
}
