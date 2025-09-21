using UnityEngine;
using UnityEditor;

/// <summary>
/// 子弹方向测试工具
/// 用于测试子弹是否沿着武器方向发射
/// </summary>
public class BulletDirectionTest : EditorWindow
{
    [MenuItem("Tools/Bullet Direction Test")]
    public static void ShowWindow()
    {
        GetWindow<BulletDirectionTest>("子弹方向测试");
    }
    
    private bool enableBulletTracking = false;
    private bool showDebugRays = true;
    
    private void OnGUI()
    {
        GUILayout.Label("子弹方向测试工具", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "这个工具用于测试子弹是否正确沿着武器方向发射。\n\n" +
            "修改内容：\n" +
            "• 简化了Bullet.Init()方法，直接使用武器旋转\n" +
            "• 优化了DirectBullet.Move()，使用transform.forward\n" +
            "• 添加了调试日志显示发射信息\n\n" +
            "测试方法：\n" +
            "1. 运行游戏并装备武器\n" +
            "2. 移动鼠标让武器转向\n" +
            "3. 开火观察子弹方向\n" +
            "4. 查看Console中的调试信息",
            MessageType.Info
        );
        
        GUILayout.Space(10);
        
        if (Application.isPlaying)
        {
            enableBulletTracking = EditorGUILayout.Toggle("启用子弹跟踪", enableBulletTracking);
            showDebugRays = EditorGUILayout.Toggle("显示调试射线", showDebugRays);
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("查找场景中的武器和子弹"))
            {
                FindWeaponsAndBullets();
            }
            
            if (GUILayout.Button("测试武器朝向"))
            {
                TestWeaponDirection();
            }
            
            if (GUILayout.Button("强制发射子弹"))
            {
                ForceFireBullet();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("请运行游戏以使用测试功能", MessageType.Warning);
        }
        
        GUILayout.Space(10);
        
        // 预期行为说明
        GUILayout.Label("预期行为：", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(
            "1. 武器跟踪鼠标方向旋转\n" +
            "2. 子弹从武器位置发射\n" +
            "3. 子弹沿着武器当前朝向飞行\n" +
            "4. 子弹方向 = 武器的transform.forward\n" +
            "5. Console显示发射信息包含正确的旋转值", 
            GUILayout.Height(80)
        );
        
        GUILayout.Space(10);
        
        // 调试信息
        if (Application.isPlaying)
        {
            GUILayout.Label("实时信息：", EditorStyles.boldLabel);
            
            // 显示武器信息
            var weapons = FindObjectsOfType<WeapenModel>();
            foreach (var weapon in weapons)
            {
                if (weapon.weapen != null)
                {
                    Vector3 forward = weapon.transform.forward;
                    EditorGUILayout.LabelField($"武器 {weapon.name}:", 
                        $"角度: {weapon.transform.rotation.eulerAngles.y:F1}°, 朝向: ({forward.x:F2}, {forward.y:F2}, {forward.z:F2})");
                }
            }
            
            // 显示子弹信息
            var bullets = FindObjectsOfType<Bullet>();
            EditorGUILayout.LabelField("活跃子弹数量:", bullets.Length.ToString());
            
            if (bullets.Length > 0 && bullets.Length <= 5) // 只显示前5个子弹的信息
            {
                foreach (var bullet in bullets)
                {
                    Vector3 forward = bullet.transform.forward;
                    EditorGUILayout.LabelField($"子弹 {bullet.name}:", 
                        $"朝向: ({forward.x:F2}, {forward.y:F2}, {forward.z:F2})");
                }
            }
        }
    }
    
    private void OnInspectorUpdate()
    {
        if (enableBulletTracking && Application.isPlaying)
        {
            Repaint();
        }
    }
    
    private void FindWeaponsAndBullets()
    {
        Debug.Log("=== 查找武器和子弹 ===");
        
        // 查找武器
        var weapons = FindObjectsOfType<WeapenModel>();
        Debug.Log($"找到 {weapons.Length} 个武器:");
        
        foreach (var weapon in weapons)
        {
            Vector3 pos = weapon.transform.position;
            Vector3 forward = weapon.transform.forward;
            float angle = weapon.transform.rotation.eulerAngles.y;
            
            string weaponInfo = $"武器: {weapon.name} - 位置: ({pos.x:F1}, {pos.y:F1}, {pos.z:F1})";
            weaponInfo += $" - 角度: {angle:F1}° - 朝向: ({forward.x:F2}, {forward.y:F2}, {forward.z:F2})";
            
            if (weapon.weapen != null)
            {
                weaponInfo += $" - 武器类型: {weapon.weapen.name}";
            }
            
            Debug.Log(weaponInfo);
        }
        
        // 查找子弹
        var bullets = FindObjectsOfType<Bullet>();
        Debug.Log($"找到 {bullets.Length} 个活跃子弹:");
        
        foreach (var bullet in bullets)
        {
            Vector3 pos = bullet.transform.position;
            Vector3 forward = bullet.transform.forward;
            float angle = bullet.transform.rotation.eulerAngles.y;
            
            string bulletInfo = $"子弹: {bullet.name} - 位置: ({pos.x:F1}, {pos.y:F1}, {pos.z:F1})";
            bulletInfo += $" - 角度: {angle:F1}° - 朝向: ({forward.x:F2}, {forward.y:F2}, {forward.z:F2})";
            
            Debug.Log(bulletInfo);
        }
        
        Debug.Log("=== 查找完成 ===");
    }
    
    private void TestWeaponDirection()
    {
        Debug.Log("=== 测试武器朝向 ===");
        
        var weapons = FindObjectsOfType<WeapenModel>();
        
        if (weapons.Length == 0)
        {
            Debug.LogWarning("场景中没有找到武器");
            return;
        }
        
        foreach (var weapon in weapons)
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            Vector3 weaponPos = weapon.transform.position;
            Vector3 directionToMouse = (mouseWorldPos - weaponPos).normalized;
            Vector3 weaponForward = weapon.transform.forward;
            
            float angleToMouse = Mathf.Atan2(directionToMouse.x, directionToMouse.z) * Mathf.Rad2Deg;
            float weaponAngle = weapon.transform.rotation.eulerAngles.y;
            float angleDifference = Mathf.DeltaAngle(weaponAngle, angleToMouse);
            
            Debug.Log($"武器 {weapon.name}:");
            Debug.Log($"  鼠标世界位置: {mouseWorldPos}");
            Debug.Log($"  鼠标方向角度: {angleToMouse:F1}°");
            Debug.Log($"  武器当前角度: {weaponAngle:F1}°");
            Debug.Log($"  角度差: {angleDifference:F1}°");
            Debug.Log($"  武器朝向: {weaponForward}");
        }
        
        Debug.Log("=== 测试完成 ===");
    }
    
    private void ForceFireBullet()
    {
        Debug.Log("=== 强制发射子弹 ===");
        
        var hero = RoomController.instance?.localPlayer?.hero;
        if (hero == null)
        {
            Debug.LogWarning("没有找到本地玩家的英雄");
            return;
        }
        
        // 获取当前组的武器
        int currentGroup = hero.GetCurrentGroup();
        var weaponGroup = hero.weapenGroup[currentGroup];
        
        if (weaponGroup.Count == 0)
        {
            Debug.LogWarning("当前武器组没有武器");
            return;
        }
        
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        
        foreach (var weapon in weaponGroup)
        {
            if (weapon.weapen != null)
            {
                Debug.Log($"强制发射武器: {weapon.name}");
                Debug.Log($"  武器位置: {weapon.transform.position}");
                Debug.Log($"  武器旋转: {weapon.transform.rotation.eulerAngles}");
                Debug.Log($"  目标位置: {mouseWorldPos}");
                
                weapon.weapen.Fire(null, mouseWorldPos, weapon);
            }
        }
        
        Debug.Log("=== 强制发射完成 ===");
    }
    
    private Vector3 GetMouseWorldPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float t = -mouseRay.origin.y / mouseRay.direction.y;
        return mouseRay.origin + mouseRay.direction * t;
    }
}
