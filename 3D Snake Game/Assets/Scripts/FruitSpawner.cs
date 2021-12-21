using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField]
    [Range(.1f, 3f)]
    private float fruitSpawnDelay;
    [SerializeField]
    private Transform[] fruitPrefabs;
    private bool spawning = false;

    IEnumerator SpawnFruit()
    {
        while (true)
        {
            addFruit();
/*            Debug.Log("Added fruit!");*/
            yield return new WaitForSecondsRealtime(.1f);
        }
    }

    void addFruit()
    {
        Transform fruitVariant = fruitPrefabs[Random.Range(0, 10)];
        Transform spawnedFruit = Instantiate(fruitVariant, new Vector3(Random.Range(-27f,27),Random.Range(3f,57f),Random.Range(-27f,27f)), Quaternion.identity);
    }

    void startFruitSpawn()
    {
        StartCoroutine(SpawnFruit());
    }
    private void Update()
    {
     if (!spawning)
        {
            startFruitSpawn();
            spawning = true;
        }   
    }
}
