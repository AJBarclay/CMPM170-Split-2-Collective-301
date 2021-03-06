using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [Header("Game Plane Settings")]
    [Tooltip("Game Plane to spawn")]
    public GameObject gamePlaneToSpawn;
    [Tooltip("Size of the game plane")]
    public float planeSize;
    [Tooltip("Color of the game plane")]
    public Color planeColor;
    [Tooltip("Distance between the spawnable area and the game plane edge")]
    [Range(0, 100)]
    public int spawnedObjectsOffset;
    
    [Header("Smoke Settings")]
    [Tooltip("Smoke effect on the edges of the game plane")]
    public GameObject smokeEffect;
    
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
	[Space(5)]
	[Header("Fire Settings")]
	[Space(25)]
	[Tooltip("Fire Prefab")]
	public GameObject fire;
	[Space(10)]
	[Tooltip("Number of Fires in the scene at start")]
	public int startFireDensity;
	[Space(10)]
	[Tooltip("Number of Fires in the scene")]
	public int fireDensity;
	[Space(10)]
	[Tooltip("Number of Fires active")]
	public int fireCount = 0;
	[Space(10)]
	[Tooltip("Grid Divisions (per side)")]
	public int gridDivisions;
	public int[,] stateGrid;
	public GameObject[,] fireGrid;
    [Header("Score System")]
    [Tooltip("Holds the score of saving houses")]
    public float score = 10f;
    [Tooltip("Holds the text object")]
    public GameObject scoreText;

    private GameObject _gamePlane;
    public List<GameObject> _spawnedObjects = new List<GameObject>();
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
		GenerateGrid();
        GenerateLevel();
        GenerateTeleport();
        GeneratePlane();
        //fireCount = fireDensity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ScoreSystem();
    }

    private void GenerateTeleport()
    {
        for (var x = 0; x < 4; x++)
        {
            GameObject teleportBound;
            GameObject smokeEdge;
            if (x == 0)
            { 
                teleportBound =  Instantiate(teleportBoundary, new Vector3(0 - _planeSizeToPositionMod, 0, 0 ),
                    Quaternion.identity);
                smokeEdge = Instantiate(smokeEffect, new Vector3(0 - _planeSizeToPositionMod - 10, 0, 0 ),
                    Quaternion.identity);
                ParticleSystem.ShapeModule module = smokeEdge.GetComponent<ParticleSystem>().shape;
                module.scale = new Vector3(28.7f, planeSize * 10, 1);
                teleportBound.GetComponent<Teleportation>().teleportationAxis = Teleportation.ColliderEnum.XAxis;
                teleportBound.GetComponent<BoxCollider>().size = new Vector3(teleportBound.GetComponent<BoxCollider>().size.x, teleportBound.GetComponent<BoxCollider>().size.y, planeSize * 10);

            } else if (x == 1)
            {
                teleportBound = Instantiate(teleportBoundary, new Vector3(0 + _planeSizeToPositionMod, 0, 0 ),
                    Quaternion.identity);
                smokeEdge = Instantiate(smokeEffect, new Vector3(0 + _planeSizeToPositionMod + 10 , 0, 0 ),
                    Quaternion.identity);
                ParticleSystem.ShapeModule module = smokeEdge.GetComponent<ParticleSystem>().shape;
                module.scale = new Vector3(28.7f, planeSize * 10, 1);
                teleportBound.GetComponent<Teleportation>().teleportationAxis = Teleportation.ColliderEnum.XAxis;
                teleportBound.GetComponent<BoxCollider>().size = new Vector3(teleportBound.GetComponent<BoxCollider>().size.x, teleportBound.GetComponent<BoxCollider>().size.y, planeSize * 10);
            } else if (x == 2)
            {
                teleportBound = Instantiate(teleportBoundary, new Vector3(0, 0, 0 + _planeSizeToPositionMod),
                    Quaternion.identity);
                smokeEdge = Instantiate(smokeEffect, new Vector3(0, 0, 0 + _planeSizeToPositionMod + 10 ),
                    Quaternion.Euler(0,90.0f,0));
                ParticleSystem.ShapeModule module = smokeEdge.GetComponent<ParticleSystem>().shape;
                module.scale = new Vector3(28.7f, planeSize * 10, 1);
                teleportBound.transform.rotation = Quaternion.Euler(0,90,0);
                teleportBound.GetComponent<Teleportation>().teleportationAxis = Teleportation.ColliderEnum.ZAxis;
                teleportBound.GetComponent<BoxCollider>().size = new Vector3(teleportBound.GetComponent<BoxCollider>().size.x, teleportBound.GetComponent<BoxCollider>().size.y, planeSize * 10);
            } else if (x == 3)
            {
                teleportBound = Instantiate(teleportBoundary, new Vector3(0, 0, 0 - _planeSizeToPositionMod),
                    Quaternion.identity);
                smokeEdge = Instantiate(smokeEffect, new Vector3(0, 0, 0 - _planeSizeToPositionMod - 10 ),
                    Quaternion.Euler(0,90.0f,0));
                ParticleSystem.ShapeModule module = smokeEdge.GetComponent<ParticleSystem>().shape;
                module.scale = new Vector3(28.7f, planeSize * 10, 1);
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
		for (var x = 0; x < startFireDensity; x++)
		{
			fireCount++;
			PlaceObject(fire);
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
            //changing the color of the wall color material of house meshes
            for(var x = 0; x < placedObject.transform.GetChild(1).GetComponent<Renderer>().materials.Length; x++) {
                if (placedObject.transform.GetChild(1).GetComponent<Renderer>().materials[x].name == "WallColor (Instance)")
                {
                    placedObject.transform.GetChild(1).GetComponent<Renderer>().materials[x].color = houseColors[Random.Range(0, houseColors.Length)];
                }
            }
            
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
        } else if(toBePlaced.CompareTag("Fire"))
		{
			var xpos = Random.Range(0,gridDivisions - 1);
			var ypos = Random.Range(0,gridDivisions - 1);
			while(stateGrid[xpos,ypos] != 0)
			{
				xpos = Random.Range(0,gridDivisions - 1);
				ypos = Random.Range(0,gridDivisions - 1);
			}
			Debug.Log(xpos);
			Debug.Log(ypos);
			
			var currentFire = fireGrid[xpos,ypos];
			FireSpreading fireScript = currentFire.GetComponent<FireSpreading>();
			fireScript.state = 1;
			currentFire.SetActive(true);
			stateGrid[xpos,ypos] = 1;
		}else 
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


	private void GenerateGrid()
	{
		stateGrid = new int[gridDivisions,gridDivisions];
		fireGrid = new GameObject[gridDivisions,gridDivisions];
		for (var x = 0; x < gridDivisions; x++)
		{
			for (var y = 0; y < gridDivisions; y++)
			{
				stateGrid[x,y] = 0;
				var newFire = Instantiate(fire,
				new Vector3(((planeSize*10)/gridDivisions)*(x - (gridDivisions/2)), 0, ((planeSize*10)/gridDivisions)*(y - (gridDivisions/2)))
				,Quaternion.identity);
				FireSpreading newFireScript = newFire.GetComponent<FireSpreading>();
				newFireScript.state = 0;
				newFireScript.x = x;
				newFireScript.y = y;	
				fireGrid[x,y] = newFire;
			}
		}
	}

    //changes the plane that was used to create the level with to low poly production plane prefab 
    private void GeneratePlane()
    {
        var _spawnedLowPolyPlane = Instantiate(gamePlaneToSpawn, Vector3.zero, Quaternion.identity);
        _spawnedLowPolyPlane.transform.localScale = new Vector3(planeSize * 5, 50, planeSize * 5);
        _gamePlane.SetActive(false);
        
    }


    private void ScoreSystem()
    {
        
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    public void QuitGame1()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ReturnMain ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
}

	