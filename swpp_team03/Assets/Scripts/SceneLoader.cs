using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadThisScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
