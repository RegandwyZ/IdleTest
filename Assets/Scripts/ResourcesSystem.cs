using System;
using PlayerCurrentProgress;
using UnityEngine;

public class ResourcesSystem : MonoBehaviour
{
    public static ResourcesSystem Instance { get; private set; }

    public event Action<int> OnMoneyChanged;

    private int _money;

    public void InitializeResourcesSystem()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _money = CurrentProgress.Instance.CurrentGameData.Money;
        
    }

    public int Money
    {
        get => _money;
        private set
        {
            _money = value;
            OnMoneyChanged?.Invoke(_money);
        }
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        CurrentProgress.Instance.ChangeMoney(amount);
    }

    public bool SpendMoney(int amount)
    {
        if (_money >= amount)
        {
            Money -= amount;
            CurrentProgress.Instance.ChangeMoney(-amount);
            return true;
        }
        else
        {
            Debug.Log("Not enough money!");
            return false;
        }
    }
}