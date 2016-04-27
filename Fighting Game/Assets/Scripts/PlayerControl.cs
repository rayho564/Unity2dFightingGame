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

    public float attackRate = 0.3f;
    bool[] attack = new bool[2]; //Array so we can add new attacks later
    float[] attackTimer = new float[2];
    int[] timesPressed = new int[2];

    public bool damage;
    public float invincibility = 1; //time it doesn't take damage after hurt
    float invincibilityTimer;

    public bool specialAttack;
    public GameObject projectile;

    public float health;

    // Use this for initialization
    void Start () {

        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        jmpForce = JumpForce;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach( GameObject pl in players)
        {
            if(pl.transform != this.transform)
            {
                enemy = pl.transform;
            }
        }
	}

    void Update()
    {
        AttackInput();
        ScaleCheck();
        OnGroundCheck();
        Damage();
        SpecialAttack();
        UpdateAnimator();
        WinCheck();
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

        if( (attack[0] && !jumpKey) || (attack[1] && !jumpKey))
            movement = Vector3.zero;

        if (!crouch)
        {

            if (Mathf.Abs(horizontal) < maxSpeed)
            {
                rig2d.AddForce(movement * maxSpeed);
            }
        }
        else if(crouch && onGround)
            rig2d.velocity = Vector3.zero; // No sprites so we will not allow movement for now **Change if adding features
	}

    void WinCheck()
    {
        if (enemy.GetComponent<PlayerControl>().health < 0)
            Application.LoadLevel("WinScreen");
    }

    void ScaleCheck()
    {
        if(transform.position.x < enemy.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    void AttackInput()
    {
        if (Input.GetButtonDown("Attack1" + PlayerNumber.ToString()))
        {
            attack[0] = true;
            attackTimer[0] = 0;
            timesPressed[0]++;
        }
        if(attack[0])
        {
            attackTimer[0] += Time.deltaTime;

            if(attackTimer[0] > attackRate || timesPressed[0] >= 4)
            {
                attackTimer[0] = 0;
                attack[0] = false;
                timesPressed[0] = 0;
            }
        }
        if (Input.GetButtonDown("Attack2" + PlayerNumber.ToString()))
        {
            attack[1] = true;
            attackTimer[1] = 0;
            timesPressed[1]++;
        }
        if (attack[1])
        {
            attackTimer[1] += Time.deltaTime;

            if (attackTimer[1] > attackRate || timesPressed[1] >= 4)
            {
                attackTimer[1] = 0;
                attack[1] = false;
                timesPressed[1] = 0;
            }
        }
    }

    void Damage()
    {
        if (damage)
        {

            invincibilityTimer += Time.deltaTime;

            if(invincibilityTimer > invincibility )
            {
                damage = false;
                invincibilityTimer = 0;
            }
        }

    }

    void SpecialAttack()
    {
        if(specialAttack)
        {
            Vector3 pos;
            if (enemy.position.x - transform.position.x > 0)
            {
                pos = transform.position + new Vector3(1, 0, 0);
            }
            else
            {
                pos = transform.position + new Vector3(-2, 0, 0);
            }
            
            GameObject pr = Instantiate(projectile, pos, Quaternion.identity) as GameObject;
            Vector3 nrDir = new Vector3(enemy.position.x, transform.position.y, 0);
            Vector3 dir = nrDir - transform.position;
            if (transform.position.x > enemy.position.x)
            {
                pr.transform.localScale = new Vector3(-1, 1, 1);
            }

            pr.GetComponent<Rigidbody2D>().AddForce(dir * 3, ForceMode2D.Impulse);

            specialAttack = false;
            Destroy(pr, 2);
        }

    }

    void OnGroundCheck()
    {
        if (!onGround)
            rig2d.gravityScale = 3;
        else
            rig2d.gravityScale = 1;
    }

    void UpdateAnimator()
    {
        anim.SetBool("Crouch", this.crouch);
        anim.SetBool("OnGround", this.onGround);
        anim.SetBool("Falling", this.falling);
        anim.SetFloat("Movement", Mathf.Abs(horizontal));
        anim.SetBool("Attack1", this.attack[0]);
        anim.SetBool("Attack2", this.attack[1]);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            Destroy(col.gameObject);
        }
    }

    //Enter collision with ground - on ground
    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.collider.tag == "Ground")
        {
            onGround = true;

            //Change later
            rig2d.velocity = new Vector3(.1f, 0, 0 );

            jumpKey = false;
            jmpDuration = 0;
            jmpForce = JumpForce;
            falling = false;
        }
    }

    //Exit collision with ground - air
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.tag == "Ground")
        {
            onGround = false;
        }
    }

}
