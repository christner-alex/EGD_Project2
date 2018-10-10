using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleHand : MonoBehaviour {

    private SphereCollider col;
    private bool grabbing_pick;
    private HingeJoint toothjoint;

    public float plane_coord;

	// Use this for initialization
	void Start () {
        col = this.gameObject.GetComponent<SphereCollider>();
        col.enabled = false;
        toothjoint = null;
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 hover_pt = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        transform.position = new Vector3(hover_pt.x, plane_coord, hover_pt.z); ;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals("Toothpick"))
            {
                //grab the toothpick at that point
                Debug.Log("touched a toothpick");
                
                toothjoint = hit.collider.gameObject.GetComponent<HingeJoint>();
                toothjoint.connectedBody = this.gameObject.GetComponent<Rigidbody>();
            }
            else
            {
                col.enabled = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            col.enabled = false;

            if (toothjoint)
            {
                toothjoint.connectedBody = null;
                toothjoint = null;
            }
        }
    }
}
