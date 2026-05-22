using UnityEngine;

public class RandomSpritePlacer : MonoBehaviour
{
    // Define the minimum and maximum x and y boundries in the inspector
    public float minX = 5f;
    public float maxX = 5f;
    public float minY = -3f;
    public float maxY = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            // Generate a random x and y position within the specified bounds
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            // Create a new Vector3 with the random x and y positions (z is set to 0)
            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
            // Set the position of the GameObject this script is attached to the random position
            transform.position = randomPosition;
        }
    }
}
