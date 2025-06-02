using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static PersistentObject _instance;

    // Manitain that there is only one PersistentObject in all gamelife
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Remove Duplicate Manager Object");
            Destroy(gameObject);
            return;
        }
        Debug.Log("Create the first Manager Object");
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
