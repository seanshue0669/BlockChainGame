using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // Singleton class of controller
    private static SceneController _instance;

    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SceneController>();
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("Manager");
                    _instance = go.AddComponent<SceneController>();
                }
            }

            return _instance;
        }
    }

    private Dictionary<string, int> _sceneNameToIndex = new Dictionary<string, int>
    {
        { "MainMenu", 0 },
        { "Game", 1 },
        { "Lottery", 2 },
        { "Market", 3 }
    };

    public void SwitchToScene(string sceneName)
    {
        if (_sceneNameToIndex.TryGetValue(sceneName, out int index))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(index);
            //Call the transition animation?

        }
        else
        {
            Debug.LogError($"Scene name '{sceneName}' not found in scene index table.");
        }
    }
}
