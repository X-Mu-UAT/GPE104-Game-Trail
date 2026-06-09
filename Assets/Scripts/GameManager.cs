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

    [Header("Score & Lives Systems (Requirement 2 & UI Lives)")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText; // Added to fulfill lives requirements
    [SerializeField] private int startingLives = 3;
    private int currentScore = 0;
    private int currentLives;

    [Header("Game Boundary Limits (Screen Warping Requirement)")]
    [Tooltip("Exposed values for designers to determine space warp looping bounds.")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -6f;
    [SerializeField] private float maxY = 6f;

    [Header("Audio Configuration (Exposed to Designers)")]
    public AudioClip backgroundMusicClip;
    public AudioClip playerShootClip;
    public AudioClip targetTakeDamageClip;
    public AudioClip targetDeathClip; // Explicitly added to cover required audio hooks

    private AudioSource musicAudioSource;
    // Changed to track a base Enemy script instead of generic Health components for cleaner polymorphic lookups
    private List<Enemy> activeObstacles = new List<Enemy>(); 
    private bool isGameOver = false;

    // Direct read-only properties for external pawn/movement tracking
    public float MinX => minX;
    public float MaxX => maxX;
    public float MinY => minY;
    public float MaxY => maxY;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetupBackgroundMusic();
            // Don't destroy on load keeps the manager alive across scene flashes if necessary
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadGameSettings();
        SetGameState("MainMenu");
    }

    private void SetupBackgroundMusic()
    {
        // Guard check to ensure multiple AudioSources aren't generated on persistent re-entries
        musicAudioSource = GetComponent<AudioSource>();
        if (musicAudioSource == null)
        {
            musicAudioSource = gameObject.AddComponent<AudioSource>();
        }
        
        musicAudioSource.clip = backgroundMusicClip;
        musicAudioSource.loop = true;
        musicAudioSource.playOnAwake = true;
        
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
            // PlayClipAtPoint naturally handles 3D world panning for spatial effects
            AudioSource.PlayClipAtPoint(clip, position, PlayerPrefs.GetFloat("SFXVolume", 0.8f));
        }
    }

    public void SetGameState(string stateName)
    {
        Debug.Log($"[GameManager] Switching state to: {stateName}");
        GameObject[] allStateObjects = new GameObject[] { TitleScreenStateObject, MainMenuStateObject, OptionsScreenStateObject, CreditsScreenStateObject, GameplayStateObject, GameOverScreenStateObject, VictoryScreenStateObject };
        
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
                currentLives = startingLives; // Initialize lives on game startup
                UpdateScoreUI();
                UpdateLivesUI();
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

    // ========================================== //
    //          SCREEN WARPING VECTOR MATH        //
    // ========================================== //
    public Vector3 WrapPosition(Vector3 currentPosition)
    {
        Vector3 newPosition = currentPosition;

        if (currentPosition.x > maxX) newPosition.x = minX;
        else if (currentPosition.x < minX) newPosition.x = maxX;

        if (currentPosition.y > maxY) newPosition.y = minY;
        else if (currentPosition.y < minY) newPosition.y = maxY;

        return newPosition;
    }

    public Vector3 GetRandomBoundaryPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector3(randomX, randomY, 0f);
    }

    // ========================================== //
    //             CORE GAMEPLAY HOOKS            //
    // ========================================== //
    public void AddPoints(int pointsToGive)
    {
        if (isGameOver) return;
        currentScore += pointsToGive;
        UpdateScoreUI();
    }

    public void LoseLife()
    {
        if (isGameOver) return;
        
        currentLives--;
        UpdateLivesUI();
        PlaySoundEffect(targetTakeDamageClip, Vector3.zero); // Plays local feedback tracking

        if (currentLives <= 0)
        {
            TriggerDefeat();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {currentScore:D5}";
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = $"LIVES: {currentLives}";
        }
    }

    private void ClearOldObstaclesOnly()
    {
        activeObstacles.Clear();
        // Dynamically gathers starting run elements safely
        Enemy[] existingEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in existingEnemies)
        {
            if (!activeObstacles.Contains(enemy))
            {
                activeObstacles.Add(enemy);
            }
        }
        Debug.Log($"[GameManager] Gameplay started! Tracking {activeObstacles.Count} total core enemies.");
        CheckVictoryCondition();
    }

    public void RegisterObstacle(Enemy obstacle)
    {
        if (!activeObstacles.Contains(obstacle))
        {
            activeObstacles.Add(obstacle);
            Debug.Log($"[GameManager] Registered entity tracking. Total remaining: {activeObstacles.Count}");
        }
    }

    public void UnregisterObstacle(Enemy obstacle)
    {
        if (activeObstacles.Contains(obstacle))
        {
            activeObstacles.Remove(obstacle);
            Debug.Log($"[GameManager] Removed tracking point. Remaining count: {activeObstacles.Count}");
        }
        CheckVictoryCondition();
    }

    private void CheckVictoryCondition()
    {
        // Delayed check verification to ensure spawned split-fragments are accounted for first
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
        // Clean destruction setup to reload singletons natively
        if (gameObject != null) Destroy(gameObject); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToTitleScreen() => SetGameState("TitleScreen");
    public void GoToMainMenu() => SetGameState("MainMenu");
    public void GoToOptions() => SetGameState("Options");
    public void GoToCredits() => SetGameState("Credits");
    public void StartNewGame() => SetGameState("Gameplay");

    public void QuitToDesktop()
    {
        Debug.Log("[GameManager] Quitting application to desktop environment.");
        Application.Quit();
    }

    // ========================================== //
    // PERSISTENT PLAYER DATA MANAGEMENT SYSTEM //
    // ========================================== //
public void LoadGameSettings()
{
float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.1f);
float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
int highscore = PlayerPrefs.GetInt("HighScore", 0);

if (musicAudioSource != null)
{
musicAudioSource.volume = musicVolume;
}
Debug.Log($"[Settings] Loaded Profiles -> Music Vol:
{musicVolume}, SFX Vol: {sfxVolume}, Legacy Highscore:
{highscore}");
}
public void UpdateAndSaveVolume(float newMusicVolume, float newSFXVolume)
{
PlayerPrefs.SetFloat("MusicVolume", newMusicVolume);
PlayerPrefs.SetFloat("SFXVolume", newSFXVolume);
PlayerPrefs.Save();
if (musicAudioSource != null)
{musicAudioSource.volume = newMusicVolume;
}
Debug.Log("[Settings] Profile files successfully modified on machine.");}}
