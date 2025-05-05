using UnityEngine;
using UnityEngine.UI;

public class DebugMenuController : MonoBehaviour
{
    public GameObject debugMenu; // Ссылка на объект меню отладки
    public Text fpsCounter; // Ссылка на текстовый элемент для отображения FPS
    private bool isDebugMenuVisible = false; // Флаг состояния меню отладки

    private float deltaTime = 0.0f; // Для расчета FPS

    private void Update()
    {
        // Обновляем deltaTime для расчета FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Проверяем нажатие клавиши ~ (Tilde)
        if (Input.GetKeyDown(KeyCode.BackQuote)) // BackQuote — это клавиша ~
        {
            ToggleDebugMenu();
        }

        // Если меню отладки активно, обновляем FPS
        if (isDebugMenuVisible)
        {
            UpdateFPSCounter();
        }
    }

    /// <summary>
    /// Переключает видимость меню отладки.
    /// </summary>
    private void ToggleDebugMenu()
    {
        isDebugMenuVisible = !isDebugMenuVisible;
        debugMenu.SetActive(isDebugMenuVisible); // Включаем или выключаем меню
    }

    /// <summary>
    /// Обновляет текст счетчика FPS.
    /// </summary>
    private void UpdateFPSCounter()
    {
        float fps = 1.0f / deltaTime;
        fpsCounter.text = $"FPS: {Mathf.Ceil(fps)}";
    }
}