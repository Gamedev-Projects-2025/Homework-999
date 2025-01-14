using Fusion;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    [Networked] private TickTimer lifeTimer { get; set; }
    public Player Owner { get; set; } // The owner of the ball

    [SerializeField] float lifeTime = 5.0f;
    [SerializeField] float speed = 5.0f;
    [SerializeField] int damagePerHit = 1;

    public override void Spawned()
    {
        lifeTimer = TickTimer.CreateFromSeconds(Runner, lifeTime);
    }

    public override void FixedUpdateNetwork()
    {
        if (lifeTimer.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
        else
        {
            transform.position += speed * transform.forward * Runner.DeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Prevent the ball from damaging objects other than players
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (Owner != null && !IsFriendlyFire(Owner, player))
            {
                Health health = other.GetComponent<Health>();
                if (health != null)
                {
                    health.DealDamageRpc(damagePerHit);
                    Runner.Despawn(Object); // Despawn ball after hit
                }
            }
        }
    }

    private bool IsFriendlyFire(Player ownerPlayer, Player hitPlayer)
    {
        // Compare tags or team assignments
        Debug.Log(ownerPlayer.PlayerTag+" vs "+hitPlayer.PlayerTag);
        return ownerPlayer.PlayerTag == hitPlayer.PlayerTag; // Replace with your team logic
    }
}
