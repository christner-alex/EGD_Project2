using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleHand : MonoBehaviour {

    private SphereCollider col;
    private bool grabbing_pick;
    private GameObject tp;

    public float plane_coord;
    public float rotateSpeed;

	// Use this for initialization
	void Start () {
        col = this.gameObject.GetComponent<SphereCollider>();
        col.enabled = false;
        tp = null;
    }
	
	// Update is called once per frame
	void Update () {

       // Vector3 hover_pt = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        //transform.position = new Vector3(hover_pt.x, plane_coord, hover_pt.z); ;

        Vector3 hover_pt = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        hover_pt = new Vector3(hover_pt.x, plane_coord, hover_pt.z);
        //print("hover pt: " + hover_pt);

        Vector3 ball_pt_2d = new Vector3(transform.position.x, hover_pt.y, transform.position.z);
        //print("ball_pt_2d: " + hover_pt);

        Vector3 to_cursor = hover_pt - ball_pt_2d;
        this.gameObject.GetComponent<Rigidbody>().velocity = to_cursor / Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals("Toothpick"))
            {
                //grab the toothpick at that point
                Debug.Log("touched a toothpick");
                
                tp = hit.collider.gameObject;
            }
            else
            {
                col.enabled = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            col.enabled = false;

            if (tp)
            {
                tp = null;
            }
        }
        else if(tp && Input.GetMouseButton(0))
        {
            tp.transform.position = transform.position;
        }
    }
}
