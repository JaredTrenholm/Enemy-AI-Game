using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{

    private bool canPickUp = false;
    public bool pickedUp = false;
    private Rigidbody rb;

    public Text PickupMessage;

    private Transform playerCam = null;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();   
    }

    private void Update()
    {
        if((canPickUp == true)&&(pickedUp == false))
        {
            PickupMessage.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.gameObject.transform.parent = playerCam;
                pickedUp = true;
                rb.useGravity = false;
                PickupMessage.enabled = false;
            }
        } else if( pickedUp == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.gameObject.transform.SetParent(null);
                pickedUp = false;
                rb.useGravity = true;
            }
        }
        else
        {
            if (PickupMessage.enabled == true)
            {
                PickupMessage.enabled = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canPickUp = true;
            playerCam = other.gameObject.transform.GetChild(0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (pickedUp != true)
        {
            if (other.tag == "Player")
            {
                canPickUp = false;
            }
        }
    }
}
