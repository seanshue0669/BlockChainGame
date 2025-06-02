using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static CameraController _instance;

    public static CameraController Instance
    {
        get
        {
            // Make sure only one scipt exisit in the scene
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<CameraController>();
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("Manager");
                    _instance = go.AddComponent<CameraController>();
                }
            }

            return _instance;
        }
    }

    public void OnEnable()
    {
        _camera = Camera.main;
    }
    public void SwitchBetween(string target)
    {
        if (_camera == null)
        {
            Debug.LogError("Cant find camera");
            return;
        }
        GameObject lobby = GameObject.Find("lobby_pos");
        GameObject setting = GameObject.Find("setting_pos");
        if (lobby == null || setting == null)
        {
            Debug.LogWarning("pos cant find¡I");
        }

        Transform lobbyPos = lobby.transform;
        Transform settingPos = setting.transform;
        switch (target.ToLower())
        {
            case "settings":
                _camera.transform.position = settingPos.position;
                break;
            case "lobby":
                _camera.transform.position = lobbyPos.position;
                break;
            default:
                Debug.LogWarning("Unknown Target¡G" + target);
                break;
        }

    }
}
