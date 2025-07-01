using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Transform pontoAtaque;
    [SerializeField] private float raioAtaque;
    [SerializeField] private LayerMask layersAtaque;
     [SerializeField] private int danoDeAtaque = 10;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Atacar();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Atacar();
        }
         if (Input.GetKeyDown(KeyCode.Q))
        {
            Atacar();
        }


    }

    private void OnDrawGizmos()
    {
        if (pontoAtaque != null)
        {
            Gizmos.DrawWireSphere(pontoAtaque.position, raioAtaque);
        }
    }



private void Atacar()
{
    Collider2D colliderInimigo = Physics2D.OverlapCircle(pontoAtaque.position, raioAtaque, layersAtaque);
    if (colliderInimigo != null)
    {
        // Primeiro tenta BossController
        BossController boss = colliderInimigo.GetComponent<BossController>();
        if (boss != null)
        {
            boss.ReceberDano(danoDeAtaque);
            return;
        }

        // Se não for boss, tenta EnemyGen normal
        EnemyGen inimigo = colliderInimigo.GetComponent<EnemyGen>();
        if (inimigo != null)
        {
            inimigo.ReceberDano();
        }
        else
        {
            Debug.Log("O objeto atingido não tem o script EnemyGen nem BossController");
        }
    }
}



}
