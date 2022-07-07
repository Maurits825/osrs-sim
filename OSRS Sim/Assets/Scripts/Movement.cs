using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public RunEnergy runEnergy;

    public const int RUN_ENERGY_MAX = 100;
    private float runEnergyAmount;
    private float runEnergyRegen;

    private void Start()
    {
        runEnergyAmount = 0;
        runEnergyRegen = 10f;
    }

    private void Update()
    {
        runEnergyAmount += runEnergyRegen * Time.deltaTime;
        runEnergyAmount = Mathf.Clamp(runEnergyAmount, 0f, RUN_ENERGY_MAX);

        runEnergy.SetRunEnergy(GetRunEnergyNormalized());
    }

    public void TrySpendEnergy(int amount)
    {
        if (runEnergyAmount >= amount)
        {
            runEnergyAmount -= amount;
        }
    }

    private float GetRunEnergyNormalized()
    {
        return runEnergyAmount / RUN_ENERGY_MAX;
    }

}
