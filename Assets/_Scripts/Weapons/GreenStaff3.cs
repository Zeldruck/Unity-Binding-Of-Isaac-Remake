using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenStaff3 : GreenStaff
{
    [SerializeField] protected int nbBullet;
    [SerializeField] protected float shootRadius;
    
    public override void Attack()
    {
        if (isDropped || !canAttack)
            return;

        //TODO
        
        GameObject pObject = Instantiate(projectilePrefab, shootAnchor.position, Quaternion.identity);
        pObject.GetComponent<Projectile>()?.Initialization(damages, projectilePushForce, attackLayerMask, direction, speed, player.gameObject);
        
        canAttack = false;
        attackEvent.Start();
    }
}
