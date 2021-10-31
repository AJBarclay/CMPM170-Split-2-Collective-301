using UnityEngine;
public class Teleportation : MonoBehaviour
{
   
    [Space]
    [Header("and box colliders should be resized accordingly")]
    [Header("Game Plane must be centered to origin")]
    [Header("with isKinematic turned off for the triggers to work")]
    [Header("Player requires a Player tag and a rigidbody ")]
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
        Vector3 posToTeleport = toBeTeleported.position;
        if (teleportationAxis == ColliderEnum.XAxis)
        {
            if (toBeTeleported.position.x <= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x + amountToTeleport, posToTeleport.y,posToTeleport.z), Quaternion.identity);
            } 
            else if (toBeTeleported.position.x >= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x - amountToTeleport, posToTeleport.y,posToTeleport.z), Quaternion.identity);
            }
        }
        else if (teleportationAxis == ColliderEnum.ZAxis)
        {
            if (toBeTeleported.position.z <= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x, posToTeleport.y,posToTeleport.z + amountToTeleport), Quaternion.identity);
            } 
            else if (toBeTeleported.position.z >= 0)
            {
                toBeTeleported.SetPositionAndRotation(new Vector3(posToTeleport.x, posToTeleport.y,posToTeleport.z - amountToTeleport), Quaternion.identity);
            }
        }
    }
}
