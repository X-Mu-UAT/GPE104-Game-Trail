using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;

    public float spawnTime = 4f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            int currentMeteors = GameObject.FindGameObjectsWithTag("Asteroid").Length;

            if (currentMeteors < 5)
            {
                Instantiate(meteorPrefab, transform.position, transform.rotation);
            }

            timer = 0f;
        }
    }
}