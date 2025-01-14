using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private NumberField HealthDisplay;
    [SerializeField] private float invulnerabilityDuration = 2f; // Duration of invulnerability in seconds
    [SerializeField] private MeshRenderer meshRenderer; // Reference to the MeshRenderer

    [Networked]
    private Color NetworkedColor { get; set; }

    [Networked]
    public int NetworkedHealth { get; set; } = 100;

    private bool isInvulnerable = false;
    private float invulnerabilityEndTime;
    private Color originalColor;

    public override void Spawned()
    {
        if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
            NetworkedColor = originalColor; // Initialize the networked color
        }

        HealthDisplay.SetNumber(NetworkedHealth);
    }

    public override void Render()
    {
        // Synchronize the material color for all clients
        if (meshRenderer != null && meshRenderer.material.color != NetworkedColor)
        {
            meshRenderer.material.color = NetworkedColor;
        }

        // Update the health display
        HealthDisplay.SetNumber(NetworkedHealth);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(int damage)
    {
        if (isInvulnerable)
        {
            Debug.Log("Damage ignored due to invulnerability.");
            return;
        }
        Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
        NetworkedHealth -= damage;

        // Trigger invulnerability frames
        isInvulnerable = true;
        invulnerabilityEndTime = Time.time + invulnerabilityDuration;

        // Change color to white
        NetworkedColor = Color.white;
    }

    private void Update()
    {
        if (isInvulnerable && Time.time >= invulnerabilityEndTime)
        {
            isInvulnerable = false;
            Debug.Log("Invulnerability ended.");

            // Revert color to original
            NetworkedColor = originalColor;
        }
    }
}
