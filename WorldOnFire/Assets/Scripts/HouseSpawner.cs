using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public int numOfHouses = 10;
    public GameObject houses;
    private int houseCounter = 0;
    private Vector3 randomPosition;
    public Transform plane;

    void Start()
    {
        //Will need to figure out how to get max coordinates later
        //float _maxXPos = plane.transform.position.x + plane.transform.localScale.x / 2;

        while ( houseCounter <= numOfHouses)
        {
            // position vector that randomizes the houses positions with hard coded coordinates
            randomPosition.Set(Random.Range(-24.5f, 19.6f), 1, Random.Range(-19.5f, 24.4f));
            Color color = new Color(r: Random.Range(0F, 1F), g: Random.Range(0F, 1F), b: Random.Range(0F, 1F));
            houses = Instantiate(houses, randomPosition, Quaternion.identity);
            houses.GetComponent<Renderer>().material.color = color;

            houseCounter++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
