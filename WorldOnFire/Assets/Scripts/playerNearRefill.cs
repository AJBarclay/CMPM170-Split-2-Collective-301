using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNearRefill : MonoBehaviour
{
    public bool playerIsNear = false;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            
            playerIsNear = true;
            //Debug.Log(playerIsNear);
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsNear = false;
            //Debug.Log(playerIsNear);
        }

    }

    public bool thisBool()
    {
        return playerIsNear;
    }
}
