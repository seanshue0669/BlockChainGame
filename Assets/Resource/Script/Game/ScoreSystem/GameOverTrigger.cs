using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameDataManager.Instance.TriggerGameEnd();
            //Debug.Log("Get Score");
        }
    }
}
