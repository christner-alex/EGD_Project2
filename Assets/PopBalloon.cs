using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopBalloon : MonoBehaviour {

    public string balloon_tag;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals(balloon_tag))
        {
            Pop(collision.gameObject);
        }
    }

    private void Pop(GameObject balloon)
    {
        Destroy(balloon);
    }
}
