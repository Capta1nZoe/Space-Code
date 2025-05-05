using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Управляет главным меню.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Загружает игровую сцену.
    /// </summary>
    public void StartGame() => SceneManager.LoadScene("Play");

    /// <summary>
    /// Выход из игры.
    /// </summary>
    public void QuitGame() => Application.Quit();
}
