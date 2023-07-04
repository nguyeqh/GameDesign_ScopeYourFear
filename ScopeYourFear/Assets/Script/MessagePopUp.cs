using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePopUp : MonoBehaviour
{
    public GameObject Message;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Message.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Message.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
}
