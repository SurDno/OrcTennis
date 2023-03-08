using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSlow : MonoBehaviour
{
    public GameObject myObject;
    public int myVar = 0;
    private float timer = 0.0f;
    public float walkSpeed = 5f;
    

    void Update() {
        if (myObject == null && myVar == 0) {
            myVar = 1;
            timer = 0.0f;
        }

        if (myVar == 1 && timer < 2.0f) {
            timer += Time.deltaTime;
        }
        else if (myVar == 1) {
            myVar = 0;
        }  
        
        if (myVar == 1) {
                // set walkSpeed to half its value
                walkSpeed = 2.5f;
        }
        else {
                // reset walkSpeed to its original value
                walkSpeed = 5.0f;
        }
    }  
}