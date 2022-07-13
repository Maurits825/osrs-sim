using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void OnGameTick();

    private void Start()
    {
        Debug.Log("Register");
        EnemyController.Instance.RegisterEnemy(this);
    }
}
