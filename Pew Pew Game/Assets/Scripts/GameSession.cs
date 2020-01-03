using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{

    //acces
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI jumpText;


    //Info
    [SerializeField] public int jumpAmmount = 0;
    [SerializeField] public int playerHealth = 3;
    //score board stuff
    public List<int> scoreBoard =   new List<int>();
    bool addToScoreBoard = false;
    TextMeshProUGUI boardTextUI;
    string boardText;
    //end
    int currentLevel;
    int startJumpAmmount = 0;


    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddToScoreBoard()
    {
        scoreBoard.Add(jumpAmmount);
    }

    public void DisplayScoreboard()
    {
        var tempBoard = GameObject.Find("ScoreBoard");
     
        if (scoreBoard.Count != 0 && tempBoard != null)
        {
            boardTextUI = tempBoard.GetComponent<TextMeshProUGUI>();
            for (int i = 0; i < scoreBoard.Count; i++)
           {
                
                int b = i + 1;
                boardText += "Jumps in level " + b + " : " + scoreBoard[i]+ System.Environment.NewLine;
                boardTextUI.text = boardText;
   


            }
        }
        else
        {
            Debug.Log("list is empty");
        }
    
    }


    public void lowerHealth()
    {
        //var tempHealthVar = GameObject.Find("PlayerHealth");
      // healthText = tempHealthVar.GetComponent<TextMeshProUGUI>();

        playerHealth -= 1;
        healthText.text = playerHealth.ToString();
    }

    public void increaseJumps()
    {
       // var tempJumpVar = GameObject.Find("Jumps");
      //  jumpText = tempJumpVar.GetComponent<TextMeshProUGUI>();


        
        if (currentLevel == 0)
        {
            startJumpAmmount ++;
            jumpText.text = startJumpAmmount.ToString();
           // Debug.Log(jumpAmmount);
        }
        else 
        {
           
            jumpAmmount ++;
           
            jumpText.text = jumpAmmount.ToString();
            addToScoreBoard = true;
            // Debug.Log(jumpAmmount);
        }
    }

    public IEnumerator ResetLevel()
    {
        yield return new WaitForSeconds(3f);
        playerHealth = 3;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);

    }


    public void GetInfo()
    {
        var tempJumpVar = GameObject.Find("Jumps");
        jumpText = tempJumpVar.GetComponent<TextMeshProUGUI>();
        var tempHealthVar = GameObject.Find("PlayerHealth");
        healthText = tempHealthVar.GetComponent<TextMeshProUGUI>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void ResetInfo()
    {


        
        playerHealth = 3;
        healthText.text = playerHealth.ToString();
            
        jumpAmmount=0;
        jumpText.text = jumpAmmount.ToString();
        //Debug.Log("info reset");
    }

    


    // Start is called before the first frame update
    void Start()
    {


      

        GetInfo();

       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
