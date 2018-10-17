using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOnBounce : MonoBehaviour {

    private Color col;

	// Use this for initialization
	void Start () {
        col = gameObject.GetComponent<Renderer>().material.color;
	}

    private void OnCollisionEnter(Collision collision)
    { 
        if(collision.gameObject.tag.Equals("WallSegment"))
        {
            collision.gameObject.GetComponent<Renderer>().material.color = col;
        }
    }
}
