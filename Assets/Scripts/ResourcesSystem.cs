using System;
using UnityEngine;

public class ResourcesSystem : MonoBehaviour
{
    public static ResourcesSystem Instance { get; private set; }

    public event Action<int> OnMoneyChanged;

    private int _money;

    private void Awake()
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

        _money = 100000; 
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
    }

    public bool SpendMoney(int amount)
    {
        if (_money >= amount)
        {
            Money -= amount;
            return true;
        }
        else
        {
            Debug.Log("Not enough money!");
            return false;
        }
    }
}
