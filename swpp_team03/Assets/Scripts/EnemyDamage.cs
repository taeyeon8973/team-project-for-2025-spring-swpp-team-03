using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damageAmount = 20f;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[충돌 감지] {other.name}"); // 로그 찍기
        Debug.Log($"[충돌 감지] {other.gameObject.tag}"); // 로그 찍기
        if (other.CompareTag("Player"))
        {
            HyunmuMode hyunmu = other.GetComponent<HyunmuMode>();
            if (hyunmu != null && hyunmu.IsInvincible()) return;

            StatusBar status = FindObjectOfType<StatusBar>();
            if (status != null)
            {
                status.TakeDamage(damageAmount);
            }

            // 필요시 적 제거 (선택)
            // Destroy(gameObject);
        }
    }
}
