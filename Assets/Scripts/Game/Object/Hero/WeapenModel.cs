using System;
using UnityEngine;

public class WeapenModel : MonoBehaviour
{
    [Header("武器设置")]
    public WeapenBase weapen;
    public Transform firePoint;
    [SerializeField]private Transform modelAnchor;    
    [Header("旋转限制")]
    public float directionAngle;
    public float includedAngle;
    
    protected Quaternion startQuaternion;
    protected Vector3 startPosition;
    protected Vector3 startAnchorPosition;
    protected Vector3 targetVector;
    public int group { get; set; }

    private Transform parentModel;

    
    private void Awake()
    {
        startQuaternion = transform.localRotation;
        startAnchorPosition = modelAnchor.position;
        startPosition = transform.position;
        parentModel = GetParentModel();
    }
    private Transform GetParentModel()
    {
        Transform current = transform.parent;
        while (current != null)
        {
            if (current.GetComponent<HeroController>() != null)
            {
                return current;
            }
            current = current.parent;
        }
        return transform.parent;
    }
    private void Update()
    {
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        Vector3 offset = modelAnchor.position - startAnchorPosition;
        transform.position = startPosition + offset;
    }
    public void TurnTowardsMouse(Vector3 pos = default)
    {
        if (weapen == null) return;

        if(pos == default)
            pos = GetMouseWorldPosition();
        
        // 2. 计算从武器到鼠标的方向向量
        Vector3 directionToMouse = (pos - transform.position).normalized;
        
        // 3. 计算目标旋转（仅Y轴旋转，保持水平）
        float targetYAngle = Mathf.Atan2(directionToMouse.x, directionToMouse.z) * Mathf.Rad2Deg;
        
        // 4. 应用旋转限制（相对于父对象的旋转）
        float constrainedYAngle = ApplyRotationConstraints(targetYAngle);
        
        // 5. 创建目标旋转（仅Y轴）
        Quaternion targetRotation = Quaternion.Euler(0, constrainedYAngle, 0);
        
        // 6. 匀速旋转到目标方向
        ApplyUniformRotation(targetRotation);
        
    }
    private Vector3 GetMouseWorldPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // 在Y=0平面上进行射线投射
        float t = -mouseRay.origin.y / mouseRay.direction.y;
        Vector3 worldPosition = mouseRay.origin + mouseRay.direction * t;
        
        return worldPosition;
    }
    private float ApplyRotationConstraints(float targetYAngle)
    {
        // 获取父对象的世界旋转角度
        float parentWorldYAngle = parentModel != null ? parentModel.rotation.eulerAngles.y : 0f;
        
        // 计算武器相对于父对象的默认朝向（世界坐标）
        float weaponDefaultWorldAngle = parentWorldYAngle + directionAngle;
        
        // 计算目标角度与默认朝向的角度差
        float angleDifference = Mathf.DeltaAngle(weaponDefaultWorldAngle, targetYAngle);
        
        // 将角度差限制在允许范围内
        float constrainedDifference = Mathf.Clamp(angleDifference, -includedAngle / 2f, includedAngle / 2f);
        
        // 计算最终的受限角度（世界坐标）
        float finalAngle = weaponDefaultWorldAngle + constrainedDifference;
        
        return finalAngle;
    }
    private void ApplyUniformRotation(Quaternion targetRotation)
    {
        // 计算最终目标旋转（包含原始旋转）
        Quaternion finalTargetRotation = targetRotation * startQuaternion;
        
        // 获取当前Y轴角度（不包含原始旋转）
        Quaternion currentWithoutStart = transform.rotation * Quaternion.Inverse(startQuaternion);
        float currentYAngle = currentWithoutStart.eulerAngles.y;
        
        // 获取目标Y轴角度
        float targetYAngle = targetRotation.eulerAngles.y;
        
        // 计算角度差（使用最短路径）
        float angleDifference = Mathf.DeltaAngle(currentYAngle, targetYAngle);
        
        // 如果角度差很小，直接设置到目标位置
        if (Mathf.Abs(angleDifference) < 0.5f)
        {
            transform.rotation = finalTargetRotation;
            return;
        }
        
        // 计算这一帧应该旋转的角度（匀速）
        float rotationThisFrame = weapen.turnSpeed * Time.deltaTime;
        
        // 确定旋转方向
        float rotationDirection = Mathf.Sign(angleDifference);
        
        // 限制旋转角度不超过剩余角度
        rotationThisFrame = Mathf.Min(rotationThisFrame, Mathf.Abs(angleDifference));
        
        // 计算新的Y角度
        float newYAngle = currentYAngle + rotationDirection * rotationThisFrame;
        
        // 应用新的旋转
        Quaternion newRotation = Quaternion.Euler(0, newYAngle, 0);
        transform.rotation = newRotation * startQuaternion;
    }
    private void OnDrawGizmos()
    {
        if (weapen == null) return;
        
        // 绘制武器当前朝向（红色）
        Gizmos.color = Color.red;
        Vector3 forwardDirection = transform.rotation * Vector3.forward;
        Gizmos.DrawLine(transform.position, transform.position + forwardDirection * 5f);
        
        // 绘制旋转限制范围（黄色）- 现在相对于父对象旋转
        if (parentModel != null)
        {
            Gizmos.color = Color.yellow;
            float parentYAngle = parentModel.rotation.eulerAngles.y;
            
            // 左边界（相对于父对象旋转）
            float leftAngle = parentYAngle + directionAngle - includedAngle / 2f;
            Vector3 leftBoundary = Quaternion.Euler(0, leftAngle, 0) * Vector3.forward;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * 3f);
            
            // 右边界（相对于父对象旋转）
            float rightAngle = parentYAngle + directionAngle + includedAngle / 2f;
            Vector3 rightBoundary = Quaternion.Euler(0, rightAngle, 0) * Vector3.forward;
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * 3f);
            
            // 绘制武器默认朝向（蓝色）
            Gizmos.color = Color.blue;
            float defaultAngle = parentYAngle + directionAngle;
            Vector3 defaultDirection = Quaternion.Euler(0, defaultAngle, 0) * Vector3.forward;
            Gizmos.DrawLine(transform.position, transform.position + defaultDirection * 4f);
        }
        
        // 绘制武器中心位置
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
    public float GetCurrentAngle()
    {
        return transform.rotation.eulerAngles.y;
    }
    public bool CanRotateToAngle(float worldAngle)
    {
        float parentWorldYAngle = parentModel != null ? parentModel.rotation.eulerAngles.y : 0f;
        float weaponDefaultWorldAngle = parentWorldYAngle + directionAngle;
        float angleDifference = Mathf.DeltaAngle(weaponDefaultWorldAngle, worldAngle);
        return Mathf.Abs(angleDifference) <= includedAngle / 2f;
    }
    public float GetRelativeAngle()
    {
        if (parentModel == null) return GetCurrentAngle();
        
        float parentYAngle = parentModel.rotation.eulerAngles.y;
        float weaponYAngle = GetCurrentAngle();
        return Mathf.DeltaAngle(parentYAngle, weaponYAngle);
    }
    public void OnFire()
    {
        Animator animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        animator.Play("Shoot");
    }
}