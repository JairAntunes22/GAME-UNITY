using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    private GameObject inimigoAtual;

    public void Spawn(GameObject prefab)
    {
        if (inimigoAtual != null) return;

        inimigoAtual = Instantiate(prefab, transform.position, Quaternion.identity);
        EnemyGen script = inimigoAtual.GetComponent<EnemyGen>();

        if (script != null)
            script.spawner = this;
    }

    public void InimigoMorreu()
    {
        inimigoAtual = null;
        StartCoroutine(RespawnDelay());
    }

    private IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(10f); // Tempo para reaparecer
        Spawner.Instance.SpawnInPoint(this);
    }

}
