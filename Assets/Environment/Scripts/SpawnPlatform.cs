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

    [Header("Collectibles")]
    public List<GameObject> collectibleSections = new List<GameObject>();
    public List<Transform> currentCollectibleSections = new List<Transform>();

    [Header("Safety Configuration")]
    public int safePlatformCount = 2;

    public int offset;
    private Transform player;
    private Transform currentPlatformPoint;
    private int platformIndex;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        offset = 0;

        for (int i = 0; i < platforms.Count; i++)
        {
            Transform p = Instantiate(platforms[i], new Vector3(0, 0, offset), transform.rotation).transform;
            currentPlatforms.Add(p);

            Transform o;

            if (i < safePlatformCount)
            {
                if (obstacleSections.Count > 0)
                {
                    o = Instantiate(obstacleSections[0], new Vector3(0, 0, offset), transform.rotation).transform;
                }
                else
                {
                    o = new GameObject("SafeSpawn Fallback").transform;
                    o.position = new Vector3(0, 0, offset);
                }
            }
            else
            {
                int randomObstacleIndex = Random.Range(1, obstacleSections.Count);
                o = Instantiate(obstacleSections[randomObstacleIndex], new Vector3(0, 0, offset), transform.rotation).transform;
            }

            currentObstacleSections.Add(o);

            int randomCollectibleIndex = Random.Range(0, collectibleSections.Count);
            Transform c = Instantiate(collectibleSections[randomCollectibleIndex], new Vector3(0, 0, offset), transform.rotation).transform;
            currentCollectibleSections.Add(c);

            offset += 30;
        }

        platformIndex = 0;
        currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
    }

    void Update()
    {
        float distance = player.position.z - currentPlatformPoint.position.z;

        if (distance >= 5)
        {
            Recycle(
                currentPlatforms[platformIndex].gameObject,
                currentObstacleSections[platformIndex].gameObject,
                currentCollectibleSections[platformIndex].gameObject
             );

            platformIndex++;

            if (platformIndex > currentPlatforms.Count - 1)
            {
                platformIndex = 0;
            }

            currentPlatformPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
        }
    }

    public void Recycle(GameObject platform, GameObject obstacleSection, GameObject collectibleSection)
    {
        platform.transform.position = new Vector3(0, 0, offset);
        offset += 30;

        Destroy(obstacleSection);

        if (obstacleSections.Count > 1)
        {
            int randomObstacleIndex = Random.Range(1, obstacleSections.Count);
            GameObject newObstaclePrefab = obstacleSections[randomObstacleIndex];

            Transform newObstacle = Instantiate(newObstaclePrefab, new Vector3(0, 0, offset - 30), transform.rotation).transform;
            currentObstacleSections[platformIndex] = newObstacle;
        }
        else
        {
            Transform newObstacle = Instantiate(obstacleSections[0], new Vector3(0, 0, offset - 30), transform.rotation).transform;
            currentObstacleSections[platformIndex] = newObstacle;
        }


        Destroy(collectibleSection);

        int randomCollectibleIndex = Random.Range(0, collectibleSections.Count);
        GameObject newCollectiblePrefab = collectibleSections[randomCollectibleIndex];

        Transform newCollectible = Instantiate(newCollectiblePrefab, new Vector3(0, 0, offset - 30), transform.rotation).transform;
        currentCollectibleSections[platformIndex] = newCollectible;
    }
}