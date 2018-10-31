
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanScript : MonoBehaviour {


	private float waitTime = 1.5f;
	private bool ready = false;

	[SerializeField] private AudioClip[] clips;
	[SerializeField] private AudioClip yeah;
	[SerializeField] private AudioClip[] sad;
	AudioSource aud;

	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
		aud.volume = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!ready) {
			waitTime -= Time.deltaTime;
			if (waitTime <= 0f) {
				ready = true;
				aud.volume = 1f;
			}
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.name == "can"||col.gameObject.name.Contains("Trash")) {
			return;
		}else if (col.gameObject.name == "lid") {
			aud.clip = yeah;
			aud.Play ();
		} else {
			int i = Random.Range (0, clips.Length);
			aud.clip = clips [i];
			aud.Play ();

		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.name != "lid"&&!col.gameObject.name.Contains("can") && !col.gameObject.name.Contains("Trash")) {
			int i = Random.Range (0, sad.Length);
			aud.clip = sad [i];
			aud.Play ();


		}
	}

}
