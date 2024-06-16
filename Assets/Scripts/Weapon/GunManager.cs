using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private GameObject _fx;
    [SerializeField] protected Transform _shootPlace;
    [SerializeField] private int currentBullet = 30;
    public int CurrentBullet { get { return currentBullet; } }
    [SerializeField] private int maxBullet = 30;
    public int MaxBullet { get { return maxBullet; } }
    [SerializeField] private int reserveBullet = 150;
    public int ReserveBullet { get { return reserveBullet; } }
    private List<GameObject> _fxPool = new List<GameObject>();
    [SerializeField]private Animator _animator;
    public Animator Gun_Animator { get { return _animator; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddBullet(int value)
    {
        reserveBullet += value;
    }


    public bool ReLoadBullet()
    {
        int needBullet = maxBullet - currentBullet;
        if (needBullet <= reserveBullet)
        {
            currentBullet += needBullet;
            reserveBullet -= needBullet;
            return true;
        }
        else
        {
            currentBullet += reserveBullet;
            reserveBullet = 0;
            return true;
        }
    }


    public bool Shot()
    {
        if (currentBullet <= 0)
            return false;
        currentBullet--;

        foreach (GameObject go in _fxPool)
        {
            if (!go.activeSelf)
            {
                go.SetActive(true);
                return true;
            }
        }
        GameObject obj = Instantiate(_fx, _shootPlace);
        _fxPool.Add(obj);
        return true;
    }

    
}
