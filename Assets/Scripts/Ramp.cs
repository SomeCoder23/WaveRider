using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    public float direction = 1;
    public float speed = 5f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Boat>() != null)
        {
            Boat boat = other.gameObject.GetComponent<Boat>();
            boat.SpeedUp(direction * speed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Boat>() != null)
        {
            Boat boat = other.gameObject.GetComponent<Boat>();
            boat.StopSpeedUp();
        }
    }

}
