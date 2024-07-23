using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    public float spawnHeight = 6;
    public ObstacleSize size;
    
}

public enum ObstacleSize
{
    small,
    medium,
    big
}

