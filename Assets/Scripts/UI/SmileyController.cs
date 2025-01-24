using System.Collections;
using UnityEngine;

namespace UI
{
    public class SmileyController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _showTime = 2f;

        private Coroutine _hideCoroutine;

        private void Awake()
        {
            _spriteRenderer.enabled = false;
        }
        
        public void ShowSmiley()
        {
            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);

            _spriteRenderer.enabled = true;
            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(_showTime);
            _spriteRenderer.enabled = false;
            _hideCoroutine = null;
        }
    }
}