using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashcanRanomMovement : MonoBehaviour {
    public Transform minZ;
    public Transform maxZ;
    public Transform minX;
    public Transform maxX;
    public float minMoveDuration;
    public float maxMoveDuration;
    // Use this for initialization
    void Start () {
        StartCoroutine("StartCan");

    }

    // Update is called once per frame
    void Update () {
		
	}
    IEnumerator StartCan()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine("MoveRandom");
        yield return new WaitForEndOfFrame();
    }
    IEnumerator MoveRandom()
    {
        //choose random pos
        Vector3 randomPos = new Vector3(Random.Range(minX.position.x, maxX.position.x), transform.position.y, Random.Range(minZ.position.z, maxZ.position.z));
        Vector3 startPosition = transform.position;
        float startTime = Time.time;
        float duration = Random.Range(minMoveDuration, maxMoveDuration);
        while(Time.time-startTime<duration)
        {
            GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(startPosition, randomPos, (Time.time - startTime) / duration));
            yield return new WaitForEndOfFrame();

        }
        StartCoroutine("MoveRandom");
        yield return new WaitForEndOfFrame();
    }
}
