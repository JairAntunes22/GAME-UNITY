using UnityEngine;
using TMPro;

public class PlayerOuro : MonoBehaviour
{
    public int ouroAtual = 0;
    public TMP_Text textoOuro;

    public void AdicionarOuro(int valor)
    {
        ouroAtual += valor;
        Debug.Log("Ouro coletado: " + valor + ", total: " + ouroAtual);
        AtualizarHUD();
    }

    void AtualizarHUD()
    {
        if (textoOuro != null)
            textoOuro.text = "" + ouroAtual;
    }

    private void Start()
    {
        AtualizarHUD();
    }
}
