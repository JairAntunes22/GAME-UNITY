using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    public GameObject inimigoPrefab;
    public Transform[] spawnPoints;

  private void Awake()
{
    if (Instance == null)
        Instance = this;
    else
        Destroy(gameObject);

    if(inimigoPrefab == null)
        Debug.LogError("O prefab do inimigo NÃO está configurado no Spawner!");
    else
        Debug.Log("Prefab do inimigo configurado corretamente!");
}


    private void Start()
    {
        foreach (var sp in spawnPoints)
        {
            SpawnInPoint(sp.GetComponent<SpawnPoint>());
        }
    }

    public void SpawnInPoint(SpawnPoint spawnPoint)
    {
        if (spawnPoint != null && inimigoPrefab != null)
        {
            spawnPoint.Spawn(inimigoPrefab);
        }
        else
        {
            Debug.LogWarning("Spawner ou inimigoPrefab está nulo!");
        }
    }
}
