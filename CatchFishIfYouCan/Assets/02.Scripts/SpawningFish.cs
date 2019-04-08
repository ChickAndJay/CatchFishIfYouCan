using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningFish : MonoBehaviour
{
    public GameObject _right;
    public GameObject _left;
    public GameObject _up;
    public GameObject _down;

    public Transform[][] _spawnPositions = new Transform[4][];
    Transform[] _rightSpawnPositions;
    Transform[] _leftSpawnPositions;
    Transform[] _upSpawnPositions;
    Transform[] _downSpawnPositions;

    public List<GameObject> _fishesPrefabs = new List<GameObject>();
    public GameObject _jellyFishPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _spawnPositions[0] = _right.GetComponentsInChildren<Transform>();
        _spawnPositions[1] = _left.GetComponentsInChildren<Transform>();
        _spawnPositions[2] = _up.GetComponentsInChildren<Transform>();
        _spawnPositions[3] = _down.GetComponentsInChildren<Transform>();
                
        SpawnFishes();
    }

    public void SpawnFishes()
    {
        // spawn fishes
        for(int i=0; i<4; i++)
        {
            int spawningFishCount = Random.Range(1, 3);
            for (int j=0; j< spawningFishCount; j++)
            {
                int fishIndex = Random.Range(0, 3);
                int spawnPos = Random.Range(1, 6);
                Fish fish = _fishesPrefabs[fishIndex].GetComponent<Fish>();

                GameObject spawnedFish = Instantiate(_fishesPrefabs[fishIndex], _spawnPositions[i][spawnPos].position, Quaternion.identity);
                spawnedFish.GetComponent<FishMoving>()._spawnPoint = _spawnPositions[i][spawnPos].gameObject;
            }
        }

        // spawn jelly fish
        int jellyfishCount = Random.Range(2, 6);
        for(int i=0; i<jellyfishCount; i++)
        {
            int spawnPos = Random.Range(1, 4);
            GameObject spawnedJellyFish = Instantiate(_jellyFishPrefab, _spawnPositions[3][spawnPos].position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
