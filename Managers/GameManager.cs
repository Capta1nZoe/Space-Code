using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InputField commandInput;
    public CommandExecutor executor;
    public PlayerController player; // Ссылка на PlayerController
    public Button runButton; // Кнопка "Run"
    public Button resetButton; // Кнопка "Reset"

    /// <summary>
    /// Запускает выполнение команд из многострочного текстового поля.
    /// </summary>
    public void RunCommands()
    {
        string input = commandInput.text;
        var commands = CommandParser.ParseCommands(input);

        if (commands.Count > 0)
        {
            StartCoroutine(ExecuteWithLock(commands));
        }
        else
        {
            Debug.LogWarning("Нет корректных команд для выполнения!");
        }
    }

    /// <summary>
    /// Отключает кнопки "Run" и "Reset" на время выполнения команд.
    /// </summary>
    private IEnumerator ExecuteWithLock(System.Collections.Generic.List<(string command, string argument)> commands)
    {
        runButton.interactable = false; // Отключаем кнопку "Run"
        resetButton.interactable = false; // Отключаем кнопку "Reset"
        yield return StartCoroutine(executor.ExecuteCommands(commands));
        runButton.interactable = true; // Включаем кнопку "Run" после выполнения
        resetButton.interactable = true; // Включаем кнопку "Reset" после выполнения
    }

    /// <summary>
    /// Сбрасывает позицию игрока на начальную.
    /// </summary>
    public void ResetPlayerPosition()
    {
        if (player != null)
        {
            player.ResetPlayerPosition();
        }
        else
        {
            Debug.LogError("PlayerController reference is missing in GameManager.");
        }
    }
}
