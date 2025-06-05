

using System.Collections.Generic;
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

    private VisualElement _settingsGroup;

    private Button _backButton;
    private Button _bindWalletButton;
    private Button _chooseSkinButton;

    private VisualElement _rightPanelGroup;

    private DropdownField _skinSelector;

    [SerializeField]
    public SkinSO skinSO;
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

        _rightPanelGroup = root.Q<VisualElement>("right-panel");
        _rightPanelGroup.style.display = DisplayStyle.None;


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


        //settings
        _backButton       = root.Q<Button>("BackButton");
        _backButton?.RegisterCallback<ClickEvent>(evt => OnBackClicked());

        _bindWalletButton = root.Q<Button>("BindWalletButton");
        _bindWalletButton?.RegisterCallback<ClickEvent>(evt => OnBindWalletClicked());

        _chooseSkinButton = root.Q<Button>("ChooseSkinButton");
        _chooseSkinButton?.RegisterCallback<ClickEvent>(evt => OnChooseSkinClicked());

        _skinSelector = root.Q<DropdownField>("SkinSelector");
        _skinSelector.RegisterValueChangedCallback(evt =>
        {
            int index = _skinSelector.index;
            if (index >= 0 && index < WalletManager.myNftStats.Count)
            {
                var (rarityStr, wear) = WalletManager.myNftStats[index];
                skinSO.rarity = Convert2Int(rarityStr);
                skinSO.wear = wear / 100f;
                SkinManager.Instance.ApplySkinChange();

                Debug.Log($"Applied skin {index}: {rarityStr}, wear: {wear}");
            }
        });
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
        bool isVisible = _rightPanelGroup.style.display == DisplayStyle.Flex;
        _rightPanelGroup.style.display = isVisible ? DisplayStyle.None : DisplayStyle.Flex;

        if (!isVisible&& WalletManager.isBind)
        {
            var nftStats = WalletManager.myNftStats;

            if (nftStats == null || nftStats.Count == 0)
            {
                Debug.LogWarning("myNftStats is empty");
                _skinSelector.choices = new List<string> { "no skin" };
                _skinSelector.value = "no skin";
                return;
            }

            var choices = new List<string>();
            for (int i = 0; i < nftStats.Count; i++)
            {
                var (rarity, wearValue) = nftStats[i];
                choices.Add($"{i}: {rarity} / wear: {wearValue}");
            }

            _skinSelector.choices = choices;
            _skinSelector.index = 0;
            _skinSelector.value = choices[0];

            var (rarityStr, wear) = nftStats[0];
            skinSO.rarity = Convert2Int(rarityStr);
            skinSO.wear = wear / 100f;
            SkinManager.Instance.ApplySkinChange();

            Debug.Log($"Loaded {choices.Count} skin options.");
        }
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
    private int Convert2Int(string rarityStr)
    {
        switch (rarityStr.ToLower())
        {
            case "common":
                return 0;
            case "rare":
                return 1;
            case "epic":
                return 2;
            default:
                return 0; // fallback 
        }
    }


}
