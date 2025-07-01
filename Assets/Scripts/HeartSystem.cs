using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeartSystem : MonoBehaviour
{
    public int vida;
    public int vidaMaxima;
    public Image[] coracao;
    public Sprite coracaoCheio;
    public Sprite coracaoVazio;

    private bool isDead;
    private PlayerController player;

    void Start()
    {
        player = GetComponent<PlayerController>(); // Corrigido se você usa PlayerController
    }

    void Update()
    {
        HealthLogic();
        DeadState();
    }

    void HealthLogic()
    {
        if (vida > vidaMaxima) vida = vidaMaxima;
        if (vida < 0) vida = 0;

        for (int i = 0; i < coracao.Length; i++)
        {
            if (i < vida)
            {
                coracao[i].sprite = coracaoCheio;
            }
            else
            {
                coracao[i].sprite = coracaoVazio;
            }

            coracao[i].enabled = (i < vidaMaxima);
        }
    }

    void DeadState()
    {
        if (isDead || vida > 0) return;

        isDead = true;
        vida = 0;

        if (player != null)
        {
            player.anim.SetTrigger("Die");
            player.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null) col.enabled = false;

        Invoke("LoadScene", 2f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("GameOver");
    }
}
