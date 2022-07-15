using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarUI : HealthBarUI
{
    private EnemyInfo enemyInfo;

    public override void Start()
    {
        base.Start();
        enemyInfo = transform.parent.parent.GetComponent<Enemy>().enemyInfo;
    }

    public override void Update()
    {
        base.Update();
        SetHealth((float)enemyInfo.health / enemyInfo.maxHealth);
    }
}
