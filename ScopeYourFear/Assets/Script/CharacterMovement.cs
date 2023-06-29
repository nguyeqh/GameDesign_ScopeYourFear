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

    [SerializeField] private LayerMask jumpableGround;
    public GameOverController gameOverController; //It's a screen only
    public StageClearScene stageClearScene;

    public float speed;
    private float gravity = 4f;
    private bool canHide = false;
    private float dirX;
    private GameObject hideStationObject, teleporter, teleporterZone2;
    private GameObject blindMonsterObject, monster1Object;
    private bool monsterInSight, monsterBlindInSight, able2Teleport;
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

        able2Teleport = false;
     

        hideStationObject = GameObject.FindGameObjectWithTag("HideStation");
        monster1Object = GameObject.FindGameObjectWithTag("Monster");
        blindMonsterObject = GameObject.FindGameObjectWithTag("BlindMonster");
        teleporter = GameObject.FindGameObjectWithTag("Teleporter");
        teleporterZone2 = GameObject.FindGameObjectWithTag("TeleporterZone2");

    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");


        if (canHide && Input.GetKeyDown("up"))
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

        } else
        {
            Physics2D.IgnoreLayerCollision(8, 9, false);
            sprite.sortingOrder = 2;
            characterIsHiding = false;
            state = MovementState.idle;
            anim.SetBool("isHiding", characterIsHiding);
            Debug.Log("Character is not hiding...");
        }

        if (can_collect && Input.GetKeyDown(KeyCode.E))
        {
            anim.Play(CHAR_COLLECT);
        }


        if (stage_finish)
        {
            ShowFinishScene();
        } else
        {
            checkRun4YourLife();
            MovementController();
        }


    }

    private void checkRun4YourLife()
    {
        monsterInSight = monster1Object.GetComponent<MonsterController>().playerInSight;
        monsterBlindInSight = blindMonsterObject.GetComponent<MonsterBlindController>().playerInSight;

        if (monsterInSight)
        {
            Debug.Log("CharacterController: Monster in sight!");
        }

        if (monsterBlindInSight)
        {
            Debug.Log("CharacterController: Blind monster in sight!");
        }

        
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
        if (other.gameObject.tag == "ItemCollect")
        {
            can_collect = true;
        }

        if (other.gameObject.tag == "FinishPoint")
        {
            stage_finish = true;
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "HideStation")
        {
            canHide = true;
        }
        if (other.gameObject.tag == "ItemCollect")
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
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FinishPoint"))
        {
            stage_finish = true;
        }
    }



    //---------------------------------------------------
    private void MovementController()
    {
        
        if (state == MovementState.running) {
            speed= 30f;
        } else
        {
            speed= 20f;
        }
        
        player.velocity = new Vector2(dirX * speed , player.velocity.y);

        if (Input.GetButtonDown("Jump") && state != MovementState.jumping && isGrounded())
        {
            player.velocity = new Vector2(player.velocity.x, gravity);
            //anim.Play(CHAR_JUMPING);
            
        }

        
        if (dirX > 0f)
        {
            if (monsterInSight || monsterBlindInSight)
            {
                state = MovementState.running;
                if (isGrounded())
                {
                    anim.Play(CHAR_RUNNING);
                }
            }
            else
            {
                state = MovementState.walking;
                if (isGrounded())
                {
                    anim.Play(CHAR_WALKING);
                }
            }
            sprite.flipX = false;
            sprite.flipY = false;
        }
        else
        {
            if (dirX < 0f)
            {
                if (monsterInSight || monsterBlindInSight)
                {
                    state = MovementState.running;
                    if (isGrounded())
                    {
                        anim.Play(CHAR_RUNNING);
                    }

                }
                else
                {
                    state = MovementState.walking;
                    if (isGrounded())
                    {
                        anim.Play(CHAR_WALKING);
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

        var getCaughtPosition = new Vector2(transform.position.x - 3f, transform.position.y + 3f) ;

        transform.position = Vector2.MoveTowards(transform.position, getCaughtPosition, speed * Time.deltaTime);

        characterIsDead = true;
        gameOverController.Setup(0);

        player.bodyType = RigidbodyType2D.Static;
        //monster1Object.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //blindMonsterObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

    }

    private void ShowFinishScene()
    {
        stageClearScene.Setup(2);
        player.bodyType = RigidbodyType2D.Static;

        var finishPosition = new Vector2(transform.position.x + 0.01f, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, finishPosition, speed * Time.deltaTime);


    }



}
