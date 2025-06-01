using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public static RouteManager Instance;
    public int selectedRoute = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // <--- SUPER IMPORTANT
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
