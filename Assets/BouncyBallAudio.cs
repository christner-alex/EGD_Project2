using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBallAudio : MonoBehaviour {
    public AudioClip[] bounceSounds;
    public GameObject[] balls;
	// Use this for initialization
	void Start () {
        int randomIndex = Random.Range(0, bounceSounds.Length);
        foreach(GameObject g in balls)
        {
            g.GetComponent<ColorOnBounce>().bounceSound = bounceSounds[randomIndex];
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
