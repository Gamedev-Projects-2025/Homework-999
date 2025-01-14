using UnityEngine;

public class DestroySelfAndOther : MonoBehaviour
{
    public string targetTag = "TargetTag";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag(targetTag))
        {
            Debug.Log("HIT");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
