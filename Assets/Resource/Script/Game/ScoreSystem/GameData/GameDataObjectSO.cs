using UnityEngine;
[CreateAssetMenu(fileName = "GameDataObjectSO", menuName = "ScriptableObjects/GameData")]
public class GameDataObjectSO : ScriptableObject
{
    public float _stratTimeStrap;
    public float _endTimeStrap;
    public float _totalPlayTime;

    public float earn;
    public int score;

    //Getter And Setter
    public void setStartTime(float t)
    {
        _stratTimeStrap = t;
    }
    public void setEndTime(float t)
    {
        _endTimeStrap = t;
    }
    public float getTotalTime()
    {
        return _totalPlayTime;
    }
    public void resetData()
    {
        score = 0;
        earn = 0;
        _stratTimeStrap = 0;
        _endTimeStrap = 0;
        _totalPlayTime = 0;       
    }
    public void setEarn(float p_earn)
    {
        earn = p_earn;
    }
    //Utitly function
    public void calTotalPlayTime()
    {
        _totalPlayTime = _endTimeStrap - _stratTimeStrap;
    }
    public void scoreIncreaseByValue(int p_score)
    {
        score += p_score; 
    }
    public float calScoreMultiply()
    {
        float a = 0.01f;
        float b = 0.5f;

        return 1f + a * _totalPlayTime;
    }
}
