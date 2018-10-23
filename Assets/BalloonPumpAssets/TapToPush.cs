using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToPush : MonoBehaviour {

    public string balloon_tag;
    public float force_dist_ratio;
    public float grav;

    public float max_vel_after_click;
    public float max_vel_base;
    public float max_vel_decay_per_sec;

    private Rigidbody rb;
    private float max_vel;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        max_vel = max_vel_base;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals(balloon_tag))
            {
                Vector3 balloon_center = transform.position;
                Vector3 tap_loc = new Vector3(hit.point.x, hit.point.y, 0);

                Vector3 toward_balloon = balloon_center - tap_loc;
                float dist = toward_balloon.magnitude;

                rb.AddForce(toward_balloon.normalized * dist * force_dist_ratio, ForceMode.Impulse);

                max_vel = max_vel_after_click;
            }
        }

        rb.AddForce(grav * Vector3.down, ForceMode.Acceleration);

        max_vel -= max_vel_decay_per_sec * Time.deltaTime;
        max_vel = Mathf.Clamp(max_vel, max_vel_base, max_vel_after_click);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, max_vel);
    }
}
