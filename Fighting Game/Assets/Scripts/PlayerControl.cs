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

    public float JumpForce = 20;
    public float jumpDuration = .1f;
    float jmpDuration;
    float jmpForce;
    bool jumpKey = false;
    bool falling;
    bool onGround;
    bool crouch;

    // Use this for initialization
    void Start () {

        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

	}

    void Update()
    {
        UpdateAnimator();
    }

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
            if (!jumpKey)
            {
                jmpDuration += Time.deltaTime;
                jmpForce += Time.deltaTime;

                if(jmpDuration < jumpDuration)
                {
                    rig2d.velocity = new Vector2(rig2d.velocity.x, jmpForce);
                }
                else
                {
                    jumpKey = true;
                }
            }
        }

        if(!onGround && vertical < 0.1f)
        {
            falling = true;
        }

        if (!crouch)
            rig2d.AddForce(movement * maxSpeed);
        else
            rig2d.velocity = Vector3.zero; // No sprites so we will not allow movement for now **Change if adding features
	}

    

    void UpdateAnimator()
    {
        anim.SetBool("Crouch", this.crouch);
        anim.SetBool("OnGround", this.onGround);
        anim.SetBool("Falling", this.falling);
        anim.SetFloat("Movement", Mathf.Abs(horizontal));
    }

    //Enter collision with ground - on ground
    void OnCollisionEnter2d(Collision2D col)
    {
        if(col.collider.tag == "Ground")
        {
            onGround = true;

            jumpKey = false;
            jmpDuration = 0;
            jmpForce = JumpForce;
            falling = false;
        }
    }

    //Exit collision with ground - air
    void OnCollisionExit2d(Collision2D col)
    {
        if (col.collider.tag == "Ground")
        {
            onGround = false;
        }
    }
}
