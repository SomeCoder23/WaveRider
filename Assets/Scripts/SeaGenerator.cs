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


    public Vector3 GetRandomPos(float z)
    {
        Vector3 pos = new Vector3(0, 0, transform.position.z + z);
        if (spawnArea == null)
            return pos;

        int tries = 100;
        do
        {
            pos.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            tries--;
        } while (!isValid(pos) || tries > 0);
        return pos;
    }

    bool isValid(Vector3 position)
    {
        if (Physics.Raycast(position + Vector3.up * 40f, Vector3.down, out RaycastHit hit, 40f * 2, obstaclesLayer))
            return false;

        return true;
    }
}
