using System.Collections;
using UnityEngine;

/// <summary>
/// Управляет движением игрока по Tilemap.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public TileManager tileManager;
    public SpriteRenderer spriteRenderer; // Ссылка на SpriteRenderer игрока
    public Sprite upSprite, downSprite; // Спрайты для разных направлений

    private Vector3Int currentCell; // Текущая клетка игрока
    private Vector3 initialPosition; // Начальная позиция игрока
    private Vector3 initialScale; // Начальный масштаб игрока

    public bool IsDestroyed { get; private set; } = false; // Флаг уничтожения игрока

    public void StartPlayerPosition()
    {
        // Определяем стартовую клетку
        currentCell = tileManager.groundTilemap.WorldToCell(transform.position);
        transform.position = tileManager.groundTilemap.GetCellCenterWorld(currentCell);

        // Сохраняем начальную позицию, поворот и масштаб игрока
        initialPosition = transform.position;
        initialScale = transform.localScale;
        spriteRenderer.sprite = downSprite; // Устанавливаем вид сверху
        transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
        
    }

    /// <summary>
    /// Сбрасывает позицию игрока на начальную.
    /// </summary>
    public void ResetPlayerPosition()
    {
        currentCell = tileManager.groundTilemap.WorldToCell(initialPosition); // Сбрасываем клетку
        transform.position = tileManager.groundTilemap.GetCellCenterWorld(currentCell);

        transform.position = initialPosition; // Сбрасываем позицию
        spriteRenderer.sprite = downSprite; // Устанавливаем вид сверху
        transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
        transform.localScale = initialScale; // Сбрасываем масштаб
        IsDestroyed = false; // Сбрасываем флаг уничтожения
        Debug.Log("Player position, rotation, and scale reset to initial values.");
    }

    /// <summary>
    /// Перемещает игрока в указанном направлении на заданное количество шагов.
    /// </summary>
    public IEnumerator Move(Vector3Int direction, int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            Vector3Int newCell = currentCell + direction;

            // Определяем направление движения и меняем спрайт
            if (direction == Vector3Int.up)
            {
                spriteRenderer.sprite = upSprite; // Устанавливаем вид сверху
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z); // Сбрасываем отражение
            }
            else if (direction == Vector3Int.down)
            {
                spriteRenderer.sprite = upSprite; // Устанавливаем вид сверху
                transform.localScale = new Vector3(initialScale.x, -initialScale.y, initialScale.z); // Отражаем по вертикали
            }
            else if (direction == Vector3Int.left)
            {
                spriteRenderer.sprite = downSprite; // Устанавливаем вид сбоку
                transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z); // Отражаем по горизонтали
            }
            else if (direction == Vector3Int.right)
            {
                spriteRenderer.sprite = downSprite; // Устанавливаем вид сбоку
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z); // Сбрасываем отражение
            }

            // Проверяем, можно ли двигаться в указанную клетку
            if (tileManager.IsWalkable(newCell))
            {
                currentCell = newCell;
                Vector3 targetPos = tileManager.groundTilemap.GetCellCenterWorld(newCell);

                Vector3 startPos = transform.position;
                float time = 0;
                while (time < 1)
                {
                    transform.position = Vector3.Lerp(startPos, targetPos, time);
                    time += Time.deltaTime * moveSpeed;
                    yield return null;
                }

                transform.position = targetPos;

                // Проверяем, не упал ли игрок в пропасть
                if (tileManager.IsHazard(newCell))
                {
                    Debug.Log("Игрок попал на hazard и уничтожен!");
                    //Destroy(gameObject); // Уничтожаем игрока
                    IsDestroyed = true;
                    yield break; // Прерываем выполнение команды
                }
            }
            else
            {
                Debug.Log("Движение невозможно: препятствие.");
                yield break;
            }
        }
    }
}
