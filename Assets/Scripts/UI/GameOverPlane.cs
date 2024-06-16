using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPlane : MonoBehaviour
{
    [SerializeField] private Button againBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private TMP_Text gameTIme_Text;
    // Start is called before the first frame update
    void Start()
    {
        againBtn.onClick.AddListener(Again);
        exitBtn.onClick.AddListener(ExitGame);
        gameTIme_Text.text = ((int)GameManager.Instance.GameTIme).ToString();
    }

    public void Again() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
