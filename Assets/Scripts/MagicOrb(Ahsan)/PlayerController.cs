using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float magicOrbRadius = 1f;
    [SerializeField] private float magicOrbDuration = 10f;
    [SerializeField] private GameObject magicOrbPrefab;
    [SerializeField] private Transform orbSpawnPoint;
    [SerializeField] private float freezeDuration = 5f;
    [SerializeField] private float instantFreezeRadius = 3f;
    [SerializeField] private float instantFreezeDuration = 3f;

    private int teamColor;
    private float currentSpeed;
    private bool canCollectOrb = true;
    private bool canCastInstantFreeze = false;
    private GameObject collectedOrb;


    void Update()
    {
        if (collectedOrb != null) // check if player has a collected magic orb
        {
            collectedOrb.transform.position = transform.position; // move the magic orb to the player's position
        }

        // cast speed boost spell
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) // A button on Xbox or X button on PS4
        {
            if (collectedOrb != null) // player can only cast speed boost if they have collected an orb
            {
                currentSpeed = GetComponent<Rigidbody>().velocity.magnitude; // get current speed of player
                GetComponent<Rigidbody>().velocity = transform.forward * (currentSpeed + speed); // increase player's speed
                float speedBoostDuration = magicOrbDuration / 2f; // set duration of speed boost spell to half the duration of the magic orb
                float speedBoostEndTime = Time.time + speedBoostDuration; // calculate end time of speed boost spell
                Destroy(collectedOrb); // destroy the magic orb
                collectedOrb = null;
                canCollectOrb = false; // player cannot collect another orb until the speed boost spell has ended
            }
        }
    }


}