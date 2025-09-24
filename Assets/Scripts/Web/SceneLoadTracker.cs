using Mirror;
using Mirror.BouncyCastle.Tls;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 场景加载追踪器 - 用于检测所有玩家是否完成场景切换
/// </summary>
public class SceneLoadTracker : NetworkBehaviour
{
    [Header("场景加载追踪")]
    [Tooltip("场景加载超时时间（秒）")]
    public float loadTimeout = 30f;
    
    [Header("事件")]
    public UnityEvent<string> OnAllPlayersLoaded = new UnityEvent<string>();
    public UnityEvent<string> OnSceneLoadTimeout = new UnityEvent<string>();
    
    // 服务器端数据
    private readonly Dictionary<NetworkConnectionToClient, bool> playerLoadStatus = new Dictionary<NetworkConnectionToClient, bool>();
    private string currentSceneName;
    private float loadStartTime;
    private bool isTrackingLoad = false;
    
    // 客户端状态
    private bool hasNotifiedServer = false;
    
    public static SceneLoadTracker Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    #region Server Methods
    
    /// <summary>
    /// 服务器开始追踪场景加载
    /// </summary>
    [Server]
    public void StartTrackingSceneLoad(string sceneName)
    {
        Debug.Log($"[Server] 开始追踪场景加载: {sceneName}");
        
        currentSceneName = sceneName;
        loadStartTime = Time.time;
        isTrackingLoad = true;
        hasNotifiedServer = false;
        
        // 清空之前的加载状态
        playerLoadStatus.Clear();
        
        // 为所有连接的玩家初始化加载状态
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn != null && conn.isReady)
            {
                playerLoadStatus[conn] = false;
                Debug.Log($"[Server] 添加玩家到加载追踪: {conn.connectionId}");
            }
        }
        
        Debug.Log($"[Server] 需要等待 {playerLoadStatus.Count} 个玩家加载场景");
        
        // 如果没有玩家需要等待，直接完成
        if (playerLoadStatus.Count == 0)
        {
            OnAllPlayersLoadedInternal();
        }
    }
    
    /// <summary>
    /// 服务器接收客户端加载完成通知
    /// </summary>
    /*[Command(requiresAuthority = false)]*/
    public void NotifySceneLoadedServerRpc(string sceneName, ServerSrpParams serverRpcParams = default)
    {
        //var senderConnection = serverRpcParams.Receive.SenderConnection as NetworkConnectionToClient;
        NetworkConnectionToClient senderConnection = null;
        if (senderConnection == null)
        {
            Debug.LogError("[Server] 无法获取发送者连接");
            return;
        }
        
        Debug.Log($"[Server] 收到玩家 {senderConnection.connectionId} 场景加载完成通知: {sceneName}");
        
        // 检查是否是当前正在追踪的场景
        if (!isTrackingLoad || sceneName != currentSceneName)
        {
            Debug.LogWarning($"[Server] 场景名称不匹配或未在追踪加载. 当前: {currentSceneName}, 收到: {sceneName}");
            return;
        }
        
        // 更新玩家加载状态
        if (playerLoadStatus.ContainsKey(senderConnection))
        {
            playerLoadStatus[senderConnection] = true;
            Debug.Log($"[Server] 玩家 {senderConnection.connectionId} 标记为已加载");
            
            // 检查是否所有玩家都已加载完成
            CheckAllPlayersLoaded();
        }
        else
        {
            Debug.LogWarning($"[Server] 玩家 {senderConnection.connectionId} 不在追踪列表中");
        }
    }
    
    /// <summary>
    /// 检查所有玩家是否都已加载完成
    /// </summary>
    [Server]
    private void CheckAllPlayersLoaded()
    {
        int totalPlayers = playerLoadStatus.Count;
        int loadedPlayers = playerLoadStatus.Values.Count(loaded => loaded);
        
        Debug.Log($"[Server] 场景加载进度: {loadedPlayers}/{totalPlayers}");
        
        if (loadedPlayers >= totalPlayers && totalPlayers > 0)
        {
            OnAllPlayersLoadedInternal();
        }
    }
    
    /// <summary>
    /// 所有玩家加载完成的内部处理
    /// </summary>
    [Server]
    private void OnAllPlayersLoadedInternal()
    {
        if (!isTrackingLoad) return;
        
        float loadTime = Time.time - loadStartTime;
        Debug.Log($"[Server] 所有玩家场景加载完成! 场景: {currentSceneName}, 耗时: {loadTime:F2}秒");
        
        isTrackingLoad = false;
        
        // 通知所有客户端场景加载完成
        NotifyAllPlayersLoadedClientRpc(currentSceneName);
        
        // 触发服务器端事件
        OnAllPlayersLoaded?.Invoke(currentSceneName);
    }
    
    /// <summary>
    /// 通知所有客户端场景加载完成
    /// </summary>
    [ClientRpc]
    private void NotifyAllPlayersLoadedClientRpc(string sceneName)
    {
        Debug.Log($"[Client] 收到所有玩家场景加载完成通知: {sceneName}");
        OnAllPlayersLoaded?.Invoke(sceneName);
    }
    
    /// <summary>
    /// 服务器更新 - 检查超时
    /// </summary>
    void Update()
    {
        if (isServer && isTrackingLoad)
        {
            // 检查加载超时
            if (Time.time - loadStartTime > loadTimeout)
            {
                Debug.LogWarning($"[Server] 场景加载超时! 场景: {currentSceneName}");
                HandleLoadTimeout();
            }
        }
    }
    
    /// <summary>
    /// 处理加载超时
    /// </summary>
    [Server]
    private void HandleLoadTimeout()
    {
        Debug.LogError($"[Server] 场景加载超时: {currentSceneName}");
        
        // 记录未加载完成的玩家
        foreach (var kvp in playerLoadStatus)
        {
            if (!kvp.Value)
            {
                Debug.LogError($"[Server] 玩家 {kvp.Key.connectionId} 未完成场景加载");
            }
        }
        
        isTrackingLoad = false;
        
        // 触发超时事件
        OnSceneLoadTimeout?.Invoke(currentSceneName);
        
        // 可以选择强制继续游戏或断开未加载完成的玩家
        // ForceCompleteSceneLoad();
    }
    
    /// <summary>
    /// 强制完成场景加载（可选）
    /// </summary>
    [Server]
    public void ForceCompleteSceneLoad()
    {
        Debug.Log($"[Server] 强制完成场景加载: {currentSceneName}");
        OnAllPlayersLoadedInternal();
    }
    
    /// <summary>
    /// 获取加载进度
    /// </summary>
    [Server]
    public (int loaded, int total) GetLoadProgress()
    {
        if (!isTrackingLoad) return (0, 0);
        
        int total = playerLoadStatus.Count;
        int loaded = playerLoadStatus.Values.Count(l => l);
        return (loaded, total);
    }
    
    #endregion
    
    #region Client Methods
    
    /// <summary>
    /// 客户端通知服务器场景加载完成
    /// </summary>
    [Client]
    public void NotifySceneLoaded(string sceneName)
    {
        if (hasNotifiedServer)
        {
            Debug.LogWarning($"[Client] 已经通知过服务器场景加载完成: {sceneName}");
            return;
        }
        
        Debug.Log($"[Client] 通知服务器场景加载完成: {sceneName}");
        hasNotifiedServer = true;
        
        NotifySceneLoadedServerRpc(sceneName);
    }
    
    /// <summary>
    /// 重置客户端通知状态
    /// </summary>
    [Client]
    public void ResetNotificationStatus()
    {
        hasNotifiedServer = false;
    }
    
    #endregion
    
    #region Network Callbacks
    
    public override void OnStartServer()
    {
        Debug.Log("[Server] SceneLoadTracker 服务器启动");
    }
    
    public override void OnStartClient()
    {
        Debug.Log("[Client] SceneLoadTracker 客户端启动");
    }
    
    public override void OnStopServer()
    {
        playerLoadStatus.Clear();
        isTrackingLoad = false;
    }
    
    #endregion
}
