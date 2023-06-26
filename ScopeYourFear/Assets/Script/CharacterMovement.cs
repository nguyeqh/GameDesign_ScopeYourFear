using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;
    //private enum MovementState { idle = 0, running = 1, jumping = 2, falling = 3, hiding = 4 };
    private enum MovementState {idle, running, jumping, falling, hiding};
    private MovementState state = MovementState.idle;
    public bool characterIsHiding = true;

    [SerializeField] private LayerMask jumpableGround;

    public float speed;
    private float gravity = 4f;
    private bool canHide = false;
    private float dirX;
    private GameObject hideStationObject;
    private const string CHAR_WALKING = "char_run";
    private const string CHAR_JUMPING = "char_jump";
    private const string CHAR_DIE = "char_die";
    private const string CHAR_IDLE = "char_idle";
    private const string CHAR_HIDE = "char_hiding";



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Start!");
        boxCollider = GetComponent<BoxCollider2D>();
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        anim.Play(CHAR_IDLE);
        hideStationObject = GameObject.FindGameObjectWithTag("HideStation");
        

    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        MovementController();

        if (canHide && Input.GetKeyDown("up"))
        {
            anim.Play(CHAR_HIDE);
            Physics2D.IgnoreLayerCollision(8, 9, true);
            sprite.sortingOrder = 0;
            characterIsHiding = true;
            state = MovementState.hiding;
            anim.SetBool("isHiding", characterIsHiding);
            Debug.Log("Character is hiding...");
            hideStationObject.layer = 2;

        } else
        {
            Physics2D.IgnoreLayerCollision(8, 9, false);
            sprite.sortingOrder = 2;
            characterIsHiding = false;
            state = MovementState.idle;
            anim.SetBool("isHiding", characterIsHiding);
            Debug.Log("Character is not hiding...");
        }

        Debug.Log(characterIsHiding);
        
    }

    private void FixedUpdate()
    {
        if (!characterIsHiding)
        {
            player.velocity = new Vector2(dirX, player.velocity.y);
        } else
        {
            player.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Monster")
        {
            if (!characterIsHiding || state != MovementState.hiding || !canHide)
            {
                Die();
            }
        }

        if (other.gameObject.tag == "HideStation")
        {
            canHide = true;
        }

        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "HideStation")
        {
            canHide = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "HieStation")
        {
            canHide = false;
        }
    }


    //---------------------------------------------------
    private void MovementController()
    {
        
        player.velocity = new Vector2(dirX * speed , player.velocity.y);

        if (Input.GetButtonDown("Jump") && state != MovementState.jumping && isGrounded())
        {
            player.velocity = new Vector2(player.velocity.x, gravity);
            
        }

        if (dirX > 0f)
        {
            state = MovementState.running;
            anim.Play(CHAR_WALKING);
            sprite.flipX = false;
            sprite.flipY = false;
        }
        else
        {
            if (dirX < 0f)
            {
                state = MovementState.running;
                anim.Play(CHAR_WALKING);
                sprite.flipX = true;
                sprite.flipY = false;
            }
            else
            {
                state = MovementState.idle;
                sprite.flipY = false;
            }
        }

        if (!isGrounded())
        {
            state = MovementState.jumping;
        } 
        

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Die()
    {
       
        anim.SetTrigger("death");
        anim.Play(CHAR_DIE);
        Debug.Log("You died!");
        player.bodyType = RigidbodyType2D.Static;
        
        
    }

   

}
