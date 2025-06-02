using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject playerPrefab;

    void Start()
    {
        StartCoroutine(SpawnPlayer());
    }

    IEnumerator SpawnPlayer()
    {
        yield return null; // Wait for RouteManager

        int routeIndex = RouteManager.Instance != null ? RouteManager.Instance.selectedRoute : 0;

        if (routeIndex < 0 || routeIndex >= spawnPoints.Length)
        {
            Debug.LogError("Invalid route index: " + routeIndex);
            yield break;
        }

        Transform spawnPoint = spawnPoints[routeIndex];
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        // 🔥 Tell the camera to follow the new player
        FollowCamera cameraScript = Camera.main.GetComponent<FollowCamera>();
        if (cameraScript != null)
        {
            cameraScript.player = player.transform;
        }

        Debug.Log("Spawning player at route index: " + routeIndex);
    }
}
