using Mirror;
using System;
using UnityEngine;

public class WeapenModel : NetworkBehaviour
{
    [SerializeField]
    private Transform modelAnchor; 
    
    public float directionAngle;
    public float includedAngle;
    public Transform firePoint { get; set; }
    public WeapenBase weapen { get; set; }

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
        Vector3 directionToMouse = (pos - transform.position).normalized;
        float targetYAngle = Mathf.Atan2(directionToMouse.x, directionToMouse.z) * Mathf.Rad2Deg;
        float constrainedYAngle = ApplyRotationConstraints(targetYAngle);
        Quaternion targetRotation = Quaternion.Euler(0, constrainedYAngle, 0);
        ApplyUniformRotation(targetRotation);
    }
    private Vector3 GetMouseWorldPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float t = -mouseRay.origin.y / mouseRay.direction.y;
        Vector3 worldPosition = mouseRay.origin + mouseRay.direction * t;
        return worldPosition;
    }
    private float ApplyRotationConstraints(float targetYAngle)
    {
        float parentWorldYAngle = parentModel != null ? parentModel.rotation.eulerAngles.y : 0f;
        float weaponDefaultWorldAngle = parentWorldYAngle + directionAngle;
        float angleDifference = Mathf.DeltaAngle(weaponDefaultWorldAngle, targetYAngle);
        float constrainedDifference = Mathf.Clamp(angleDifference, -includedAngle / 2f, includedAngle / 2f);
        float finalAngle = weaponDefaultWorldAngle + constrainedDifference;
        
        return finalAngle;
    }
    private void ApplyUniformRotation(Quaternion targetRotation)
    {
        Quaternion finalTargetRotation = targetRotation * startQuaternion;
        Quaternion currentWithoutStart = transform.rotation * Quaternion.Inverse(startQuaternion);
        float currentYAngle = currentWithoutStart.eulerAngles.y;
        float targetYAngle = targetRotation.eulerAngles.y;
        float angleDifference = Mathf.DeltaAngle(currentYAngle, targetYAngle);
        if (Mathf.Abs(angleDifference) < 0.5f)
        {
            transform.rotation = finalTargetRotation;
            return;
        }
        float rotationThisFrame = weapen.turnSpeed * Time.deltaTime;
        float rotationDirection = Mathf.Sign(angleDifference);
        rotationThisFrame = Mathf.Min(rotationThisFrame, Mathf.Abs(angleDifference));
        float newYAngle = currentYAngle + rotationDirection * rotationThisFrame;
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
    [Command(requiresAuthority = false)]
    public void OnFireServer()
    {
        OnFireClient();
    }
    [ClientRpc]
    public void OnFireClient()
    {
        Animator animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        animator.Play("Shoot");
    }
}