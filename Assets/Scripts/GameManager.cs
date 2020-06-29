using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;         //A reference to our game control script so we can access it statically.
    public UIManager UIManager;
    public LeaderboardManager LeaderboardManager;
    public ColumnPool ColumnPool;

    public ScrollingObject[] ScrollingObjects;

    public TextMeshProUGUI ScoreText;                       //A reference to the UI text component that displays the player's score.
    public GameObject GameOvertext;             //A reference to the object that displays the text which appears when the player dies.

    public bool GameOver = false;               //Is the game over?
    public float ScrollSpeed = -1.5f;

    [HideInInspector]
    public int CurrentScore = 0;                      //The player's score.
    [HideInInspector]
    public bool PlayerCanMove = false;

    private Rigidbody2D playerRigidbody;
    private string playerName;


    void Awake()
    {

        if (Instance == null)
            Instance = this;
    }


    private void Start()
    {
        Application.targetFrameRate = 60;

        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerRigidbody.gravityScale = 0;
        playerRigidbody.isKinematic = true;

        UIManager.TurnOnMainMenu();

        if (PlayerPrefs.HasKey("PlayerName") == false)
        {
            //First time the players plays will be asked to input a name
            UIManager.TurnOnInputNameMenu();
            UIManager.TurnOffMainMenuTapToStartText();
        }
        else
        {
            playerName = PlayerPrefs.GetString("PlayerName");
            LeaderboardManager.PlayerName = playerName;
            UIManager.TurnOnMainMenuTapToStartText();
        }

    }

    //Called from UIManager's MainMenuTapToStartButton
    public void StartGame()
    {
        ColumnPool.InvokeSpawnColumnsCoroutine(3);
        PlayerCanMove = true;
        playerRigidbody.isKinematic = false;
        playerRigidbody.gravityScale = 0.6f;

    }

    //Called from UIManager's OnGameOverTapToContinueButtonClick
    public void ReloadScene()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BirdScored()
    {
        //The bird can't score if the game is over.
        if (GameOver)
            return;
        CurrentScore++;
        UIManager.AddScoreToCurrent(CurrentScore.ToString());
    }

    public void BirdDied()
    {
        GameOver = true;
        foreach (var item in ScrollingObjects)
        {
            item.StopScrolling();
        }
        if(PlayerPrefs.GetInt("PlayerHighScore") < CurrentScore)
        {
            PlayerPrefs.SetInt("PlayerHighScore", CurrentScore);
                //New Highscore
        }
        LeaderboardManager.InvokePostCurrentPlayerScoreCoroutine(playerName, CurrentScore.ToString());
        LeaderboardManager.InvokeGetTopFiveLeaderboardCoroutine();
        
    }
}
