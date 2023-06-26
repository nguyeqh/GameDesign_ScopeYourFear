using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlaceController : MonoBehaviour
{
    private Rigidbody2D hidingObject;
    private BoxCollider2D hidingZone;
    private bool canHide = false;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        hidingObject = GetComponent<Rigidbody2D>();
        hidingZone = GetComponent<BoxCollider2D>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canHide= true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canHide = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canHide && Input.GetKey("e"))
        {
            Debug.Log("Player is hiding");
        }
        
    }
}
