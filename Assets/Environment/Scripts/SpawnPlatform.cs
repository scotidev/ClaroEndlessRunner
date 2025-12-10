using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    [Header("Platform")]
    public List<GameObject> platforms = new List<GameObject>();
    public List<Transform> currentPlatforms = new List<Transform>();

    [Header("Obstacles")]
    public List<GameObject> obstacleSections = new List<GameObject>();
    public List<Transform> currentObstacleSections = new List<Transform>();

    public int offset;
    private Transform player;
    private Transform currentPlatformPoint;
    private int platformIndex;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < platforms.Count; i++)
        {
            Transform p = Instantiate(platforms[i], new Vector3(0, 0, i * 30), transform.rotation).transform;
            currentPlatforms.Add(p);

            int randomObstacleIndex = Random.Range(0, obstacleSections.Count);

            Transform o = Instantiate(obstacleSections[randomObstacleIndex], new Vector3(0, 0, i * 30), transform.rotation).transform;
            currentObstacleSections.Add(o);

            offset += 30;
        }

        currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
    }

    void Update()
    {
        float distance = player.position.z - currentPlatformPoint.position.z;

        if (distance >= 5)
        {
            Recycle(currentPlatforms[platformIndex].gameObject, currentObstacleSections[platformIndex].gameObject);
            platformIndex++;

            if (platformIndex > currentPlatforms.Count - 1)
            {
                platformIndex = 0;
            }

            currentPlatformPoint = currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
        }
    }

    public void Recycle(GameObject platform, GameObject obstacleSection)
    {
        platform.transform.position = new Vector3(0, 0, offset);
        obstacleSection.transform.position = new Vector3(0, 0, offset);
        offset += 30;

        Destroy(obstacleSection);

        int randomObstacleIndex = Random.Range(0, obstacleSections.Count);
        GameObject newObstaclePrefab = obstacleSections[randomObstacleIndex];

        Transform newObstacle = Instantiate(newObstaclePrefab, new Vector3(0, 0, offset - 30), transform.rotation).transform;

        currentObstacleSections[platformIndex] = newObstacle;
    }
}