using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Button start_Btn;
    [SerializeField] private GameObject loadingPlane;
    [SerializeField]private SceneLoaderWithProgress progress;
    [SerializeField] private Button exit_Btn;
    // Start is called before the first frame update
    void Start()
    {
        start_Btn.onClick.AddListener(StartGame);
        exit_Btn.onClick.AddListener(ExitGame);
    }


    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StartGame() 
    {
        loadingPlane.SetActive(true);
        progress.LoadSceneAsync("Main");
        //SceneManager.LoadScene(Defines.gameScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
