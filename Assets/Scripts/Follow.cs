using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        if(target != null)
            transform.position = target.position + offset;
    }
}
