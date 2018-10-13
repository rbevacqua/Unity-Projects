using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemyShipPrefab;

    [SerializeField] private GameObject[] _powerUpPrefabs;

    [SerializeField] private float _enemySpawnRate = 5;

    [SerializeField] private float _powerUpSpawnRate = 12;

    private Renderer _galaxyRenderer;

	// Use this for initialization
	void Start ()
	{
	    _galaxyRenderer = GameObject.Find("Galaxy").GetComponent<Renderer>();

	}

    public void StartSpawningEnemies(int numberEnemies)
    {
        for (int i = 0; i < numberEnemies; i++)
        {
            StartCoroutine(SpawnEnemyRoutine());
        }

    }

    public void StartAllSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpsRoutine());
    }

    public void StopAllSpawning()
    {
        StopAllCoroutines();
    }

    // create a coroutine to spawn an enemy every 5 seconds
    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {

            var enemyRenderer = _enemyShipPrefab.GetComponent<Renderer>();
            var minEnemySpawnX = _galaxyRenderer.bounds.min.x + enemyRenderer.bounds.extents.x;
            var maxEnemySpawnX = _galaxyRenderer.bounds.max.x - enemyRenderer.bounds.extents.x;
            var enemySpawnY = _galaxyRenderer.bounds.max.y + enemyRenderer.bounds.extents.y;

            
            var randomX = Random.Range(minEnemySpawnX, maxEnemySpawnX);
            Instantiate(_enemyShipPrefab, new Vector3(randomX, enemySpawnY, 0), Quaternion.identity);
            

            // wait a certain amount of seconds than continue this function after _enemySpawnRate seconds
            yield return new WaitForSeconds(_enemySpawnRate);

        }
    }

    IEnumerator SpawnPowerUpsRoutine()
    {
        while (true)
        {
            // All Power Up Objects should be rendered the same size so we only need one of their renderers
            var powerUpRenderer = _powerUpPrefabs[0].GetComponent<Renderer>();

            // Get the spawn location bounds
            var minPowerUpSpawnX = _galaxyRenderer.bounds.min.x + powerUpRenderer.bounds.extents.x;
            var maxPowerUpSpawnX = _galaxyRenderer.bounds.max.x - powerUpRenderer.bounds.extents.x;
            var powerUpSpawnY = _galaxyRenderer.bounds.max.y + powerUpRenderer.bounds.extents.y;

            
            var randomX = Random.Range(minPowerUpSpawnX, maxPowerUpSpawnX);

            var randomPowerUp = Random.Range(0, 3);

            Instantiate(_powerUpPrefabs[randomPowerUp], new Vector3(randomX, powerUpSpawnY, 0), Quaternion.identity);

            // Power Up Spawn cool down
            yield return new WaitForSeconds(_powerUpSpawnRate);
        }
        
    }

}
