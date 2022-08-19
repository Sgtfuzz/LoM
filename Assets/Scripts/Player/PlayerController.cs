using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool moveing;
    private Animator myAnimator;


   private void Awake() 
   {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
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
        moveInput = playerControls.Land.Move.ReadValue<Vector2>();
        rb.velocity = moveInput * moveSpeed;
        if(moveInput.x == -1)
        {
            transform.localScale = new Vector2 (Mathf.Sign(rb.velocity.x), 1f);
        }
        if(moveInput.x == 1)
        {
            transform.localScale = new Vector2 (Mathf.Sign(rb.velocity.x), 1f);
        }
        if(playerControls.Land.Move.IsPressed())
        {
            myAnimator.SetBool("isWalking", true);      
        }
        else
        {
            myAnimator.SetBool("isWalking", false);   
        }
   }

   private void PlayerAttack()
   {
        if(playerControls.Land.Attack.WasPressedThisFrame())
        {
            myAnimator.SetTrigger("Attack");
        }
   }
}
