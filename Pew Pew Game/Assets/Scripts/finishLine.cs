using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishLine : MonoBehaviour
{
    BoxCollider2D finishLineColider ;
    // Update is called once per frame
    void Update()
    {
        finishLineColider = GetComponent<BoxCollider2D>();
        if (finishLineColider.IsTouchingLayers(LayerMask.GetMask("Player")))
            {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex+1);

        }
    }
}
