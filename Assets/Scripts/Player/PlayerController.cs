using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
     walk,
     attack,
     interact,
     hurt,
     idle,
}
public class PlayerController : MonoBehaviour
{
     
    public PlayerState currentState;
    [SerializeField]
    private float moveSpeed;
    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool moveing;
    private Animator myAnimator;
    public FloatValue currentHealth;
    public float health;
    public SignalSender playerHealthSignal;
    public VectorValue startingPosition;


   private void Awake() 
   {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        transform.position = startingPosition.initialValue;
   }

   private void OnEnable() 
   {
        playerControls.Land.Move.Enable();
        playerControls.Land.Attack.Enable(); 
   }

   private void OnDisable() 
   {
        playerControls.Land.Move.Disable();
        playerControls.Land.Attack.Disable(); 
   }

   void Update() 
   {
        PlayerAttack();  
   }

   private void FixedUpdate() 
   {
        Move();  
   }

   private void Move()
   {
        currentState = PlayerState.walk;
        moveInput = playerControls.Land.Move.ReadValue<Vector2>();
        rb.velocity = moveInput * moveSpeed;
        if(moveInput.x == -1 && currentState != PlayerState.attack)
        {
            transform.localScale = new Vector2 (Mathf.Sign(rb.velocity.x), 1f);
        }
        if(moveInput.x == 1 && currentState != PlayerState.attack)
        {
            transform.localScale = new Vector2 (Mathf.Sign(rb.velocity.x), 1f);
        }
        if(playerControls.Land.Move.IsPressed())
        {   
            myAnimator.SetBool("isWalking", true);      
        }
        else
        {
            currentState = PlayerState.idle;
            myAnimator.SetBool("isWalking", false);   
        }
   }

   private void PlayerAttack()
   {
        if(playerControls.Land.Attack.WasPressedThisFrame())
        {
            currentState = PlayerState.attack;   
            myAnimator.SetTrigger("Attack");
        }
   }

       public void Knock(float knockDuration, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if(currentHealth.RuntimeValue > 0)
        {
            
            StartCoroutine(KnockCo(knockDuration));   
        }
        else
        {   
            this.gameObject.SetActive(false);

        }
    }
    
    private IEnumerator KnockCo(float knockDuration)
    {
        if(rb != null)
        {
            yield return new WaitForSeconds(knockDuration);
            rb.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            rb.velocity = Vector2.zero;
        }
    }



}
