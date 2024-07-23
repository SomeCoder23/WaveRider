using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGenerator : MonoBehaviour
{
    MapManager map;
    public LayerMask obstaclesLayer;

    Collider spawnArea;

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
        
    }
    private void Start()
    {
        map = GetComponentInParent<MapManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boat")
        {
            //remove sea section from behind and add it up front
            Invoke("GenerateNextSection", 0.2f);
        }
        
    }

    void GenerateNextSection()
    {
         map.MoveSection();
    }


    public Vector3 GetRandomPos(float z, ObstacleSize size)
    {
        Vector3 pos = new Vector3(0, 0, transform.position.z + z);
        if (spawnArea == null)
            return pos;

        int tries = 100;
        do
        {
            pos.x = Random.Range(spawnArea.bounds.min.x + 25, spawnArea.bounds.max.x - 25);
            tries--;
        } while (!isValid(pos, size) || tries > 0);
        return pos;
    }

    bool isValid(Vector3 position, ObstacleSize size)
    {
        Vector3 halfExtents = new Vector3(2.5f, 2.5f, 2.5f);
        switch (size)
        {
            case ObstacleSize.big: halfExtents = new Vector3(20, 10f, 20f); break;
            case ObstacleSize.medium: halfExtents = new Vector3(10, 5, 10); break;
        }

        if (Physics.BoxCast(position, halfExtents, Vector3.forward, Quaternion.identity, 2, obstaclesLayer)) 
            return false;

        return true;
    }
}
