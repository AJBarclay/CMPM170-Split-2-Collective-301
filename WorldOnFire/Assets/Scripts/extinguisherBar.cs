using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class extinguisherBar : MonoBehaviour
{
    private Image barImage;
    Fuel fuel;
    private playerNearRefill Instance;
    GameObject[] players;
    private void Awake()
    {
        barImage = transform.Find("fuelBar").GetComponent<Image>();
        fuel = new Fuel();
        barImage.fillAmount = 0;
        players = GameObject.FindGameObjectsWithTag("Refill");

    }
    private void Update()
    {


        checkingRefills(players);
        //Debug.Log(playerIsNearRefill.playerIsNear);
        
        
        
        
    }

    private void checkingRefills(GameObject[] cols)
    {
        foreach (GameObject station in cols)
        {
            playerNearRefill checker = station.GetComponent<playerNearRefill>();
            if (checker.playerIsNear)
            {
                fuel.Update();
                barImage.fillAmount = fuel.GetFuelNormalized();
            }
        }
    }
}

public class Fuel
{
    public const int FUEL_MAX = 100;
    private float fuelAmount;
    public float fuelRegentAmount;

    public Fuel()
    {
        fuelAmount = 0;
        fuelRegentAmount = 30f;
    }

    public void Update()
    {
        fuelAmount += fuelRegentAmount * Time.deltaTime;
    }

    public void usingFuel(float amount)
    {
        if(fuelAmount >= amount)
        {
            fuelAmount -= amount * Time.deltaTime;
        }
    }

    public float GetFuelNormalized()
    {
        return fuelAmount / FUEL_MAX;
    }
}
 
