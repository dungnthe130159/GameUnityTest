using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovemont : MonoBehaviour 
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    float gravityScaleStart;
    bool isAlive = true;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    BoxCollider2D myFeetCollider;

    [SerializeField] private MobilJoystick joystick;

    void Awake()
    {
        joystick = FindObjectOfType<MobilJoystick>();
    }
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleStart = myRigidbody.gravityScale;
        bullet.transform.localScale = transform.localScale;
    }

    private void Move(Vector2 input)
    {
        moveInput= input;
    }

    void Update()
    {
        if(!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    //Get x,y input by keybroad
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();    
    }

    public void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){ return;}

        if(value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
        
    }

    public void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        //Create what , where , 
        Instantiate(bullet, gun.position, transform.rotation );        
    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climb"))) 
        {
            myRigidbody.gravityScale = gravityScaleStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbPlayer = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbPlayer;

        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Run()
    {
        Vector2 runPlayer = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = runPlayer;

        //check play move or not
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);

        joystick.OnMove += Move;
    }

    //Flip player when turn left or right
    void FlipSprite()
    {
        //check play move or not
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        //if player dont move => dont flip
        if (playerHasHorizontalSpeed )
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
            bullet.transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void Die()
    {
        if (myRigidbody.IsTouchingLayers(LayerMask.GetMask("Enemies","Hazards"))){
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    public void Jump()
    {
        if (!isAlive) { return; }

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
    }
}
