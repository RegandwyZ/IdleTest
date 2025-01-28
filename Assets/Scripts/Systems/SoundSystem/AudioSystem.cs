using UnityEngine;

namespace Systems.SoundSystem
{
    public enum SfxType
    {
        ClickBuilding,
        ExitBuilding,
        ClickUpgrade
    }

    public class AudioSystem : MonoBehaviour
    {
        public static AudioSystem Instance { get; private set; }

        [SerializeField] private AudioSource _backgroundSource;
        [SerializeField] private AudioClip _backgroundMusic;

        [SerializeField] private AudioSource _sfxSource;

        [SerializeField] private AudioClip _onClickBuilding;
        [SerializeField] private AudioClip _onExitBuilding;
        [SerializeField] private AudioClip _onClickUpgrade;

        public bool IsMusicMuted { get; private set; }
        public bool IsSfxMuted { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (_backgroundSource != null && _backgroundMusic != null)
            {
                _backgroundSource.clip = _backgroundMusic;
                _backgroundSource.loop = true;
                _backgroundSource.playOnAwake = false;
                _backgroundSource.Play();
            }
        }

        public void ToggleMusic()
        {
            if (_backgroundSource == null) return;

            IsMusicMuted = !IsMusicMuted;
            _backgroundSource.mute = IsMusicMuted;
        }

        public void ToggleSfx()
        {
            IsSfxMuted = !IsSfxMuted;

            if (_sfxSource != null)
                _sfxSource.mute = IsSfxMuted;
        }

        public void PlaySfx(SfxType sfxType)
        {
            if (_sfxSource == null) return;

            AudioClip clipToPlay = null;

            switch (sfxType)
            {
                case SfxType.ClickBuilding:
                    clipToPlay = _onClickBuilding;
                    break;

                case SfxType.ExitBuilding:
                    clipToPlay = _onExitBuilding;
                    break;

                case SfxType.ClickUpgrade:
                    clipToPlay = _onClickUpgrade;
                    break;
            }

            if (clipToPlay != null)
            {
                _sfxSource.PlayOneShot(clipToPlay);
            }
        }
    }
}