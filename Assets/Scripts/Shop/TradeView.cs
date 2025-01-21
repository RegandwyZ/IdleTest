using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class TradeView : MonoBehaviour
    {
        [SerializeField] private Slider _progressSlider;
        
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
        }
    }
}