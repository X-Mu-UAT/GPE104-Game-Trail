using UnityEngine;
using UnityEngine.UI;

public class OptionsScreenController : MonoBehaviour
{
    [Header("UI Slider Components")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void OnEnable()
    {
        // When the Options Screen turns on, read what is already saved in PlayerPrefs
        // Fall back to defaults (0.1f and 0.8f) if no data exists yet
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.1f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        // Position the UI slider handles to match the saved values
        if (musicVolumeSlider != null) musicVolumeSlider.value = savedMusic;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = savedSFX;

        // Listen for the player dragging the sliders
        if (musicVolumeSlider != null) musicVolumeSlider.onValueChanged.AddListener(OnSliderChanged);
        if (sfxVolumeSlider != null) sfxVolumeSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnDisable()
    {
        // Clean up listeners when the screen turns off to prevent memory leaks
        if (musicVolumeSlider != null) musicVolumeSlider.onValueChanged.RemoveListener(OnSliderChanged);
        if (sfxVolumeSlider != null) sfxVolumeSlider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    // This runs automatically whenever a slider handle moves
    private void OnSliderChanged(float unusedValue)
    {
        if (GameManager.Instance != null && musicVolumeSlider != null && sfxVolumeSlider != null)
        {
            // Push the values into our GameManager to change volume instantly and save to disk
            GameManager.Instance.UpdateAndSaveVolume(musicVolumeSlider.value, sfxVolumeSlider.value);
        }
    }
}