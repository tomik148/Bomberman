using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        var c = collision.gameObject.GetComponent<controler>();
        if (c != null)
        {
            c.Die();
        }
    }
}
