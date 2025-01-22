using PlayerCurrentProgress;
using SaveSystem;
using TMPro;
using UnityEngine;


public class PlayerResources : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _textMoney;

   private void Awake()
   {
      ResourcesSystem.Instance.OnMoneyChanged += UpdateMoneyText;
      UpdateMoneyText(ResourcesSystem.Instance.Money); 
   }

   private void OnDisable()
   {
      ResourcesSystem.Instance.OnMoneyChanged -= UpdateMoneyText;
   }

   private void UpdateMoneyText(int money)
   {
      _textMoney.text = $"{money}";
   }
   
  
}
