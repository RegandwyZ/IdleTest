using UnityEngine;
using UnityEngine.UI;

namespace SoundSystem
{
    public enum ToggleSoundType
    {
        Music,
        Sfx
    }

    [RequireComponent(typeof(Button))]
    public class ToggleSoundButton : MonoBehaviour
    {
        [SerializeField] private ToggleSoundType _toggleType;
        
        [SerializeField] private Sprite _buttonOn;   
        [SerializeField] private Sprite _buttonOff;  

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnToggleClick);
        }

        private void OnEnable()
        {
            UpdateButtonSprite();
        }

        private void OnToggleClick()
        {
            switch (_toggleType)
            {
                case ToggleSoundType.Music:
                    AudioSystem.Instance.ToggleMusic();
                    break;
                
                case ToggleSoundType.Sfx:
                    AudioSystem.Instance.ToggleSfx();
                    break;
            }
            
            UpdateButtonSprite();
        }
        
        private void UpdateButtonSprite()
        {
            bool isMuted = false;
            
            switch (_toggleType)
            {
                case ToggleSoundType.Music:
                    isMuted = AudioSystem.Instance.IsMusicMuted;
                    break;

                case ToggleSoundType.Sfx:
                    isMuted = AudioSystem.Instance.IsSfxMuted;
                    break;
            }
            
            _button.image.sprite = isMuted ? _buttonOff : _buttonOn;
        }
    }
}
