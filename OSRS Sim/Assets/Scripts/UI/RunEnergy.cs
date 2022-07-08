using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunEnergy : MonoBehaviour
{
    public Image barImage;
    public Gradient gradient;

    [SerializeField] private PlayerVariables playerVariables;

    private void Update()
    {
        float amount = playerVariables.runEnergy / playerVariables.maxRunEnergy;

        barImage.fillAmount = amount;

        if (playerVariables.isRunning)
        {
            barImage.color = gradient.Evaluate(amount);
        }
        else
        {
            barImage.color = Color.grey;
        }
        
    }
}
