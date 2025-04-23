using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    private class AudioChannel
    {
        public string volumeParam;
        public Slider slider;
        public Toggle toggle;
        public string muteKey;
        [HideInInspector] public float lastVolume = 1f;

        public void Init(AudioMixer mixer, float multiplier)
        {
            // Load saved volume or default
            float savedVol = PlayerPrefs.HasKey(volumeParam)
                ? PlayerPrefs.GetFloat(volumeParam)
                : slider.value;
            bool isMuted = PlayerPrefs.GetInt(muteKey, 0) == 1;

            // Apply initial state
            if (isMuted)
            {
                toggle.isOn = true;
                slider.value = slider.minValue;
                SetVolume(mixer, multiplier, slider.minValue);
            }
            else
            {
                toggle.isOn = false;
                slider.value = savedVol;
                lastVolume = savedVol;
                SetVolume(mixer, multiplier, savedVol);
            }

            // UI events
            slider.onValueChanged.AddListener(v => OnSliderChanged(v, mixer, multiplier));
            toggle.onValueChanged.AddListener(m => OnToggleChanged(m, mixer, multiplier));
        }

        private void OnSliderChanged(float value, AudioMixer mixer, float multiplier)
        {
            if (toggle.isOn && value > slider.minValue)
            {
                toggle.isOn = false;
                PlayerPrefs.SetInt(muteKey, 0);
            }
            if (value > slider.minValue)
                lastVolume = value;

            SetVolume(mixer, multiplier, value);
            PlayerPrefs.SetFloat(volumeParam, value);
            PlayerPrefs.Save();
        }

        private void OnToggleChanged(bool isMuted, AudioMixer mixer, float multiplier)
        {
            PlayerPrefs.SetInt(muteKey, isMuted ? 1 : 0);
            float target = isMuted ? slider.minValue : (lastVolume > slider.minValue ? lastVolume : 1f);

            slider.value = target;
            SetVolume(mixer, multiplier, target);
            PlayerPrefs.Save();
        }

        private void SetVolume(AudioMixer mixer, float multiplier, float value)
        {
            float vol = value <= slider.minValue ? -80f : Mathf.Log10(value) * multiplier;
            mixer.SetFloat(volumeParam, vol);
        }
    }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private float multiplier = 30f;

    [Header("Channels")]
    [SerializeField] private AudioChannel musicChannel;
    [SerializeField] private AudioChannel sfxChannel;

    private void Start()
    {
        musicChannel.Init(mixer, multiplier);
        sfxChannel.Init(mixer, multiplier);
    }
}