using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Configurações do Boss")]
    public int vidaMaxima = 100;
    public int vidaAtual;
    public int quantidadeOuro = 5;  // Quantidade de ouro que o boss dropa

    public GameObject ouroPrefab;
    public GameObject player;

    [Header("Movimentação")]
    public Transform leftPoint;
    public Transform rightPoint;
    public float speed = 2.5f;
    public float chaseDistance = 10f;
    public float maxHeightDifference = 1.0f;

    [Header("Verificação de chão")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

    private bool movingRight = true;
    private bool isChasing = false;

    [HideInInspector]
    public SpawnPoint spawner; // ponto de origem

    private Animator anim;

    void Start()
    {
        vidaAtual = vidaMaxima;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Boss"), LayerMask.NameToLayer("Inimigo"), true);

        anim = GetComponent<Animator>();
    }

    void Update()
{
    if (player == null) return;

    float distanceX = Mathf.Abs(player.transform.position.x - transform.position.x);
    float heightDifference = Mathf.Abs(player.transform.position.y - transform.position.y);

    isChasing = distanceX <= chaseDistance && heightDifference <= maxHeightDifference;

    if (isChasing)
    {
        anim.SetBool("isRunning", true);
        speed = 15f;  // Aumenta a velocidade para 30 quando está correndo
        ChasePlayer();
    }
    else
    {
        anim.SetBool("isRunning", false);
        speed = 10f; // Volta para a velocidade normal quando não está correndo
        Patrol();
    }
}


    void Patrol()
    {
        if (!IsGroundAhead())
        {
            Flip();
        }

        Vector2 moveDir = movingRight ? Vector2.right : Vector2.left;
        transform.Translate(moveDir * speed * Time.deltaTime);

        if (movingRight && transform.position.x >= rightPoint.position.x)
            Flip();
        else if (!movingRight && transform.position.x <= leftPoint.position.x)
            Flip();
    }

    bool IsGroundAhead()
    {
        Vector2 checkPosition = groundCheck.position;
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(checkPosition, Vector2.down * groundCheckDistance, Color.red);
        return hit.collider != null;
    }

    void ChasePlayer()
    {
        if (player.transform.position.x > transform.position.x)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (!movingRight) Flip();
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (movingRight) Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Método para o boss receber dano
    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        anim.SetBool("isHurt", true);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
        else
        {
            // Pode implementar um Coroutine para resetar o isHurt depois de um tempo,
            // ou chamar FinalizarHurt via Animation Event no final da animação Hurt
        }
    }

    // Método para ser chamado por Animation Event no final da animação Hurt
    public void FinalizarHurt()
    {
        anim.SetBool("isHurt", false);
    }

    void Morrer()
    {
        // Dropa ouro em várias posições próximas, igual à quantidadeOuro
        for (int i = 0; i < quantidadeOuro; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 1f, 0);

            RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.down, 5f, groundLayer);
            if (hit.collider != null)
            {
                spawnPosition.y = hit.point.y + 0.1f;
            }
            else
            {
                spawnPosition.y -= 0.5f;
            }

            Instantiate(ouroPrefab, spawnPosition, Quaternion.identity);
        }

        if (spawner != null)
            spawner.InimigoMorreu();

        Destroy(gameObject);
    }
}
