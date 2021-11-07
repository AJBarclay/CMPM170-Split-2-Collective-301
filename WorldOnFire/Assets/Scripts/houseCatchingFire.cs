using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class houseCatchingFire : MonoBehaviour
{
    public float houseTimer = 10;
    public bool houseIsOnFire = false;
	public List<GameObject> fires;
	public int fireCount = 0;
    // public gameObject fire;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fires.Count > 0)
		{
			foreach (var fire in fires)
			{
				var fireScript = fire.GetComponent<FireSpreading>();
				fireCount += fireScript.state;
			}
		}
		if (fireCount <= 0)
		{
			houseIsOnFire = false;
			StopCoroutine(houseBurningDown());
		}
		else
		{
			houseIsOnFire = true;
			StartCoroutine(houseBurningDown());
		}
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Fire"))
        {
            houseIsOnFire = true;
            StartCoroutine(houseBurningDown());
			if(!fires.Contains(other.gameObject))
			{
				fires.Add(other.gameObject);
			}

        }
        if(other.gameObject.name == "Extinguisher" && houseIsOnFire == true)
        {
            
            GameManager.Instance.score += 10;
            StopCoroutine(houseBurningDown());
			houseIsOnFire = false;
		}
    }

    IEnumerator houseBurningDown()
    {
        yield return new WaitForSeconds(houseTimer);
		if (houseIsOnFire == true)
		{
			Destroy(gameObject);			
		}
    }
	
	public void houseExtinguish()
	{
		if (houseIsOnFire == false) {return;}
		houseIsOnFire = false;
        GameManager.Instance.score += 10;
        StopCoroutine(houseBurningDown());
		if(fires.Count > 0)
		{
			foreach(var fire in fires)
			{
				if(!fire.CompareTag("Fire")){continue;}
				var fireScript = fire.GetComponent<FireSpreading>();
				fireScript.Extinguish();
			}
		}
		
	}
}
