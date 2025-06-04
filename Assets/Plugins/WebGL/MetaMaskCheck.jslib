mergeInto(LibraryManager.library, {
    CheckMetaMaskInstalled: function() {
        // 檢查 window.ethereum 是否存在且是 MetaMask
        var isMetaMask = !!(window.ethereum && window.ethereum.isMetaMask);
        return isMetaMask;
    }
});