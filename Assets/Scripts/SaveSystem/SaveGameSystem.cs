﻿using System.Collections;
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

        private void Awake()
        {
            GameData loadedData = SaveSystem.LoadGame();
            if (loadedData != null)
            {
                CurrentGameData = loadedData;
                ApplyLoadedData();
            }
            else
            {
                CurrentGameData = new GameData
                {
                    Money = 1000,
                    Buildings = new List<BuildingData>()
                };
                
                AddCandyShopAtFirstTime();
                AddFruitShopAtFirstTime();
                
                ApplyLoadedData();
            }
        }

        private void AddFruitShopAtFirstTime()
        {
            CurrentGameData.Buildings.Add(new BuildingData
            {
                BuildingId = ShopType.Fruit,
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
        }

        private IEnumerator AutoSaveCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_autoSaveInterval);
                SaveCurrentGame();
            }
        }
        
        public void SaveCurrentGame()
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
        
        public void ClearAllData()
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