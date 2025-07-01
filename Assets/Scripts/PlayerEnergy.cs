using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int currentEnergy;

    [Header("UI Settings")]
    [SerializeField] private Slider energyBar;

    [Header("Energy Regen Settings")]
    [SerializeField] private int energyRegenAmount = 30; // energia que vai ser recuperada
    [SerializeField] private float energyRegenInterval = 5f; // tempo entre recargas em segundos

    private float regenTimer = 0f;

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyUI();
    }

    void Update()
    {
        RegenEnergyOverTime();
    }

    private void RegenEnergyOverTime()
    {
        if (currentEnergy < maxEnergy)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= energyRegenInterval)
            {
                currentEnergy += energyRegenAmount;
                currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
                UpdateEnergyUI();
                regenTimer = 0f;
            }
        }
        else
        {
            regenTimer = 0f; // reset timer se energia cheia
        }
    }

    public bool UseEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            UpdateEnergyUI();
            return true;
        }
        return false;
    }

    public void GainEnergy(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        UpdateEnergyUI();
    }

    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public int GetMaxEnergy()
    {
        return maxEnergy;
    }

    private void UpdateEnergyUI()
    {
        if (energyBar != null)
        {
            energyBar.value = currentEnergy;
        }
    }

    public void SetEnergyBar(Slider bar)
    {
        energyBar = bar;
        UpdateEnergyUI();
    }
}
