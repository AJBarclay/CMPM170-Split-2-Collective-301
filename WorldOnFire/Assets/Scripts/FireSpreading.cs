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
	public int x;
	public int y;
	public bool putout = true;
	
	
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
        
		if(timer > 0 && state > 0)
		{
			timer -= Time.deltaTime;
		} 
		else if (timer <= 0 && state > 0)
		{
			state++;
			gameManagerScript.stateGrid[x,y]++;
			if (state > 4) { state = 4;}
			if(gameManagerScript.stateGrid[x,y] > 4) { gameManagerScript.stateGrid[x,y] = 4;}
			timer = 5.0f;
		}
		state = gameManagerScript.stateGrid[x,y];
		switch(state)
		{
			case(4):
			gameObject.SetActive(true);
			if(gameManagerScript.fireCount < gameManagerScript.fireDensity)
			{
				SpreadFire();
			}	
			state -= 2;
			break;
			case(3):
			gameObject.SetActive(true);
			transform.localScale = new Vector3(1.5f,1.5f,1.5f);
			break;
			case(2):
			gameObject.SetActive(true);
			transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			break;
			case(1):
			gameObject.SetActive(true);
			transform.localScale = new Vector3(0.5f,0.5f,0.5f);
			if(putout == true)
			{
				gameManagerScript.fireCount++;
				putout = false;
			}
			break;
			case(0):
			transform.localScale = new Vector3(0.0f,0.0f,0.0f);
			if(	putout == false)
			{
				gameManagerScript.fireCount--;
				putout = true;
			}
			//gameObject.SetActive(false);
			break;
			default:
			state = 0;
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
		Debug.Log(direction);
		Vector2 nextPos = new Vector2(x,y);
		switch (direction)
		{
			case(4):
			if(nextPos.y + 1 < gameManagerScript.gridDivisions)
			{
				nextPos.y++;
			}else
			{
				nextPos.y = 0;
			}
			break;
			case(3):
			if(nextPos.x + 1 < gameManagerScript.gridDivisions)
			{
				nextPos.x++;
			}else
			{
				nextPos.x = 0;
			}
			break;
			case(2):
			if(nextPos.y - 1 >= 0)
			{
				nextPos.y--;
			}else
			{
				nextPos.y = gameManagerScript.gridDivisions - 1;
			}
			break;
			case(1):
			if(nextPos.x - 1 >= 0)
			{
				nextPos.x--;
			}else
			{
				nextPos.x = gameManagerScript.gridDivisions - 1;
			}
			break;
			default:
			state++;
			break;
		}
		
		var newFire = gameManagerScript.fireGrid[(int)nextPos.x,(int)nextPos.y];
		newFire.SetActive(true);
		FireSpreading newFireScript = newFire.GetComponent<FireSpreading>();
		newFireScript.state += 1;
		gameManagerScript.stateGrid[(int)nextPos.x,(int)nextPos.y]++;
	}
}