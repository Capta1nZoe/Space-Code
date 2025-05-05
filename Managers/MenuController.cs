using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu; // Панель паузы
    public GameObject buttonPrefab; // Префаб кнопки уровня
    public Transform buttonContainer; // Контейнер для кнопок выбора уровня
    public LevelManager levelManager; // Ссылка на LevelManager
    [Header("Buttons")]
    public GameObject LevelSelectionButton; 
    public GameObject OptionsButton; 
    public GameObject MainMenuButton; 
    public GameObject QuitButton; 
    public GameObject BackButton;
    
    private bool isPaused = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
        BackButton.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Ставит игру на паузу и отображает меню.
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Возобновляет игру и скрывает меню.
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Переход в меню выбора уровня.
    /// </summary>
    public void ShowLevelSelection()
    {
        QuitButton.SetActive(false);
        MainMenuButton.SetActive(false);
        LevelSelectionButton.SetActive(false);
        OptionsButton.SetActive(false);
        BackButton.SetActive(true);

        // Создаем кнопки для каждого уровня
        int levelCount = levelManager.GetLevelCount(); // Получаем количество уровней из LevelManager
        for (int i = 0; i < levelCount; i++)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer); // Создаем кнопку
            button.GetComponentInChildren<Text>().text = $"Level {i + 1}"; // Устанавливаем текст кнопки
            int levelIndex = i; 
            button.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelIndex)); // Добавляем обработчик нажатия
        }
    }

    /// <summary>
    /// Возврат в меню паузы из меню выбора уровня.
    /// </summary>
    public void BackToPauseMenu()
    {
        for (int i = buttonContainer.transform.childCount - 1; i >= 4; i--)
        {
            Transform child = buttonContainer.GetChild(i);
            // Удаляем все кнопки выбора уровня
            Destroy(child.gameObject);
        }
        
        QuitButton.SetActive(true);
        MainMenuButton.SetActive(true);
        LevelSelectionButton.SetActive(true);
        OptionsButton.SetActive(true);
        BackButton.SetActive(false);
    }

    /// <summary>
    /// Загрузка указанного уровня.
    /// </summary>
    public void LoadLevel(int levelIndex)
    {
        Debug.Log($"Загрузка уровня {levelIndex + 1}");
        Time.timeScale = 1f;
        ResumeGame(); // Возобновляем игру перед загрузкой уровня
        levelManager.LoadLevel(levelIndex); // Загружаем уровень через LevelManager
    }

    /// <summary>
    /// Выход в главное меню.
    /// </summary>
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Выход из игры.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();
    }
}