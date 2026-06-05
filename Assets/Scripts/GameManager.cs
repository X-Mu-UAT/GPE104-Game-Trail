using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
            SetupBackgroundMusic(); // Configures background audio automatically
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Launches straight into the Main Menu layout inside your scene
        SetGameState("MainMenu");
    }

    // Builds a loop audio channel for background tracking
    private void SetupBackgroundMusic()
    {
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.clip = backgroundMusicClip;
        musicAudioSource.loop = true;
        musicAudioSource.playOnAwake = true;
        musicAudioSource.volume = 0.1f; // Safe background level

        // Tells it to bypass standard pause loops if needed
        musicAudioSource.ignoreListenerPause = true;

        if (backgroundMusicClip != null)
        {
            musicAudioSource.Play();
        }
    }

    // Universal helper so any weapon or damage zone can trigger a clip instantly
    public void PlaySoundEffect(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }

    public void SetGameState(string stateName)
    {
        // 1. Safely disable all screens dynamically to avoid NullReferenceExceptions
        GameObject[] allStateObjects = new GameObject[]
        {
            TitleScreenStateObject, MainMenuStateObject, OptionsScreenStateObject,
            CreditsScreenStateObject, GameplayStateObject, GameOverScreenStateObject, VictoryScreenStateObject
        };

        foreach (GameObject stateObject in allStateObjects)
        {
            if (stateObject != null) stateObject.SetActive(false);
        }

        // 2. Handle the new state setup
        switch (stateName)
        {
            case "TitleScreen":
                if (TitleScreenStateObject != null) TitleScreenStateObject.SetActive(true);
                Time.timeScale = 0f;
                isGameOver = true;
                break;
            case "MainMenu":
                if (MainMenuStateObject != null) MainMenuStateObject.SetActive(true);
                Time.timeScale = 0f; // Pauses gameplay physics behind the menu
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
                // Unpauses Unity physics and updates the gameplay HUD
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
        Debug.Log($"Gameplay started! Tracking {activeObstacles.Count} hand-placed asteroids.");
    }

    public void RegisterObstacle(Health obstacle)
    {
        if (!activeObstacles.Contains(obstacle))
        {
            activeObstacles.Add(obstacle);
        }
    }

    public void UnregisterObstacle(Health obstacle)
    {
        if (activeObstacles.Contains(obstacle))
        {
            activeObstacles.Remove(obstacle);
        }

        if (activeObstacles.Count == 0 && !isGameOver)
        {
            TriggerVictory();
        }
    }

    public void TriggerVictory() => SetGameState("Victory");
    public void TriggerDefeat() => SetGameState("GameOver");

    // FIXED: Instead of loading an asset name, it just reloads your current active scene layout
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToTitleScreen() => SetGameState("TitleScreen");
    public void GoToMainMenu() => SetGameState("MainMenu");
    public void GoToOptions() => SetGameState("Options");
    public void GoToCredits() => SetGameState("Credits");

    // FIXED: Changes state entirely within the single scene frame layout
    public void StartNewGame()
    {
        SetGameState("Gameplay");
    }

    public void TargetDestroyed(Health targetHealth)
    {
        UnregisterObstacle(targetHealth);
    }

    public void TargetDestroyed()
    {
        if (activeObstacles.Count == 0 && !isGameOver) TriggerVictory();
    }
}