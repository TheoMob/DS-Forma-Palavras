using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void ToggleTimerVisibility(bool visibility)
    {
        GameController.instance.DisplayTimer = visibility;
    }

    public void ExitApp()
    {
        // Fecha o aplicativo no Android
        Application.Quit();

        // Para garantir o encerramento no Android
        #if UNITY_ANDROID
        System.Diagnostics.Process.GetCurrentProcess().Kill();
        #endif
    }
}
