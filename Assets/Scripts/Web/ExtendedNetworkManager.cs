using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 扩展的NetworkManager，集成场景加载追踪功能
/// </summary>
public class ExtendedNetworkManager : NetworkManager
{
    [Header("场景加载追踪")]
    public GameObject sceneLoadTrackerPrefab;
    
    private SceneLoadTracker sceneLoadTracker;
    
    public override void Start()
    {
        base.Start();
        
        // 创建场景加载追踪器
        if (sceneLoadTrackerPrefab != null)
        {
            GameObject trackerObj = Instantiate(sceneLoadTrackerPrefab);
            sceneLoadTracker = trackerObj.GetComponent<SceneLoadTracker>();
            
            if (sceneLoadTracker != null)
            {
                // 监听场景加载完成事件
                sceneLoadTracker.OnAllPlayersLoaded.AddListener(OnAllPlayersSceneLoaded);
                sceneLoadTracker.OnSceneLoadTimeout.AddListener(OnSceneLoadTimeout);
            }
        }
    }
    
    /// <summary>
    /// 重写ServerChangeScene以集成追踪功能
    /// </summary>
    public override void ServerChangeScene(string newSceneName)
    {
        Debug.Log($"[ExtendedNetworkManager] 开始切换场景: {newSceneName}");
        
        // 开始追踪场景加载
        if (sceneLoadTracker != null && NetworkServer.active)
        {
            sceneLoadTracker.StartTrackingSceneLoad(newSceneName);
        }
        
        // 调用基类方法执行实际的场景切换
        base.ServerChangeScene(newSceneName);
    }
    
    /// <summary>
    /// 服务器场景切换完成回调
    /// </summary>
    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log($"[ExtendedNetworkManager] 服务器场景切换完成: {sceneName}");
        base.OnServerSceneChanged(sceneName);
        
        // 服务器场景加载完成，但还需要等待客户端
        // 追踪器会自动处理客户端加载状态
    }
    
    /// <summary>
    /// 客户端场景切换完成回调
    /// </summary>
    public override void OnClientSceneChanged()
    {
        Debug.Log($"[ExtendedNetworkManager] 客户端场景切换完成: {networkSceneName}");
        base.OnClientSceneChanged();
        
        // 通知服务器客户端场景加载完成
        if (sceneLoadTracker != null && NetworkClient.isConnected)
        {
            // 延迟一帧确保场景完全加载
            StartCoroutine(NotifySceneLoadedDelayed());
        }
    }
    
    /// <summary>
    /// 延迟通知场景加载完成
    /// </summary>
    private System.Collections.IEnumerator NotifySceneLoadedDelayed()
    {
        yield return null; // 等待一帧
        
        if (sceneLoadTracker != null)
        {
            sceneLoadTracker.NotifySceneLoaded(networkSceneName);
        }
    }
    
    /// <summary>
    /// 所有玩家场景加载完成的回调
    /// </summary>
    private void OnAllPlayersSceneLoaded(string sceneName)
    {
        Debug.Log($"[ExtendedNetworkManager] 所有玩家场景加载完成: {sceneName}");
        
        // 在这里可以执行游戏开始逻辑
        if (NetworkServer.active)
        {
            // 服务器端处理
            OnServerAllPlayersReady(sceneName);
        }
        
        // 客户端和服务器都会收到这个回调
        OnAllPlayersReady(sceneName);
    }
    
    /// <summary>
    /// 场景加载超时的回调
    /// </summary>
    private void OnSceneLoadTimeout(string sceneName)
    {
        Debug.LogError($"[ExtendedNetworkManager] 场景加载超时: {sceneName}");
        
        // 可以选择强制继续或处理超时情况
        if (NetworkServer.active)
        {
            OnServerSceneLoadTimeout(sceneName);
        }
    }
    
    /// <summary>
    /// 服务器端所有玩家准备就绪
    /// </summary>
    [Server]
    protected virtual void OnServerAllPlayersReady(string sceneName)
    {
        Debug.Log($"[Server] 所有玩家准备就绪，开始游戏: {sceneName}");
        
        // 在这里可以：
        // 1. 生成游戏对象
        // 2. 初始化游戏状态
        // 3. 发送游戏开始消息
        
        // 示例：通知所有客户端游戏开始
        RpcGameStart(sceneName);
    }
    
    /// <summary>
    /// 服务器端场景加载超时处理
    /// </summary>
    [Server]
    protected virtual void OnServerSceneLoadTimeout(string sceneName)
    {
        Debug.LogWarning($"[Server] 处理场景加载超时: {sceneName}");
        
        // 可以选择：
        // 1. 强制开始游戏
        // 2. 断开未加载完成的玩家
        // 3. 重新尝试加载场景
        
        // 示例：强制开始游戏
        if (sceneLoadTracker != null)
        {
            sceneLoadTracker.ForceCompleteSceneLoad();
        }
    }
    
    /// <summary>
    /// 客户端和服务器都会调用的准备就绪回调
    /// </summary>
    protected virtual void OnAllPlayersReady(string sceneName)
    {
        Debug.Log($"[All] 所有玩家准备就绪: {sceneName}");
        
        // 在这里可以执行客户端和服务器都需要的逻辑
        // 例如：启用UI、开始游戏音乐等
    }
    
    /// <summary>
    /// 通知所有客户端游戏开始
    /// </summary>
    //[ClientRpc]
    private void RpcGameStart(string sceneName)
    {
        Debug.Log($"[Client] 收到游戏开始信号: {sceneName}");
        OnGameStart(sceneName);
    }
    
    /// <summary>
    /// 游戏开始的客户端处理
    /// </summary>
    protected virtual void OnGameStart(string sceneName)
    {
        Debug.Log($"[Client] 游戏开始: {sceneName}");
        
        // 在这里执行游戏开始的客户端逻辑
        // 例如：启用玩家控制、显示游戏UI等
    }
    
    /// <summary>
    /// 获取当前场景加载进度
    /// </summary>
    [Server]
    public (int loaded, int total) GetSceneLoadProgress()
    {
        if (sceneLoadTracker != null)
        {
            return sceneLoadTracker.GetLoadProgress();
        }
        return (0, 0);
    }
    
    /// <summary>
    /// 强制完成场景加载
    /// </summary>
    [Server]
    public void ForceCompleteSceneLoad()
    {
        if (sceneLoadTracker != null)
        {
            sceneLoadTracker.ForceCompleteSceneLoad();
        }
    }
}
