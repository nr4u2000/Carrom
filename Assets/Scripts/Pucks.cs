using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pucks : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Rigidbody2D>().mass = 1;      // Here set the Default mass of Each Pucks
    }

    // Update is called once per frame
    void Update()
    {
        if(StrikerController.Instance.Shoot == false)
        {
            this.GetComponent<Rigidbody2D>().mass = 100;        // Here increases mass of Each Pucks When Striker not in Shoot mode, It is used only for Restrict the Stricker to Overlap it
        }
        else
        {
            this.GetComponent<Rigidbody2D>().mass = 1;          // Here decreases mass of Each Pucks When Striker Shoot mode, i.e. Whenever Stricker Got Touch touched it back to Default mass 
        }     
    }
}
