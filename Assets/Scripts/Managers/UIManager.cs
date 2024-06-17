using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIType
{ 
    Pause,
    Death,
    GunBulletNumber,
}

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [SerializeField] private BulletNumberUIPlane BulletNumberUIPlane;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button game_Btn;
    [SerializeField] private Button again_Btn;
    [SerializeField] private Button exit_Btn;
    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        game_Btn.onClick.AddListener(Game);
        again_Btn.onClick.AddListener(Again);
        exit_Btn.onClick.AddListener(ExitGame);
    }

    public void Game() 
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Again() 
    {
        SceneManager.LoadScene("Main");
    }
    public void ExitGame() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    public void OpenUIPlane(UIType uiType) 
    {
        switch (uiType)
        {
            case UIType.Pause:
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                break;
            case UIType.Death:
                break;
            case UIType.GunBulletNumber:
                break;
            default:
                break;
        }
    }
    public void SetUIPlane(UIType uiType,int value ,int value02)
    {
        switch (uiType)
        {
            case UIType.Pause:
                break;
            case UIType.Death:
                break;
            case UIType.GunBulletNumber:
                BulletNumberUIPlane.UpdateText(value, value02);
                break;
            default:
                break;
        }
    }
}
