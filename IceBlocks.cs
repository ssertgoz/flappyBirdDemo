using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;


public class IceBlocks : MonoBehaviour
{
    class PoolObject
    {
        public Transform transform;
        public bool inUse = false;
        public PoolObject(Transform t) { transform = t; }
        public void Use() { inUse = true; }
        public void Dispose() { inUse = false; }
    }
    [System.Serializable]
    public struct YSpawnRange
    {
        public float min;
        public float max;
    }
    
    public GameObject Prefab;
    public int poolSize;
    public float shiftSepeed;
    public float spawnRate;

    public YSpawnRange ySpawnRange;
    public Vector3 defoultSpawnPos;
    public bool spawnImmediate;
    public Vector3 immediateSpawnPos;
    public Vector2 targetAspectRatio;
    
    private float spawnTimer;
    private float targetAspect;
    private PoolObject[] poolObjects;
    private GameManager game;

    private void Awake()
    {
        Configure();
    }

    private void Start()
    {
        game = GameManager.Instance;
    }

    private void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnDisable()
    {
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameOverConfirmed()
    {
        for (int i = 0; i < poolObjects.Length; i++)
                 {
                     poolObjects[i].Dispose();
                     poolObjects[i].transform.position = new Vector3(1550,0);
                 }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }

    private void Update()
    {
        if(game.GameOver) return;

        Shift();
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnRate)
        {
            Spawn();
            spawnTimer = 0;
            
        }
    }

    void Configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        int a = 0;
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Instantiate(Prefab) as GameObject;
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = new Vector3(1550 + a ,Random.Range(-15,25));
            poolObjects[i] = new PoolObject(t);
            //a += 25;
        }

        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }

    void Spawn()
    {
        Transform t = GetPoolObject();
        if (t == null) return; // if true its indicates that poolSize is too small
        Vector3 pos = new Vector3(0,0);
        pos.x = defoultSpawnPos.x;
        
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
    }

    void SpawnImmediate()
    {
        Transform t = GetPoolObject();
        if (t == null) return; // if true its indicates that poolSize is too small
        Vector3 pos = Vector3.zero;
        pos.x = immediateSpawnPos.x;
        pos.y = UnityEngine.Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
        Spawn();
    }

    void Shift()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].transform.position += -Vector3.right * (shiftSepeed * Time.deltaTime);
            CheckDisposeObject(poolObjects[i]);
        }
    }

    void CheckDisposeObject(PoolObject poolObject)
    {
        if (poolObject.transform.position.x < -defoultSpawnPos.x-80)
        {
            poolObject.Dispose();
            poolObject.transform.position = new Vector3(defoultSpawnPos.x,Random.Range(ySpawnRange.min,ySpawnRange.max));
        }
    }

    Transform GetPoolObject()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }

        return null;
    }
}
