using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        TripleLaserPowerUp,
        SpeedPowerUp,
        SheildPowerUp
    }

    [SerializeField]
    private float _speed;

    [SerializeField]
    private int _power;

    [SerializeField]
    private AudioClip _audioPowerUpClip;

	// Use this for initialization
	void Start ()
	{
	    name = ((PowerUpType)_power).ToString();

    }

	// Update is called once per frame
	void Update ()
	{
	    transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Destroy This object if it falls below the screen.
	    var powerUpRenderer = GetComponent<Renderer>();

        if (transform.position.y < 0 && !powerUpRenderer.isVisible)
	    {
	        Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            // defensive programming checking 
            if (player != null)
            {
                if (_power == (int)PowerUpType.TripleLaserPowerUp)
                {
                    player.EnableTripleLaserPowerUp();
                }
                else if (_power == (int)PowerUpType.SpeedPowerUp)
                {
                    player.EnableSpeedPowerUp();
                }
                else if (_power == (int) PowerUpType.SheildPowerUp)
                {
                    player.EnableShieldPowerUp();
                }

            }

            // play audio clip in world space before this power up is destroyed
            AudioSource.PlayClipAtPoint(_audioPowerUpClip, Camera.main.transform.position, 1f);
            
            Destroy(this.gameObject);
        }
        
    }
}
