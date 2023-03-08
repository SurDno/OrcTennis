using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // check if the player has collected the object
        {
            Destroy(gameObject); // make the object disappear
        }
    }
}
