using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 2f;
    bool damagesBoat = false;
    private void Start()
    {
        if (GetComponent<Collectable>() == null)
            damagesBoat = true;
    }
    void FixedUpdate()
    {
        transform.position += Vector3.forward * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damagesBoat) return;

        if(other.tag == "Boat")
        {
            Debug.Log("Boat collided with " + name + " NOW ENDING GAME!!");
            HUD_Manager.instance.LoseGame();
            //checks if player has hearts if not displays lose window
            //player can resume if they pay gems.
        }
    }
}
