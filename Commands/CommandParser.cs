using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Разбирает введенные игроком команды из многострочного InputField.
/// </summary>
public class CommandParser
{
    private static readonly Regex commandPattern = new Regex(@"^\s*(\w+)\((.*?)\)\s*$", RegexOptions.Compiled | RegexOptions.Multiline);

    /// <summary>
    /// Разбирает строки с командами и возвращает список пар (команда, аргумент).
    /// </summary>
    public static List<(string command, string argument)> ParseCommands(string input)
    {
        List<(string command, string argument)> commands = new List<(string, string)>();
        Stack<string> blockStack = new Stack<string>(); // Стек для отслеживания вложенных блоков

        string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("if(") || trimmedLine.StartsWith("else"))
            {
                // Добавляем команду if или else
                Match match = commandPattern.Match(trimmedLine);
                if (match.Success)
                {
                    string command = match.Groups[1].Value;
                    string argument = match.Groups[2].Value;
                    commands.Add((command, argument));
                    blockStack.Push(command); // Добавляем блок в стек
                }
            }
            else if (trimmedLine == "{")
            {
                // Начало блока
                commands.Add(("begin", blockStack.Peek())); // Добавляем метку начала блока
            }
            else if (trimmedLine == "}")
            {
                // Конец блока
                if (blockStack.Count > 0)
                {
                    string blockCommand = blockStack.Pop();
                    commands.Add(("end", blockCommand)); // Добавляем метку конца блока
                }
                else
                {
                    Debug.LogError("Ошибка: Найдена закрывающая скобка без соответствующего блока.");
                }
            }
            else
            {
                // Обычная команда
                Match match = commandPattern.Match(trimmedLine);
                if (match.Success)
                {
                    string command = match.Groups[1].Value;
                    string argument = match.Groups[2].Value;
                    commands.Add((command, argument));
                }
            }
        }

        if (blockStack.Count > 0)
        {
            Debug.LogError("Ошибка: Не все блоки были закрыты.");
        }

        return commands;
    }
}
