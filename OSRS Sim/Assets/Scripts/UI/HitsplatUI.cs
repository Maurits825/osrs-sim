using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitsplatUI : MonoBehaviour
{
    public static Transform hitsplat;
    private TextMeshPro text;

    public static HitsplatUI Create()
    {
        Transform newHitsplat = Instantiate(hitsplat, Vector3.zero, Quaternion.identity);
        HitsplatUI hitsplatUI = newHitsplat.GetComponent<HitsplatUI>();
        hitsplatUI.Setup(20);
        return hitsplatUI;
    }

    private void Awake()
    {
        text = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount)
    {
        text.SetText(damageAmount.ToString());
    }
}
