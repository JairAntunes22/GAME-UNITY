using UnityEngine;

public class Slice : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float livingTimer = 3f;

    private Vector3 move = Vector3.zero;

    private float direction = 1f; // 1 = direita, -1 = esquerda

    void Start()
    {
        Destroy(gameObject, livingTimer);
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        move.x = speed * direction * Time.deltaTime;
        transform.position += move;
    }

    // Define a direção do slice (1 para direita, -1 para esquerda)
   public void SetDirection(float dir)
{
    direction = dir;

    Vector3 scale = transform.localScale;
    // Inverte o sinal para compensar sprite "naturalmente virado para esquerda"
    scale.x = Mathf.Abs(scale.x) * -dir;
    transform.localScale = scale;
}




  private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Inimigo"))
    {
        Debug.Log("Slice colidiu com inimigo!");

        // Destrói o inimigo
        Destroy(collision.gameObject);

        // NÃO destruir o slice aqui para ele continuar
        // O slice vai continuar se movendo até o tempo definido em livingTimer no Start()
    }

    // Opcional: se quiser destruir slice ao colidir com parede, chao, etc:
    else if (collision.CompareTag("Parede") || collision.CompareTag("Chao"))
    {
        Destroy(gameObject);
    }
}


}
