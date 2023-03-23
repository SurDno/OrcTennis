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
    [SerializeField] private float freezeRadius = 5f;
    [SerializeField] private float freezeDuration = 5f;
    [SerializeField] private float instantFreezeRadius = 3f;
    [SerializeField] private float instantFreezeDuration = 3f;

    private int teamColor;
    private float currentSpeed;
    private bool canCollectOrb = true;
    private bool canCastFreeze = false;
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

        // cast freeze spell
        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame) // B button on Xbox or Circle button on PS4
        {
            if (canCastFreeze && collectedOrb == null) // player can only cast freeze if they have not collected an orb
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, freezeRadius); // get all colliders within freeze radius
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Player") && collider.GetComponent<PlayerHolder>().teamColor != teamColor) // check if collider is a player on the opposite team
                    {
                        collider.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; // freeze the player's movement
                        StartCoroutine(UnfreezePlayer(collider.gameObject, freezeDuration)); // start coroutine to unfreeze the player after a certain

                    }
                }
                canCastInstantFreeze = false; // player can no longer cast instant freeze
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MagicOrb") && canCollectOrb) // check if player collides with a magic orb and is able to collect it
        {
            collectedOrb = other.gameObject;
            canCollectOrb = false; // player cannot collect another orb until the current one is casted
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MagicOrb")) // check if player exits the trigger of a magic orb
        {
            canCollectOrb = true; // player can now collect another orb
        }
    }

    IEnumerator UnfreezePlayer(GameObject player, float delay)
    {
        yield return new WaitForSeconds(delay);
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // unfreeze the player's movement
    }

}