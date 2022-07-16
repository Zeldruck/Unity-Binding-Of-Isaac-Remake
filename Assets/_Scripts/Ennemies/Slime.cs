using System;
using System.Collections.Generic;
using Interfaces;
using MoreMountains.Feedbacks;
using UnityEngine;

public class Slime : Enemy
{
    private TimedEvent attackTimer, stunTimer;

    private Vector2 direction;
    
    private bool canAttack = true;
    private bool canMove = true;
    
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float attackRange;
    [SerializeField] private int damage;
    [Space] [SerializeField] private MMFeedbacks hitFeedbacks;

    protected override void Start()
    {
        base.Start();

        attackTimer = new TimedEvent(1.5f, () => { canAttack = true; SearchCloserTarget(); });
        stunTimer = new TimedEvent(0.5f, () => { canMove = true; });
    }

    private void Update()
    {
        IterateTimedEvents();
    }

    private void FixedUpdate()
    {
        // Move
        if (canMove && indexPlayerTarget < players.Length && players[indexPlayerTarget] != null)
        {
            direction = (players[indexPlayerTarget].transform.position - transform.position).normalized;
        }
        else if (players.Length > 0)
        {
            direction = Vector2.zero;
            SearchCloserTarget();
        }
        else
        {
            direction = Vector2.zero;
        }
        
        transform.position += (Vector3)(direction * speed * Time.fixedDeltaTime);
        
        // Attack
        if (canAttack)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, attackRange, playerMask);

            bool hasAttacked = cols.Length > 0;
            
            foreach (Collider2D col in cols)
            {
                IDamageable damageable = col.GetComponent<IDamageable>();
                damageable.TakeDamage(damage, transform.position, 2f);
            }

            if (hasAttacked)
            {
                attackTimer.Start();
                canAttack = false;
                
                stunTimer.ChangeTime(1.5f);
                stunTimer.Start();
                canMove = false;
            }
        }

        animator.SetFloat("Speed", canMove ? direction.magnitude : 0f);
    }

    private void IterateTimedEvents()
    {
        attackTimer.UpdateTimer(Time.deltaTime);
        stunTimer.UpdateTimer(Time.deltaTime);
    }

    public override void TakeDamage(int _damages)
    {
        base.TakeDamage(_damages);
        
        hitFeedbacks?.PlayFeedbacks();
    }

    public override void TakeDamage(int _damages, Vector2 _point, float _force)
    {
        base.TakeDamage(_damages, _point, _force);
        
        hitFeedbacks?.PlayFeedbacks();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
