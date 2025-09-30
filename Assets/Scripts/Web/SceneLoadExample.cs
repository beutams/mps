using Mirror;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 场景加载示例 - 演示如何使用SceneLoadTracker
/// </summary>
public class SceneLoadExample : NetworkBehaviour
{
    [Header("UI组件")]
    public Button changeSceneButton;
    public Text progressText;
    public Text statusText;
    
    [Header("场景设置")]
    public string targetScene = "GameScene";
    
    private SceneLoadTracker sceneTracker;
    
    void Start()
    {
        // 获取场景加载追踪器
        sceneTracker = SceneLoadTracker.Instance;
        
        if (sceneTracker != null)
        {
            // 监听事件
            sceneTracker.OnAllPlayersLoaded.AddListener(OnAllPlayersLoaded);
            sceneTracker.OnSceneLoadTimeout.AddListener(OnSceneLoadTimeout);
        }
        
        // 设置按钮事件
        if (changeSceneButton != null)
        {
            changeSceneButton.onClick.AddListener(OnChangeSceneClicked);
            // 只有服务器可以切换场景
            changeSceneButton.interactable = NetworkServer.active;
        }
        
        UpdateUI();
    }
    
    void Update()
    {
        UpdateUI();
    }
    
    /// <summary>
    /// 更新UI显示
    /// </summary>
    void UpdateUI()
    {
        if (sceneTracker == null) return;
        
        if (NetworkServer.active)
        {
            var (loaded, total) = sceneTracker.GetLoadProgress();
            
            if (progressText != null)
            {
                progressText.text = $"加载进度: {loaded}/{total}";
            }
            
            if (statusText != null)
            {
                if (total > 0)
                {
                    statusText.text = loaded >= total ? "所有玩家已加载完成" : "等待玩家加载中...";
                    statusText.color = loaded >= total ? Color.green : Color.yellow;
                }
                else
                {
                    statusText.text = "未在追踪场景加载";
                    statusText.color = Color.gray;
                }
            }
        }
        else
        {
            if (progressText != null)
            {
                progressText.text = "客户端模式";
            }
            
            if (statusText != null)
            {
                statusText.text = NetworkClient.isConnected ? "已连接到服务器" : "未连接";
                statusText.color = NetworkClient.isConnected ? Color.green : Color.red;
            }
        }
    }
    
    /// <summary>
    /// 切换场景按钮点击事件
    /// </summary>
    public void OnChangeSceneClicked()
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning("只有服务器可以切换场景");
            return;
        }
        
        Debug.Log($"开始切换场景到: {targetScene}");
        NetworkManager.singleton.ServerChangeScene(targetScene);
    }
    
    /// <summary>
    /// 所有玩家加载完成回调
    /// </summary>
    private void OnAllPlayersLoaded(string sceneName)
    {
        Debug.Log($"[SceneLoadExample] 所有玩家加载完成: {sceneName}");
        
        if (statusText != null)
        {
            statusText.text = $"场景 {sceneName} 加载完成!";
            statusText.color = Color.green;
        }
        
        // 在这里可以执行游戏开始逻辑
        StartGame(sceneName);
    }
    
    /// <summary>
    /// 场景加载超时回调
    /// </summary>
    private void OnSceneLoadTimeout(string sceneName)
    {
        Debug.LogError($"[SceneLoadExample] 场景加载超时: {sceneName}");
        
        if (statusText != null)
        {
            statusText.text = $"场景 {sceneName} 加载超时!";
            statusText.color = Color.red;
        }
        
        // 可以选择强制继续或重新加载
        if (NetworkServer.active)
        {
            // 询问是否强制继续
            ShowForceCompleteDialog(sceneName);
        }
    }
    
    /// <summary>
    /// 开始游戏
    /// </summary>
    private void StartGame(string sceneName)
    {
        Debug.Log($"[SceneLoadExample] 开始游戏: {sceneName}");
        
        // 在这里执行游戏开始的逻辑
        // 例如：
        // - 生成玩家角色
        // - 初始化游戏状态
        // - 启用游戏UI
        // - 开始游戏循环
        
        if (NetworkServer.active)
        {
            // 服务器端逻辑
            RpcNotifyGameStart(sceneName);
        }
    }
    
    /// <summary>
    /// 通知所有客户端游戏开始
    /// </summary>
    [ClientRpc]
    private void RpcNotifyGameStart(string sceneName)
    {
        Debug.Log($"[Client] 收到游戏开始通知: {sceneName}");
        
        // 客户端游戏开始逻辑
        OnClientGameStart(sceneName);
    }
    
    /// <summary>
    /// 客户端游戏开始处理
    /// </summary>
    private void OnClientGameStart(string sceneName)
    {
        Debug.Log($"[Client] 客户端游戏开始: {sceneName}");
        
        // 客户端特定的游戏开始逻辑
        // 例如：启用玩家输入、显示游戏UI等
    }
    
    /// <summary>
    /// 显示强制完成对话框
    /// </summary>
    private void ShowForceCompleteDialog(string sceneName)
    {
        // 这里可以显示一个UI对话框询问是否强制继续
        // 为了简化示例，直接在控制台显示选项
        Debug.LogWarning($"场景 {sceneName} 加载超时，是否强制继续？");
        Debug.LogWarning("在控制台输入 'force' 来强制继续游戏");
        
        // 实际项目中应该显示UI对话框
    }
    
    /// <summary>
    /// 强制完成场景加载（用于调试）
    /// </summary>
    [ContextMenu("强制完成场景加载")]
    public void ForceCompleteSceneLoad()
    {
        if (NetworkServer.active && sceneTracker != null)
        {
            Debug.Log("强制完成场景加载");
            sceneTracker.ForceCompleteSceneLoad();
        }
    }
    
    /// <summary>
    /// 获取当前加载进度（用于调试）
    /// </summary>
    [ContextMenu("显示加载进度")]
    public void ShowLoadProgress()
    {
        if (NetworkServer.active && sceneTracker != null)
        {
            var (loaded, total) = sceneTracker.GetLoadProgress();
            Debug.Log($"当前加载进度: {loaded}/{total}");
        }
    }
}

