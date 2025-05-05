using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelExporter : MonoBehaviour
{
    public Tilemap groundTilemap; // Тайловая карта для земли
    public Tilemap hazardTilemap; // Тайловая карта для опасных зон
    public Tilemap goalTilemap; // Тайловая карта для целей
    public Tilemap wallsTilemap; // Тайловая карта для стен
    public Transform player; // Объект игрока

    public string outputFileName = "ExportedLevels.json"; // Имя выходного файла

    [ContextMenu("Export Level to JSON")]
    public void ExportLevel()
    {
        // Создаем объект уровня
        LevelData levelData = new LevelData
        {
            playerStartPosition = player.position, // Сохраняем начальную позицию игрока
            tiles = new List<TileData>()
        };

        // Считываем тайлы из groundTilemap
        foreach (var position in groundTilemap.cellBounds.allPositionsWithin)
        {
            if (groundTilemap.HasTile(position))
            {
                levelData.tiles.Add(new TileData
                {
                    tileType = "ground",
                    position = position
                });
            }
        }

        // Считываем тайлы из hazardTilemap
        foreach (var position in hazardTilemap.cellBounds.allPositionsWithin)
        {
            if (hazardTilemap.HasTile(position))
            {
                levelData.tiles.Add(new TileData
                {
                    tileType = "hazard",
                    position = position
                });
            }
        }

        // Считываем тайлы из goalTilemap
        foreach (var position in goalTilemap.cellBounds.allPositionsWithin)
        {
            if (goalTilemap.HasTile(position))
            {
                levelData.tiles.Add(new TileData
                {
                    tileType = "goal",
                    position = position
                });
            }
        }

        // Считываем тайлы из wallsTilemap
        foreach (var position in wallsTilemap.cellBounds.allPositionsWithin)
        {
            if (wallsTilemap.HasTile(position))
            {
                levelData.tiles.Add(new TileData
                {
                    tileType = "wall",
                    position = position
                });
            }
        }

        // Сериализуем уровень в JSON
        string json = JsonUtility.ToJson(levelData, true);

        // Сохраняем JSON в файл
        string path = Path.Combine(Application.dataPath, outputFileName);
        File.WriteAllText(path, json);

        Debug.Log("Уровень экспортирован в файл: " + path);
    }
}