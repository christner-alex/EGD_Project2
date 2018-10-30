using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMass : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(Random.Range(0,2)==1)
        {
            GetComponent<Rigidbody>().mass = .05f;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
