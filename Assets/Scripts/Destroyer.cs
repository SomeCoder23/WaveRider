using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item" || other.tag == "Obstacle")
        {
            //Debug.Log("Destroying: " + other.name);
            Destroy(other.gameObject);
        }
    }
}
