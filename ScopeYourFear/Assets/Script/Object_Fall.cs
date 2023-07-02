using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Fall : MonoBehaviour
{
    [SerializeField] private AudioSource fallingSoundEffect;


    // Update is called once per frame
    void Start()
    {
        fallingSoundEffect.enabled = false;
    }

    void Update()
    { 
    
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            fallingSoundEffect.enabled = true;
        }
    }
}
