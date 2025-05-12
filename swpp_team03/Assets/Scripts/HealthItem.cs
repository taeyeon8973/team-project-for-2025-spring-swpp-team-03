using UnityEngine;

public class HealthItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StatusBar status = FindObjectOfType<StatusBar>();
            if (status != null)
            {
                status.AddHealth(10f);
            }

            Destroy(gameObject); // 아이템 제거
        }
    }
}
