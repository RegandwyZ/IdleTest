using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChangeButtonToImage : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
    
        public void Change()
        {
            _image.gameObject.SetActive(true);
            _button.gameObject.SetActive(false);
        }
    }
}
