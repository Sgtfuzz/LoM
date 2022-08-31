using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float thrust;
    [SerializeField] private float knockDuration;
    [SerializeField] private string otherTag;
    public float damage;
    
    

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag(otherTag) && other.isTrigger)
        {
            Rigidbody2D hit = other.GetComponentInParent<Rigidbody2D>();
            if(hit != null)
            {
                Vector3 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.DOMove(hit.transform.position + difference, knockDuration);
                
                if(other.gameObject.CompareTag("Enemy"))
                {
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    other.GetComponent<Enemy>().Knock(hit, knockDuration, damage);
                }
                if(other.gameObject.CompareTag("Player") && other.isTrigger)
                {
                    if(other.GetComponentInParent<PlayerController>().currentState != PlayerState.hurt)
                    {
                        hit.GetComponent<PlayerController>().currentState = PlayerState.hurt;
                        other.GetComponentInParent<PlayerController>().Knock(knockDuration, damage);
                    }
                }
            }
        }
    }   
}


