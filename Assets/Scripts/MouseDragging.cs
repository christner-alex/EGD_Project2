using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragging : MonoBehaviour {

    public GameObject invisible_ball;
    public float plane_coord;

    private GameObject ball;
    private bool grabbing_pick;

	// Use this for initialization
	void Start () {
        ball = null;
        grabbing_pick = false;
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("mouse pos = " + Input.mousePosition);
        Vector3 hover_pt = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        //Debug.Log("hover pos = " + hover_pt);
        hover_pt = new Vector3(hover_pt.x, plane_coord, hover_pt.z);
        //Debug.Log("plane pos = " + hover_pt);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals("Toothpick"))
            {
                //grab the toothpick at that point
                Debug.Log("touched a toothpick");

                grabbing_pick = true;
            }
            else
            {
                ball = Instantiate(invisible_ball, hover_pt, Quaternion.identity);
            }
        }

        else if(Input.GetMouseButtonUp(0))
        {
            if(ball)
            {
                Destroy(ball);
                ball = null;
            }
            if(grabbing_pick)
            {
                grabbing_pick = false;
            }
        }

        else if(ball)
        {
            ball.transform.position = hover_pt;
        }
    }
}
