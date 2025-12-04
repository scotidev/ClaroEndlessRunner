using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();
    public int offset;

    public GameObject platformPrefab;

    void Start()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            Instantiate(platforms[i], new Vector3(0, 0, i * 31), transform.rotation);
            offset += 31;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Pressionei R");
            Recycle(platformPrefab);
            Debug.Log(platformPrefab);
        }
    }

    public void Recycle(GameObject platform)
    {
        platform.transform.position = new Vector3(0, 0, offset);
        offset += 31;
    }
}