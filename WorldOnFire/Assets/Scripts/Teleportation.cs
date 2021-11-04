using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Teleportation : MonoBehaviour
{
   
    [Space]
    [Header("needs to get amountoteleport from game manager")]
    [Header("and box colliders should be resized accordingly")]
    [Header("Game Plane must be centered to origin")]
    [Header(" with detectcollisions enabled to work ")]
    [Header("Player requires a Player tag and a character controller")]
    [Tooltip("Change in the position of the current axis")]
    public float amountToTeleport;
    public enum ColliderEnum{XAxis, ZAxis}; 
    public ColliderEnum teleportationAxis;
    
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {

            StartCoroutine(Teleport(other.transform));
        }
    }

    IEnumerator Teleport(Transform toBeTeleported)
    {
        //Debug.Log("active");
        GameObject ui = GameObject.Find("UI Camera");
        var eff = ui.GetComponent<Transform>().GetChild(0);
        eff.gameObject.SetActive(!eff.gameObject.activeSelf);

        yield return new WaitForSeconds(1);


        CharacterController cc = toBeTeleported.GetComponent<CharacterController>();
        cc.enabled = false;
        Vector3 posToTeleport = toBeTeleported.position;
        if (teleportationAxis == ColliderEnum.XAxis)
        {
            if (toBeTeleported.position.x <= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x + GameManager.Instance.amountToTeleport, 0,posToTeleport.z), cc.transform.localRotation);
                Debug.Log("player teleported");
            } 
            else if (toBeTeleported.position.x >= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x - GameManager.Instance.amountToTeleport, 0,posToTeleport.z), cc.transform.localRotation);
                Debug.Log("player teleported");
            }
        }
        else if (teleportationAxis == ColliderEnum.ZAxis)
        {
            if (toBeTeleported.position.z <= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x, 0,posToTeleport.z + GameManager.Instance.amountToTeleport), cc.transform.localRotation);
                Debug.Log("player teleported");
            } 
            else if (toBeTeleported.position.z >= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x, 0,posToTeleport.z - GameManager.Instance.amountToTeleport), cc.transform.localRotation);
                Debug.Log("player teleported");
            }
        }
        cc.enabled = true;
        yield return new WaitForSeconds(1);
        eff.gameObject.SetActive(!eff.gameObject.activeSelf);
    }
}
