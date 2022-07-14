using System;
using Interfaces;
using UnityEngine;
using Rewired;

public class Player : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    
    private Rewired.Player rewiredPlayer;

    private TimedEvent stunEvent, immuneEvent;
    
    private Vector2 moveDirection, mouseDirection;

    private int health;

    private bool isStun, isImmune, isFrozen;

    [SerializeField] private int playerId;
    [Space]
    [SerializeField] private float speed;
    [Space]
    [SerializeField] private float grabRange;
    [SerializeField] private LayerMask grabLayer;
    [Space]
    [SerializeField] private Transform weaponAnchor;
    [SerializeField] private Weapon weapon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        immuneEvent = new TimedEvent(1.5f, () => isImmune = false);
        stunEvent = new TimedEvent(0.5f, () => 
        { 
            isStun = false;
            animator.SetBool("Hit", false);
            immuneEvent.Start(); 
        });
        
        isStun = false;
        isImmune = false;
        
        SetPlayerID(playerId);
    }

    private void Update()
    {
        if (isFrozen)
            return;
        
        IterateTimedEvents();
        
        moveDirection.x = rewiredPlayer.GetAxisRaw("Move Horizontal");
        moveDirection.y = rewiredPlayer.GetAxisRaw("Move Vertical");
        mouseDirection = rewiredPlayer.GetAxis2DRaw("Ori Horizontal", "Ori Vertical");
        
        if (isStun) return;
        
        RotateSprite(mouseDirection);

        if (weapon != null)
        {
            weapon.Rotate(mouseDirection);

            if (rewiredPlayer.GetButtonDown("Drop"))
            {
                weapon.Drop();
                weapon = null;
            }
            else if (rewiredPlayer.GetButtonDown("Grab"))
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(weaponAnchor.position, grabRange, grabLayer);

                if (cols.Length > 0)
                {
                    weapon.Drop();
                    weapon = null;
                    cols[0].GetComponent<Weapon>().Grab(this); // Change to generic component "Item" or smth like that
                }
            }

            if (rewiredPlayer.GetButton("Attack"))
            {
                weapon.Attack();
            }
        }
        else if (rewiredPlayer.GetButtonDown("Grab"))
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(weaponAnchor.position, grabRange, grabLayer);

            if (cols.Length > 0)
            {
                cols[0].GetComponent<Weapon>().Grab(this); // Change to generic component "Item" or smth like that
            }
        }
    }

    private void FixedUpdate()
    {
        if (isFrozen)
            return;
        
        if (!isStun)
        {
            rb.velocity = moveDirection.normalized * speed;
        }

        animator.SetFloat("Speed", moveDirection.normalized.magnitude);
    }
    
    private void IterateTimedEvents()
    {
        stunEvent.UpdateTimer(Time.deltaTime);
        immuneEvent.UpdateTimer(Time.deltaTime);
    }

    private void RotateSprite(Vector2 mousePosition)
    {
        if (mousePosition.x < 0f && !sr.flipX)
        {
            sr.flipX = true;
        }
        else if (mousePosition.x > 0f && sr.flipX)
        {
            sr.flipX = false;
        }
    }

    public void SetPlayerWeapon(Weapon _weapon)
    {
        if (weapon != null)
        {
            weapon.Drop();
        }

        weapon = _weapon;
        weapon.transform.SetParent(weaponAnchor);
    }
    
    public void SetPlayerID(int _newId)
    {
        if (_newId < 0 || _newId >= ReInput.players.playerCount)
            return;
        
        playerId = _newId;
        rewiredPlayer = ReInput.players.GetPlayer(playerId);
        
        rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Menu");
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");
    }

    public void TakeDamage(int _damages)
    {
        if (isImmune)
            return;
        
        health -= _damages;
        
        animator.SetBool("Hit", true);
        
        isStun = true;
        isImmune = true;
        stunEvent.Start();
    }

    public void TakeDamage(int _damages, Vector2 _point, float _force)
    {
        if (isImmune)
            return;
        
        health -= _damages;
        
        rb.velocity = ((Vector2)weaponAnchor.position - _point).normalized * _force;
        animator.SetBool("Hit", true);

        isStun = true;
        isImmune = true;
        stunEvent.Start();
    }

    public void FreezeState(bool _isFrozen)
    {
        isFrozen = _isFrozen;

        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            
            animator.SetFloat("Speed", 0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (weaponAnchor != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(weaponAnchor.position, grabRange);
        }
    }
}
