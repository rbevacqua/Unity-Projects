using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.5f;

    private Renderer _galaxyRenderer;

    [SerializeField]
    private AudioClip _audioExplosionClip;

    private static int sortingOrderNum = 10;

	// Use this for initialization
	void Start ()
	{
	    this.gameObject.GetComponent<Renderer>().sortingOrder = sortingOrderNum;

	    sortingOrderNum++;

	    if (sortingOrderNum > 100)
	    {
	        sortingOrderNum = 10;
	    }

        // this plays the clip fully even if this explosion object is destroyed before audio finishes
        AudioSource.PlayClipAtPoint(_audioExplosionClip, Camera.main.transform.position, 1f);

        Destroy(this.gameObject, 4.0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Movement();

	}

    public void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }


}
