using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemyManager : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyCreateData
    {
        public float createInterval;
        public GameObject enemy;
    }

    [SerializeField] private EnemyCreateData[] enemyCreateDatas;
    [SerializeField] private Transform[] createPos;
    [SerializeField] private float minGivenTime=20;
    [SerializeField] private float maxGivenTime=40;

    [SerializeField] private Vector2 maxPoint;
    [SerializeField] private Vector2 minPoint;
    [SerializeField] private GameObject[] given;
    private int upgradesLevel=0;
    public int UpgradesLevel { get { return upgradesLevel; } }
    private static CreateEnemyManager instance;
    public static CreateEnemyManager Instance { get { return instance; } }


    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in enemyCreateDatas)
        {
            StartCoroutine( Create(item));
        }
        StartCoroutine(CreateGiven());
        StartCoroutine(Upgrade());
    }

    public IEnumerator Upgrade() 
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            upgradesLevel++;

        }
    }

    public IEnumerator Create(EnemyCreateData data)
    {
        while (true)
        {
            yield return new WaitForSeconds(data.createInterval);
            int range = Random.Range(0, createPos.Length);
            GameObject obj= Instantiate(data.enemy, createPos[range].position, createPos[range].rotation);
            EnemyManager enemyManager = obj.GetComponentInChildren<EnemyManager>();
            enemyManager.AddAttribute(upgradesLevel*2 , upgradesLevel/10f, upgradesLevel/3);
            GameManager.Instance.AddEnemyManager(enemyManager);
        }
    }
    public IEnumerator CreateGiven() 
    {
        float timer = Random.Range(minGivenTime, maxGivenTime);
        while (true)
        {
            yield return new WaitForSeconds(timer- GameManager.Instance.Attribute.ReplenishmentDropRateLevel);
            timer = Random.Range(minGivenTime, maxGivenTime);
            float x = Random.Range(minPoint.x, maxPoint.x);
            float y= Random.Range(minPoint.y, maxPoint.y);
            int range= Random.Range(0, given.Length);
            Instantiate(given[range], new Vector3(x, 150, y),Quaternion.identity);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
