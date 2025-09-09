using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public BulletData data;
    private GameObjectController target;
    private Vector3 direction;
    private float curTime;
    private float speed;
    private Player player;

    private UnityEvent onDestory;
    private UnityEvent<GameObjectController> onCollision;

    private Quaternion startRotation;
    private void Awake()
    {
        startRotation = transform.rotation;
    }
    private void Update()
    {
        if (!RoomController.instance.gameReady) return;
        CanDestory();
        Move();
    }
    public void Init(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        Quaternion x = Quaternion.AngleAxis(startRotation.eulerAngles.x, Vector3.right);
        transform.rotation *= x;
    }
    public void SetTarget(GameObjectController target,Vector3 direction,Player player)
    {
        curTime = 0;
        speed = data.startSpeed;
        this.player = player;
        this.target = target;
        this.direction = direction.normalized;
    }
    public void Move()
    {
        if(data.tail && target != null)
        {
            Vector3 cos = Vector3.Dot(direction, target.transform.position) * (target.transform.position - transform.position).normalized;
            Vector3 sin = direction - cos;
            cos = Mathf.Clamp(cos.magnitude + data.turnSpeed * Time.deltaTime, 0, speed) * cos.normalized;
            sin = Mathf.Clamp(sin.magnitude - data.turnSpeed * Time.deltaTime, 0, speed) * sin.normalized;
            direction = (cos + sin).normalized;
        }
        transform.position += direction * Time.deltaTime * speed;
        speed += data.accelerateSpeed * Time.deltaTime;
    }
    private void CanDestory()
    {
        if (curTime < data.liveTime)
            curTime += Time.deltaTime;
        else
        {
            Destory();
        }
    }
    private void Destory()
    {
        onDestory?.Invoke();
        GameEntry.ObjectPoolComponent.Release(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out GameObjectController controller))
        {
            if(player != null && controller.player != player)
            {
                onCollision?.Invoke(controller);
                Destory();
            }
        }
    }
}