using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RouteSelectManager : MonoBehaviour
{
    public GameObject[] maps;                  // 6 Map GameObjects
    public string[] routeDescriptions;         // 6 route descriptions
    public TextMeshProUGUI routeDescription;   // Description UI Text
    public Button startButton;                 // Shared Start button

    private int selectedRouteIndex = -1;       // Keeps track of selected route

    public void OnHoverRoute(int index)
    {
        // Show correct map
        for (int i = 0; i < maps.Length; i++)
            maps[i].SetActive(i == index);

        // Show corresponding route description
        routeDescription.text = routeDescriptions[index];

        // Save selected route index
        selectedRouteIndex = index;

        // Enable the start button
        startButton.gameObject.SetActive(true);

        // Remove previous listeners and add a new one
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => StartRoute(index));
    }

    void StartRoute(int index)
    {
        // Construct the scene name, like "Track1Scene", "Track2Scene"
        string sceneName = $"Track{index + 1}Scene";
        Debug.Log("Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
