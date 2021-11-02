using UnityEngine;
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
            Teleport(other.transform);
        }
    }

    private void Teleport(Transform toBeTeleported)
    {
        CharacterController cc = toBeTeleported.GetComponent<CharacterController>();
        cc.enabled = false;
        Vector3 posToTeleport = toBeTeleported.position;
        if (teleportationAxis == ColliderEnum.XAxis)
        {
            if (toBeTeleported.position.x <= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x + GameManager.Instance.amountToTeleport, posToTeleport.y,posToTeleport.z), Quaternion.identity);
                Debug.Log("player teleported");
            } 
            else if (toBeTeleported.position.x >= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x - GameManager.Instance.amountToTeleport, posToTeleport.y,posToTeleport.z), Quaternion.identity);
                Debug.Log("player teleported");
            }
        }
        else if (teleportationAxis == ColliderEnum.ZAxis)
        {
            if (toBeTeleported.position.z <= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x, posToTeleport.y,posToTeleport.z + GameManager.Instance.amountToTeleport), Quaternion.identity);
                Debug.Log("player teleported");
            } 
            else if (toBeTeleported.position.z >= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x, posToTeleport.y,posToTeleport.z - GameManager.Instance.amountToTeleport), Quaternion.identity);
                Debug.Log("player teleported");
            }
        }
        cc.enabled = true;
    }
}
