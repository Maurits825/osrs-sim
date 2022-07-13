using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : MonoBehaviour, IEnemy
{
    public void OnGameTick()
    {
        Debug.Log("man attacks");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
