using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro.EditorUtilities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [Header("Game Plane Settings")]
    [Tooltip("Size of the game plane")]
    public float planeSize;
    [Tooltip("Color of the game plane")]
    public Color planeColor;
    [Tooltip("Distance between the spawnable area and the game plane edge")]
    [Range(0, 100)]
    public int spawnedObjectsOffset;
    
    [Space(5)]
    [Header("Teleport Settings")]
    [Space(25)]
    [Tooltip("Teleport Boundary Prefab")]
    public GameObject teleportBoundary;
    [Tooltip("Teleportation amount calculated from the plane size at awake")]
    public float amountToTeleport;

    [Space(5)]
    [Header("Refill Station Settings")]
    [Space(25)]
    [Tooltip("Refill Station Prefab")]
    public GameObject refillStation;
    [Tooltip("Number of Refill Station in the scene")]
    public int refillstationDensity;
    
    [Space(5)]
    [Header("Foliage Settings")]
    [Space(25)]
    [Tooltip("Foliage Prefabs")]
    public GameObject[] foliages;
    [Space(10)]
    [Tooltip("Number of Foliage in the scene")]
    public int foliageDensity;
    
    [Space(5)]
    [Header("House Settings")]
    [Space(25)]
    [Tooltip("House Prefabs")]
    public GameObject[] houses;
    [Space(10)]
    [Tooltip("Number of Houses in the scene")]
    public int houseDensity;
    [Space(10)]
    [Tooltip("Colors to be used for different house parts")]
    public Color[] houseColors;
    
    
    private GameObject _gamePlane;
    private List<GameObject> _spawnedObjects = new List<GameObject>();
    private float _planeSizeToPositionMod;
    
    

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {
        _planeSizeToPositionMod = ((planeSize*10)/2);
        amountToTeleport = (planeSize * 10) - ((planeSize / 20) * 10);
        
        GenerateLevel();
        GenerateTeleport();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateTeleport()
    {
        for (var x = 0; x < 4; x++)
        {
            GameObject teleportBound;
            if (x == 0)
            { 
                teleportBound =  Instantiate(teleportBoundary, new Vector3(0 - _planeSizeToPositionMod, 0, 0 ),
                    Quaternion.identity);
                teleportBound.GetComponent<Teleportation>().teleportationAxis = Teleportation.ColliderEnum.XAxis;
                teleportBound.GetComponent<BoxCollider>().size = new Vector3(teleportBound.GetComponent<BoxCollider>().size.x, teleportBound.GetComponent<BoxCollider>().size.y, planeSize * 10);

            } else if (x == 1)
            {
                teleportBound = Instantiate(teleportBoundary, new Vector3(0 + _planeSizeToPositionMod, 0, 0 ),
                    Quaternion.identity);
                teleportBound.GetComponent<Teleportation>().teleportationAxis = Teleportation.ColliderEnum.XAxis;
                teleportBound.GetComponent<BoxCollider>().size = new Vector3(teleportBound.GetComponent<BoxCollider>().size.x, teleportBound.GetComponent<BoxCollider>().size.y, planeSize * 10);
            } else if (x == 2)
            {
                teleportBound = Instantiate(teleportBoundary, new Vector3(0, 0, 0 + _planeSizeToPositionMod),
                    Quaternion.identity);
                teleportBound.transform.rotation = Quaternion.Euler(0,90,0);
                teleportBound.GetComponent<Teleportation>().teleportationAxis = Teleportation.ColliderEnum.ZAxis;
                teleportBound.GetComponent<BoxCollider>().size = new Vector3(teleportBound.GetComponent<BoxCollider>().size.x, teleportBound.GetComponent<BoxCollider>().size.y, planeSize * 10);
            } else if (x == 3)
            {
                teleportBound = Instantiate(teleportBoundary, new Vector3(0, 0, 0 - _planeSizeToPositionMod),
                    Quaternion.identity);
                teleportBound.transform.rotation = Quaternion.Euler(0,90,0);
                teleportBound.GetComponent<Teleportation>().teleportationAxis = Teleportation.ColliderEnum.ZAxis;
                teleportBound.GetComponent<BoxCollider>().size = new Vector3(teleportBound.GetComponent<BoxCollider>().size.x, teleportBound.GetComponent<BoxCollider>().size.y, planeSize * 10);
            }
        }
    }

    private void GenerateLevel()
    {
        //Creating the Game Plane according to the color and the size 
        _gamePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        _gamePlane.transform.localScale = new Vector3(planeSize,_gamePlane.transform.localScale.y,planeSize);
        _gamePlane.GetComponent<Renderer>().material.color = planeColor;
        for (var x = 0; x < foliageDensity; x++) 
        {
            PlaceObject(foliages[Random.Range(0,foliages.Length)]);
        }
        for (var x = 0; x < houseDensity; x++) 
        {
            PlaceObject(houses[Random.Range(0,houses.Length)]);
        }
        for (var x = 0; x < refillstationDensity; x++) 
        {
            PlaceObject(refillStation);
        }
    }

    private void PlaceObject(GameObject toBePlaced)
    {
        if (toBePlaced.CompareTag("House"))
        {
            //placing the object
            var placedObject = Instantiate(toBePlaced, new Vector3(Random.Range(0-_planeSizeToPositionMod + spawnedObjectsOffset,0 + _planeSizeToPositionMod - spawnedObjectsOffset), 0, Random.Range(0 - _planeSizeToPositionMod + spawnedObjectsOffset, 0 + _planeSizeToPositionMod - spawnedObjectsOffset)), Quaternion.identity);
            
            //checking if the object is overlapping with any other object
            if (_spawnedObjects.Count > 1)
            {
                for (var x = 0; x < _spawnedObjects.Count; x++)
                {
                    if (placedObject.GetComponent<BoxCollider>().bounds
                        .Intersects(_spawnedObjects[x].GetComponent<BoxCollider>().bounds))
                    {
                        Destroy(placedObject);
                        PlaceObject(toBePlaced);
                        return;
                    }
                }
            }
            //changing
            placedObject.transform.GetChild(2).GetComponent<Renderer>().material.color = houseColors[Random.Range(0, houseColors.Length)];
            
            //Adding to the collection of spawned objects
            _spawnedObjects.Add(placedObject);
            
        } else if(toBePlaced.CompareTag("Refill"))
        {
            var placedObject = Instantiate(toBePlaced, new Vector3(Random.Range(0-_planeSizeToPositionMod + spawnedObjectsOffset,0 + _planeSizeToPositionMod - spawnedObjectsOffset), 2.8f, Random.Range(0 - _planeSizeToPositionMod + spawnedObjectsOffset, 0 + _planeSizeToPositionMod - spawnedObjectsOffset)), Quaternion.identity);
            if (_spawnedObjects.Count > 1)
            {
                for (var x = 0; x < _spawnedObjects.Count; x++)
                {
                    if (placedObject.GetComponent<BoxCollider>().bounds
                        .Intersects(_spawnedObjects[x].GetComponent<BoxCollider>().bounds))
                    {
                        Destroy(placedObject);
                        PlaceObject(toBePlaced);
                    }
                }
            }
            _spawnedObjects.Add(placedObject);
        } else 
        {
            var placedObject = Instantiate(toBePlaced, new Vector3(Random.Range(0-_planeSizeToPositionMod + spawnedObjectsOffset,0 + _planeSizeToPositionMod - spawnedObjectsOffset), 0, Random.Range(0 - _planeSizeToPositionMod + spawnedObjectsOffset, 0 + _planeSizeToPositionMod - spawnedObjectsOffset)), Quaternion.identity);
            if (_spawnedObjects.Count > 1)
            {
                for (var x = 0; x < _spawnedObjects.Count; x++)
                {
                    if (placedObject.GetComponent<BoxCollider>().bounds
                        .Intersects(_spawnedObjects[x].GetComponent<BoxCollider>().bounds))
                    {
                        Destroy(placedObject);
                        PlaceObject(toBePlaced);
                    }
                }
            }
            _spawnedObjects.Add(placedObject);
        }
        
    }
}
