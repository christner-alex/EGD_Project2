using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

	public Camera camera;

	public GameObject[] blocks;
	public GameObject[] silhouette;
	private GameObject cSilhouette;
	[SerializeField] private int currentBlock = 0;


	public struct Grip{
		public float y;
		public Vector3 vel;
		public Vector3 prevPos;
		public Vector3 offset;


	}

	private Grip firstGrip;
	private GameObject blockHolding;
	public enum Grab{
		grabing,
		holding,
		releasing,
		waiting

	}
	public Grab _grab = Grab.waiting;

	public enum State {
		place,
		flood,
		push,
		grab


	}
	public State _state = State.place;


	public enum Structure {
		SWITCHING,
		HOVERING,


	}
	public Structure _type = Structure.SWITCHING;
	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {
		switch (_state) {
		case State.place:

			Indicate();
			//on left click up
			if (Input.GetMouseButtonUp (0)) {
				place ();

			}

			break;
		case State.flood:
			Indicate ();
			if (Input.GetMouseButton(0)) {
				place ();

			}

			break;

		case State.grab:

			GrabHandler ();

			break;


		}
	}

	void place(){
		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			GameObject temp = Instantiate (blocks[currentBlock]);
			Vector3 pointLocation = hit.point;
			temp.transform.position = new Vector3 (Mathf.Floor(pointLocation.x),Mathf.Ceil(pointLocation.y),Mathf.Floor(pointLocation.z));
		}


	}

	void Indicate(){
		switch (_type) {

		case Structure.SWITCHING:
			
			if (currentBlock == silhouette.Length) {
				currentBlock = 0;
			}
			if (currentBlock == -1) {
				currentBlock = silhouette.Length - 1;
			}
			Destroy (cSilhouette);
			cSilhouette = Instantiate (silhouette[currentBlock]);
			_type = Structure.HOVERING;
			break;



		case Structure.HOVERING:
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {

				Vector3 pointLocation = hit.point;
				cSilhouette.transform.position = new Vector3 (Mathf.Floor (pointLocation.x), Mathf.Ceil (pointLocation.y), Mathf.Floor (pointLocation.z));
			}


			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				currentBlock++;
				_type = Structure.SWITCHING;
			}
			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				currentBlock--;
				_type = Structure.SWITCHING;
			}

			break;


		}


	}

	RaycastHit checkIfBlock(){
		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.gameObject.GetComponent<BlockScript> () != null) {
				return hit;
			}
		}
		return hit;

	}


	void GrabHandler(){
		RaycastHit hit;

		switch (_grab) {
		case Grab.waiting:
			if (Input.GetMouseButtonDown (0)) {
				hit = checkIfBlock ();
				if(hit.collider != null && hit.collider.gameObject.GetComponent<BlockScript>() != null){
					blockHolding = hit.collider.gameObject;
					firstGrip.offset = hit.point - hit.collider.gameObject.transform.position;
					_grab = Grab.grabing;
				}
			}
			break;

		case Grab.grabing:
			firstGrip.y = blockHolding.transform.position.y;
			firstGrip.prevPos = blockHolding.transform.position;
			firstGrip.vel = Vector3.zero;
			blockHolding.layer = 2;
			blockHolding.transform.localScale *= .99f;
			_grab = Grab.holding;


			break;
		case Grab.holding:
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				firstGrip.y++;
			}
			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				firstGrip.y--;
			}
			if (Physics.Raycast (ray, out hit)) {
				firstGrip.vel = blockHolding.transform.position - firstGrip.prevPos;
				firstGrip.prevPos = blockHolding.transform.position;
				blockHolding.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				Vector3 pointLocation = hit.point;
				blockHolding.transform.rotation = Quaternion.identity;
				//blockHolding.transform.position = new Vector3 (pointLocation.x, firstGrip.y, pointLocation.z);
				//blockHolding.transform.position = new Vector3(Mathf.Floor (pointLocation.x), firstGrip.y, Mathf.Floor (pointLocation.z));
				//blockHolding.transform.position = Vector3.Lerp (blockHolding.transform.position,new Vector3(pointLocation.x, firstGrip.y, pointLocation.z), .5f);
				blockHolding.transform.position = new Vector3 (pointLocation.x + firstGrip.offset.x, firstGrip.y, pointLocation.z + firstGrip.offset.z);
				//blockHolding.transform.position = new Vector3 (pointLocation.x, firstGrip.y, pointLocation.z);

			}

			if (Input.GetMouseButtonUp (0)) {
				_grab = Grab.releasing;
			}

			break;
		case Grab.releasing:
			blockHolding.transform.localScale /= .99f;
			blockHolding.GetComponent<Rigidbody> ().velocity = firstGrip.vel;
			blockHolding.layer = 0;
			blockHolding = null;
			_grab = Grab.waiting;

			break;

		}


	}
}
