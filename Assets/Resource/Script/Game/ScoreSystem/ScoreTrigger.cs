using UnityEngine;

public class ScoreTrigger: MonoBehaviour
{
    [SerializeField]
    public int score=5;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameDataManager.Instance.IncreaseScoreByValue(score);

            Destroy(gameObject);

            //Debug.Log("Get Score");
        }
    }
}
