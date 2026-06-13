using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    public GameObject UFOPrefab;
    public float spawnTime = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            int currentUFOs = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (currentUFOs < 5)
            {
                Instantiate(UFOPrefab, transform.position, transform.rotation);
            }

            timer = 0f;
        }
    }
}