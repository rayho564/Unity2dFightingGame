using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    //To Identify Player
    public int PlayerNumber = 1;
    Transform enemy;

    Rigidbody2D rig2d;
    Animator anim;

    float horizontal;
    float vertical;
    public float maxSpeed = 25;
    Vector3 movement;
    bool crouch;

	// Use this for initialization
	void Start () {

        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

	}

    // Update is called once per frame
    //Fixed Update because we are using their physics
    void FixedUpdate() {
        //Reason why we do this is if we want to have more than 2 players it's easier for the future
        horizontal = Input.GetAxis("Horizontal" + PlayerNumber.ToString());
        vertical = Input.GetAxis("Vertical" + PlayerNumber.ToString());

        // Only horizontal movement. Vertical is crouch and jump
        Vector3 movement = new Vector3(horizontal, 0, 0);

        // if vertical < -0.1f crouch = true/false
        crouch = (vertical < -0.1f);

        if (vertical > 0.1f)
        {

        }
        if (!crouch)
            rig2d.AddForce(movement * maxSpeed);
        else
            rig2d.velocity = Vector3.zero; // No sprites so we will not allow movement for now **Change if adding features
	}
}
