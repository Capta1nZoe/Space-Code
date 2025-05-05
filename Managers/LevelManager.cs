using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public Tilemap groundTilemap; 
    public Tilemap hazardTilemap; 
    public Tilemap goalTilemap; 
    public Tilemap wallsTilemap; 

    public Tile groundTile; 
    public Tile hazardTile;
    public Tile goalTile;     
    public Tile wallTile; 

    public PlayerController player; // Ссылка на игрока

    private List<LevelData> levels;
    private int currentLevelIndex = 0;

    public int GetLevelCount()
    {
        return levels != null ? levels.Count : 0;
    }

    /// <summary>
    /// Загружает данные уровней из JSON-файла.
    /// </summary>
    public void LoadLevels()
    {
        string path = Path.Combine(Application.dataPath, "Levels.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            try
            {
                LevelList levelList = JsonUtility.FromJson<LevelList>(json);

                if (levelList != null && levelList.levels != null)
                {
                    levels = levelList.levels;
                    Debug.Log(levels.Count + " уровней загружено.");
                }
                else
                {
                    Debug.LogError("Ошибка при загрузке уровней: некорректный формат JSON.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Ошибка при десериализации JSON: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Файл Levels.json не найден!");
        }
    }

    /// <summary>
    /// Загружает указанный уровень.
    /// </summary>
    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError("Неверный индекс уровня!");
            return;
        }

        ClearTilemaps();

        LevelData level = levels[levelIndex];

        // Устанавливаем начальную позицию игрока
        player.transform.position = level.playerStartPosition;
        player.StartPlayerPosition();

        // Размещаем тайлы
        foreach (TileData tileData in level.tiles)
        {
            switch (tileData.tileType)
            {
                case "ground":
                    groundTilemap.SetTile(tileData.position, groundTile);
                    break;
                case "hazard":
                    hazardTilemap.SetTile(tileData.position, hazardTile);
                    break;
                case "goal":
                    goalTilemap.SetTile(tileData.position, goalTile);
                    break;
                case "wall":
                    wallsTilemap.SetTile(tileData.position, wallTile);
                    break;
                default:
                    Debug.LogWarning($"Неизвестный тип тайла: {tileData.tileType}");
                    break;
            }
        }

        Debug.Log($"Уровень {levelIndex + 1} загружен.");
    }

    /// <summary>
    /// Очищает все тайловые карты.
    /// </summary>
    private void ClearTilemaps()
    {
        groundTilemap.ClearAllTiles();
        hazardTilemap.ClearAllTiles();
        goalTilemap.ClearAllTiles();
        wallsTilemap.ClearAllTiles();
    }

    /// <summary>
    /// Загружает следующий уровень.
    /// </summary>
    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count)
        {
            Debug.Log("Все уровни пройдены!");
            return;
        }

        LoadLevel(currentLevelIndex);
    }
}