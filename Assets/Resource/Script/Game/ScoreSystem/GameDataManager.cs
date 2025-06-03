using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    //Singleton for GameDataManager
    private static GameDataManager _instance;
    public static GameDataManager Instance
    {
        get 
        {        
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameDataManager>();
                //Just for Safe
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("Manager");
                    _instance = go.AddComponent<GameDataManager>();
                }
            }
            return _instance;
        }
    }

    
    public GameDataObjectSO gameDataSO;

    private void Start()
    {
        gameDataSO.resetData();
        gameDataSO.setStartTime(Time.time);
    }

    //Utility
    public void IncreaseScoreByValue(int p_Score)
    {
        gameDataSO.scoreIncreaseByValue(p_Score);
    }

    public void TriggerGameEnd()
    {
        StopTheGame();
        float giveCoinAmount = CalGameResult();
        gameDataSO.setEarn(giveCoinAmount);
        //Trigger End UI
        GameUIController.Instance.ShowScoreBoard();
        //Send eth to target wallet
    }
    private void StopTheGame()
    {
        gameDataSO.setEndTime(Time.time);
        Time.timeScale = 0f;
    }
    private float CalGameResult()
    {
        gameDataSO.calTotalPlayTime();
        float totalCoinGiven = gameDataSO.calScoreMultiply();
        return totalCoinGiven;
    }
}
