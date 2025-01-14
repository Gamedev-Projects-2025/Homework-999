using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            FollowGameObject followScript = spawnedObject.GetComponent<FollowGameObject>();
            if (followScript != null)
            {
                followScript.target = other.transform;
                
            }
            Destroy(gameObject);
        }
    }
}