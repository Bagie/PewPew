using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishLine : MonoBehaviour
{
    BoxCollider2D finishLineColider ;
    // Update is called once per frame
    float playerSize = 1;
    bool didintDo = true;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        finishLineColider = GetComponent<BoxCollider2D>();
        if (finishLineColider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            Transform playerBody = collision.GetComponent<Transform>();
            Player player = collision.gameObject.GetComponent(typeof(Player)) as Player;
            player.isFinished(true);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            
            

            StartCoroutine(DeacreaseSize(playerBody));
            StartCoroutine(LoadNextLevel());
            



        }

     
            if (didintDo)
        {
          GameSession  gameSession = FindObjectOfType<GameSession>();
            gameSession.AddToScoreBoard();
            didintDo = false;
            return;
        }
            else { return; }


    }

    IEnumerator DeacreaseSize(Transform playerBody)
    {
        Vector3 currentPossitionForDeath = playerBody.position;

        while (playerSize > 0f)
        {
            playerBody.position = currentPossitionForDeath;
            playerSize -= 0.01f;
            playerBody.localScale = new Vector3(Mathf.Clamp(playerSize,0,1), Mathf.Clamp(playerSize, 0, 1), 1f);
            yield return new WaitForFixedUpdate();

        }
     
    }

    IEnumerator LoadNextLevel()
    {

  
        yield return new WaitForSeconds(1f);
      

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
    }
}
