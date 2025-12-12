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

    public int offset;
    private Transform player;
    private Transform currentPlatformPoint;
    private int platformIndex;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < platforms.Count; i++)
        {
            // 1. Cria a Plataforma na posição padrão (0, 30, 60...)
            Transform p = Instantiate(platforms[i], new Vector3(0, 0, i * 30), transform.rotation).transform;
            currentPlatforms.Add(p);

            // 2. Lógica do Obstáculo
            int randomObstacleIndex = Random.Range(0, obstacleSections.Count);

            // Define a posição Z do obstáculo
            float obstacleZPosition = i * 30;

            // SE for o primeiro obstáculo (i == 0), empurra ele 10 metros para frente
            if (i == 0)
            {
                obstacleZPosition = 10f;
            }

            Transform o = Instantiate(obstacleSections[randomObstacleIndex], new Vector3(0, 0, obstacleZPosition), transform.rotation).transform;
            currentObstacleSections.Add(o);


            // 3. Lógica do Colecionável (Moedas)
            int randomCollectibleIndex = Random.Range(0, collectibleSections.Count);

            // Sugestão: Também empurrar a moeda para 10m se for a primeira, para não nascer dentro do player
            float collectibleZPosition = i * 30;
            if (i == 0) collectibleZPosition = 10f;

            Transform c = Instantiate(collectibleSections[randomCollectibleIndex], new Vector3(0, 0, collectibleZPosition), transform.rotation).transform;
            currentCollectibleSections.Add(c);

            offset += 30;
        }

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
        int randomObstacleIndex = Random.Range(0, obstacleSections.Count);
        GameObject newObstaclePrefab = obstacleSections[randomObstacleIndex];

        // Na reciclagem, volta ao padrão normal (offset - 30), pois o offset já está lá longe
        Transform newObstacle = Instantiate(newObstaclePrefab, new Vector3(0, 0, offset - 30), transform.rotation).transform;
        currentObstacleSections[platformIndex] = newObstacle;

        Destroy(collectibleSection);

        int randomCollectibleIndex = Random.Range(0, collectibleSections.Count);
        GameObject newCollectiblePrefab = collectibleSections[randomCollectibleIndex];

        Transform newCollectible = Instantiate(newCollectiblePrefab, new Vector3(0, 0, offset - 30), transform.rotation).transform;
        currentCollectibleSections[platformIndex] = newCollectible;
    }
}


/* 

 */