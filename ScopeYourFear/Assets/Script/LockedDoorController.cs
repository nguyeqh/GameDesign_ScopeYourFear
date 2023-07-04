using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    public GameObject player;
    private BoxCollider2D doorCollision;
    private SpriteRenderer spriteRenderer;
    public GameObject Message;
    [SerializeField] private AudioSource opendSoundEffect, lockedDoorSoundEffect;

    private bool playerAtDoor = false;

    public bool locked;
    // Start is called before the first frame update
    void Start()
    {
        doorCollision = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerAtDoor)
        {
            if (Input.GetKeyDown(KeyCode.T) && !locked)
            {
                doorCollision.isTrigger = true;
                opendSoundEffect.enabled = true;

                Color spriteColor = spriteRenderer.color;

                // Update the alpha value to make the sprite invisible (set it to 0)
                spriteColor.a = 0f;

                // Apply the updated color to the sprite
                spriteRenderer.color = spriteColor;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.T) && locked)
                {
                    lockedDoorSoundEffect.enabled = true;
                    lockedDoorSoundEffect.Play();

                    Message.SetActive(true);
                }
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Play at door!");
        if (other.gameObject.tag == "Player")
        {
            playerAtDoor= true;

        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        Debug.Log("Play in door!" + locked);
        if (other.gameObject.tag == "Player")
        {
            playerAtDoor = true;

        }
    }
}
