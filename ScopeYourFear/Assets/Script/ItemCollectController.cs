using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectController : MonoBehaviour
{
    private GameObject player;

    private bool playerWouldCollect = false;
    private bool itemCollected;

    private SpriteRenderer sprite;
    


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.SetActive(true);
        itemCollected = false;

        sprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        itemCollected = player.GetComponent<CharacterMovement>().charJustCollectSomething;
        if (itemCollected)
        {
            sprite.sortingOrder = 0;
            gameObject.SetActive(false);
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" )
        {
            playerWouldCollect = true;

            
        }
    }


}
