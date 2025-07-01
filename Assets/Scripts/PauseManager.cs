using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGerente : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        // NÃO PAUSA O TEMPO AQUI
        if (!SceneManager.GetSceneByName("Pause").isLoaded)
        {
            SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
        }

        isPaused = true;
    }

    public void ResumeGame()
    {
        // Fecha a cena do menu
        if (SceneManager.GetSceneByName("Pause").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Pause");
        }

        // Volta o tempo ao normal
        Time.timeScale = 1f;
        isPaused = false;
    }
}
