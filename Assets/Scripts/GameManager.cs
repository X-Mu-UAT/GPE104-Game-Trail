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
    private List<Health> activeObstacles = new List<Health>();
    private bool isGameOver = false;

   

    private void Start()
    {
        SetGameState("Gameplay");
    }

    // ADDED: Listens for Unity scene switches
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ADDED: Cleans up the listener if the manager is destroyed
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ADDED: Automatically runs the gameplay layout once the scene loads
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // CHANGE THIS: Replace "YourGameplaySceneName" with your exact scene file name
        if (scene.name == "Gameplay")
        {
            SetGameState("Gameplay");
        }
    }

    public void SetGameState(string stateName)
    {
        // 1. Safely disable all screens dynamically to avoid NullReferenceExceptions
        GameObject[] allStateObjects = new GameObject[] { TitleScreenStateObject, MainMenuStateObject, OptionsScreenStateObject, CreditsScreenStateObject, GameplayStateObject, GameOverScreenStateObject, VictoryScreenStateObject };
        foreach (GameObject stateObject in allStateObjects)
        {
            if (stateObject != null)
                stateObject.SetActive(false);
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
                // CRITICAL: Set timeScale to 1 FIRST so Unity physics and UI can process updates
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

        // 1. Find all asteroids physically placed in the scene
        GameObject[] existingAsteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        // 2. Loop through them and manually force them into the tracking list right now
        foreach (GameObject asteroid in existingAsteroids)
        {
            Health asteroidHealth = asteroid.GetComponent<Health>();
            if (asteroidHealth != null)
            {
                if (!activeObstacles.Contains(asteroidHealth))
                {
                    activeObstacles.Add(asteroidHealth);
                }
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

        // Safeguard: Only win if the game is active AND we actually had obstacles to clear
        if (activeObstacles.Count == 0 && !isGameOver)
        {
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

    // UPDATED: Clear out references before switching scenes
    public void StartNewGame()
    {
        Time.timeScale = 1f;

        // Disconnect the Menu references so GameManager stops trying to control them
        TitleScreenStateObject = null;
        MainMenuStateObject = null;
        OptionsScreenStateObject = null;
        CreditsScreenStateObject = null;

        // Load the new scene
        SceneManager.LoadScene("Gameplay");
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
