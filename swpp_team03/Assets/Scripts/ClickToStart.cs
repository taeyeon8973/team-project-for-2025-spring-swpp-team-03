using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToStart : MonoBehaviour
{
    public string startSceneName = "1.StartScene"; // Replace with your actual start scene name

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = left mouse click (also works for touch)
        {
            SceneManager.LoadScene(startSceneName);
        }
    }
}
