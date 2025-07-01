using UnityEngine;

public class EnemyGen : MonoBehaviour
{
    public GameObject ouroPrefab;
    public GameObject player;
    public Transform leftPoint;
    public Transform rightPoint;

    [Header("Movimentação")]
    public float speed = 2f;
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


    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Inimigo"), LayerMask.NameToLayer("Inimigo"), true);
        
    }

    void Update()
    {
        float distanceX = Mathf.Abs(player.transform.position.x - transform.position.x);
        float heightDifference = player.transform.position.y - transform.position.y;

        isChasing = distanceX <= chaseDistance && heightDifference <= maxHeightDifference;

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        // Verifica se há chão à frente
        if (!IsGroundAhead())
        {
            Flip();
        }

        Vector2 moveDir = movingRight ? Vector2.right : Vector2.left;
        transform.Translate(moveDir * speed * Time.deltaTime);

        // Verifica se chegou a um dos pontos limites
        if (movingRight && transform.position.x >= rightPoint.position.x)
            Flip();
        else if (!movingRight && transform.position.x <= leftPoint.position.x)
            Flip();
    }

    bool IsGroundAhead()
    {
        Vector2 checkPosition = groundCheck.position;
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(checkPosition, Vector2.down * groundCheckDistance, Color.red); // visual na cena
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


public void ReceberDano()
{
    if (ouroPrefab != null)
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5f, groundLayer);
        if (hit.collider != null)
        {
            spawnPosition.y = hit.point.y + 0.1f;
        }
        else
        {
            spawnPosition.y -= 0.5f;
        }

        GameObject ouroInstanciado = Instantiate(ouroPrefab, spawnPosition, Quaternion.identity);

        // Ignorar colisão entre o inimigo e o ouro instanciado
        Collider2D inimigoCol = GetComponent<Collider2D>();
        Collider2D ouroCol = ouroInstanciado.GetComponent<Collider2D>();

        if (inimigoCol != null && ouroCol != null)
        {
            Physics2D.IgnoreCollision(inimigoCol, ouroCol, true);
        }
    }
    else
    {
        Debug.LogWarning("ouroPrefab NÃO está atribuído no Inspector!");
    }

    if (spawner != null)
        spawner.InimigoMorreu();

    Destroy(gameObject);
}







}
