﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed = 1f;
    [SerializeField] float shiftRunSpeed = 2f;
    [SerializeField] float jumpSpeed = 0.75f;
    [SerializeField] float climbSpeed = 5f;
    bool isShiftRuning = false;
   public bool immortal = false;
    [SerializeField] Color immortalityColor;
    [SerializeField] Color originalColor;

    //Acces cache
    Rigidbody2D myRigidbody2D;
    Animator animator;
    CapsuleCollider2D myCapsuleColider2D;
    BoxCollider2D myFeetBoxCol2D;
    float gravityScaleAtStart;
    SpriteRenderer spriteRenderer;
    GameSession gameSession;



    //vars
    bool isTouchingGround;
    bool isTouchingLadder;
    bool isAlive = true;
    bool isFinishedLevel = false;



    // Start is called before the first frame update
    void Start()
    {
        ComponentGetter();
        gravityScaleAtStart = myRigidbody2D.gravityScale;
        gameSession.ResetInfo();  // reets health and jumps each level
        gameSession.DisplayScoreboard();
    }

    private void ComponentGetter()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myCapsuleColider2D = GetComponent<CapsuleCollider2D>();
        myFeetBoxCol2D = GetComponent<BoxCollider2D>();
        gameSession = FindObjectOfType<GameSession>();
        gameSession.GetInfo();  // resets info in game manager each level
    }

    public void isFinished(bool finishPlayer)
    {
         isFinishedLevel = finishPlayer;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (!isAlive) { return; }
        if (isFinishedLevel) { return; }
        Die();
        SuperSpeed();
        Run();
        Jump();
        FlipSpriteOnMove();
        RunAnimaton();
        ClimbLadder();
        JumpOnEnemy();
        testColor();


    }

  void testColor()
    {
        if (immortal) { 
       // Color lerpedColorTest = Color.Lerp(originalColor, immortalityColor, Mathf.PingPong(Time.time, 10f));
            Color lerpedColorTest = Color.Lerp(originalColor, immortalityColor, Mathf.PingPong(Time.time*7,1f));
            spriteRenderer.color = lerpedColorTest;
        }
    }
   

    private void JumpOnEnemy()
    {
        if (myFeetBoxCol2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(10, 28);
        }
    }

    private void SuperSpeed()
    {
        if (CrossPlatformInputManager.GetButton("Shift"))
        {
            isShiftRuning = true;
        }
        else
        {
            isShiftRuning = false;
        }

    }

    private void Die()
    {
        if (myCapsuleColider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
           


            if (gameSession.playerHealth == 0)
                {
                GetComponent<Rigidbody2D>().velocity = new Vector2(10, 220);
                animator.SetTrigger("Death");
               
                Destroy(myRigidbody2D);
                Destroy(myCapsuleColider2D);
                Destroy(myFeetBoxCol2D);
                isAlive = false;
                gameSession.ResetLevel();
            }
            else if (!immortal)
                StartCoroutine(immortality());
            GetComponent<Rigidbody2D>().velocity = new Vector2(10, 20);
        }

    }

    IEnumerator immortality ()
    {
        immortal = true;
        ImortalColorChange();
        gameSession.lowerHealth();
        yield return new WaitForSeconds(2f);

        
       // Debug.Log(playerHealth);
       spriteRenderer.color = originalColor;
       immortal = false;
      

    }

    private void ImortalColorChange()
    {
        Color lerpedColor = Color.Lerp(originalColor, immortalityColor, Mathf.PingPong(Time.time, 1f));
        spriteRenderer.color = lerpedColor;
      
    }


    private void ClimbLadder()
    {
        isTouchingLadder = myCapsuleColider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (!myCapsuleColider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            animator.SetBool("Climbing", false);
            myRigidbody2D.gravityScale = gravityScaleAtStart;
            return;

        }


        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical") * climbSpeed;  //-1 to +1
        Vector2 playerClimbVelocity = new Vector2(myRigidbody2D.velocity.x, controlThrow);
        myRigidbody2D.velocity = playerClimbVelocity;
        myRigidbody2D.gravityScale = 0f;

        bool playerVerticalSpeed = Mathf.Abs(myRigidbody2D.velocity.y) > Mathf.Epsilon;
        animator.SetBool("Climbing", playerVerticalSpeed);

    }


    private void Run()
    {

        if (isShiftRuning)
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal") * shiftRunSpeed;  //-1 to +1
            Vector2 playerVelocity = new Vector2(controlThrow, myRigidbody2D.velocity.y);
            myRigidbody2D.velocity = playerVelocity;
        }
        else
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal") * runSpeed;  //-1 to +1
            Vector2 playerVelocity = new Vector2(controlThrow, myRigidbody2D.velocity.y);
            myRigidbody2D.velocity = playerVelocity;
        }
    }

    private void Jump()
    {
        isTouchingGround = myFeetBoxCol2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        isTouchingLadder = myCapsuleColider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (CrossPlatformInputManager.GetButtonDown("Jump") && isTouchingGround)
        {
            Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
            myRigidbody2D.velocity += jumpVelocity;
            gameSession.increaseJumps();
        }
    }

  


    private void RunAnimaton()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        animator.SetBool("Runing", playerHasHorizontalSpeed);
    }

    private void FlipSpriteOnMove()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }


    
}
