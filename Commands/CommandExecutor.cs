using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Выполняет команды, переданные игроком.
/// </summary>
public class CommandExecutor : MonoBehaviour
{
    public PlayerController player;

    /// <summary>
    /// Выполняет список команд.
    /// </summary>
    public IEnumerator ExecuteCommands(List<(string command, string argument)> commands)
    {
        for (int i = 0; i < commands.Count && !player.IsDestroyed; i++)
        {
            var (command, argument) = commands[i];

            switch (command.ToLower())
            {
                case "moveup":
                    yield return player.Move(Vector3Int.up, ParseSteps(argument));
                    break;

                case "movedown":
                    yield return player.Move(Vector3Int.down, ParseSteps(argument));
                    break;

                case "moveleft":
                    yield return player.Move(Vector3Int.left, ParseSteps(argument));
                    break;

                case "moveright":
                    yield return player.Move(Vector3Int.right, ParseSteps(argument));
                    break;

                case "repeat":
                    // Обрабатываем команду repeat
                    yield return StartCoroutine(ExecuteRepeat(argument, commands, i, newIndex => i = newIndex));
                    break;

                default:
                    Debug.LogWarning($"Неизвестная команда: {command}");
                    break;
            }       
        }
    }

    /// <summary>
    /// Выполняет команду repeat.
    /// </summary>
    private IEnumerator ExecuteRepeat(string argument, List<(string command, string argument)> commands, int currentIndex, System.Action<int> updateIndex)
    {
        int repeatCount;
        if (!int.TryParse(argument, out repeatCount))
        {
            Debug.LogError($"Некорректный аргумент для Repeat: {argument}");
            yield break;
        }

        int startIndex = currentIndex + 1;

        for (int i = 0; i < repeatCount; i++)
        {
            for (int j = startIndex; j < commands.Count; j++)
            {
                var (command, arg) = commands[j];
                if (command.ToLower() == "endrepeat")
                {
                    break;
                }

                yield return StartCoroutine(ExecuteCommands(new List<(string, string)> { (command, arg) }));
            }
        }

        // Найти индекс EndRepeat
        for (int j = startIndex; j < commands.Count; j++)
        {
            if (commands[j].command.ToLower() == "endrepeat")
            {
                currentIndex = j;
                break;
            }
        }

        // Обновляем индекс через callback
        updateIndex?.Invoke(currentIndex);
    }

    /// <summary>
    /// Парсит количество шагов из аргумента.
    /// </summary>
    private int ParseSteps(string argument)
    {
        if (int.TryParse(argument, out int steps))
        {
            return Mathf.Max(1, steps); // Минимум 1 шаг
        }

        Debug.LogWarning($"Некорректный аргумент для движения: {argument}. Используется значение по умолчанию: 1.");
        return 1;
    }
}
