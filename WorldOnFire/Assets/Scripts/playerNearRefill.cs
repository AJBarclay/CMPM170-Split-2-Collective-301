using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerNearRefill : MonoBehaviour
{
    public bool playerIsNear = false;

    private Image barImage;
    Fuel fuel;
    private void Awake()
    {
        fuel = new Fuel();

    }
    private void Update()
    {


        if (playerIsNear && Input.GetKey(KeyCode.R))
        {
            fuel.Update();
        }
        //Debug.Log(playerIsNearRefill.playerIsNear);




    }


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
}
