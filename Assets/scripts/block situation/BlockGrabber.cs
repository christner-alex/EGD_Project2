using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrabber : MonoBehaviour {
    public LayerMask interactionPlaneLayerMask;
    public LayerMask ignoreInteractionPlaneLayerMask;
    public bool hasHitBlock;
    Vector3 intialPlaneHitPos;
    GameObject selectedBlock;
    Vector3 selectionPointOffset;
    public GameObject fingerCollider;
    public bool playingCards;
    public bool balloons;
    public float hotdogSpeed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //frame the mouse button is pressed
		if(Input.GetMouseButtonDown(0))
        {

            //raycast to see where it hits the interaction plane
            RaycastHit interactionPlaneHit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out interactionPlaneHit, 100f, interactionPlaneLayerMask.value))
            {
                intialPlaneHitPos = interactionPlaneHit.point; //stores the position the player has tapped on the interaction plane
                fingerCollider.GetComponent<Rigidbody>().position = (intialPlaneHitPos);

            }

            //raycast to see if block was hit
            RaycastHit blockHit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out blockHit, 100f, ignoreInteractionPlaneLayerMask.value))
            {
                hasHitBlock = (blockHit.collider.tag == "block"); //if it hits something with a block tag, then it hit a block
                selectedBlock = blockHit.collider.gameObject;
                selectionPointOffset = intialPlaneHitPos- selectedBlock.transform.position; //stores the offset from the raycast hit point to the object's axis
                selectionPointOffset = Vector3.Scale(selectionPointOffset, new Vector3(1, 1, 0)); //removes forward and backwards movement
            }
        }


        //if the mouse is held
        if(Input.GetMouseButton(0))
        {
            //raycast to see where the block should attempt to position itself
            RaycastHit interactionPlaneHit;
            Vector3 currentPlaneHitPos = Vector3.zero;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out interactionPlaneHit, 100f, interactionPlaneLayerMask.value))
            {
                currentPlaneHitPos = interactionPlaneHit.point; //stores the position the player has tapped on the interaction plane
                fingerCollider.GetComponent<Rigidbody>().MovePosition(currentPlaneHitPos); //dragging finger to push blocks (rather than picking them up)
                //fingerCollider.GetComponent<Rigidbody>().velocity = (currentPlaneHitPos- fingerCollider.GetComponent<Rigidbody>().position)*100*Vector3.Distance(currentPlaneHitPos,fingerCollider.transform.position); //dragging finger to push blocks (rather than picking them up)
                fingerCollider.transform.forward = fingerCollider.transform.position - Camera.main.transform.position;
                currentPlaneHitPos = new Vector3(currentPlaneHitPos.x, Mathf.Clamp(currentPlaneHitPos.y, 0, 1000), currentPlaneHitPos.z); //prevents the block from glitching through the floor
            }

            //if it has hit a block
            if (hasHitBlock)
            {
                fingerCollider.GetComponent<CapsuleCollider>().isTrigger = true; //finger pushing disabled

                if(selectedBlock.name!="banana")
                {
                    selectedBlock.GetComponent<Rigidbody>().mass = .5f; //reduce mass of block so it doesn't push other blocks too much

                }

                //set velocity to move it. Moves it toward the target position on the interation plane, scaled by the distance it is from it.
                selectedBlock.GetComponent<Rigidbody>().velocity = ((currentPlaneHitPos - (selectedBlock.transform.position)) * 4*Vector3.Distance(selectedBlock.transform.position, currentPlaneHitPos));

                //rotation stuff
                //playing cards need to be rotated differently becuase they're axis is different
                selectedBlock.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                if(playingCards)
                {
                    selectedBlock.transform.rotation = Quaternion.Lerp(selectedBlock.transform.rotation, Quaternion.Euler(new Vector3(0, 90, 0)), 3 * Time.deltaTime);
                }
                else if(balloons)
                {

                }
                else if(selectedBlock.name=="lid") //the lid on the trashcan needs this if statement to rotate correctly
                {
                    selectedBlock.transform.localRotation = Quaternion.Lerp(selectedBlock.transform.localRotation, Quaternion.Euler(new Vector3(61.437f, 0, 0)), 3 * Time.deltaTime);
                }
                else if (selectedBlock.name == "7hotdog") //hot dog rotation
                {
                    selectedBlock.GetComponent<Rigidbody>().angularVelocity=(new Vector3(Random.Range(-1f,1f) * hotdogSpeed, Random.Range(-1f, 1f) * hotdogSpeed, Random.Range(-1f, 1f) * hotdogSpeed));
                }
                else
                {
                    selectedBlock.transform.rotation = Quaternion.Lerp(selectedBlock.transform.rotation, Quaternion.Euler(Vector3.zero), 3 * Time.deltaTime);
                }

            }
            else
            {
                fingerCollider.GetComponent<CapsuleCollider>().isTrigger = false;
            }
        }
        else
        {
            selectedBlock = null;
            //if the mouse is not held reset some variables
            if (selectedBlock != null)
            {
                selectedBlock.GetComponent<Rigidbody>().mass = 1;
            }
            hasHitBlock = false;
            fingerCollider.GetComponent<SphereCollider>().isTrigger = true;

        }

    }
}
