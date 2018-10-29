using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAudioScript : MonoBehaviour {

	AudioSource aud;
	Rigidbody rb;

	float pitchRangeLow = .6f;
	float pitchRangeHight = .65f;
	float collisionThreshold = 0f;
	float thresholdWeight = 5f; 
    float maxVol=.1f;
	// Use this for initialization
	void Start () {
		aud = GetComponent <AudioSource>();
		rb = GetComponent <Rigidbody>();
		aud.pitch = Random.Range (pitchRangeLow, pitchRangeHight);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnCollisionEnter(Collision collision){
		float mag = collision.relativeVelocity.magnitude;

		if (mag > collisionThreshold) {
			manageAudio (aud.clip, Mathf.Clamp01((mag- collisionThreshold) / 20f)*maxVol, GetComponent<Collider>().bounds.size.magnitude);
			//manageAudio (aud.clip, Mathf.Sqrt(Mathf.Sqrt(mag))/(collisionThreshold * thresholdWeight) - .3f);
		} 


		//aud.Play ();


	}

	void manageAudio(AudioClip a, float volume, float boundsSizeMagnitude){
        //if (!aud.isPlaying) {
        boundsSizeMagnitude = Mathf.Clamp01(Mathf.Clamp(boundsSizeMagnitude - 1.7f,0,1000) / 2); //normalized (0-1) scale of how big the object it

            aud.pitch = Random.Range (pitchRangeLow- boundsSizeMagnitude*.15f, pitchRangeHight - boundsSizeMagnitude * .15f);
			aud.PlayOneShot (a, volume);
			//aud.Play ();
		//}

	}
}
