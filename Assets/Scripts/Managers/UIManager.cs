using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Awake()
    {
        instance = this;
    }


    public void OpenUIPlane(UIType uiType) 
    {
        switch (uiType)
        {
            case UIType.Pause:
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
