using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpreading : MonoBehaviour
{
	public GameObject GameManager;
	public GameObject fire;
	public GameManager gameManagerScript;
	public float timer = 5.0f;
	public int state = 0;
	
	
    // Start is called before the first frame update
    void Start()
    {
		GameManager  = GameObject.Find("GameManager");
		fire = GameObject.Find("Fire");
		//fire = gameObject;
		gameManagerScript = GameManager.GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
		if(timer > 0)
		{
			timer -= Time.deltaTime;
		} 
		else if (timer <= 0)
		{
			state++;
			if (state > 4) { state = 4;}
			timer = 5.0f;
		}
		
		switch(state)
		{
			case(4):
			if(gameManagerScript.fireCount < gameManagerScript.fireDensity)
			{
				SpreadFire();
				gameManagerScript.fireCount += 1;
			}	
			state -= 2;
			break;
			case(3):
			transform.localScale = new Vector3(1.5f,1.5f,1.5f);
			break;
			case(2):
			transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			break;
			case(1):
			gameObject.SetActive(true);
			transform.localScale = new Vector3(0.5f,0.5f,0.5f);
			break;
			case(0):
			gameObject.SetActive(false);
			break;
			default:
			gameObject.SetActive(false);
			break;
		}
	
	}

	void SpreadFire()
	{	
		//fire = GameObject.Find("Fire");
		fire = gameObject;
		GameManager  = GameObject.Find("GameManager");
		gameManagerScript = GameManager.GetComponent<GameManager>();
        
		int direction = Random.Range(1,4);
		Vector2 nextPos = new Vector2(transform.position.x,transform.position.y);
		switch (direction)
		{
			case(4):
			if(nextPos.y + (gameManagerScript.planeSize/gameManagerScript.gridDivisions) <= (gameManagerScript.planeSize/2))
			{
				nextPos.y += (gameManagerScript.planeSize/gameManagerScript.gridDivisions);
			}
			break;
			case(3):
			if(nextPos.x + (gameManagerScript.planeSize/gameManagerScript.gridDivisions) <= (gameManagerScript.planeSize/2))
			{
				nextPos.x += (gameManagerScript.planeSize/gameManagerScript.gridDivisions);
			}
			break;
			case(2):
			if(nextPos.y - (gameManagerScript.planeSize/gameManagerScript.gridDivisions) >= -(gameManagerScript.planeSize/2))
			{
				nextPos.y -= (gameManagerScript.planeSize/gameManagerScript.gridDivisions);
			}
			break;
			case(1):
			if(nextPos.x - (gameManagerScript.planeSize/gameManagerScript.gridDivisions) >= -(gameManagerScript.planeSize/2))
			{
				nextPos.x -= (gameManagerScript.planeSize/gameManagerScript.gridDivisions);
			}
			break;
			default:
			state++;
			break;
		}
		
		var newFire = Instantiate(fire, new Vector3(nextPos.x, 0, nextPos.y), Quaternion.identity);
		for (var x = 0; x < gameManagerScript._spawnedObjects.Count; x++)
                {
                    if (newFire.GetComponent<BoxCollider>().bounds
                        .Intersects(gameManagerScript._spawnedObjects[x].GetComponent<BoxCollider>().bounds))
                    {
                        Destroy(newFire);
                        SpreadFire();
                    }
                }
	}
}