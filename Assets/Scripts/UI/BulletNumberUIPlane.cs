using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletNumberUIPlane : MonoBehaviour
{

    [SerializeField]private TMP_Text bullet_Text;

    public void Start()
    {
        bullet_Text = GetComponent<TMP_Text>();
    }

    public void UpdateText(int currentBulletNumber,int reserveBulletNumber) 
    {
        bullet_Text.text = currentBulletNumber+"/"+reserveBulletNumber;
    }


}
