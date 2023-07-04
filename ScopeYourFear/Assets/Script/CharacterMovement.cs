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

    //private enum MovementState { idle = 0, walking = 1, jumping = 2, falling = 3, hiding = 4 , running = 5, collect = 6};
    private enum MovementState {idle, walking, jumping, falling, hiding, running, collect};
    private MovementState state = MovementState.idle;
    public bool characterIsHiding = true;
    public bool characterIsDead = false;
    public bool stage_finish = false;
    public bool can_collect = false;
    public bool charJustCollectSomething = false;
    public bool runningMode = false;
    private bool walkingOnObject = false;


    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private AudioSource walkingSoundEffect, breathHidingSoundEffect, runningSoundEffect, jumpSoundEffect;
    [SerializeField] private AudioSource screamSoundEffect;

    public float runningSpeed;
    public Vector2 speed = new Vector2(1, 0);
    private float gravity = 4f;
    private bool canHide = false;
    private float dirX, dirY;

    private GameObject hideStationObject;
    GameObject currentTeleporter, secondFloorTeleporter;
    private GameObject blindMonsterObject, monster1Object;
    private const string CHAR_WALKING = "char_walking";
    private const string CHAR_RUNNING = "char_run";
    private const string CHAR_JUMPING = "char_jump";
    private const string CHAR_DIE = "char_die";
    private const string CHAR_IDLE = "char_idle";
    private const string CHAR_HIDE = "char_hiding";
    private const string CHAR_SEE_MONSTER = "char_see_monster";
    private const string CHAR_COLLECT = "char_collect";


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Start!");
        boxCollider = GetComponent<BoxCollider2D>();
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        anim.Play(CHAR_IDLE);

        runningMode = false;
        screamSoundEffect.enabled = false;

        
        hideStationObject = GameObject.FindGameObjectWithTag("HideStation");
        monster1Object = GameObject.FindGameObjectWithTag("Monster");
        blindMonsterObject = GameObject.FindGameObjectWithTag("BlindMonster");

      

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("State: " + state.ToString());
        dirX = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentTeleporter != null)
            {
                var positionTele = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
                transform.position = positionTele;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (secondFloorTeleporter != null)
            {
                var positionTele = secondFloorTeleporter.GetComponent<Teleporter>().GetSecondFloorDestination().position;
                transform.position = positionTele;
            }
        }


        if (canHide && Input.GetKeyDown("up"))
        {
            if (!characterIsHiding)
            {
                anim.Play(CHAR_HIDE);
                Physics2D.IgnoreLayerCollision(8, 9, true);
                sprite.sortingOrder = 0;
                characterIsHiding = true;
                state = MovementState.hiding;

                Debug.Log("Character is hiding...");
                hideStationObject.layer = 2;
                

                anim.SetBool("isHiding", characterIsHiding);
                anim.SetInteger("state", (int)state);

            } 
            else
            {
                Physics2D.IgnoreLayerCollision(8, 9, false);
                sprite.sortingOrder = 2;
                characterIsHiding = false;
                state = MovementState.idle;
                anim.SetBool("isHiding", characterIsHiding);
                Debug.Log("Character is not hiding...");
                
            }

            

        } 

        
        if (can_collect && Input.GetKeyDown(KeyCode.E))
        {
            anim.Play(CHAR_COLLECT);
            checkCanCollectedItem();
        }

        if (characterIsHiding)
        {
            anim.Play(CHAR_HIDE);
        } else
        {
            Physics2D.IgnoreLayerCollision(8, 9, false);
        }


        if (stage_finish)
        {
            ShowFinishScene();
        } else
        {
            if (!characterIsHiding)
            {
                MovementController();
              
            }
            checkAnimation2SetSoundEffect();
        }
        


    }

    // --------------- CHECK FUNCTIONS ------------- //

   

    private void checkCanCollectedItem()
    {
        if (can_collect)
        {
            can_collect = false;
            charJustCollectSomething = true;
        }
    }

    private void checkAnimation2SetSoundEffect()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!characterIsDead)
        {
            breathHidingSoundEffect.enabled = true;
        } else { breathHidingSoundEffect.enabled = false; }
       

        if (stateInfo.IsName(CHAR_WALKING))
        { 
            walkingSoundEffect.enabled = true;
        } else 
        {   walkingSoundEffect.enabled = false; }

        if (stateInfo.IsName(CHAR_HIDE) || characterIsHiding || stateInfo.IsName(CHAR_IDLE) || stateInfo.IsName(CHAR_WALKING))
        {

            breathHidingSoundEffect.pitch = 0.5f;
        }
        else
        { breathHidingSoundEffect.pitch = 0.9f; }

        if (stateInfo.IsName(CHAR_RUNNING))
        {
            runningSoundEffect.enabled = true;
          
        }
        else
        { runningSoundEffect.enabled = false;
 
        }


        if (stateInfo.IsName(CHAR_RUNNING))
        {
            jumpSoundEffect.enabled = true;
        }
        else
        { jumpSoundEffect.enabled = false; }


    }




    // ---------------- ON TRIGGER/COLLISION ENTER ----------------- //

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Monster" || other.gameObject.tag == "BlindMonster")
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
        if (other.gameObject.tag == "ItemCollect" && other.gameObject.activeSelf)
        {
            can_collect = true;
            charJustCollectSomething = false;
            Debug.Log("CharacterMovement: Can collect Item");
        }

        if (other.gameObject.tag == "FinishPoint")
        {
            stage_finish = true;
        }

        if (other.gameObject.tag == "Allow2WalkOnObject")
        {
            walkingOnObject = true;
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Allow2WalkOnObject")
        {
            walkingOnObject= true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Allow2WalkOnObject")
        {
            walkingOnObject = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FinishPoint"))
        {
            stage_finish = true;
        }

        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
            secondFloorTeleporter= collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "HideStation")
        {
            canHide = true;
        }

        if (other.gameObject.tag == "ItemCollect" && !charJustCollectSomething && other.gameObject.activeSelf)
        {
            can_collect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "HieStation")
        {
            canHide = false;
        }

        if (other.gameObject.tag == "ItemCollect")
        {
            can_collect = false;
            charJustCollectSomething = false;
        }

        if (other.CompareTag("Teleporter"))
        {
            if (other.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
                
            }

            if (other.gameObject == secondFloorTeleporter)
            {
                secondFloorTeleporter = null;

            }
        }


    }

  



    //---------------------------------------------------
    private void MovementController()
    {
        float normalSpeed;
        if (runningMode) {
            normalSpeed = runningSpeed*1.5f;
            //runningSpeed = 4.5f;
        } else
        {
            normalSpeed = runningSpeed;
        }

        if (!characterIsHiding)
        {
            player.velocity = new Vector2(dirX * normalSpeed * Time.deltaTime, player.velocity.y);

            Vector3 movement = new Vector3(speed.x = dirX, 0, 0);
            movement = movement.normalized * normalSpeed * Time.deltaTime;
            transform.Translate(movement);
        }

       


        if (Input.GetButtonDown("Jump") && state != MovementState.jumping && isGrounded())
        {
            player.velocity = new Vector2(player.velocity.x, gravity);
            state= MovementState.jumping;
            //anim.Play(CHAR_JUMPING);
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            state = MovementState.running;
            runningMode = true;
            anim.SetInteger("state", (int)state);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            state = MovementState.walking;
            runningMode = false;
            anim.SetInteger("state", (int)state);
           
        }


        if (dirX > 0f)
        {
            if (runningMode && isGrounded())
            {
                anim.Play(CHAR_RUNNING);
                state = MovementState.running;
            }
            else
            {
                if (isGrounded())
                {
                 
                    anim.Play(CHAR_WALKING);
                    state = MovementState.walking;
                }
            }

            sprite.flipX = false;
            sprite.flipY = false;
        }
        else
        {
            if (dirX < 0f)
            {


                if (runningMode && isGrounded())
                {
                    if (isGrounded())
                    {
                        anim.Play(CHAR_RUNNING);
                        state = MovementState.running;
                    }
                }
                else
                {
                    if (isGrounded())
                    {
                       
                        anim.Play(CHAR_WALKING);
                        state = MovementState.walking;
                    }
                }

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
            anim.SetInteger("state", (int)state);
        } 
        

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround) || walkingOnObject;
    }

    private void Die()
    {
       
        anim.SetTrigger("death");
        anim.Play(CHAR_DIE);

        Debug.Log("You died!");

        var getCaughtPosition = new Vector2(transform.position.x - 3f, transform.position.y + 3f) ;

        transform.position = Vector2.MoveTowards(transform.position, getCaughtPosition, runningSpeed * Time.deltaTime);

        characterIsDead = true;

        player.bodyType = RigidbodyType2D.Static;
        screamSoundEffect.enabled = true; 
        //monster1Object.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //blindMonsterObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

    }

    private void ShowFinishScene()
    {
        player.bodyType = RigidbodyType2D.Static;

        var finishPosition = new Vector2(transform.position.x + 0.01f, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, finishPosition, runningSpeed * Time.deltaTime);


    }



}
