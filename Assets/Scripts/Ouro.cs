using UnityEngine;

public class Ouro : MonoBehaviour
{
    public int valor = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player tocou no ouro");

            PlayerOuro playerOuro = other.GetComponent<PlayerOuro>();
            if (playerOuro != null)
            {
                playerOuro.AdicionarOuro(valor);
            }

            Destroy(gameObject);
        }
    }
}
