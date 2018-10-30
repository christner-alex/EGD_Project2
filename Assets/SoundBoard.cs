using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoard : MonoBehaviour {
    public Transform leftMost; //extreme
    public Transform rightMost; //axis
    public Transform upperRightMost; //upper one
    public Transform target;
    public float minRepeatRate;
    public AudioSource audioSource;
    public AudioClip audioClip;
	// Use this for initialization
	void Start () {
        StartCoroutine("MakeSound");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator MakeSound()
    {
        audioSource.pitch = .1f+Mathf.Lerp(0,1,Mathf.Clamp01((target.position.y-rightMost.position.y)/(upperRightMost.position.y-rightMost.position.y)));
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSeconds(.05f + Mathf.Lerp(0, 1, Mathf.Clamp01((target.position.x - rightMost.position.x) / (leftMost.position.x - rightMost.position.x)))*.1f);
        // yield return new WaitForSeconds(Mathf.Lerp(0, 1, Mathf.Clamp01(Vector3.Distance(rightMost.position, target.position) / Vector3.Distance(rightMost.position, upperRightMost.position))));
        StartCoroutine("MakeSound");
        yield return new WaitForEndOfFrame();
    }
}
