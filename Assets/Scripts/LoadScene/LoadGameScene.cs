using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace LoadScene
{
    public class LoadGameScene : MonoBehaviour
    {
        [SerializeField] private SliderRun _sliderRun;
        private const string GAME_SCENE = "Game";

        private void Start()
        {
            StartCoroutine(LoadGameAfterSlider());
        }

        private IEnumerator LoadGameAfterSlider()
        {
            yield return _sliderRun.RunSliderCoroutine();
            SceneManager.LoadScene(GAME_SCENE);
        }
    }
}