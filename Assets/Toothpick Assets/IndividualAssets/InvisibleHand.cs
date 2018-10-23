using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleHand : MonoBehaviour {

    private SphereCollider col;
    private bool grabbing_pick;
    private GameObject tp;

    public float plane_y_coord;
    public float max_drag_vel;

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
        hover_pt = new Vector3(hover_pt.x, plane_y_coord, hover_pt.z);
        //print("hover pt: " + hover_pt);

        Vector3 ball_pt_2d = new Vector3(transform.position.x, hover_pt.y, transform.position.z);
        //print("ball_pt_2d: " + hover_pt);

        Vector3 ball_to_cursor = hover_pt - ball_pt_2d;
        Vector3 hand_vel = ball_to_cursor / Time.deltaTime;
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(hand_vel, max_drag_vel);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals("Toothpick"))
            {
                //Debug.Log("touched a toothpick");
                
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
            //tp.transform.position = transform.position;

            Vector3 tp_pt_2d = new Vector3(tp.transform.position.x, hover_pt.y, tp.transform.position.z);

            Vector3 tp_to_cursor = hover_pt - tp_pt_2d;
            Vector3 tpVel = tp_to_cursor / Time.deltaTime;
            tp.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(tpVel, max_drag_vel);
        }
    }
}
