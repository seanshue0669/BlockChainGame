using TMPro;
using UnityEngine;

public class BalanceTextListener : MonoBehaviour
{
    [SerializeField] private TMP_Text balanceText;   // Inspector 直接拖入 TextMeshPro 物件

    private void OnEnable()
    {
        WalletManager.OnBalanceChanged += HandleBalanceChanged;

        // 若 WalletManager 在載入此場景前就已經抓過餘額，立刻顯示一次
        if (WalletManager.balance >= 0)
            HandleBalanceChanged(WalletManager.balance);
    }

    private void OnDisable()
    {
        WalletManager.OnBalanceChanged -= HandleBalanceChanged;
    }

    private void HandleBalanceChanged(decimal newBalance)
    {
        // 這裡可以依需求決定顯示格式
        balanceText.text = $"Balance: {newBalance}";
    }
}
