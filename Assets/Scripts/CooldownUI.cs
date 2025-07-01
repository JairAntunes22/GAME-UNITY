using UnityEngine;
using UnityEngine.UI;

public class SliceCooldownUI1 : MonoBehaviour
{
    public Image cooldownImage;
    private float cooldownDuration;
    private float cooldownTimer;

    private bool isCoolingDown = false;

    public void StartCooldown(float duration)
    {
        cooldownDuration = duration;
        cooldownTimer = 0f;
        isCoolingDown = true;
        cooldownImage.fillAmount = 0f;
    }

    void Update()
    {
        if (isCoolingDown)
        {
            cooldownTimer += Time.deltaTime;
            cooldownImage.fillAmount = cooldownTimer / cooldownDuration;

            if (cooldownTimer >= cooldownDuration)
            {
                isCoolingDown = false;
                cooldownImage.fillAmount = 1f;
            }
        }
    }
}
