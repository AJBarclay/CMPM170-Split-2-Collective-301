using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        var player = collider.GetComponent<Player>();

        if (player != null)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
