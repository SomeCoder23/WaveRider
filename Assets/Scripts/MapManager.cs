using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //TODO:
    //maybe add hearts or shields?
    //organize rock spawning

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
    public Obstacle[] bigObstacles;
    public Obstacle[] smallObstacles;
    public Obstacle[] powerUps;
    public Obstacle coin, gem;
    [Tooltip("The offset between each coin and the next when spawning in a straight line")]
    public float coinSpawnOffset;
    public Vector2 gemSpawnInterval = new Vector2(5, 15);
    public Vector2 powerUpsSpawnInterval = new Vector2(5, 10);
    public int maxPowerUpsSpawnCount = 4;
    [Range(1, 100)]
    public float obstaclesSpawnPercentage;
    //public float obstaclesIncreaseRate = 0.2f;

    [Range(1, 100)]
    public float collectableSpawnPercentage;
    //public float collectablesIncreaseRate = 0.1f;

    [Tooltip("Min and max items to be spawned in each section")]
    public Vector2 itemSpawnCount;
    public Vector2 zOffset;

    List<Transform> seaSections = new List<Transform>();
    List<SeaGenerator> spawnPoints = new List<SeaGenerator>();

    float spawnTime;
    Vector3[] startPositions;
    bool spawnGem = false, spawnPowerUp = false;

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
        zOffset.y -= 200;
        SpawnItems(0);
        zOffset.y += 200;

        for (int i = 1; i < spawnPoints.Count; i++)
            SpawnItems(i);

        StartCoroutine(GemSpawnTimer());
        StartCoroutine(PowerUpSpawnTimer());
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

    IEnumerator PowerUpSpawnTimer()
    {
        float elapsedTime = 0;
        float timer = GetRandom(powerUpsSpawnInterval);
        while (elapsedTime < timer)
        {
            elapsedTime++;
            yield return new WaitForSeconds(1);
        }

        spawnPowerUp = true;
    }


    void SpawnItems(int start)
    {
        //Debug.Log("SPAWNING ITEMS AT START POINT: " + spawnPoints[start].name + " INDEX: " + start);
        float itemCount = (int)GetRandom(itemSpawnCount);
        int index;

        int rockCount = (int)(itemCount * (obstaclesSpawnPercentage / 100f));
        int collectableCount = (int)itemCount - rockCount;

        //First spawn rocks
        for (int i = 0; i < (rockCount / 3) * 2; i++)
        {           
            index = Random.Range(0, smallObstacles.Length);
            Spawn(smallObstacles[index].gameObject, spawnPoints[start], smallObstacles[index].spawnHeight, smallObstacles[index].size);
        }

        for (int i = 0; i < rockCount / 3 ; i++)
        {
            index = Random.Range(0, bigObstacles.Length);
            Spawn(bigObstacles[index].gameObject, spawnPoints[start], bigObstacles[index].spawnHeight, bigObstacles[index].size);
        }

        //Second spawn coins
        for (int i = 0; i < collectableCount; i++)
        {
            int coinCount = Random.Range(0, collectableCount - i);
            Vector3 pos = Spawn(coin.gameObject, spawnPoints[start], coin.spawnHeight, coin.size);
            i += coinCount;
            while (coinCount != 0)
            {
                pos.z += coinSpawnOffset * coinCount;
                coinCount--;
                Instantiate(coin, pos, coin.transform.rotation);

            }

        }

        //Spawn gem if its time to spawn 
        if (spawnGem)
        {
            collectableCount--;
            spawnGem = false;
            Spawn(gem.gameObject, spawnPoints[start], gem.spawnHeight, gem.size);
            StartCoroutine(GemSpawnTimer());
        }

        //Spawn Power Up if its time to spawn
        if (spawnPowerUp)
        {
            spawnPowerUp = false;
            index = Random.Range(0, powerUps.Length);
            Spawn(powerUps[index].gameObject, spawnPoints[start], powerUps[index].spawnHeight, powerUps[index].size);
            StartCoroutine(PowerUpSpawnTimer());

            //int count = Random.Range(0, maxPowerUpsSpawnCount);
            //while (count > 0)
            //{
            //    count--;
            //    index = Random.Range(0, powerUps.Length);
            //    Spawn(powerUps[index].gameObject, spawnPoints[start], powerUps[index].spawnHeight);
            //}
        }

    }


    Vector3 Spawn(GameObject item, SeaGenerator spawnArea, float height, ObstacleSize size)
    {
        Vector3 pos;
        pos.z = GetRandom(zOffset);
        pos = spawnArea.GetRandomPos(pos.z, size);
        pos.y = height;
        Instantiate(item, pos, item.transform.rotation);
        return pos;
    }

    public void ResetMap()
    {
        for(int i = 0; i < startPositions.Length; i++)
            seaSections[i].transform.localPosition = startPositions[i];

        Obstacle[] obs = FindObjectsOfType<Obstacle>();
        foreach (Obstacle o in obs)
            Destroy(o.gameObject);

        Ramp[] ramps = FindObjectsOfType<Ramp>();
        foreach (Ramp r in ramps)
            Destroy(r.gameObject);
    }
}
