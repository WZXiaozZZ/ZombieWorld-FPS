using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Button start_Btn;
    // Start is called before the first frame update
    void Start()
    {
        start_Btn.onClick.AddListener(StartGame);
    }


    public void StartGame() 
    {
        SceneManager.LoadScene(Defines.gameScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
