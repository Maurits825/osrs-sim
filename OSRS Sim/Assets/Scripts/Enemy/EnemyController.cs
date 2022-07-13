using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance { get; private set; }

    private List<Enemy> enemies = new();

    private void Awake()
    {
        if (EnemyController.Instance == null)
        {
            EnemyController.Instance = this;
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void OnGameTick()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.OnGameTick();
        }
    }
}
