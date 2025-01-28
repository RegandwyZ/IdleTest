using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FadeOutImage : MonoBehaviour
    {
        [SerializeField] private Image[] _imagesToFade;
        [SerializeField] private GameObject _panel;

        private const float DURATION = 2f;

        private void Start()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            Color[] startColors = new Color[_imagesToFade.Length];
            for (int i = 0; i < _imagesToFade.Length; i++)
            {
                startColors[i] = _imagesToFade[i].color;
            }

            float elapsedTime = 0f;
            while (elapsedTime < DURATION)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / DURATION);

                for (int i = 0; i < _imagesToFade.Length; i++)
                {
                    _imagesToFade[i].color = new Color(
                        startColors[i].r,
                        startColors[i].g,
                        startColors[i].b,
                        alpha
                    );
                }

                yield return null;
            }

            for (int i = 0; i < _imagesToFade.Length; i++)
            {
                _imagesToFade[i].color = new Color(
                    startColors[i].r,
                    startColors[i].g,
                    startColors[i].b,
                    0f
                );
            }

            if (_panel != null)
            {
                _panel.SetActive(false);
            }
        }
    }
}