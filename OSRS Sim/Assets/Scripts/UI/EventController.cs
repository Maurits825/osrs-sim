using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static EventController Instance { get; private set; }
    private void Awake()
    {
        if (EventController.Instance == null)
        {
            EventController.Instance = this;
        }
    }

    public event Action<int, Transform> OnSpawnHitsplat;
    public void SpawnHitsplat(int amount, Transform position)
    {
        OnSpawnHitsplat?.Invoke(amount, position);
    }
}
