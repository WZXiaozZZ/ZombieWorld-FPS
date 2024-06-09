using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private GameObject _fx;
    [SerializeField] protected Transform _shootPlace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shot() 
    {
        GameObject obj= Instantiate(_fx, _shootPlace);
        Destroy(obj,0.5f);
    }
}
