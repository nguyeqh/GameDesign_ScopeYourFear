using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private Transform secondFloorDestination;
    [SerializeField] private AudioSource teleSoundEffect;


    public Transform GetDestination() 
    {
        teleSoundEffect.enabled = true;
        teleSoundEffect.Play();
        return destination; 
    }

    public Transform GetSecondFloorDestination() 
    { 
        teleSoundEffect.enabled = true;
        teleSoundEffect.Play();
        return secondFloorDestination; 
    }
}
