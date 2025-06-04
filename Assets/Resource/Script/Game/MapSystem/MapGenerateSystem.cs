using System.Collections.Generic;
using UnityEngine;

public class MapGenerateSystem : MonoBehaviour
{
    [Header("Map Prefab")]
    [SerializeField]
    public List<GameObject> MapPrefab;

    [Header("Map Limit")]
    [SerializeField]
    private int maxSize = 6;

    private Queue<GameObject> _mapQueue = new Queue<GameObject>();

    [Header("StartPoint")]
    [SerializeField]
    private Transform spawnPoint;

    [Header("Interval")]
    [SerializeField]
    private float mapLength = 10f;

    private Vector3 _nextSpawnPosition;
    private Vector3 _nextCheckPosition;

    private Collider _collider;
    private void Start()
    {
        _collider = GetComponent<Collider>();
        _nextSpawnPosition = spawnPoint.position;
        InitMap(maxSize);
        InitCheckPosTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            updateCheckPos();
            ExtendMap();
        }
    }

    // Exxternal Operation for map
    private void InitMap(int size)
    {
        for (int i = 0; i < size-1; i++)
        {
            EnqueueMap(RandomPickMap());
        }
    }
    private void ExtendMap()
    {
        EnqueueMap(RandomPickMap());
    }

    // Internal Operation for map

    private void EnqueueMap(GameObject map)
    {
        while (_mapQueue.Count >= maxSize)
        {
            if (_mapQueue.Count > 0)
                Destroy(_mapQueue.Dequeue());
            if (_mapQueue.Count > 0)
                Destroy(_mapQueue.Dequeue());
        }

        _mapQueue.Enqueue(map);
    }

    private GameObject RandomPickMap()
    {
        if (MapPrefab == null || MapPrefab.Count == 0)
        {
            Debug.LogError("MapPrefab List empty!!¡I");
            return null;
        }

        int index = Random.Range(0, MapPrefab.Count);
        GameObject map = Instantiate(MapPrefab[index], _nextSpawnPosition, Quaternion.identity);
        _nextSpawnPosition += new Vector3(0, 0, mapLength);
        return map;
    }
    private void InitCheckPosTarget()
    {
        _nextCheckPosition = new Vector3(0, 0, 7);
    }
    private void updateCheckPos()
    {
        gameObject.transform.position = _nextCheckPosition;
        _nextCheckPosition += new Vector3(0, 0, mapLength);
    }
}
