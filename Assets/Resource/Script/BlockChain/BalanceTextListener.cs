using TMPro;
using UnityEngine;

public class BalanceTextListener : MonoBehaviour
{
    [SerializeField] private TMP_Text balanceText;   // Inspector ������J TextMeshPro ����

    private void OnEnable()
    {
        WalletManager.OnBalanceChanged += HandleBalanceChanged;

        // �Y WalletManager �b���J�������e�N�w�g��L�l�B�A�ߨ���ܤ@��
        if (WalletManager.balance >= 0)
            HandleBalanceChanged(WalletManager.balance);
    }

    private void OnDisable()
    {
        WalletManager.OnBalanceChanged -= HandleBalanceChanged;
    }

    private void HandleBalanceChanged(decimal newBalance)
    {
        // �o�̥i�H�̻ݨD�M�w��ܮ榡
        balanceText.text = $"Balance: {newBalance}";
    }
}
