using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndThrow : MonoBehaviour {

    public float plane_coord;
    public float max_drag_vel;

    private GameObject grabbed_ball;

	// Use this for initialization
	void Start () {
        grabbed_ball = null;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals("BouncyBall"))
            {
                grabbed_ball = hit.collider.gameObject;
            }
        }
        else if(Input.GetMouseButton(0) && grabbed_ball)
        {
            Vector3 hover_pt = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1 * Camera.main.transform.position.z));
            hover_pt = new Vector3(hover_pt.x, hover_pt.y, plane_coord);
            print("hover pt: " + hover_pt);
            
            Vector3 ball_pt_2d = new Vector3(grabbed_ball.transform.position.x, grabbed_ball.transform.position.y, hover_pt.z);
            print("ball_pt_2d: " + hover_pt);

            Vector3 to_cursor = hover_pt - ball_pt_2d;

            grabbed_ball.GetComponent<Rigidbody>().velocity = to_cursor / Time.deltaTime;
        }
        else if (!Input.GetMouseButton(0))
        {
            grabbed_ball = null;
        }
    }

    
}
