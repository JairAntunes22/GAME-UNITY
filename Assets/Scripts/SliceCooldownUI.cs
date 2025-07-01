using UnityEngine;
using UnityEngine.UI;

public class SliceCooldownUI : MonoBehaviour
{
    public Image cooldownImage;
    private float cooldownDuration;
    private float cooldownTimer;

    private bool isCoolingDown = false;

    void Start()
    {
        cooldownImage.fillAmount = 1f; // Começa cheia
        cooldownImage.enabled = true;  // Garantir que está visível
    }

    public void StartCooldown(float duration)
    {
        cooldownDuration = duration;
        cooldownTimer = 0f;      // Reinicia o timer
        isCoolingDown = true;
        cooldownImage.fillAmount = 0f;  // Esvazia a barra para começar a recarregar
    }

    void Update()
    {
        if (isCoolingDown)
        {
            cooldownTimer += Time.deltaTime;
            cooldownImage.fillAmount = Mathf.Clamp01(cooldownTimer / cooldownDuration);

            if (cooldownTimer >= cooldownDuration)
            {
                isCoolingDown = false;
                cooldownImage.fillAmount = 1f; // Barra cheia ao final do cooldown
            }
        }
    }
}
