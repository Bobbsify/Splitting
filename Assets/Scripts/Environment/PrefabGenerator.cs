using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PrefabGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float maxInstances = 1;
    [SerializeField] private Transform spawn;

    [SerializeField] private UnityEvent generationEvents;

    private List<GameObject> instances = new List<GameObject>();

    public void Generate()
    {
        Debug.Log(instances.Count);
        if (instances.Count >= maxInstances)
        {
            removeOldestInstance();
        }
        createNewInstance();
    }

    private void removeOldestInstance()
    {
        Destroy(instances.ToArray()[0]);
        instances.RemoveAt(0);
    }

    private void createNewInstance()
    {
        GameObject instanced = Instantiate(prefabToSpawn);
        instanced.transform.position = spawn.position;
        instances.Add(instanced);
        generationEvents.Invoke();
    }

}
