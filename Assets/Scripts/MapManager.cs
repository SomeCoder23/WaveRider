using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //TODO:
    //organize spawning coins and gems
    //maybe add hearts?

    #region Singleton
    public static MapManager instance;
    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("More than one Map Manager!");
            return;
        }

        instance = this;
    }

    #endregion
    public float moveOffset = -550;
    public GameObject[] obstacles;
    public GameObject coin, gem;
    public Vector2 gemSpawnInterval = new Vector2(5, 15);
    [Range(1, 100)]
    public float obstaclesStartPercentage;
    //public float obstaclesIncreaseRate = 0.2f;

    [Range(1, 100)]
    public float collectableStartPercentage;
    //public float collectablesIncreaseRate = 0.1f;

    [Tooltip("Min and max items to be spawned in each section")]
    public Vector2 itemSpawnCount;

    public float spawnedItemsHeight = 7f;
    public Vector2 zOffset;

    List<Transform> seaSections = new List<Transform>();
    List<SeaGenerator> spawnPoints = new List<SeaGenerator>();

    float spawnTime;
    Vector3[] startPositions;
    bool spawnGem = false;

    private void Start()
    {
        SeaGenerator sea;
        startPositions = new Vector3[transform.childCount];
        int index = 0;
        foreach (Transform child in transform)
        {
            seaSections.Add(child);
            startPositions[index] = child.localPosition;
            index++;
            sea = child.GetComponentInChildren<SeaGenerator>();
            if (sea != null)
                spawnPoints.Add(sea);
        }

        //spawnTime = GetRandom(timeInterval);
        //StartCoroutine(Spawner());

    }

    public void InitializeMap()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
            SpawnItems(i);

        StartCoroutine(GemSpawnTimer());
    }

    float GetRandom(Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }


    public void MoveSection()
    {
        Transform section = seaSections[0];
        SeaGenerator spawnPoint = spawnPoints[0];
        section.localPosition = seaSections[seaSections.Count - 1].localPosition + new Vector3(0, 0, moveOffset);
        
        seaSections.Remove(section);
        seaSections.Add(section);

        spawnPoints.Remove(spawnPoint);
        spawnPoints.Add(spawnPoint);
        SpawnItems(spawnPoints.Count - 1);


    }

    //IEnumerator Spawner()
    //{
    //    float elapsedTime = 0;
    //    while(elapsedTime < spawnTime)
    //    {
    //        elapsedTime += 0.1f;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    spawnTime = GetRandom(timeInterval);
    //    SpawnItems(spawnPoints.Count - 1);
    //    StartCoroutine(Spawner());
    //}

    //IEnumerator IncreaseSpawnRate()
    //{
    //    float elapsedTime = 0;
    //    float increaseTime = 30;
    //    while (elapsedTime < increaseTime)
    //    {
    //        elapsedTime++;
    //        yield return new WaitForSeconds(1f);
    //    }
    //    if (obstaclesStartPercentage + obstaclesIncreaseRate <= 75)
    //        obstaclesStartPercentage += obstaclesIncreaseRate;
    //    else obstaclesStartPercentage = 75;

    //    if (collectableStartPercentage + collectablesIncreaseRate <= 75)
    //        collectableStartPercentage += collectablesIncreaseRate;
    //    else collectableStartPercentage = 75;

    //    StartCoroutine(IncreaseSpawnRate());
    //}

    IEnumerator GemSpawnTimer()
    {
        float elapsedTime = 0;
        float timer = GetRandom(gemSpawnInterval);
        while(elapsedTime < timer)
        {
            elapsedTime++;
            yield return new WaitForSeconds(1);
        }

        spawnGem = true;
    }


    void SpawnItems(int start)
    {
        //Debug.Log("SPAWNING ITEMS AT START POINT: " + spawnPoints[start].name + " INDEX: " + start);
        float itemCount = (int)GetRandom(itemSpawnCount);
        int index;

        int rockCount = (int)(itemCount * (obstaclesStartPercentage / 100f));
        int collectableCount = (int)itemCount - rockCount;
        for (int i = 0; i < rockCount; i++)
        {           
            index = Random.Range(0, obstacles.Length);
            Spawn(obstacles[index], spawnPoints[start], spawnedItemsHeight);
        }

        if (spawnGem)
        {
            collectableCount--;
            spawnGem = false;
            Spawn(gem, spawnPoints[start], spawnedItemsHeight * 2.5f);
            StartCoroutine(GemSpawnTimer());
        }

        for (int i = 0; i < collectableCount; i++)
            Spawn(coin, spawnPoints[start], spawnedItemsHeight * 2.5f);
        
    }


    void Spawn(GameObject item, SeaGenerator spawnArea, float height)
    {
        Vector3 pos;
        pos.z = GetRandom(zOffset);
        pos = spawnArea.GetRandomPos(pos.z);
        pos.y = height;
        Instantiate(item, pos, item.transform.rotation);
    }

    public void ResetMap()
    {
        for(int i = 0; i < startPositions.Length; i++)
            seaSections[i].transform.localPosition = startPositions[i];

        Obstacle[] obs = FindObjectsOfType<Obstacle>();
        foreach (Obstacle o in obs)
            Destroy(o.gameObject);
    }
}
