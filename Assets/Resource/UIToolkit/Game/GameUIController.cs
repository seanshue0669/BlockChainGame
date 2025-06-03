using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class GameUIController : MonoBehaviour
{
    private Button _backToMenuButton;
    private VisualElement _resultBoard;

    private static GameUIController _instance;
    public static GameUIController Instance {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameUIController>();
                //Just for Safe
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("UIToolkitScript");
                    _instance = go.AddComponent<GameUIController>();
                }
            }
            return _instance;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _resultBoard = root.Q<VisualElement>("scoreboard");
        _resultBoard.style.display = DisplayStyle.None;
        bindUIEvent(root);

    }


    private void bindUIEvent(VisualElement root)
    {
        _backToMenuButton = root.Q<Button>("backToMenu");
        _backToMenuButton?.RegisterCallback<ClickEvent>(evt => OnBackToMenu());
    }
    #region CB_Function
    private void OnBackToMenu()
    {
        Debug.Log("BackToMenu button clicked");
        SceneController.Instance.SwitchToScene("MainMenu");
    }
    public void ShowScoreBoard()
    {
        _resultBoard.style.display = DisplayStyle.Flex;
    }
    #endregion
}
