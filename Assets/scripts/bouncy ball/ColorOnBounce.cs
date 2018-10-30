using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOnBounce : MonoBehaviour {

    private Color col;
    public AudioClip bounceSound;
    public AudioSource a;
	// Use this for initialization
	void Start () {
        col = gameObject.GetComponent<Renderer>().material.color;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<AudioSource>().clip = bounceSound;
        gameObject.GetComponent<AudioSource>().volume = .2f;
        gameObject.GetComponent<AudioSource>().Play();
        //gameObject.GetComponent<AudioSource>().PlayOneShot(bounceSound, .2f);
        if (collision.gameObject.tag.Equals("WallSegment"))
        {

            collision.gameObject.GetComponent<Renderer>().material.color = col;
            

        }
    }
}
