
using UnityEngine;
using UnityEngine.UIElements;

using Button = UnityEngine.UIElements.Button;


//using static UnityEngine.Rendering.DebugUI.MessageBox;

public class MainUIController : MonoBehaviour
{
    private VisualElement _title;

    private VisualElement _lobbyGroup;

    private Button _startButton;
    private Button _settingsButton;
    private Button _lotteryButton;
    private Button _marketButton;

    private VisualElement _settingsGroup;

    private Button _backButton;
    private Button _bindWalletButton;
    private Button _chooseSkinButton;
    private Button _viewSkinButton;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Find VE in root
        if (root != null)
        {
            _title = root.Q<VisualElement>("GameTitle");
        }
        FindButtonList(root);
        BindUIEvent(root);

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
    private void FindButtonList(VisualElement root)
    {
        _lobbyGroup = root.Q<VisualElement>("buttton-container-lobby");
        _lobbyGroup.style.display = DisplayStyle.Flex;
        _settingsGroup = root.Q<VisualElement>("buttton-container-settings");
        _settingsGroup.style.display = DisplayStyle.None;

    }

    private void BindUIEvent(VisualElement root)
    {
        //lobby
        _startButton    = root.Q<Button>("StartButton");
        _startButton?.RegisterCallback<ClickEvent>(evt => OnStartClicked());

        _settingsButton = root.Q<Button>("SettingButton");
        _settingsButton?.RegisterCallback<ClickEvent>(evt => OnSettingsClicked());

        _lotteryButton  = root.Q<Button>("LotteryButton");
        _lotteryButton?.RegisterCallback<ClickEvent>(evt => OnLotteryClicked());

        _marketButton   = root.Q<Button>("MarketButton");
        _marketButton?.RegisterCallback<ClickEvent>(evt => OnMarketClicked());

        //settings
        _backButton       = root.Q<Button>("BackButton");
        _backButton?.RegisterCallback<ClickEvent>(evt => OnBackClicked());

        _bindWalletButton = root.Q<Button>("BindWalletButton");
        _bindWalletButton?.RegisterCallback<ClickEvent>(evt => OnBindWalletClicked());

        _chooseSkinButton = root.Q<Button>("ChooseSkinButton");
        _chooseSkinButton?.RegisterCallback<ClickEvent>(evt => OnChooseSkinClicked());


    }

    #region CB_Function(lotteryButtons)
    private void OnStartClicked()
    {
        if (WalletManager.isBind)
        {
            Debug.Log("Start button clicked");
            SceneController.Instance.SwitchToScene("Game");
        }
    }

    private async void OnSettingsClicked()
    {
        Debug.Log("Settings button clicked");
        CameraController.Instance.SwitchBetween("settings");
        switchSettingMenu();

        await WalletManager.GetMyNftRarityAndWear();
    }

    private void OnLotteryClicked()
    {
        if (WalletManager.isBind)
        {
            Debug.Log("Lottery button clicked");
            SceneController.Instance.SwitchToScene("Lottery");
        }
    }

    private void OnMarketClicked()
    {
        Debug.Log("Market button clicked");
        SceneController.Instance.SwitchToScene("Market");
    }
    #endregion
    #region CB_Function(lotteryButtons)
    private void OnBackClicked()
    {
        Debug.Log("Back button clicked");
        CameraController.Instance.SwitchBetween("lobby");
        switchMainMenu();
    }
    private void OnBindWalletClicked()
    {
        WalletManager.ConnectWalletOnclick();
    }
    private void OnChooseSkinClicked()
    {
        Debug.Log("Choose Skin button clicked");
    }

    #endregion

    #region UISwitch
    private void switchMainMenu() 
    {
        _lobbyGroup.style.display = DisplayStyle.Flex; 

        _settingsGroup.style.display = DisplayStyle.None;
    }
    private void switchSettingMenu()
    {
        _settingsGroup.style.display = DisplayStyle.Flex;

        _lobbyGroup.style.display = DisplayStyle.None;
    }
    #endregion

   
}
