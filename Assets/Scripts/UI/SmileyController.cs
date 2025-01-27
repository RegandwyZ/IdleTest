using System.Collections;
using UnityEngine;

namespace UI
{
    public class SmileyController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _happySmile;
        [SerializeField] private SpriteRenderer _angrySmile;

        [SerializeField] private float _showTime = 2f;

        private Coroutine _hideCoroutine;

        private void Awake()
        {
            _happySmile.enabled = false;
            _angrySmile.enabled = false;
        }

        public void ShowSmile(SmileType smileType)
        {
            var spriteRenderer = smileType switch
            {
                SmileType.Happy => _happySmile,
                SmileType.Angry => _angrySmile,
                _ => _happySmile,
            };

            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);

            spriteRenderer.enabled = true;
            _hideCoroutine = StartCoroutine(HideAfterDelay(spriteRenderer));
        }

        private IEnumerator HideAfterDelay(SpriteRenderer sprite)
        {
            yield return new WaitForSeconds(_showTime);
            sprite.enabled = false;
            _hideCoroutine = null;
        }
    }
}