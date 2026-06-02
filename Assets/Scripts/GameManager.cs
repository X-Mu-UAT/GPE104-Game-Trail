using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; // Added back to reference the Screen Space Score Text element

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
    [SerializeField] private TextMeshProUGUI scoreText; // Drag your Screen Space Score Text here
    private int currentScore = 0;

    private List<Health> activeObstacles = new List<Health>();
    private bool isGameOver = false;
    private GameObject[] allStateObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        allStateObjects = new GameObject[]
        {
            TitleScreenStateObject,
            MainMenuStateObject,
            OptionsScreenStateObject,
            CreditsScreenStateObject,
            GameplayStateObject,
            GameOverScreenStateObject,
            VictoryScreenStateObject
        };
    }

    private void Start()
    {
        SetGameState("TitleScreen");
    }

    public void SetGameState(string stateName)
    {
        foreach (GameObject stateObject in allStateObjects)
        {
            if (stateObject != null) stateObject.SetActive(false);
        }

        switch (stateName)
        {
            case "TitleScreen":
                TitleScreenStateObject.SetActive(true);
                Time.timeScale = 0f; 
                isGameOver = true;
                break;

            case "MainMenu":
                MainMenuStateObject.SetActive(true);
                Time.timeScale = 0f;
                isGameOver = true;
                break;

            case "Options":
                OptionsScreenStateObject.SetActive(true);
                Time.timeScale = 0f;
                break;

            case "Credits":
                CreditsScreenStateObject.SetActive(true);
                Time.timeScale = 0f;
                break;

            case "Gameplay":
                GameplayStateObject.SetActive(true);
                Time.timeScale = 1f; 
                isGameOver = false;
                
                // Reset Score System when starting gameplay loop
                currentScore = 0;
                UpdateScoreUI();

                ClearOldObstaclesOnly(); 
                break;

            case "GameOver":
                GameOverScreenStateObject.SetActive(true);
                Time.timeScale = 0f; 
                isGameOver = true;
                break;

            case "Victory":
                VictoryScreenStateObject.SetActive(true);
                Time.timeScale = 0f; 
                isGameOver = true;
                break;
        }
    }

    // --- REQUIREMENT 2: CORE SCORE LOGIC ---
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
        GameObject[] oldAsteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject asteroid in oldAsteroids)
        {
            Destroy(asteroid);
        }
    }

    // ==========================================
    // ASTEROID & DAMAGE TRACKING LOGIC
    // ==========================================

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
        if (activeObstacles.Count == 0 && !isGameOver) TriggerVictory();
    }
}
