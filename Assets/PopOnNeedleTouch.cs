using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopOnNeedleTouch : MonoBehaviour {
    public AudioClip[] options;
    public AudioClip popSound;
    public float volume;
    public GameObject particleEffect;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, Random.Range(1f, -1f));
        popSound = options[Random.Range(0, options.Length)];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.name.Contains("needle") && col.collider.GetType().ToString() == typeof(SphereCollider).ToString())
        {
            col.gameObject.GetComponent<AudioSource>().clip = popSound;
            col.gameObject.GetComponent<AudioSource>().PlayOneShot(popSound, volume);
            GameObject g = Instantiate(particleEffect, transform.position, Quaternion.identity) as GameObject;
            g.transform.parent = col.transform;
            gameObject.SetActive(false);
        }
    }
}
