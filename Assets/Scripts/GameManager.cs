using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;         //A reference to our game control script so we can access it statically.
   

    public TextMeshProUGUI ScoreText;                       //A reference to the UI text component that displays the player's score.
    public GameObject GameOvertext;             //A reference to the object that displays the text which appears when the player dies.

    public bool GameOver = false;               //Is the game over?
    public float ScrollSpeed = -1.5f;

    private int score = 0;                      //The player's score.


    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    private void Start()
    {
        Application.targetFrameRate = 60;

    }


    void Update()
    {
        //If the game is over and the player has pressed some input...
        if (GameOver && Input.GetMouseButtonDown(0))
        {
            //...reload the current scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void BirdScored()
    {
        //The bird can't score if the game is over.
        if (GameOver)
            return;
        //If the game is not over, increase the score...
        score++;
        //...and adjust the score text.
        ScoreText.text = "Score: " + score.ToString();
    }

    public void BirdDied()
    {
        //Activate the game over text.
        GameOvertext.SetActive(true);
        //Set the game to be over.
        GameOver = true;
    }
}
