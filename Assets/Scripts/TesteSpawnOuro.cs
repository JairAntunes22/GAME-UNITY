using UnityEngine;

public class TesteSpawnOuro : MonoBehaviour
{
    public GameObject ouroPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (ouroPrefab != null)
            {
                Instantiate(ouroPrefab, transform.position + Vector3.up, Quaternion.identity);
                Debug.Log("Ouro instanciado no teste");
            }
            else
            {
                Debug.LogWarning("ouroPrefab NÃO está atribuído no Inspector!");
            }
        }
    }
}
