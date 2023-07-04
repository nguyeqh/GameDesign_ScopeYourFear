using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class GrabObjects : MonoBehaviour
{

    [SerializeField] private Transform grabPoint, rayPoint;
    [SerializeField] private float rayDistance;

    private GameObject grabedObject; 
    private int layerIndex;
    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Objects");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance);
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex) 
        {
            //grab object
            if (Input.GetKeyDown(KeyCode.E) && grabedObject == null)
            {
                grabedObject= hitInfo.collider.gameObject; 
                grabedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                grabedObject.transform.position = grabPoint.position;
                grabedObject.transform.SetParent(transform);
            }
            //release object 
            else if (Input.GetKeyDown(KeyCode.E))
            {
                grabedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                grabedObject.transform.SetParent(null);
                grabedObject = null;
            }
        }
    }
}
