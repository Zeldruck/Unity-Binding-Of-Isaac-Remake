using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
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

        GameObject pObject = Instantiate(projectilePrefab, shootAnchor);
        pObject.transform.SetParent(null);
        //pObject.transform.Rotate(Vector3.forward, -90f);
        pObject.GetComponent<Projectile>()?.Initialization(damages, projectilePushForce, attackLayerMask, direction, speed, player.gameObject);
        
        canAttack = false;
        attackEvent.Start();
    }
}