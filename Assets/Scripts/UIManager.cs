using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Managers references")]
    public LeaderboardManager LeaderboardManager;
    public GameManager GameManager;

    [Header("Top five leaderboards")]
    //Index are 0 for Top one, 1 for Top two, 2 for Top three etc
    public TextMeshProUGUI[] TopFiveScoresTexts;
    public TextMeshProUGUI[] TopFiveNamesTexts;

    [Space(10)]

    [Header("Player scores")]

    public TextMeshProUGUI MainMenuPlayerHighscore;
    public TextMeshProUGUI GameOverMenuPlayerHighscoreText;
    public TextMeshProUGUI CurrentPlayerScoreText;

    [Space(10)]

    [Header("Menu UI elements")]
    public GameObject MainMenu;
    public GameObject MainMenuTapToStartText;
    public GameObject NameInputMenu;
    public GameObject GameOverMenu;
    public Button GameOverMenuTapToContinueButton;
    public GameObject GameOverMenuTapToContinueText;
    public TMP_InputField PlayerNameInputField;
    public GameObject NameNotAvailableText;

    [Space(10)]
    public Animator CurrentScoreFeedbackAnimator;


    private Coroutine reloadSceneWithDelayCoroutine;
    private int refreshLeaderboardIndex;
    private string currentPlayerHighscore;

    private void Start()
    {
        //sets initial values 
        currentPlayerHighscore = PlayerPrefs.GetInt("PlayerHighScore", 0).ToString();
        MainMenuPlayerHighscore.text = currentPlayerHighscore;
        GameOverMenuPlayerHighscoreText.text = currentPlayerHighscore;
    }

    //OnclickListener of the GameOverMenu fullscreen button
    public void OnGameOverTapToContinueButtonClick()
    {
        reloadSceneWithDelayCoroutine = StartCoroutine(ReloadSceneWithDelay(0.5f));
    }

    //OnclickListener of the MainMenu fullscreen button
    public void MainMenuTapToStartButton()
    {
        TurnOffMainMenu();
        TurnOnCurrentPlayerScoreText();
        GameManager.Instance.StartGame();
    }

    //Reloads scene with a little delay  to give time at the end so the injection gets done before it can be reloaded
    public IEnumerator ReloadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.ReloadScene();
    }


    //This method is called every time you score a point and updates the current score visualization.
    public void AddScoreToCurrent(string score)
    {

        CurrentPlayerScoreText.text = "Score: " + score;
        CurrentScoreFeedbackAnimator.SetTrigger("FeedbackAnimation");
    }


    //Called from LeaderboardManager triggered by death and injects data for the game over menu before turning it on.
    public void RefreshLeaderboardData(LeaderboardModel.Root leaderboardDataModel)
    {
        CurrentPlayerScoreText.gameObject.SetActive(false);
        refreshLeaderboardIndex = 0;
        foreach (var item in leaderboardDataModel.dreamlo.leaderboard.entry)
        {
            TopFiveNamesTexts[refreshLeaderboardIndex].text = item.name;
            TopFiveScoresTexts[refreshLeaderboardIndex].text = item.score;
            refreshLeaderboardIndex++;
        }
        TurnOnGameOverMenuTapToContinueButton();
        currentPlayerHighscore = PlayerPrefs.GetInt("PlayerHighScore", 0).ToString();
        MainMenuPlayerHighscore.text = currentPlayerHighscore;
        GameOverMenuPlayerHighscoreText.text = currentPlayerHighscore;


        CurrentPlayerScoreText.text = GameManager.CurrentScore.ToString();
    }


    //This method is called from the button of the input field for the name, the LeaderboardManager takes 
    //care of format and checking availability, if everything's ok the flow comes back here, to SetPlayerNameOnPlayerPreferences()
    public void CheckPlayerNameAvailabilityAndSendButton()
    {
        if (string.IsNullOrEmpty(PlayerNameInputField.text))
        {
            TurnOnNameNotAvailableNotice();
            return;
        }
        LeaderboardManager.InvokeCheckNameAvailabilityCoroutine(PlayerNameInputField.text);
    }


    //Triggered after a succesfull REST call, ony called first time when the name is asked.
    public void SetPlayerNameOnPlayerPreferences(bool nameAvailable, string playerName)
    {
        if (nameAvailable)
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.SetString("PlayerHighScore", "0");
            TurnOffInputNameMenu();
            TurnOnMainMenuTapToStartText();
        }
        else
        {
            TurnOnNameNotAvailableNotice();
        }
    }



    //Reusable on and off switches.
    #region Turn on / Turn off methods


    public void TurnOnMainMenu()
    {
        MainMenu.SetActive(true);
    }

    public void TurnOffMainMenu()
    {
        MainMenu.SetActive(false);
    }


    public void TurnOnMainMenuTapToStartText()
    {
        MainMenuTapToStartText.SetActive(true);
    }

    public void TurnOffMainMenuTapToStartText()
    {
        MainMenuTapToStartText.SetActive(false);
    }


    public void TurnOnGameOverMenu()
    {
        GameOverMenu.SetActive(true);
    }

    public void TurnOffGameOverMenu()
    {
        GameOverMenu.SetActive(false);
    }


    public void TurnOnGameOverMenuTapToContinueButton()
    {
        GameOverMenuTapToContinueText.SetActive(true);
        GameOverMenuTapToContinueButton.interactable = true;
    }

    public void TurnOffGameOverMenuTapToContinueButton()
    {
        GameOverMenuTapToContinueText.SetActive(false);
        GameOverMenuTapToContinueButton.interactable = false;
    }


    public void TurnOnInputNameMenu()
    {
        NameInputMenu.SetActive(true);

    }
    public void TurnOffInputNameMenu()
    {
        NameInputMenu.SetActive(false);

    }


    public void TurnOnNameNotAvailableNotice()
    {
        NameNotAvailableText.SetActive(true);
    }
    public void TurnOffNameNotAvailableNotice()
    {
        NameNotAvailableText.SetActive(false);
    }




    public void TurnOnCurrentPlayerScoreText()
    {
        CurrentPlayerScoreText.gameObject.SetActive(true);
    }
    public void TurnOffCurrentPlayerScoreText()
    {
        CurrentPlayerScoreText.gameObject.SetActive(false);
    }





    #endregion

}
