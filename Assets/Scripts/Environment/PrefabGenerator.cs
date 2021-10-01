using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float maxInstances = 1;
    [SerializeField] private Transform spawn;

    private List<GameObject> instances;

    public void Generate()
    {
        if (instances.Count >= maxInstances)
        {
            removeOldestInstance();
        }
        createNewInstance();
    }

    private void removeOldestInstance()
    {
        Destroy(instances.ToArray()[0]);
        //instances.RemoveAt(0);
    }

    private void createNewInstance()
    {
        GameObject instanced = Instantiate(prefabToSpawn);
        instanced.transform.position = spawn.position;
        instances.Add(instanced);
    }

}
