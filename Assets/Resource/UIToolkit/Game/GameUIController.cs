using UnityEngine;
using UnityEngine.UIElements;
public class GameUIController : MonoBehaviour
{
    private Button _backToMenuButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

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


    #endregion
}
