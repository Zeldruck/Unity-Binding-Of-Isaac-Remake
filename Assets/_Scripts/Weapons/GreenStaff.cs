using UnityEngine;

public class GreenStaff : Weapon
{
    [SerializeField] protected Transform shootAnchor;
    [SerializeField] protected GameObject projectilePrefab;
    [Space]
    [SerializeField] protected float projectilePushForce;
    
    public override void Attack()
    {
        //base.Attack(); // Juste un Debug.Log
        
        if (isDropped || !canAttack)
            return;

        GameObject pObject = Instantiate(projectilePrefab, shootAnchor.position, Quaternion.identity);
        pObject.GetComponent<Projectile>()?.Initialization(damages, projectilePushForce, attackLayerMask, direction, speed, player.gameObject);
        
        canAttack = false;
        attackEvent.Start();
    }
}
