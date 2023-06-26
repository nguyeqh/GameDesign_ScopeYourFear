using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class MonsterController : MonoBehaviour
{
    private Rigidbody2D monster;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D monsterVision;
    private PolygonCollider2D monsterBody;
    private GameObject player;

    //private enum MovementState { idle =  0, walking = 1, detecting = 2, chasing = 3, catching = 4 };
    private enum MovementState { idle, walking, detecting, chasing, catching, chaseOutOfSight };
    private MovementState state = MovementState.idle;

    public float speed;
    public bool playerInSight = false;
    public bool facingRight = false;

    public AnimatorStateInfo currentState;

    private bool outOfSight = false;
    private bool characterIsHiding = false;

    //------- Const String ----------
    private const string MONSTER1_IDLE = "monster_blind_idle";
    private const string MONSTER1_CHASE = "monster_blind_chase";
    private const string MONSTER_DETEC = "monster_blind_detec";


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

        characterIsHiding = player.GetComponent<CharacterMovement>().characterIsHiding;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Debug.Log("Can not find player!");
            return;
        }
        characterIsHiding = player.GetComponent<CharacterMovement>().characterIsHiding;
        if (characterIsHiding)
        {
            monsterBody.isTrigger = true;
            outOfSight = true;
            state = MovementState.chaseOutOfSight;
        }

        if (state == MovementState.detecting)
        {
            anim.Play(MONSTER_DETEC);
            StartCoroutine(DelayedChase(anim.GetCurrentAnimatorStateInfo(0).length));
        }

        if (state == MovementState.chasing)
        {
            ChasePlayer();
        }

        if (outOfSight && playerInSight)
        {
            ChaseOutOfSight();
        }

        anim.SetInteger("stateMon", (int)state);
        sprite.flipX = false;

    }

    IEnumerator DelayedChase(float _delayed = 0)
    {
        yield return new WaitForSeconds(_delayed);

        ChasePlayer();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInSight = true;
            state = MovementState.detecting;
            Debug.Log("Player in sight!");

            anim.SetInteger("stateMon", (int)state);
        }
    }

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Player is out of monster's sight");
    //        playerInSight = false;

    //        outOfSight= true;
    //    }
    //}

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
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        Debug.Log("Fliped! with" + facingRight);
    }
}
