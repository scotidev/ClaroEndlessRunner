using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacle : MonoBehaviour
{
    public List<GameObject> spawnList = new List<GameObject>();
    public int minSpawn;
    public int maxSpawn;
    public LayerMask blockLayer;

    public Collider map;

    private void Start()
    {
        StartSpawn();
    }

    public void StartSpawn()
    {
        if (map == null)
        {
            Debug.LogError("O Collider 'map' não foi inicializado.ollider!");
            return;
        }

        int amount = Random.Range(minSpawn, maxSpawn + 1);
        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, spawnList.Count);
            GameObject itemToSpawn = spawnList[randomIndex];
            if (Spawn(itemToSpawn) == false)
            {
                return;
            }
        }
    }

    private bool Spawn(GameObject item)
    {
        Vector3 position = Vector3.zero;
        int counter = 0;

        float halfSizeItemX = item.transform.localScale.x / 2;
        float halfSizeItemZ = item.transform.localScale.z / 2;

        float limitNegativeX = map.bounds.min.x + halfSizeItemX;
        float limitPositiveX = map.bounds.max.x - halfSizeItemX;
        float limitNegativeZ = map.bounds.min.z + halfSizeItemZ;
        float limitPositiveZ = map.bounds.max.z - halfSizeItemZ;

        do
        {
            counter++;
            if (counter > 50)
            {
                Debug.Log("too many attemps");
                return false;
            }

            position.x = Random.Range(limitNegativeX, limitPositiveX);
            position.z = Random.Range(limitNegativeZ, limitPositiveZ);
        } while (isPositionBlocked(item, position));

        Instantiate(item, position, Quaternion.identity);

        return true;
    }

    private bool isPositionBlocked(GameObject item, Vector3 position)
    {
        return Physics.CheckBox(position, item.transform.localScale / 2, Quaternion.identity, blockLayer);
    }
}