using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothpickGrabber : MonoBehaviour
{

    public LayerMask mask;
    public GameObject pusher;
    public Transform pusherLookTarget;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.root.transform.position.x<-15&&pusher!=null)
        {
            Destroy(pusher);
        }
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                gameObject.transform.position = hit.point;
            }
        }



            if (Input.GetMouseButton(0))
        {
           GetComponent<CapsuleCollider>().isTrigger = false;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, mask))
            {
                if(pusher!=null)
                {
                    pusher.transform.position = hit.point;
                    pusher.transform.up = hit.point - pusherLookTarget.position;
                }

                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;    
                gameObject.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(gameObject.GetComponent<Rigidbody>().position,hit.point,10*Time.deltaTime));
            }
        }
            else
        {
            GetComponent<CapsuleCollider>().isTrigger = true;

        }

    }
}
