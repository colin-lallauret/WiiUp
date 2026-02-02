using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [Header("Prefabs à spawn")]
    public GameObject[] prefabs;

    [Header("Points de spawn")]
    public Transform[] spawnPoints;

    [Header("Options de spawn")]
    public float spawnRate = 2f;         // Temps entre chaque spawn
    public bool randomPrefab = true;
    public bool randomSpawnPoint = true;
    public int maxObjects = 20;          // Limite d’objets

    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (spawnedObjects.Count < maxObjects)
                SpawnOne();

            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnOne()
    {
        if (prefabs.Length == 0 || spawnPoints.Length == 0)
            return;

        // Choix du prefab
        GameObject prefab = randomPrefab
            ? prefabs[Random.Range(0, prefabs.Length)]
            : prefabs[0];

        // Choix du spawnpoint
        Transform spawn = randomSpawnPoint
            ? spawnPoints[Random.Range(0, spawnPoints.Length)]
            : spawnPoints[0];

        // Instanciation
        GameObject obj = Instantiate(prefab, spawn.position, spawn.rotation);

        // Ajout dans la liste (pour gestion du maxObjects)
        spawnedObjects.Add(obj);

        // Si l’objet est détruit par un KillZone, le retirer automatiquement de la liste
        AutoRemoveOnDestroy auto = obj.AddComponent<AutoRemoveOnDestroy>();
        auto.spawner = this;
    }

    // Appelé automatiquement quand un objet spawné est détruit
    public void RemoveObject(GameObject obj)
    {
        if (spawnedObjects.Contains(obj))
            spawnedObjects.Remove(obj);
    }
}


// Petit script attaché automatiquement aux objets
public class AutoRemoveOnDestroy : MonoBehaviour
{
    public PrefabSpawner spawner;

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.RemoveObject(gameObject);
    }
}
