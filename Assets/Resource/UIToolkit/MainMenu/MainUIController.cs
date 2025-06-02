using UnityEngine;
using UnityEngine.UIElements;
public class MainUIController : MonoBehaviour
{
    private VisualElement _title;
    private Button _startButton;
    private Button _settingsButton;
    private Button _lotteryButton;
    private Button _marketButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find VE in root
        if (root != null)
        {
            _title = root.Q<VisualElement>("GameTitle");
        }
        bindUIEvent(root);

        // Start The looping animaton
        Invoke("LoopAnimation", 0.1f);

    }

    private void LoopAnimation()
    {

        _title.ToggleInClassList("titleScale");
        _title.RegisterCallback<TransitionEndEvent>(
            evt => _title.ToggleInClassList("titleScale")
        );
    }

    private void bindUIEvent(VisualElement root)
    {
        _startButton = root.Q<Button>("StartButton");
        _settingsButton = root.Q<Button>("SettingButton");
        _lotteryButton = root.Q<Button>("LotteryButton");
        _marketButton = root.Q<Button>("MarketButton");

        _startButton?.RegisterCallback<ClickEvent>(evt => OnStartClicked());
        _settingsButton?.RegisterCallback<ClickEvent>(evt => OnSettingsClicked());
        _lotteryButton?.RegisterCallback<ClickEvent>(evt => OnLotteryClicked());
        _marketButton?.RegisterCallback<ClickEvent>(evt => OnMarketClicked());
    }
    #region CB_Function
    private void OnStartClicked()
    {
        Debug.Log("Start button clicked");
    }

    private void OnSettingsClicked()
    {
        Debug.Log("Settings button clicked");
    }

    private void OnLotteryClicked()
    {
        Debug.Log("Lottery button clicked");
    }

    private void OnMarketClicked()
    {
        Debug.Log("Market button clicked");
    }

    #endregion
}
