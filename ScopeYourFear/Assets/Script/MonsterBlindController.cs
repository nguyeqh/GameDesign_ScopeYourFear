using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class MonsterBlindController : MonoBehaviour
{
    public Rigidbody2D monster;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D monsterVision;
    private PolygonCollider2D monsterBody;
    private GameObject player, monster1Object;

    //private enum MovementState { idle =  0, walking = 1, detecting = 2, chasing = 3, catching = 4 };
    private enum MovementState { idle, walking, detecting, chasing, catching, chaseOutOfSight };
    private MovementState state = MovementState.idle;

    [SerializeField] private AudioSource monsterRoarSound, monsterChaseSound;

    public float speed;
    public bool playerInSight = false;
    public bool facingRight = false;

    public AnimatorStateInfo currentState;

    private bool outOfSight = false;
    private bool characterIsHiding = false;
    private bool characterWasSeen = false;
    private bool characterIsDead = false;

    //------- Const String ----------
    private const string MONSTER1_IDLE = "monster_blind_idle";
    private const string MONSTER1_CHASE = "monster_blind_chase";
    private const string MONSTER_DETEC = "monster_blind_detec";
    private const string MONSTER_CAUGHT = "monster_blind_caught";


    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponent<Rigidbody2D>();
        monsterVision = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        monsterBody = GetComponent<PolygonCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        //currentState = anim.GetCurrentAnimatorStateInfo(0);
        monster1Object = GameObject.FindGameObjectWithTag("Monster");

        characterIsHiding = player.GetComponent<CharacterMovement>().characterIsHiding;
        characterIsDead = player.GetComponent<CharacterMovement>().characterIsDead;
        sprite.sortingOrder = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Debug.Log("Can not find player!");
            return;
        }
        bool monster1InSight = monster1Object.GetComponent<MonsterController>().playerInSight;
       // if (!monster1InSight) {

            characterIsHiding = player.GetComponent<CharacterMovement>().characterIsHiding;
            characterIsDead = player.GetComponent<CharacterMovement>().characterIsDead;
            if (characterIsHiding && characterWasSeen)
            {
                monsterBody.isTrigger = true;
                outOfSight = true;
                state = MovementState.chaseOutOfSight;
            }

            if (state == MovementState.detecting)
            {
                anim.Play(MONSTER_DETEC);
                characterWasSeen = true;
                monsterRoarSound.Play();
                StartCoroutine(DelayedChase(anim.GetCurrentAnimatorStateInfo(0).length));
            }

            if (state == MovementState.chasing && !characterIsDead)
            {
                ChasePlayer();
            }

            
            if (characterIsDead)
            {
                state = MovementState.catching;
                outOfSight = false;
                //anim.Play(MONSTER_CAUGHT);

            }
            else
            {
                if (outOfSight)
                {
                    ChaseOutOfSight();
                }
            }

            anim.SetInteger("stateMon", (int)state);
            sprite.flipX = false;
         

       // }

        checkAnimation2SetSoundEffect();


    }

    IEnumerator DelayedChase(float _delayed = 0)
    {
        yield return new WaitForSeconds(_delayed);

        ChasePlayer();
    }

    private void checkAnimation2SetSoundEffect()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(MONSTER_DETEC))
        {
            monsterRoarSound.enabled = true;
        }
        else
        {
            monsterRoarSound.enabled = false;
        }


        if (stateInfo.IsName(MONSTER1_CHASE) && !outOfSight)
        {
            monsterChaseSound.enabled = true;
        }
        else
        {
            monsterChaseSound.enabled = false;
        }
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInSight = true;
            state = MovementState.detecting;
            Debug.Log("Player in sight!");
            outOfSight = false;
            monsterBody.isTrigger = false;

            anim.SetInteger("stateMon", (int)state);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInSight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player is out of monster's sight");
            playerInSight = false;

            //outOfSight = true;
        }
    }

    private void ChasePlayer()
    {
        Debug.Log("The monster is chasing the player!");
        state = MovementState.chasing;
        var playerPosition = player.transform.position;
        anim.SetInteger("stateMon", (int)state);
        anim.Play(MONSTER1_CHASE);


        if (playerPosition.x < transform.position.x)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            //sprite.flipX = true;
            if (facingRight) Flip();

        }


        if (playerPosition.x > transform.position.x)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            //sprite.flipX = true;
            if (!facingRight) Flip();

        }

        //if (playerPosition.x > transform.position.x)
        //{
        //if (!facingRight) Flip();
        //sprite.flipX = false;
        //    transform.position += Vector3.right * speed * Time.deltaTime;
        //}
        //transform.position = Vector2.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        //transform.velocity = new Vector2(transform.dirX * speed, transform.velocity.y);
    }

    private void ChaseOutOfSight()
    {
        if (!characterIsDead)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            //transform.position.y = player.GetComponent<Rigidbody2D>().position.y;
            playerInSight = false;
            state= MovementState.chaseOutOfSight;
            if (facingRight) Flip();
        }

    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        Debug.Log("Fliped! with" + facingRight);
    }
}
