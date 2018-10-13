using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    private GameManager _manager;

    [SerializeField]
    private float _speed;

    private Renderer _enemyRenderer;

    private Renderer _galaxyRenderer;

    private float _minEnemySpawnX;

    private float _maxEnemySpawnX;

    private float _enemySpawnY;

    [SerializeField]
    private GameObject _explosionPrefab;


	// Use this for initialization
	void Start ()
	{

	    _manager = GameObject.Find("GameManager").GetComponent<GameManager>();

	    _enemyRenderer = GetComponent<Renderer>();
	    _galaxyRenderer = GameObject.Find("Galaxy").GetComponent<Renderer>();

	    _minEnemySpawnX = _galaxyRenderer.bounds.min.x + _enemyRenderer.bounds.extents.x;
	    _maxEnemySpawnX = _galaxyRenderer.bounds.max.x - _enemyRenderer.bounds.extents.x;
	    _enemySpawnY = _galaxyRenderer.bounds.max.y + _enemyRenderer.bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Movement();

	    if (_enemyRenderer.bounds.max.y < _galaxyRenderer.bounds.min.y)
	    {
            float randomX = Random.Range(_minEnemySpawnX, _maxEnemySpawnX);
            transform.position = new Vector3(randomX, _enemySpawnY, 0);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage(this.gameObject);
            }

            // Destory the enemy
            BlowUp();
        }
        else if (other.tag == "Laser")
        {
            Laser laser = other.GetComponent<Laser>();

            // add to player score for destroying this enemy object

            _manager.AddUpdateScoreUI(10);

            if (laser != null)
            {
                // Destory the laser and enemy objects
                laser.DestroyLaser();
                BlowUp();
            }
        }
    }

    private void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    public void BlowUp()
    {

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        Destroy(this.gameObject);

    }

}
