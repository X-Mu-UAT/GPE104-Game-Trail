using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State UI Objects (Requirement 1 & 3)")]
    public GameObject TitleScreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsScreenStateObject;
    public GameObject CreditsScreenStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverScreenStateObject;
    public GameObject VictoryScreenStateObject;

    [Header("Score System (Requirement 2)")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int currentScore = 0;

    [Header("Audio Configuration (Exposed to Designers)")]
    public AudioClip backgroundMusicClip;
    public AudioClip playerShootClip;
    public AudioClip targetTakeDamageClip;
    private AudioSource musicAudioSource;

    private List<Health> activeObstacles = new List<Health>();
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetupBackgroundMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Automatically load configuration data when the manager initializes
        LoadGameSettings();
        SetGameState("MainMenu");
    }

    private void SetupBackgroundMusic()
    {
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.clip = backgroundMusicClip;
        musicAudioSource.loop = true;
        musicAudioSource.playOnAwake = true;

        // Grab saved music volume profile first, fall back to 0.1f if empty
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.1f);
        musicAudioSource.volume = savedMusicVolume;

        musicAudioSource.ignoreListenerPause = true;
        if (backgroundMusicClip != null)
        {
            musicAudioSource.Play();
        }
    }

    public void PlaySoundEffect(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }

    public void SetGameState(string stateName)
    {
        Debug.Log($"[GameManager] Switching state to: {stateName}");

        GameObject[] allStateObjects = new GameObject[] {
            TitleScreenStateObject, MainMenuStateObject, OptionsScreenStateObject,
            CreditsScreenStateObject, GameplayStateObject, GameOverScreenStateObject, VictoryScreenStateObject
        };

        foreach (GameObject stateObject in allStateObjects)
        {
            if (stateObject != null) stateObject.SetActive(false);
        }

        switch (stateName)
        {
            case "TitleScreen":
                if (TitleScreenStateObject != null) TitleScreenStateObject.SetActive(true);
                Time.timeScale = 0f;
                isGameOver = true;
                break;
            case "MainMenu":
                if (MainMenuStateObject != null) MainMenuStateObject.SetActive(true);
                Time.timeScale = 0f;
                isGameOver = true;
                break;
            case "Options":
                if (OptionsScreenStateObject != null) OptionsScreenStateObject.SetActive(true);
                Time.timeScale = 0f;
                break;
            case "Credits":
                if (CreditsScreenStateObject != null) CreditsScreenStateObject.SetActive(true);
                Time.timeScale = 0f;
                break;
            case "Gameplay":
                if (GameplayStateObject != null) GameplayStateObject.SetActive(true);
                Time.timeScale = 1f;
                isGameOver = false;
                currentScore = 0;
                UpdateScoreUI();
                ClearOldObstaclesOnly();
                break;
            case "GameOver":
                if (GameOverScreenStateObject != null) GameOverScreenStateObject.SetActive(true);
                Time.timeScale = 0f;
                isGameOver = true;
                break;
            case "Victory":
                if (VictoryScreenStateObject != null) VictoryScreenStateObject.SetActive(true);
                Time.timeScale = 0f;
                isGameOver = true;
                break;
        }
    }

    public void AddPoints(int pointsToGive)
    {
        if (isGameOver) return;
        currentScore += pointsToGive;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {currentScore:D5}";
        }
    }

    private void ClearOldObstaclesOnly()
    {
        activeObstacles.Clear();
        GameObject[] existingAsteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject asteroid in existingAsteroids)
        {
            Health asteroidHealth = asteroid.GetComponent<Health>();
            if (asteroidHealth != null && !activeObstacles.Contains(asteroidHealth))
            {
                activeObstacles.Add(asteroidHealth);
            }
        }

        Debug.Log($"[GameManager] Gameplay started! Tracking {activeObstacles.Count} asteroids.");
        CheckVictoryCondition();
    }

    public void RegisterObstacle(Health obstacle)
    {
        if (!activeObstacles.Contains(obstacle))
        {
            activeObstacles.Add(obstacle);
            Debug.Log($"[GameManager] Dynamically registered asteroid. Total remaining: {activeObstacles.Count}");
        }
    }

    public void UnregisterObstacle(Health obstacle)
    {
        if (activeObstacles.Contains(obstacle))
        {
            activeObstacles.Remove(obstacle);
            Debug.Log($"[GameManager] Removed asteroid. Remaining count: {activeObstacles.Count}");
        }
        CheckVictoryCondition();
    }

    private void CheckVictoryCondition()
    {
        Debug.Log($"[GameManager] Checking Victory -> Obstacles remaining: {activeObstacles.Count}, isGameOver variable: {isGameOver}");

        if (activeObstacles.Count == 0 && !isGameOver)
        {
            Debug.Log("[GameManager] Victory conditions met! Triggering Victory Screen.");
            TriggerVictory();
        }
    }

    public void TriggerVictory() => SetGameState("Victory");
    public void TriggerDefeat() => SetGameState("GameOver");

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToTitleScreen() => SetGameState("TitleScreen");
    public void GoToMainMenu() => SetGameState("MainMenu");
    public void GoToOptions() => SetGameState("Options");
    public void GoToCredits() => SetGameState("Credits");
    public void StartNewGame() => SetGameState("Gameplay");

    public void TargetDestroyed(Health targetHealth)
    {
        UnregisterObstacle(targetHealth);
    }

    public void TargetDestroyed()
    {
        CheckVictoryCondition();
    }

    // ==========================================
    // PERSISTENT PLAYER DATA MANAGEMENT SYSTEM
    // ==========================================

    public void LoadGameSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        int highscore = PlayerPrefs.GetInt("HighScore", 0);

        // Apply background track level immediately if instance channel is alive
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolume;
        }

        Debug.Log($"[Settings] Loaded Profiles -> Music Vol: {musicVolume}, SFX Vol: {sfxVolume}, Legacy Highscore: {highscore}");
    }

    // Connect this to your UI script handlers inside your Options Menu Layout
    public void UpdateAndSaveVolume(float newMusicVolume, float newSFXVolume)
    {
        PlayerPrefs.SetFloat("MusicVolume", newMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", newSFXVolume);

        // Write instantly to data registry storage frame
        PlayerPrefs.Save();

        if (musicAudioSource != null)
        {
            musicAudioSource.volume = newMusicVolume;
        }

        Debug.Log("[Settings] Profile files successfully modified on machine.");
    }
}