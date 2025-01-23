using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class TradeView : MonoBehaviour
    {
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private Image _endOfDealImage;
        [SerializeField] private TextMeshProUGUI _earnMoneyText;

        private int _moneyForTrade;

        public void SetMoneyForTrade(int moneyForTrade)
        {
            _moneyForTrade = moneyForTrade;
        }

        public void ShowProgress(float tradeTime)
        {
            _progressSlider.gameObject.SetActive(true);
            StartCoroutine(ShowSlider(tradeTime));
        }

        private IEnumerator ShowSlider(float tradeTime)
        {
            float elapsedTime = 0f;
            _progressSlider.value = 0f;

            while (elapsedTime < tradeTime)
            {
                elapsedTime += Time.deltaTime;
                _progressSlider.value = Mathf.Clamp01(elapsedTime / tradeTime);

                yield return null;
            }

            _progressSlider.value = 1f;
            _progressSlider.gameObject.SetActive(false);

            yield return ShowEndOfDeal();
        }

        private IEnumerator ShowEndOfDeal()
        {

            _endOfDealImage.gameObject.SetActive(true);
            _earnMoneyText.text = $"+${_moneyForTrade}";
            _earnMoneyText.gameObject.SetActive(true);

            Color imageColor = _endOfDealImage.color;
            Color textColor = _earnMoneyText.color;
            imageColor.a = 0;
            textColor.a = 0;
            _endOfDealImage.color = imageColor;
            _earnMoneyText.color = textColor;

            float fadeDuration = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

                imageColor.a = alpha;
                textColor.a = alpha;

                _endOfDealImage.color = imageColor;
                _earnMoneyText.color = textColor;

                yield return null;
            }

            yield return new WaitForSeconds(1f);

            _endOfDealImage.gameObject.SetActive(false);
            _earnMoneyText.gameObject.SetActive(false);
        }
    }
}

