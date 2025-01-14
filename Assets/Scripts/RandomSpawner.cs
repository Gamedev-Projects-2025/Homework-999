using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;
    public Vector3 spawnAreaSize;
    public float spawnInterval = 3f;

    private void Start()
    {
        InvokeRepeating("SpawnPickup", 0f, spawnInterval);
    }

    private void SpawnPickup()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            spawnAreaSize.y,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );
        Instantiate(pickupPrefab, transform.position + randomPosition, Quaternion.identity);
    }
}