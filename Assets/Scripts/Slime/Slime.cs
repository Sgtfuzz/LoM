using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy 
{
    private Animator slimeAnim;
    private Rigidbody2D rb;
    private BoxCollider2D slimeBox;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public bool isAlive = true;
    public SpriteRenderer spriteRenderer;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        slimeAnim = GetComponent<Animator>();
        slimeBox = GetComponent<BoxCollider2D>();
        target = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentState = EnemyState.idle;
        
    }

  
    void FixedUpdate()
    {
        CheckDistance();
    }
    
    void CheckDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius && isAlive)
        {
            if(currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                slimeAnim.SetTrigger("walking");
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                spriteRenderer.flipX = (target.position.x < transform.position.x);
                rb.MovePosition(temp);
                ChangeState(EnemyState.walk);
            }
        }
        else
        {
            slimeAnim.SetTrigger("idle");
            ChangeState(EnemyState.idle);
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if(currentState != newState)
        {
            currentState = newState;
        }
    }

    public void Death()
    {
        if(health <= 0)
        {
            isAlive = false;
            slimeAnim.SetTrigger("dead");
            StartCoroutine(deathCo());
            ChangeState(EnemyState.dead);
        }
    }

    IEnumerator deathCo()
    {
        slimeBox.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
