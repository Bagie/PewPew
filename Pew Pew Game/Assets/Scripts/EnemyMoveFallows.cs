using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveFallows : MonoBehaviour

{
    [SerializeField] float moveSpeed = 10f;
    Rigidbody2D rigidbody2D;
    BoxCollider2D boxCollider2D;
    bool isEnemyTouchingGround;
    Transform target;
    public float speed = .1f;
    Vector2 forwardAxis;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        isEnemyTouchingGround = boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        Fallow();
        Apsisukti();
       // StartCoroutine(EnemyJumps(2f));
    }

    private void Apsisukti()
    {transform.localScale = new Vector2((Mathf.Sign(rigidbody2D.velocity.x)), 1f); }

    private void Fallow()
    {

       
        if (target.position.x == transform.position.x)
        {
            
            rigidbody2D.velocity = new Vector2(0f, 0f);
        }

        else if (target.position.x > transform.position.x+1f)
        {
            
            rigidbody2D.velocity = new Vector2(moveSpeed, 0f); }
        else if (target.position.x < transform.position.x-1)
        {
            
            rigidbody2D.velocity = new Vector2(-moveSpeed, 0f);
        }



    }
    IEnumerator EnemyJumps(float jumpTime)
    {
        
        if (isEnemyTouchingGround)
        {
            rigidbody2D.AddForce( new Vector2 (5f, 50f), ForceMode2D.Impulse);
            
        }
        yield return new WaitForSeconds(jumpTime);

    }

   /* private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Enemy paliete");
        transform.localScale = new Vector2(-(Mathf.Sign(rigidbody2D.velocity.x)), 1f);
        moveSpeed = -moveSpeed;
    }
    */
}
