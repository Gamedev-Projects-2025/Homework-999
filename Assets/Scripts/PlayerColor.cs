using Fusion;
using UnityEngine;

public class PlayerColor : NetworkBehaviour
{
    [Networked] public string PlayerTag { get; set; } // Networked tag (e.g., "blu" or "red")
    [Networked] public Color NetworkedColor { get; set; } // Networked color

    private MeshRenderer meshRendererToChange;

    public override void Spawned()
    {
        meshRendererToChange = GetComponentInChildren<MeshRenderer>();

        // Assign the tag and color for the local player with State Authority
        if (HasStateAuthority)
        {
            AssignPlayerTagAndColor(Object.InputAuthority);
        }

        // Apply the color to the player's mesh
        meshRendererToChange.material.color = NetworkedColor;
    }

    public override void Render()
    {
        // Update the player's color if it changes
        meshRendererToChange.material.color = NetworkedColor;
    }

    /// <summary>
    /// Assigns a tag and corresponding color to the player based on their PlayerRef.
    /// </summary>
    private void AssignPlayerTagAndColor(PlayerRef player)
    {
        // Assign tags dynamically (replace with your own logic)
        if (player.PlayerId % 2 == 0)
        {
            PlayerTag = "blu";
            NetworkedColor = Color.blue;
        }
        else
        {
            PlayerTag = "red";
            NetworkedColor = Color.red;
        }

        Debug.Log($"Player {player.PlayerId} assigned tag '{PlayerTag}' with color {NetworkedColor}");
    }
}
