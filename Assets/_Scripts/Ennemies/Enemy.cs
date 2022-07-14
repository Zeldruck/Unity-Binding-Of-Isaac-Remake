using System.Collections;
using Interfaces;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    protected Animator animator;
    
    protected Player[] players;
    protected int indexPlayerTarget;

    protected int health;
    
    [SerializeField] protected float timeSwapTarget;
    [Space]
    [SerializeField] protected float speed;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        
        // TEMP
        players = FindObjectsOfType<Player>();
    }

    protected virtual void Start()
    {
        StartCoroutine(SwapTarget());
    }

    private IEnumerator SwapTarget()
    {
        WaitForSeconds wfs = new WaitForSeconds(timeSwapTarget);
        
        while (true)
        {
            SearchCloserTarget();
            
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
            Destroy(gameObject);
    }

    public virtual void TakeDamage(int _damages, Vector2 _point, float _force)
    {
        health -= _damages;
        
        // Push
        
        if (health <= 0)
            Destroy(gameObject);
    }
}
