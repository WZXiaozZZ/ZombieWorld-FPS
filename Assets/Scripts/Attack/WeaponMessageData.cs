using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "WeaponMessage/Data")]
public class WeaponMessageData : ScriptableObject
{
    [SerializeField]private float shootSpeed;
    public float ShootSpeed { get { return shootSpeed; } }
    [SerializeField] private int hurt;
    public int Hurt { get { return hurt; } }

    [SerializeField]private GameObject prefabs;
    public GameObject Prefabs { get { return prefabs; } }
    public MusicName MusicName;
    public int currentBulletNumber;
    public int maxBulletNumber;
    public int reserveNumber;
}
