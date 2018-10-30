using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {
    public float launchSpeed;
    public LayerMask interactionMask;
    public Transform spawnPoint;
    //list of random objects it can shoot
    public List<GameObject> possibleObjsToShoot;
    //list of sounds the objects might emit
    public List<AudioClip> possibleAudioToEmit;
    GameObject selectedObject;
    AudioClip selectedClip;
    float coolDown;
	// Use this for initialization
	void Start () {
        selectedObject = possibleObjsToShoot[Random.Range(0, possibleObjsToShoot.Count)];
        selectedClip = possibleAudioToEmit[Random.Range(0, possibleAudioToEmit.Count)];

    }

    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)&& Physics.Raycast(ray, out hit,100, interactionMask)&& coolDown>.01f)
        {
            GameObject g = Instantiate(selectedObject, spawnPoint.position, Quaternion.Euler(new Vector3(Random.Range(0f, 259f), Random.Range(0f, 259f), Random.Range(0f, 259f)))) as GameObject;
            //g.AddComponent<Rigidbody>();  
            g.GetComponent<Rigidbody>().velocity = (hit.point - g.transform.position).normalized * launchSpeed;
            coolDown = 0;
        }
        else
        {
            coolDown += Time.deltaTime;
        }
		
	}
}
