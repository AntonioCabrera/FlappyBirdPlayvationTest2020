using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Managers references")]
    public UIManager UIManager;
    [Space(10)]
    [Header("API Keys")]
    public string DreamloPrivateKey;
    public string DreamloPublicKey;
    [HideInInspector]
    public string PlayerName;


    private bool nameAvailable = true;
    [SerializeField]
    private LeaderboardModel.Root leaderboardDataModel;
    //REST Urls
    private string postPlayerScoreUrl;
    private string getAllLeaderboardsUrl;
    private string getTopFiveLeaderboardsUrl;
    //Coroutines for memory allocation
    private Coroutine postCurrentPlayerScoreCoroutine;
    private Coroutine getTopFiveLeaderboardCoroutine;
    private Coroutine checkNameAvailabilityCoroutine;



    //Initializing urls
    private void Start()
    {
        getAllLeaderboardsUrl = "http://dreamlo.com/lb/" + DreamloPublicKey + "/json";
        getTopFiveLeaderboardsUrl = "http://dreamlo.com/lb/" + DreamloPublicKey + "/json/5";
    }




    #region Check name availability
    public void InvokeCheckNameAvailabilityCoroutine(string name)
    {

        if (checkNameAvailabilityCoroutine != null)
        {
            StopCoroutine(checkNameAvailabilityCoroutine);
            checkNameAvailabilityCoroutine = null;
        }

        checkNameAvailabilityCoroutine = StartCoroutine(CheckNameAvailability(name));

    }


    //This method is called from UIManager's Input field's "Send" button and goes back to update playerprefs and launch game
    private IEnumerator CheckNameAvailability(string playerName)
    {
        //Preformats the name to be url friendly (could be better)
        playerName = UnityWebRequest.EscapeURL(playerName).Trim();

        nameAvailable = true;
        //Turns off "not available name" notice during a retry input
        UIManager.TurnOffNameNotAvailableNotice();
        UnityWebRequest www = UnityWebRequest.Get(getAllLeaderboardsUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Deserializes the model, retrieving all data to check name availability
            leaderboardDataModel = JsonConvert.DeserializeObject<LeaderboardModel.Root>(www.downloadHandler.text);

            foreach (var item in leaderboardDataModel.dreamlo.leaderboard.entry)
            {
                if (item.name == playerName)
                {
                    nameAvailable = false;
                }
            }

            //Gets back to the UIManager to continue the flow
            UIManager.SetPlayerNameOnPlayerPreferences(nameAvailable, playerName);

            checkNameAvailabilityCoroutine = null;
        }
    }
    #endregion



    #region Get top 5 leaderboard and update the leaderboard

    //This method is triggered by the player's death
    public void InvokeGetTopFiveLeaderboardCoroutine()
    {
        if (getTopFiveLeaderboardCoroutine != null)
        {
            StopCoroutine(getTopFiveLeaderboardCoroutine);
            getTopFiveLeaderboardCoroutine = null;
        }

        getTopFiveLeaderboardCoroutine = StartCoroutine(GetLeaderboard());


    }



    private IEnumerator GetLeaderboard()
    {
        //UI flows controlls
        UIManager.TurnOffCurrentPlayerScoreText();
        UIManager.TurnOnGameOverMenu();
        UIManager.TurnOffGameOverMenuTapToContinueButton();
        UnityWebRequest www = UnityWebRequest.Get(getTopFiveLeaderboardsUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
            UIManager.TurnOnGameOverMenuTapToContinueButton();
        }
        else
        {
            //Deserializes the model, it comes ordered and limited to five entries using url params for the service
            leaderboardDataModel = JsonConvert.DeserializeObject<LeaderboardModel.Root>(www.downloadHandler.text);

            //Data is ready and gets back to the UIManager to inject the data
            UIManager.RefreshLeaderboardData(leaderboardDataModel);

        }
        getTopFiveLeaderboardCoroutine = null;

    }
    #endregion



    #region Post player's score
    public void InvokePostCurrentPlayerScoreCoroutine(string player, string score)
    {
        if (postCurrentPlayerScoreCoroutine != null)
        {
            StopCoroutine(postCurrentPlayerScoreCoroutine);
            postCurrentPlayerScoreCoroutine = null;
        }

        postCurrentPlayerScoreCoroutine = StartCoroutine(PostCurrentPlayerScore(player, score));


    }


    //Post current try score, the service will override its entry only if improved the highscore
    private IEnumerator PostCurrentPlayerScore(string Player, string Score)
    {
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");

        postPlayerScoreUrl = "http://dreamlo.com/lb/" + DreamloPrivateKey + "/add/" + Player + "/" + Score;
        UnityWebRequest www = UnityWebRequest.Post(postPlayerScoreUrl, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Posted score");
        }

        postCurrentPlayerScoreCoroutine = null;

    }
    #endregion

}
