using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectController : MonoBehaviour
{
    public GameObject player;

    private bool playerWouldCollect = false;
    private bool itemCollected = false;

    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;
    


    // Start is called before the first frame update
    void Start()
    {
        
        gameObject.SetActive(true);
        itemCollected = false;

        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerWouldCollect && Input.GetKeyDown(KeyCode.E))
        {
            itemCollected= true;
        }
        
        if (itemCollected)
        {
            sprite.sortingOrder = 1;
            boxCollider.isTrigger = true;
           
            transform.position = player.transform.position;
        }
       
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" )
        {
            playerWouldCollect = true;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerWouldCollect = true;

        }
    }


}
