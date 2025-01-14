using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (target != null)
        {
            transform.SetParent(target);
        }
    }
}