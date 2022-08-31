using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger,
    dead,

}
public class Enemy : MonoBehaviour
{
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    
    
    private void Awake()
    {
        health = maxHealth.initialValue;
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            this.GetComponent<Slime>().Death();
        }
    }
    
    public void Knock(Rigidbody2D rb, float knockDuration, float damage)
    {
        StartCoroutine(KnockCo(rb, knockDuration));
        TakeDamage(damage);
    }
    
    private IEnumerator KnockCo(Rigidbody2D rb, float knockDuration)
    {
        if(rb != null)
        {
            yield return new WaitForSeconds(knockDuration);
            rb.velocity = Vector2.zero;
            currentState = EnemyState.walk;
            rb.velocity = Vector2.zero;
        }
    }
}
