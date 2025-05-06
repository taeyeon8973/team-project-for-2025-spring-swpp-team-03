using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusBar : MonoBehaviour
{
    public Slider healthBar;
    public Slider energyBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;

    private float maxHealth = 100f;
    private float maxEnergy = 100f;

    private float currentHealth;
    private float currentEnergy;
    public GameObject gameOverText;
    private bool isGameOver = false;

    void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        healthBar.maxValue = maxHealth;
        energyBar.maxValue = maxEnergy;

        healthBar.value = currentHealth;
        energyBar.value = currentEnergy;

        UpdateUI();

        InvokeRepeating(nameof(DecreaseEnergy), 1f, 1f);
    }

    void DecreaseEnergy()
    {
        currentEnergy -= 1f;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        energyBar.value = currentEnergy;

        UpdateUI();
    }

    void UpdateUI()
    {
        healthText.text = $"{currentHealth} / {maxHealth}";
        energyText.text = $"{currentEnergy} / {maxEnergy}";
    }

    void Update()
    {
        if (!isGameOver && (currentHealth <= 0f || currentEnergy <= 0f))
        {
            isGameOver = true;
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        gameOverText.SetActive(true);
        Time.timeScale = 0f;
    }

    public void AddEnergy(float amount)
    {
        if (isGameOver) return;

        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        energyBar.value = currentEnergy;
        UpdateUI();
    }

    public void AddHealth(float amount)
    {
        if (isGameOver) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBar.value = currentHealth;
        UpdateUI();
    }

    public void TakeDamage(float amount)
    {
        if (isGameOver) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthBar.value = currentHealth;
        UpdateUI();
    }


}

