using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private GameManager _manager;
    private AudioSource _audioLaserSource;
    private Animator _playerAnimator;

    // laser prefab object to be instatiated by the player
    [SerializeField]
    private GameObject _laserPrefab;

    // triple laser prefab object to be instantiated when power up is active
    [SerializeField]
    private GameObject _tripleLaserPrefab;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private GameObject _shieldGameObject;

    [SerializeField]
    private GameObject[] _engines;

    // attribute used to show variable in inspector even though it is private
    [SerializeField]
    private static float _speed = 5.0f;

    [SerializeField]
    private static float _fireRate;

    private static float _canFire;

    //clone for wrapping
    private static bool _isCloned = false;

    // Power Up related varibales
    public static bool CanTripleFire { get; set; } = false;

    public static bool IsShieldUp { get; private set; } = false;

    public float SpeedBoost { get; set; } = 1.5f;

    // Use this for initialization
    void Start ()
	{
	    name = "Player";

        // initialize values for each instatiated player object
	    _fireRate = 0.25f;
	    _canFire = 0;

	    _manager = GameObject.Find("GameManager").GetComponent<GameManager>();
	    _audioLaserSource = GetComponent<AudioSource>();
	    _playerAnimator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update ()
	{
        Movement();

	    //if space key pressed, spawn a laser object
	    var galaxyRenderer = GameObject.Find("Galaxy").GetComponent<Renderer>();


	    if ((Input.GetKeyDown(KeyCode.Space) ||
	         Input.GetMouseButtonDown(0)) &&
	        transform.position.x >= galaxyRenderer.bounds.min.x &&
	        transform.position.x <= galaxyRenderer.bounds.max.x)
	    {

	       Shoot();

	    }

    }

    private void Shoot()
    {
        // if statement to check if can fire based on firing rate
        if (Time.time > _canFire)
        {

            // player laser audio
            _audioLaserSource.Play();

            // check if CanTripleShot is true
            if (CanTripleFire)
            {
                // instead of this we instatiate a single prefab object of triple shot
                //// right laser
                //Instantiate(_laserPrefab, transform.position + new Vector3(0.55f, 0.06f, 0), Quaternion.identity);
                
                //// Left laser 
                //Instantiate(_laserPrefab, transform.position + new Vector3(-0.55f, 0.06f, 0), Quaternion.identity);

                // instatiate prefab triple laser object
                Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);

            }

            _canFire = Time.time + _fireRate;
        }
    }

    private void Movement()
    {
        //Debug.Log("Ross!");
        float horizontalInput;
        float verticalInput;

#if UNITY_ANDROID
            horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
            verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
#else
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
#endif

        // do player turn animations
        if (horizontalInput < 0)
        {
            // player turns left
            _playerAnimator.SetBool("Turn_Left", true);
            _playerAnimator.SetBool("Turn_Right", false);

        } else if (horizontalInput > 0)
        {
            // player turns right
            _playerAnimator.SetBool("Turn_Left", false);
            _playerAnimator.SetBool("Turn_Right", true);

        }
        else
        {
            _playerAnimator.SetBool("Turn_Left", false);
            _playerAnimator.SetBool("Turn_Right", false);
        }

        // Horizontal and Vertical translations of player movement
        // new Vector(1, 0, 0) * 5 * 0 || -1 || 1 * period of time for each frame (Note** usally 60 frames per second)
        transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);

        transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);


        // Player bounds within the Galaxy Object defined bounds

        var playerRenderer = GetComponent<Renderer>();
        var galaxyRenderer = GameObject.Find("Galaxy").GetComponent<Renderer>();

        // Player bounds for the y axis

        if (playerRenderer.bounds.max.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0 - playerRenderer.bounds.extents.y, 0);
        }

        if (playerRenderer.bounds.min.y < galaxyRenderer.bounds.min.y)
        {
            transform.position = new Vector3(transform.position.x, galaxyRenderer.bounds.min.y + playerRenderer.bounds.extents.y, 0);
        }

        // Player bounds for the x-axis
        // we will try and create a wrapping functionality

        if (playerRenderer.bounds.min.x < galaxyRenderer.bounds.min.x && !_isCloned)
        {
            _isCloned = true;
            Instantiate(this, new Vector3(galaxyRenderer.bounds.max.x + playerRenderer.bounds.extents.x, transform.position.y, 0), Quaternion.identity);
        }

        if (playerRenderer.bounds.max.x > galaxyRenderer.bounds.max.x && !_isCloned)
        {
            _isCloned = true;
            Instantiate(this, new Vector3(galaxyRenderer.bounds.min.x - playerRenderer.bounds.extents.x, transform.position.y, 0), Quaternion.identity);
        }

        if (playerRenderer.bounds.max.x < galaxyRenderer.bounds.min.x || playerRenderer.bounds.min.x > galaxyRenderer.bounds.max.x)
        {
            _isCloned = false;
            // the this keyword refers to the script component only and not the entire gameObject
            Destroy(gameObject);
        }
        
    }

    public void Damage(GameObject source)
    {
        if (!IsShieldUp)
        {
            if (_manager.Lives == 1)
            {
                // Destroy players since the wrapping can possibly create a clone of the player
                var players = GameObject.FindGameObjectsWithTag("Player");
                foreach (var playerObj in players)
                {
                    Destroy(playerObj.gameObject);
                }

                //Instatiate Explosion
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

                // use manager to end the game
                _manager.EndGame();

            }

            //show engine damaged effect
            if (!_engines[0].activeSelf && !_engines[1].activeSelf)
            {
                _engines[Random.Range(0, 2)].SetActive(true);

            }
            else if (!_engines[0].activeSelf)
            {
                _engines[0].SetActive(true);

            }
            else
            {
                _engines[1].SetActive(true);
            }

            // remove life
            _manager.UpdateLiveUI(-1);

        }
        else
        {
            IsShieldUp = false;
            _shieldGameObject.SetActive(false);
        }
        
    }

    public void EnableTripleLaserPowerUp()
    {
        CanTripleFire = true;
        // start coroutine countdown until power up cools downn
        //StartCoroutine(TripleLaserPowerDownRoutine());
        _manager.RunCoroutine(TripleLaserPowerDownRoutine());
    }

    public void EnableSpeedPowerUp()
    {
        _speed *= SpeedBoost;
        _manager.RunCoroutine(SpeedUpPowerDownRoutine());
    }

    public void EnableShieldPowerUp()
    {
        IsShieldUp = true;
        _shieldGameObject.SetActive(true);
    }

    public IEnumerator TripleLaserPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        CanTripleFire = false;
    }

    public IEnumerator SpeedUpPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _speed /= SpeedBoost;

    }

}
