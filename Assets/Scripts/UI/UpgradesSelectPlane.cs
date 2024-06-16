using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesSelectPlane : MonoBehaviour
{
    [SerializeField] private Button[] upgradesBtns;
    [SerializeField] private TMP_Text[] attributesText;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < upgradesBtns.Length; i++)
        {
            int value = i;
            upgradesBtns[i].onClick.AddListener(() => Upgrades(value));
        }
        for (int i = 0; i < attributesText.Length; i++)
        {
            attributesText[i].text = GameManager.Instance.Attribute.GetLevel(i).ToString();
        }
    }

    public void Upgrades(int index) 
    {
        GameManager.Instance.UpgradesSelect(index);
        Time.timeScale = 1;
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
