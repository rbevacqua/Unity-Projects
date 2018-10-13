using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _speed = 10f;

    private Renderer _galaxyRenderer;

	// Use this for initialization
	void Start ()
	{
	    name = "Laser";
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _galaxyRenderer = GameObject.Find("Galaxy").GetComponent<Renderer>();

	    transform.Translate(Vector3.up * _speed * Time.deltaTime);

	    if (transform.position.y > _galaxyRenderer.bounds.max.y)
	    {
	        DestroyLaser();
        }
	}

    public void DestroyLaser()
    {
        // for the triple laser prefab, check if it is contained in a triple shot parent object
        // Detach laser children from parent if one of the Laser objects collide
        if (transform.parent != null)
        {
            var parent = transform.parent.gameObject;
            transform.parent.DetachChildren();
            Destroy(parent);
        }

        Destroy(this.gameObject);

    }
}
