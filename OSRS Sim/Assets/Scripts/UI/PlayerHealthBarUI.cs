using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarUI : HealthBarUI
{
    [SerializeField] private PlayerVariables playerVariables;
    public override void Update()
    {
        base.Update();
        SetHealth((float)playerVariables.health / playerVariables.maxHealth);
    }
}
