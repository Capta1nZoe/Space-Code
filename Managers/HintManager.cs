using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Управляет окном подсказок.
/// </summary>
public class HintManager : MonoBehaviour
{
    public GameObject hintPanel; // Панель с подсказкой
    public GameObject darkBackground; // Затемняющий фон
    public Button closeHintButton; // Кнопка закрытия подсказки
    public Sprite defaultSprite; // Спрайт по умолчанию
    public Sprite hoverSprite; // Спрайт при наведении
    private Image hintPanelImage; // Компонент Image панели

    private void Start()
    {
        // Получаем компонент Image панели
        hintPanelImage = hintPanel.GetComponent<Image>();
        if (hintPanelImage == null)
        {
            Debug.LogError("У hintPanel отсутствует компонент Image!");
        }

        // Добавляем обработчики событий для кнопки
        if (closeHintButton != null)
        {
            EventTrigger trigger = closeHintButton.gameObject.AddComponent<EventTrigger>();

            // Добавляем событие наведения
            EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            pointerEnterEntry.callback.AddListener((data) => OnCloseHintHover(true));
            trigger.triggers.Add(pointerEnterEntry);

            // Добавляем событие выхода
            EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            pointerExitEntry.callback.AddListener((data) => OnCloseHintHover(false));
            trigger.triggers.Add(pointerExitEntry);
        }
        else
        {
            Debug.LogError("Кнопка CloseHint не назначена!");
        }
    }

    /// <summary>
    /// Открывает окно с подсказками.
    /// </summary>
    public void ShowHint()
    {
        hintPanel.SetActive(true);
        darkBackground.SetActive(true);
    }

    /// <summary>
    /// Закрывает окно с подсказками.
    /// </summary>
    public void CloseHint()
    {
        hintPanel.SetActive(false);
        darkBackground.SetActive(false);
    }

    /// <summary>
    /// Изменяет спрайт hintPanel при наведении на кнопку CloseHint.
    /// </summary>
    /// <param name="isHovering">True, если курсор наведен на кнопку, иначе False.</param>
    private void OnCloseHintHover(bool isHovering)
    {
        if (hintPanelImage != null)
        {
            hintPanelImage.sprite = isHovering ? hoverSprite : defaultSprite;
        }
    }
}
