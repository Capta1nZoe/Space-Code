using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Управляет картой Tilemap и проверяет тип клеток.
/// </summary>
public class TileManager : MonoBehaviour
{
    public Tilemap groundTilemap;  // Дорога (проходимые клетки)
    public Tilemap wallsTilemap;   // Стены (непроходимые)
    public Tilemap hazardsTilemap; // Пропасти (опасные зоны)

    /// <summary>
    /// Проверяет, можно ли пройти на эту клетку.
    /// </summary>
    public bool IsWalkable(Vector3Int cellPosition)
    {
        return !wallsTilemap.HasTile(cellPosition);
    }

    /// <summary>
    /// Проверяет, является ли клетка пропастью.
    /// </summary>
    public bool IsHazard(Vector3Int cellPosition)
    {
        return hazardsTilemap.HasTile(cellPosition);
    }
}
