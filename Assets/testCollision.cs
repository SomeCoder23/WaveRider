using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(name + " COLLIDED WITH " + collision.gameObject.name);
    }
}
