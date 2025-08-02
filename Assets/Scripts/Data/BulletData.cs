using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObject/Bullet")]
public class BulletData : ScriptableObject, ID
{
    public float liveTime = 5f;

    public float startSpeed;
    public float accelerateSpeed;

    public bool tail;
    public float turnSpeed;

    [Header("ID")]
    [SerializeField]protected int id;
    public int ID => id;
    [SerializeField] protected IDType idType;
    public IDType searchName => idType;
}