using UnityEngine;

public class PlayerPositionSetter : MonoBehaviour
{
    public Transform player;
    public Vector3[] routeStartPositions; // Assign 6 different Vector3 start positions in the inspector

    void Start()
    {
        if (RouteManager.Instance != null && player != null)
        {
            int index = RouteManager.Instance.selectedRoute;

            if (index >= 0 && index < routeStartPositions.Length)
            {
                player.gameObject.SetActive(true); // Activate player
                player.position = routeStartPositions[index]; // Move to correct start pos
            }
        }
    }
}
