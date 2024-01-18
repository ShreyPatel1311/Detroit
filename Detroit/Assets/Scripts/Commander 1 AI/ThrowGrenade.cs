using UnityEngine;

public class ThrowGrenade : CommState
{
    private Rigidbody rb;
    public override void EnterState(Commander1AI c1AI)
    {
        this.name = "throw Grenade";
    }

    public override void UpdateState(Commander1AI c1AI)
    {
        if (c1AI.GrenadeThrown)
        {
            GameObject grenade = Object.Instantiate(c1AI.GrenadePrefab, c1AI.HandRefrence.position, Quaternion.identity);
            rb = grenade.GetComponent<Rigidbody>();
            Vector3 dir = LaunchDirection(c1AI, grenade);
            rb.velocity = dir;
            c1AI.GrenadeThrown = false;
            Object.Destroy(grenade, 3.5f);
            c1AI.SwitchState(c1AI.teleport);
        }
    }

    public Vector3 LaunchDirection(Commander1AI c1AI, GameObject grenade)
    {
        float displacementY = c1AI.Player.transform.position.y - grenade.transform.position.y;
        Vector3 displacementXZ = new Vector3((c1AI.Player.transform.position - grenade.transform.position).x, 0, (c1AI.Player.transform.position - grenade.transform.position).z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * c1AI.Height);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * c1AI.Height / Physics.gravity.y) + Mathf.Sqrt(2 * displacementY - c1AI.Height / Physics.gravity.y));

        return velocityXZ + velocityY;
    }
}