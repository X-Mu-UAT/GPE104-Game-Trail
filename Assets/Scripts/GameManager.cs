using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed to restart the game

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject victoryPanel; // Drag your Victory UI panel here 
    public GameObject defeatPanel; // Drag your Defeat UI panel here 

    private List<Health> activeObstacles = new List<Health>();
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Hide UI panels at the start of the game 
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);
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

        // VICTORY CONDITION: Check if that was the last asteroid 
        if (activeObstacles.Count == 0 && !isGameOver)
        {
            TriggerVictory();
        }
    }

    public void TriggerVictory()
    {
        isGameOver = true;
        Debug.Log("VICTORY! All asteroids destroyed!");
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f; // Pauses the game mechanics 
    }

    public void TriggerDefeat()
    {
        isGameOver = true;
        Debug.Log("DEFEAT! You crashed!");
        if (defeatPanel != null) defeatPanel.SetActive(true);
        Time.timeScale = 0f; // Pauses the game mechanics 
    }

    // Optional: Call this from a "Restart" button on your UI panels 
    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause physics 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current level 
    }

    // ==========================================
    // ADDED COMPATIBILITY FUNCTIONS BELOW HERE
    // ==========================================

    // Use this version if the other script passes the Health component
    public void TargetDestroyed(Health targetHealth)
    {
        UnregisterObstacle(targetHealth);
    }

    // Use this version if the other script calls it without any parameters
    public void TargetDestroyed()
    {
        // Fallback victory check in case a script destroys an asteroid directly
        if (activeObstacles.Count == 0 && !isGameOver)
        {
            TriggerVictory();
        }
    }

    internal void TargetDestroyed(bool v)
    {
        throw new NotImplementedException();
    }
}