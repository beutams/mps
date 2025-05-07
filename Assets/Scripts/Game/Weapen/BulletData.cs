using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObject/Bullet")]
public class BulletData : ScriptableObject
{
    public float liveTime = 5f;

    public float startSpeed;
    public float accelerateSpeed;

    public bool tail;
    public float turnSpeed;
}