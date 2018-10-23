using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBlockAudioToChildren : MonoBehaviour {
    public AudioClip clip;
	// Use this for initialization
	void Start () {
		foreach(Transform t in GetComponentInChildren<Transform>())
        {
            t.gameObject.AddComponent<AudioSource>();
            t.gameObject.AddComponent<BlockAudioScript>();
        }
        foreach (Transform t in GetComponentInChildren<Transform>())
        {
            t.gameObject.GetComponent<AudioSource>().clip = clip;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
