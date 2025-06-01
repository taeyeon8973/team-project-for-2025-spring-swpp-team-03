using UnityEngine;

public class FadeInTitle : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 2f;

    [Header("Optional Second Text")]
    public CanvasGroup subtitleCanvasGroup;
    public float subtitleDelay = 0.5f;
    public float subtitleFadeDuration = 1f;

    private void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(FadeIn());
    }

    System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        if (subtitleCanvasGroup != null)
        {
            yield return new WaitForSeconds(subtitleDelay);
            StartCoroutine(FadeInSubtitle());
        }
    }

    System.Collections.IEnumerator FadeInSubtitle()
    {
        float elapsed = 0f;

        while (elapsed < subtitleFadeDuration)
        {
            elapsed += Time.deltaTime;
            subtitleCanvasGroup.alpha = Mathf.Clamp01(elapsed / subtitleFadeDuration);
            yield return null;
        }
    }
}
