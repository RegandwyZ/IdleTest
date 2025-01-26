using System.Collections;
using System.Collections.Generic;
using System.IO;
using PlayerCurrentProgress;
using Shop;
using UnityEngine;


namespace SaveSystem
{
    public class SaveGameSystem : MonoBehaviour
    {
        [SerializeField] private float _autoSaveInterval = 30f;
        
        public GameData CurrentGameData;

        public void InitializeSaveGameSystem()
        {
            GameData loadedData = SaveSystem.LoadGame();
            if (loadedData != null)
            {
                CurrentGameData = loadedData;
            }
            else
            {
                CurrentGameData = new GameData
                {
                    Money = 2000,
                    Buildings = new List<BuildingData>()
                };
                
                AddCandyShopAtFirstTime();
                AddFruitShopAtFirstTime();
            }

            ApplyLoadedData();
        }

        private void AddFruitShopAtFirstTime()
        {
            CurrentGameData.Buildings.Add(new BuildingData
            {
                BuildingId = ShopType.Coffee,
                IncomeLevel = 1,
                TradeTimeLevel = 1
            });
        }

        private void AddCandyShopAtFirstTime()
        {
            CurrentGameData.Buildings.Add(new BuildingData
            {
                BuildingId = ShopType.Candy,
                IncomeLevel = 1,
                TradeTimeLevel = 1
            });
        }

        private void Start()
        {
            StartCoroutine(AutoSaveCoroutine());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClearAllData();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                SaveSystem.SaveGame(CurrentProgress.Instance.CurrentGameData);
            }
        }

        private IEnumerator AutoSaveCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_autoSaveInterval);
                SaveCurrentGame();
            }
        }

        private void SaveCurrentGame()
        {
            SaveSystem.SaveGame(CurrentProgress.Instance.CurrentGameData);
        }

        
        private void ApplyLoadedData()
        {
            CurrentProgress.Instance.CurrentGameData = CurrentGameData;
        }

        private void OnApplicationQuit()
        {
            SaveCurrentGame();
        }

        private void ClearAllData()
        {
           
            if (CurrentProgress.Instance != null)
            {
                CurrentProgress.Instance.CurrentGameData = new GameData
                {
                    Money = 0,
                    Buildings = new List<BuildingData>()
                };
            }
            
            string saveFilePath = Path.Combine(Application.persistentDataPath, "savedata.json");
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                Debug.Log("Файл сохранения удалён.");
            }
            else
            {
                Debug.LogWarning("Файл сохранения не найден.");
            }
        }
    }
}