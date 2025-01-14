using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    UnityEvent<int, Vector2> GotHit;

    public bool isAlive {get; private set;}

    private bool isInvicible = false;
    private float timeSinceHit = 0;
    public float invicibilityTime = 1f;

    public int maxHealth;

    [SerializeField]
    private int _health;

    public int Health
    {
        get{
            return _health;
        }
        set{
            _health = value;

            if(_health <= 0)
            {
                isAlive = false;
                _health = 0;
            }
        }
    }

    private void Start()
    {
        isAlive = true;
    }

    private void Update() {
        if(isInvicible)
        {
            if(timeSinceHit > invicibilityTime)
            {
                isInvicible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }

    public void Hit(int damage, Vector2 knockBack)
    {
        if(isInvicible)
        {
            return;
        }

        isInvicible = true;

        Health -= damage;

        timeSinceHit = 0;

        GotHit?.Invoke(damage, knockBack);
    }
}
