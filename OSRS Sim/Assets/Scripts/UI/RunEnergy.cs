using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunEnergy : MonoBehaviour
{
    public Image barImage;
    public Gradient gradient;
    public void SetRunEnergy(float amount)
    {
        barImage.fillAmount = amount;
        barImage.color = gradient.Evaluate(amount);
    }
}
