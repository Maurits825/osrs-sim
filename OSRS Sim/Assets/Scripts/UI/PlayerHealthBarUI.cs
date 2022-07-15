using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarUI : HealthBarUI
{
    [SerializeField] private PlayerVariables playerVariables;
    void Update()
    {
        SetHealth((float)playerVariables.health / playerVariables.maxHealth);
    }
}
