using System;
using Interfaces;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;

    private TimedEvent destroyEvent;

    private GameObject objectToExclude;
    
    private LayerMask layer;

    private int damages;
    private float projectilePushForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        destroyEvent = new TimedEvent(3f, () => Destroy(gameObject));
    }

    private void Update()
    {
        destroyEvent.UpdateTimer(Time.deltaTime);
    }

    public void Initialization(int _damages, float _pushForce, LayerMask _layerMask, Vector2 _direction, float speed, GameObject _objectToExclude = null)
    {
        damages = _damages;
        projectilePushForce = _pushForce;
        layer = _layerMask;

        rb.velocity = _direction * speed;

        objectToExclude = _objectToExclude;
        
        destroyEvent.Start();
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == objectToExclude)
            return;
        
        if (layer == (layer | 1 << other.gameObject.layer))
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(damages, transform.position, projectilePushForce);
            
            Destroy(gameObject);
        }
    }
}
