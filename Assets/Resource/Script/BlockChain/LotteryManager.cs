using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Thirdweb;

public class LotteryManager : MonoBehaviour
{
    private const ulong lotteryCost = 1000;
    private const string SkinIconURL = "ipfs://bafkreidknaaddxzhzvll6b5z66oxbgepc5ziipim6xv56vfio2zsa3d7ey";

    public TMP_Text Info;
    public Button Lottery;

    public async void LotteryOnClick()
    {
        // 0. 錢包檢查
        if (WalletManager.wallet == null || string.IsNullOrEmpty(WalletManager.address))
        {
            Info.text = "Connect wallet first";
            return;
        }

        // 1. 餘額檢查（WalletManager.balance 已是「代幣顆數」而非 Wei）
        if (WalletManager.balance < lotteryCost)
        {
            Info.text = $"Have no enough GameToken";
            return;
        }

        // 2. 扣款─直接丟進黑洞
        Info.text = $"Deducting {lotteryCost} GameToken…";
        bool burnSuccess = await WalletManager.Burn(lotteryCost);

        if (!burnSuccess)
        {
            Info.text = "Deduct failed";
            return;
        }

        await WalletManager.GetBalanceWithRetry(10, 1f);

        // 3. 決定隨機屬性
        int rarity = DecideRarity();
        ulong wear = DecideWear();
        long itemId = rarity + 1;

        // 4. 鑄造 NFT
        Info.text = "Minting NFT…";
        await WalletManager.Lottery(SkinIconURL, itemId, rarity, wear);

        Info.text = $"you got a nft with ItemID={itemId}, Rarity={GetRarityName(rarity)},and Wear={wear}";
    }

    public void BackOnClick()
    {
        SceneController.Instance.SwitchToScene("MainMenu");
    }

    private int DecideRarity()
    {
        int rarity = 0;
        int lots = UnityEngine.Random.Range(0, 1001);
        if (lots <= 500)        // 0-500 (50.1%)
            rarity = 0;         // Common
        else if (lots <= 800)   // 501-800 (30%)  
            rarity = 1;         // Rare
        else                    // 801-1000 (19.9%)
            rarity = 2;         // Legendary
        return rarity;
    }

    private ulong DecideWear()
    {
        return (ulong)UnityEngine.Random.Range(0, 1001);
    }

    private string GetRarityName(int rarity)
    {
        return rarity switch
        {
            0 => "Common",
            1 => "Rare",
            2 => "Legendary",
            _ => "Unknown"
        };
    }
}