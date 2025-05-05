using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public Vector3 playerStartPosition; // Начальная позиция игрока
    public List<TileData> tiles; // Список тайлов
}

[System.Serializable]
public class TileData
{
    public string tileType; // Тип тайла (например, "ground", "hazard", "goal")
    public Vector3Int position; // Позиция тайла
}

[System.Serializable]
public class LevelList
{
    public List<LevelData> levels; // Список уровней
}