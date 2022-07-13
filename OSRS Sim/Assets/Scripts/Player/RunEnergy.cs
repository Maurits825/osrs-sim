using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEnergy : MonoBehaviour
{
    [SerializeField] private PlayerVariables playerVariables;
    private float runEnergyRegen = 10f;

    public void RegenRun()
    {
        //TODO later can calc regen based on player stats/items
        playerVariables.RunEnergy += runEnergyRegen;
    }
}
