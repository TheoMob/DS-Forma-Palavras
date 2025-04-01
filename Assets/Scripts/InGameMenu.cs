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
        GameSessionLogger.instance.OnApplicationQuit();
        // Fecha o aplicativo no Android
        Application.Quit();
    }
}
