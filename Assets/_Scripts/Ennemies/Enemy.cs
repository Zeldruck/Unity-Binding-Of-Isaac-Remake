using System;
using System.Collections;
using Interfaces;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    protected Animator animator;
    
    protected Player[] players;
    protected int indexPlayerTarget;

    protected int health;

    public delegate void OnDead();
    private OnDead onDead;
    
    [SerializeField] protected float timeSwapTarget;
    [Space]
    [SerializeField] private int startHealth;
    [SerializeField] protected float speed;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        health = startHealth;
    }

    protected virtual void Start()
    {
        players = Array.Empty<Player>();
    }

    public virtual void Initialize(OnDead _onDead)
    {
        if (_onDead == null)
            return;
        
        onDead = _onDead;
    }

    private IEnumerator SwapTarget()
    {
        WaitForSeconds wfs = new WaitForSeconds(timeSwapTarget);
        
        while (true)
        {
            if (players == null || players.Length == 0)
                players = FindObjectsOfType<Player>();
            
            SearchCloserTarget();
            
            Debug.Log("There looping");
            
            yield return wfs;
        }
    }

    protected void SearchCloserTarget()
    {
        int indexCloser = -1;
        float distanceCloser = -1;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                float distance = Vector2.Distance(transform.position, players[i].transform.position);

                if (indexCloser < 0 || distance < distanceCloser)
                {
                    indexCloser = i;
                    distanceCloser = distance;
                }
            }
        }

        if (indexCloser >= 0)
        {
            indexPlayerTarget = indexCloser;
        }
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public virtual void TakeDamage(int _damages)
    {
        health -= _damages;

        if (health <= 0)
        {
            onDead?.Invoke();
            Destroy(gameObject);
        }
    }

    public virtual void TakeDamage(int _damages, Vector2 _point, float _force)
    {
        health -= _damages;
        
        // Push

        if (health <= 0)
        {
            onDead?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SwapTarget());
    }
}
