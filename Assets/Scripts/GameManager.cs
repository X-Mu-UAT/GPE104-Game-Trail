using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // tThe static reference that makes it a singleton
    public static GameManager Instance {get; private set;}

    [Header(”Game State”)]
    public List<Health> obstacles = new List<Health>();
    peivate bool isGameOver = false

void Awake()
    {
        if (Instance == null)
    {
            Instance = this;
            DontDestroyOnLoad(GameObject);
        }
        else
        {
            Destroy(GameObject); // To get rid of duplicates
        }
    }
    public void RegisterObstacle(Health obstacle)
  {
        if (!obstacles.Contains(obstacle))
        {
            obstacles.Add(obstacle);
        }
    }

    // Called when an obstacle dies
    public void UnregisterObstacle(Health obstacle)
    {
        if (obstacles.Contains(obstacle))
        {
            obstacles.Remove(obstacle);
        }

        // Check for Victory condition
        if (obstacles.Count == 0 && !isGameOver)
        {
            Victory();
        }
    }

    public void TargetDestroyed(bool isPlayer)
    {
        if (isGameOver) return;

        if (isPlayer)
        {
            isGameOver = true;
            Debug.Log("Failure");
        }
    }

    private void Victory()
    {
        isGameOver = true;
        Debug.Log("Victory");
    }
}
