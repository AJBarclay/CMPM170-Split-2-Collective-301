using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class extinguisherBar : MonoBehaviour
{
 
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
 
