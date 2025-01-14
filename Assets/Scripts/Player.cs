using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    private CharacterController _cc;

    [SerializeField] float speed = 5f;
    [SerializeField] GameObject ballPrefab;

    private Camera firstPersonCamera;

    // Declare PlayerTag as a Networked property
    [Networked] public string PlayerTag { get; set; } // Assign a tag (e.g., "red" or "blu") to each player

    public override void Spawned()
    {
        _cc = GetComponent<CharacterController>();
        if (HasStateAuthority)
        {
            firstPersonCamera = Camera.main;
            var firstPersonCameraComponent = firstPersonCamera.GetComponent<FirstPersonCamera>();
            if (firstPersonCameraComponent && firstPersonCameraComponent.isActiveAndEnabled)
                firstPersonCameraComponent.SetTarget(this.transform);

            // Assign the player's team (replace with your own logic)
            PlayerTag = (Object.InputAuthority.PlayerId % 2 == 0) ? "blu" : "red";

            // Notify the network about the PlayerTag assignment
            SetPlayerTagRpc(PlayerTag);
        }
    }

    [Rpc]
    private void SetPlayerTagRpc(string newTag)
    {
        PlayerTag = newTag;
        Debug.Log($"Player {Object.InputAuthority.PlayerId} assigned tag '{PlayerTag}'");

        // Optionally, you can update other components or visuals based on the new tag here
    }

    private Vector3 moveDirection;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData inputData))
        {
            if (inputData.moveActionValue.magnitude > 0)
            {
                inputData.moveActionValue.Normalize(); // Ensure vector magnitude is 1 to prevent cheating
                moveDirection = new Vector3(inputData.moveActionValue.x, 0, inputData.moveActionValue.y);
                Vector3 DeltaX = speed * moveDirection * Runner.DeltaTime;
                _cc.Move(DeltaX);
            }

            if (HasStateAuthority)
            { // Only the server can spawn new objects
                if (inputData.shootActionValue)
                {
                    Debug.Log("SHOOT!");
                    Runner.Spawn(ballPrefab,
                        transform.position + moveDirection,
                        Quaternion.LookRotation(moveDirection),
                        Object.InputAuthority,
                        (runner, obj) => {
                            // Set the owner of the ball
                            Ball ball = obj.GetComponent<Ball>();
                            ball.Owner = this; // Set the owner to the current player instance
                        });
                }
            }
        }
    }
}
