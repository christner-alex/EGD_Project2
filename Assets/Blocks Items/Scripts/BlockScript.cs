using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {

	AudioSource aud;
	Rigidbody rb;

	float pitchRangeLow = .5f;
	float pitchRangeHight = .8f;
	float collisionThreshold = .01f;
	float thresholdWeight = 5f;
	float maxVol = .6f;
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
		//if (mag > 10f) {
		//	print (mag);

		//}
		if (mag > collisionThreshold) {
			mag /= 20f;
			if (mag > maxVol) {
				mag = maxVol;
			}

			manageAudio (aud.clip, mag);
			//manageAudio (aud.clip, Mathf.Sqrt(Mathf.Sqrt(mag))/(collisionThreshold * thresholdWeight) - .3f);
		} 


		//aud.Play ();


	}

	void manageAudio(AudioClip a, float volume){
		//if (!aud.isPlaying) {
			aud.pitch = Random.Range (pitchRangeLow, pitchRangeHight);
			aud.volume = volume;
			//aud.PlayOneShot (a, volume);
			aud.Play ();
		//}

	}
}
