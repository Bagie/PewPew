using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour

{
    [SerializeField] float moveSpeed=10f;
    Rigidbody2D rigidbody2D;
    BoxCollider2D boxCollider2D;
    bool isEnemyTouchingGround;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        isEnemyTouchingGround = boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
        rigidbody2D.velocity = new Vector2(moveSpeed, 0f);
        StartCoroutine(EnemyJumps(2f));
    }

    IEnumerator EnemyJumps(float jumpTime)
    {
        yield return new WaitForSeconds(jumpTime);
        if (isEnemyTouchingGround)
        {
            rigidbody2D.velocity = new Vector2(1, 100);
            
        }
            
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Enemy paliete");
        transform.localScale = new Vector2(-(Mathf.Sign(rigidbody2D.velocity.x)), 1f);
        moveSpeed = -moveSpeed;
    }

}
