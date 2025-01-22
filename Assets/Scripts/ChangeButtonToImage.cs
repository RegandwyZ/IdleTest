using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonToImage : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;

    private void Awake()
    {
        _image.gameObject.SetActive(false);
    }

    public void Change()
    {
        _image.gameObject.SetActive(true);
        _button.gameObject.SetActive(false);
    }
}
