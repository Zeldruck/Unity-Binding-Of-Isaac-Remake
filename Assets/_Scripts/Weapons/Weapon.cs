using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Collider2D col;
    
    protected Player player;

    protected TimedEvent attackEvent;

    protected Vector2 direction;

    protected bool isDropped;
    protected bool canAttack;

    [SerializeField] protected int orderLayerDropped;
    [SerializeField] protected int orderLayerEquipped;
    [Space]
    [SerializeField] protected float weaponOffset;
    [Space]
    [SerializeField] protected LayerMask attackLayerMask;
    [Space]
    [SerializeField] protected float attackRate;
    [SerializeField] protected int damages;
    [SerializeField] protected float speed;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        attackEvent = new TimedEvent(1f / attackRate, () => canAttack = true);
        
        isDropped = true;
        canAttack = true;
    }

    private void Update()
    {
        IterateTimedEvents();
    }

    private void IterateTimedEvents()
    {
        attackEvent.UpdateTimer(Time.deltaTime);
    }
    
    public virtual void Rotate(Vector2 _mouseDirection)
    {
        if (Mathf.Abs(_mouseDirection.x + _mouseDirection.y) > 0.15f)
        {
            transform.localPosition = _mouseDirection.normalized * weaponOffset;
            direction = _mouseDirection.normalized;
            
            Vector3 dirAngle = (transform.position - transform.parent.position).normalized * weaponOffset;
            float angle = Mathf.Atan2(dirAngle.y, dirAngle.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            if (Mathf.Sign(transform.localScale.y) >= 0f && transform.rotation.eulerAngles.z >= 90f && transform.rotation.eulerAngles.z <= 270f)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
            else if (Mathf.Sign(transform.localScale.y) < 0f && (transform.rotation.eulerAngles.z < 90f || transform.rotation.eulerAngles.z > 270f))
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
    }
    
    public virtual void Attack()
    {
        if (isDropped || !canAttack)
            return;
        
        Debug.Log("Attack");
    }

    public void Drop()
    {
        if (isDropped)
            return;

        player = null;
        isDropped = true;
        
        sr.sortingOrder = orderLayerDropped;
        
        transform.SetParent(null);
        col.enabled = true;
    }

    public void Grab(Player _player)
    {
        if (player != null || !isDropped)
            return;

        player = _player;
        player.SetPlayerWeapon(this);
        isDropped = false;

        sr.sortingOrder = orderLayerEquipped;
        col.enabled = false;
    }
}
