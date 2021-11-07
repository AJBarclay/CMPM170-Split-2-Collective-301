using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class extinguisherBar : MonoBehaviour
{
 
}

public class Fuel
{
    public const int FUEL_MAX = 1;
    public float fuelRegentAmount;
    private Image barImage;

    public Fuel()
    {
        barImage = GameObject.Find("fuelBar").GetComponent<Image>();
        //fuelAmount = barImage.fillAmount;
        barImage.fillAmount = 1f;
        fuelRegentAmount = 0.5f;
    }

    public void Update()
    {
        if (barImage.fillAmount <= 1)
        {
            barImage.fillAmount += fuelRegentAmount * Time.deltaTime;
        }
    }


    public void usingFuel(float amount)
    {
        if(barImage.fillAmount > 0)
        {
            barImage.fillAmount -= amount * Time.deltaTime;
        }
    }

    public float GetFuelNormalized()
    {
        return barImage.fillAmount / FUEL_MAX;
    }

}
 
