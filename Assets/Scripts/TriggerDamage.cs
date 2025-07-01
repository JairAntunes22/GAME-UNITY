using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    public HeartSystem heart;
    public PlayerController player;

    private void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerController>();
        if (heart == null)
            heart = FindFirstObjectByType<HeartSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null && heart != null)
            {
                bool fromRight = collision.transform.position.x <= player.transform.position.x;
                player.ApplyKnockback(fromRight);
                heart.vida -= 1;
                player.TakeDamage();
            }
            else
            {
                Debug.LogWarning("Player ou Heart não estão atribuídos no TriggerDamage!");
            }
        }
    }
}
