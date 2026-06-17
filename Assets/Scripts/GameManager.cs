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
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private int startingLives = 3;
    private int currentScore = 0;
    private int currentLives;

    [Header("Win Condition Timer")]
    public float winTime = 60f;
    private float timer = 0f;
    private void Update()
    {
        if (isGameOver) return; // stops timer when game ends

        timer += Time.deltaTime;

        if (timer >= winTime)
        {
            TriggerVictory();
        }
    }

    [Header("Game Boundary Limits (Screen Warping Requirement)")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -6f;
    [SerializeField] private float maxY = 6f;

    [Header("Audio Configuration (Exposed to Designers)")]
    public AudioClip backgroundMusicClip;
    public AudioClip playerShootClip;
    public AudioClip targetTakeDamageClip;
    public AudioClip targetDeathClip;

    private AudioSource musicAudioSource;
    private List<Enemy> activeObstacles = new List<Enemy>();
    private bool isGameOver = false;

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

        if (VictoryScreenStateObject!= null)VictoryScreenStateObject.SetActive(false);
        if (GameOverScreenStateObject!= null)GameOverScreenStateObject.SetActive(false);
        if (GameplayStateObject!= null)GameplayStateObject.SetActive(false);

        SetGameState("MainMenu");
    }

    private void SetupBackgroundMusic()
    {
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
            AudioSource.PlayClipAtPoint(clip, position, PlayerPrefs.GetFloat("SFXVolume", 0.8f));
        }
    }

    public void SetGameState(string stateName)
    {
        Debug.Log($"[GameManager] Switching state to: {stateName}");

        GameObject[] allStateObjects = new GameObject[]
        {
            TitleScreenStateObject,
            MainMenuStateObject,
            OptionsScreenStateObject,
            CreditsScreenStateObject,
            GameplayStateObject,
            GameOverScreenStateObject,
            VictoryScreenStateObject
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

                if (GameplayStateObject != null) GameplayStateObject.SetActive(false);

                if (GameOverScreenStateObject != null) GameOverScreenStateObject.SetActive(false);

                if (CreditsScreenStateObject != null) CreditsScreenStateObject.SetActive(true);

                Time.timeScale = 0f;

                break;

            case "Gameplay":
                if (GameplayStateObject != null)
                    GameplayStateObject.SetActive(true);

                Time.timeScale = 1f;

                // Ensure gameplay state is fully initialized
                isGameOver = false;
                currentScore = 0;
                currentLives = startingLives;
                timer = 0f;

                GameObject playerShip = GameObject.FindWithTag("Player");
                if (playerShip != null)
                {
                    playerShip.transform.position = Vector3.zero;
                    playerShip.transform.rotation = Quaternion.identity;
                }

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

        if (currentLives <= 0)
        {
            TriggerDefeat();
        }
    }
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"SCORE: {currentScore:D5}";
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = $"LIVES: {currentLives}";
    }

    private void ClearOldObstaclesOnly()
    {
        activeObstacles.Clear();

        Enemy[] existingEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in existingEnemies)
        {
            if (!activeObstacles.Contains(enemy))
                activeObstacles.Add(enemy);
        }
    }

    public void RegisterObstacle(Enemy obstacle)
    {
        if (!activeObstacles.Contains(obstacle))
            activeObstacles.Add(obstacle);
    }

    public void UnregisterObstacle(Enemy obstacle)
    {
        if (activeObstacles.Contains(obstacle))
            activeObstacles.Remove(obstacle);
    }

    public void TriggerVictory() => SetGameState("Victory");
    public void TriggerDefeat() => SetGameState("GameOver");

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu() => SetGameState("MainMenu");
    public void GoToOptions() => SetGameState("Options");
    public void GoToCredits() => SetGameState("Credits");
    public void StartNewGame() => SetGameState("Gameplay");

    public void QuitToDesktop()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }

    public void LoadGameSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        if (musicAudioSource != null)
            musicAudioSource.volume = musicVolume;
    }

    public void UpdateAndSaveVolume(float newMusicVolume, float newSFXVolume)
    {
        PlayerPrefs.SetFloat("MusicVolume", newMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", newSFXVolume);
        PlayerPrefs.Save();

        if (musicAudioSource != null)
            musicAudioSource.volume = newMusicVolume;
    }
}