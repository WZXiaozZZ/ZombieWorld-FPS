using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    private float currentGameTIme = 0;
    public float GameTIme { get { return currentGameTIme; } }

    private int exp = 0;
    [SerializeField] private TMP_Text playerLevelText;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private List<EnemyManager> enemyManagers = new List<EnemyManager>();
    [SerializeField] private GameObject bossShowTime;
    [SerializeField] private GameObject chat;
    private bool isStopGame;
    public bool IsStopGame { get { return isStopGame; } }
    private Attribute attribute;
    public Attribute Attribute { get { return attribute; } }
    private int level = 0;
    [SerializeField] private GameObject upgradesPlane;
    [SerializeField] private GameObject gameOverUI;
    private int currentLevel;
    [SerializeField] private float intervalLevel=240f;
    private float levelTimer = 0;
    [SerializeField] private TMP_Text gameLevelText;
    private int currentGameLevel = 1;
    public int CurrentGameLevel { get { return currentGameLevel; } }
    [SerializeField] private GameObject warningPlane;
    public void Awake()
    {
        Time.timeScale = 1;
        instance = this;
        attribute = new Attribute(0, 0, 0, 0, 0, 0, 0, 0, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitBoss());
        Destroy(chat, 10f);
    }

    public void UpgradesSelect(int index)
    {
        attribute.UpgradesSelect(index);
    }

    public void SetStop(bool value)
    {
        isStopGame = value;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeScale == 0)
            return;
        levelTimer += Time.fixedDeltaTime;
        if (levelTimer > intervalLevel)
        {
            currentGameLevel++;
            if (currentGameLevel >= 4)
            {
                GameOver();
                return;
            }
            foreach (var item in enemyManagers)
            {
                item.AddHP(200);
            }
            warningPlane.SetActive(true);
            levelTimer = 0;
            gameLevelText.text ="Level  "+currentGameLevel.ToString();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            UIManager.Instance.OpenUIPlane(UIType.Pause);
        }
    }

    public void AddEnemyManager(EnemyManager enemyManager)
    {
        enemyManagers.Add(enemyManager);
    }

    public void RemoveEnemyManager(EnemyManager enemyManager)
    {
        enemyManagers.Remove(enemyManager);
        exp += enemyManager.exp;
        if (exp - 50 >= 0)
        {
            level++;
            exp -= 50;
            Upgrades();
        }
        scoreText.text = exp + "/50";

    }

    public void GameOver() 
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Upgrades()
    {
        currentLevel++;
        playerLevelText.text = currentLevel.ToString();
        Instantiate(upgradesPlane, GameObject.Find("Canvas").transform);
        Time.timeScale = 0;
        if (currentLevel % 5 == 0)
        {
            Player.Instance.AddWeapon();
        }
    }

    IEnumerator WaitBoss()
    {
        while (true)
        {
            yield return new WaitForSeconds(120f);
            Instantiate(bossShowTime, new Vector3(0, 0, 0), Quaternion.identity);
            BossShowTime();
        }
    }

    public void BossShowTime()
    {
        //bossShowTime.SetActive(true);
        foreach (var item in enemyManagers)
        {
            item.IsStop(true);
        }
        isStopGame = true;
    }

    public void BossShowTimeOver() 
    {
        foreach (var item in enemyManagers)
        {
            item.IsStop(false);
        }
        isStopGame = false;
    }
}
