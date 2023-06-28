using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlaceController : MonoBehaviour
{
    private Rigidbody2D hidingObject;
    private BoxCollider2D hidingZone; 
    private SpriteRenderer sprite;
    private bool canHide = false;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        hidingObject = GetComponent<Rigidbody2D>();
        hidingZone = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = 0;

        player = GameObject.FindGameObjectWithTag("Player");


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canHide= true;

            Debug.Log("HidePlaceController: Player can hide!");
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canHide = false;
            Debug.Log("HidePlaceController: Player can not hide!");
            sprite.sortingOrder = 0;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        bool playerIsHiding = player.GetComponent<CharacterMovement>().characterIsHiding;
        if (canHide && playerIsHiding)
        {
            Debug.Log("HidePlaceController: Hide Station just move up!");
            sprite.sortingOrder = 3;

        } 
        
    }
}
