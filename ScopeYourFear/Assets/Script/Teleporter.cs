using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private Transform secondFloorDestination;

    public Transform GetDestination() { return destination; }

    public Transform GetSecondFloorDestination() { return secondFloorDestination; }
}
