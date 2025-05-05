using UnityEngine;

namespace Assets.Scripts.Play
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private LevelManager _levelManager; // Ссылка на LevelManager
        [SerializeField] private int levelIndex = 0; // Индекс уровня для загрузки
        private void Awake()
        {    
            _levelManager.LoadLevels();
            _levelManager.LoadLevel(levelIndex); 
        }
    }
}