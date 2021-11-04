using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class houseCatchingFire : MonoBehaviour
{
    public float houseTimer = 10;
    public bool houseIsOnFire = false;
    // public gameObject fire;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Fire")
        {
            houseIsOnFire = true;
            StartCoroutine(houseBurningDown());

        }
        if(other.gameObject.name == "Extinguisher")
        {
            houseIsOnFire = false;
            GameManager.Instance.score += 10;
            StopCoroutine(houseBurningDown());
        }
    }

    IEnumerator houseBurningDown()
    {
        yield return new WaitForSeconds(houseTimer);
        Destroy(gameObject);
    }
}
