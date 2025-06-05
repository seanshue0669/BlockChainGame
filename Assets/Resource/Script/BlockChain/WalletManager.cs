using System.Runtime.InteropServices;
using System;
using Thirdweb.Unity;
using Thirdweb;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections.Generic;
using Nethereum.Contracts.Standards.ERC1155.ContractDefinition;

public static class WalletManager
{
    [DllImport("__Internal")]
    private static extern bool CheckMetaMaskInstalled();

    const int units = 18;
    const string tokenAddress = "0x38749c72A58bF216bd1F36FC41f71339E9E94928";
    const string nftAddress = "0xB58a5f39FABE96D6eA3DC17B0CAe814411207Cf0";

    public static event Action<decimal> OnBalanceChanged;

    public static string address = "";
    public static decimal balance = 0;
    public static IThirdwebWallet wallet;

    public static ThirdwebContract tokenContract;
    public static ThirdwebContract nftContract;

    public static List<(string rarity, ulong wear)> myNftStats;

    public static bool isBind = false;

    public static async void ConnectWalletOnclick()
    {
        try
        {
            await ConnectWallet();
            await GetAddress();
            await InitializeContract();
            await GetBalance();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Operation failed: {ex.Message}\n{ex.StackTrace}");
        }
    }

    public static async void GetReward(long randomAmount)
    {
        try
        {
            Debug.Log($"準備鑄造隨機數量的代幣: {randomAmount}");

            await MintTokens(randomAmount);
            await GetBalanceWithRetry(10, 1f);
        }
        catch (Exception ex)
        {
            Debug.LogError($"PlayOnClick 失敗: {ex.Message}\n{ex.StackTrace}");
            Debug.Log($"Error: {ex.Message}");
        }
    }

    public static async Task Lottery(string SkinIconURL, long itemId, int rarity, ulong wear)
    {
        await MintNFT(SkinIconURL, itemId, rarity, wear);
    }

    private static async Task ConnectWallet()
    {

        try
        {
            bool isMetaMaskInstalled = false;

            if (Application.platform == RuntimePlatform.WebGLPlayer && !Application.isEditor)
            {
                isMetaMaskInstalled = CheckMetaMaskInstalled();
            }
            else
            {
                Debug.Log("MetaMask check is only available in WebGL builds. Using WalletConnect.");
            }

            WalletOptions options;

            if (isMetaMaskInstalled)
            {
                options = new WalletOptions(
                    provider: WalletProvider.MetaMaskWallet,
                    chainId: 11155111
                );
                Debug.Log("Connecting to MetaMask...");
            }
            else
            {
                options = new WalletOptions(
                    provider: WalletProvider.WalletConnectWallet,
                    chainId: 11155111
                );
                Debug.Log("MetaMask not detected. Connecting with WalletConnect...");
            }

            wallet = await ThirdwebManager.Instance.ConnectWallet(options);
            Debug.Log("Connected successfully");

            isBind = true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Wallet connection failed: {ex.Message}\n{ex.StackTrace}");
            Debug.Log($"Connection failed: {ex.Message}");
            throw;
        }
    }

    private static async Task GetAddress()
    {
        if (wallet == null)
        {
            Debug.Log("Wallet is NULL");
            return;
        }

        address = await wallet.GetAddress();
        Debug.Log("Wallet Address: " + address);
    }

    private static async Task InitializeContract()
    {
        if (tokenContract == null)
        {
            try
            {
                TextAsset abiAsset = Resources.Load<TextAsset>("ABI/GameTokenABI");
                if (abiAsset == null)
                {
                    throw new Exception("無法載入代幣 ABI 檔案，請確認 Assets/Resources/ABI/GameTokenABI.json 是否存在");
                }

                string abiJson = abiAsset.text;
                Debug.Log($"載入的代幣 ABI: {abiJson}");

                tokenContract = await ThirdwebManager.Instance.GetContract(tokenAddress, 11155111, abiJson);
                Debug.Log("Token contract initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError($"代幣合約初始化失敗: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
        else
        {
            Debug.Log("Token contract already initialized");
        }

        if (nftContract == null)
        {
            try
            {
                TextAsset abiAsset = Resources.Load<TextAsset>("ABI/SkinNFTABI");
                if (abiAsset == null)
                {
                    throw new Exception("無法載入 NFT ABI 檔案，請確認 Assets/Resources/ABI/SkinNFTABI.json 是否存在");
                }

                string abiJson = abiAsset.text;
                Debug.Log($"載入的 NFT ABI: {abiJson}");

                nftContract = await ThirdwebManager.Instance.GetContract(nftAddress, 11155111, abiJson);
                Debug.Log("NFT contract initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError($"NFT 合約初始化失敗: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
        else
        {
            Debug.Log("NFT contract already initialized");
        }
    }

    private static async Task GetBalance()
    {
        if (wallet == null)
        {
            Debug.Log("Wallet is NULL");
            return;
        }

        if (address == "")
        {
            Debug.Log("Address is NULL");
            return;
        }

        try
        {
            if (tokenContract == null)
            {
                await InitializeContract();
            }

            BigInteger raw = await tokenContract.Read<BigInteger>("balanceOf", address);

            decimal divisor = (decimal)Math.Pow(10, units);
            decimal newBalance = (decimal)raw / divisor;

            if (newBalance != balance)
            {
                balance = newBalance;
                Debug.Log($"Balance: {balance}");

                // 🔔 觸發事件
                OnBalanceChanged?.Invoke(balance);
            }
            Debug.Log($"Balance: {balance}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"獲取餘額失敗: {ex.Message}\n{ex.StackTrace}");
            Debug.Log($"Error: {ex.Message}");
        }
    }

    public static async Task GetBalanceWithRetry(int maxRetries, float delaySeconds)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            decimal previousBalance = balance;
            await GetBalance();

            if (balance != previousBalance)
            {
                Debug.Log($"Balance updated successfully after {i + 1} attempt(s)");
                return;
            }

            Debug.Log($"Balance not updated, retrying in {delaySeconds} seconds...");
            await Task.Delay((int)(delaySeconds * 1000));
        }

        Debug.LogWarning($"Failed to update balance after {maxRetries} retries");
    }

    private static async Task MintTokens(long amount)
    {
        if (address == "")
        {
            Debug.Log("Address is NULL");
            return;
        }

        if (amount <= 0)
        {
            Debug.Log("Amount must be greater than zero");
            return;
        }

        try
        {
            if (tokenContract == null)
            {
                await InitializeContract();
            }

            if (wallet == null)
            {
                Debug.Log("Wallet is NULL");
                return;
            }

            BigInteger amountInWei = amount * BigInteger.Pow(10, units);

            var receipt = await tokenContract.Write(
                wallet,
                "mint",
                BigInteger.Zero,
                address,
                amountInWei
            );

            Debug.Log($"🛠️ 鑄造完成，交易哈希: {receipt}");
            Debug.Log("Mint Successfully!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ 鑄造失敗: {ex.Message}\n{ex.StackTrace}");
            Debug.Log($"Mint failed: {ex.Message}");
        }
    }

    public static async Task<bool> Burn(ulong COST_TOKENS)
    {
        BigInteger COST_WEI = BigInteger.Multiply(new BigInteger(COST_TOKENS), BigInteger.Pow(10, units));

        if (WalletManager.balance < COST_TOKENS)
        {
            Debug.LogError($"餘額不足 {COST_TOKENS} GameToken");
            return false;
        }

        try
        {
            // 使用與 MintTokens 相同的模式
            var receipt = await WalletManager.tokenContract.Write(
                WalletManager.wallet,
                "transfer",                                    // ERC20 transfer function
                BigInteger.Zero,                               // value (ETH amount)
                "0x000000000000000000000000000000000000dEaD", // to address (burn address)
                COST_WEI                                       // amount in Wei
            );

            Debug.Log($"扣款完成，交易哈希: {receipt}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"扣款失敗：{ex.Message}");
            Debug.LogError($"Transfer failed: {ex.Message}\n{ex.StackTrace}");
            return false;
        }

        return true;
    }

    private static async Task MintNFT(string tokenURI, long itemId, int rarity, ulong wear)
    {
        if (wallet == null)
        {
            Debug.Log("Wallet is NULL");
            return;
        }

        if (address == "")
        {
            Debug.Log("Address is NULL");
            return;
        }

        if (string.IsNullOrEmpty(tokenURI))
        {
            Debug.Log("TokenURI is NULL or empty");
            return;
        }

        if (rarity < 0 || rarity > 2)
        {
            Debug.Log("Invalid rarity value. Must be 0 (Common), 1 (Rare), or 2 (Legendary)");
            return;
        }

        if (wear > 1000)
        {
            Debug.Log("Wear must be between 0 and 1000");
            return;
        }

        try
        {
            if (nftContract == null)
            {
                await InitializeContract();
            }

            // 根據合約定義，使用正確的參數類型
            var receipt = await nftContract.Write(
                wallet,
                "mint",  // 合約中確實是大寫 Mint
                BigInteger.Zero,
                address,                    // to address
                tokenURI,                   // string memory uri
                new BigInteger(itemId),     // int256 itemId (在 C# 中用 BigInteger)
                rarity,                     // Rarity enum (直接傳 int，0=Common, 1=Rare, 2=Legendary)
                new BigInteger(wear)        // uint256 wear
            );

            Debug.Log($"🛠️ NFT 鑄造完成，交易哈希: {receipt}");
            Debug.Log($"NFT Minted Successfully! ItemID: {itemId}, Rarity: {(rarity == 0 ? "Common" : rarity == 1 ? "Rare" : "Legendary")}, Wear: {wear}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ NFT 鑄造失敗: {ex.Message}\n{ex.StackTrace}");

            // 提供更詳細的錯誤信息
            if (ex.Message.Contains("execution reverted"))
            {
                Debug.LogError("可能的原因：");
                Debug.LogError("1. 當前連接的錢包不是合約的 Owner");
                Debug.LogError("2. wear 值超過 1000");
                Debug.LogError("3. 合約可能處於暫停狀態");
                Debug.LogError($"當前錢包地址: {address}");

                // 嘗試獲取合約擁有者信息進行對比
                try
                {
                    var owner = await nftContract.Read<string>("owner");
                    Debug.LogError($"合約擁有者地址: {owner}");
                    if (address.ToLower() != owner.ToLower())
                    {
                        Debug.LogError("❌ 錯誤確認：當前錢包不是合約擁有者，無法鑄造 NFT！");
                        return;
                    }
                }
                catch (Exception ownerEx)
                {
                    Debug.LogError($"無法獲取合約擁有者信息: {ownerEx.Message}");
                }
            }
        }
    }


    public static async Task GetMyNftRarityAndWear()
    {
        // === 前置檢查 ===
        if (wallet == null || string.IsNullOrEmpty(address))
        {
            Debug.LogError("請先呼叫 ConnectWalletOnclick() 連上錢包");
            return;
        }

        if (nftContract == null)
            await InitializeContract();           // 確保已載入 NFT 合約

        // === 1️⃣ 取得自己擁有的 tokenId 陣列 ===
        // getTokensByOwner(address owner) → uint256[]
        List<BigInteger> tokenIdList;
        try
        {
            tokenIdList = await nftContract.Read<List<BigInteger>>(
                "getTokensByOwner",
                address                              // Solidity address -> C# string
            );
        }
        catch (Exception ex)
        {
            Debug.LogError($"讀取 tokenId 陣列失敗: {ex.Message}");
            return;
        }

        // 如果沒有 NFT，直接回傳空清單
        if (tokenIdList == null || tokenIdList.Count == 0)
            return;

        // === 2️⃣ 逐顆查詢 rarity 與 wear ===
        var result = new List<(string rarity, ulong wear)>(tokenIdList.Count);

        foreach (BigInteger tokenIdBI in tokenIdList)
        {
            ulong wearValue = 0;
            string rarityStr = "";

            try
            {
                // getRarity(uint256 tokenId) → string
                rarityStr = await nftContract.Read<string>("getRarity", tokenIdBI);

                // getWear(uint256 tokenId) → uint256
                BigInteger wearBI = await nftContract.Read<BigInteger>("getWear", tokenIdBI);
                wearValue = (ulong)wearBI;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"讀取 tokenId={tokenIdBI} 資料時失敗: {ex.Message}");
                continue;   // 跳過錯誤項
            }

            result.Add((rarityStr, wearValue));
        }

        myNftStats = result;

        for (int i = 0; i < WalletManager.myNftStats.Count; i++)
        {
            var stat = WalletManager.myNftStats[i];
            Debug.Log($"#{i + 1} 稀有度: {stat.rarity} | 磨損值: {stat.wear}");
        }
    }
}
