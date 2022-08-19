using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private Animator slimeAnim;
    private BoxCollider2D slimeBox;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;

    void Awake() 
    {
        slimeAnim = GetComponent<Animator>();
        slimeBox = GetComponent<BoxCollider2D>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        
    }

  
    void Update()
    {
        CheckDistance();
    }
    
    void CheckDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            slimeAnim.SetTrigger("walking");
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            slimeAnim.SetTrigger("idle");
        }
    }

    public void Death()
    {
        slimeAnim.SetTrigger("dead");
        StartCoroutine(deathCo());
    }

    IEnumerator deathCo()
    {
        slimeBox.enabled = false;
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
