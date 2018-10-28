using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSpot : MonoBehaviour {
    public Transform targetLocation;
    bool move;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(move)
        {
            transform.position = Vector3.Lerp(transform.position, targetLocation.position, 2 * Time.deltaTime);
        }
	}
    public void MoveToTarget()
    {
        move = true;
    }
}
