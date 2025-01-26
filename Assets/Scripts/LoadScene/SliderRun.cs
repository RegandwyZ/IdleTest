using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LoadScene
{
    public class SliderRun : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        private const float SPEED = 0.35f;
        
        public IEnumerator RunSliderCoroutine()
        {
            var time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime * SPEED;
                _slider.value = time;
                yield return null;
            }

            _slider.value = 1f; 
        }
    }
}