using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopOnNeedleTouch : MonoBehaviour {
    public AudioClip popSound;
    public float volume;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, Random.Range(1f, -1f));
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
            gameObject.SetActive(false);
        }
    }
}
