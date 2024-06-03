using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponName
{
    Rifle01,
    Rifle02,
}
public class PlayerShot : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponData
    {
        public WeaponName weaponName;
        public WeaponMessageData data;
    }

    public WeaponData[] weaponDatas;
    private int currWeaponID = 0;
    private bool isShot=true;
    public GameObject InputFx;
    private bool isRoLad;

    private void Start()
    {
        UIManager.Instance.SetUIPlane(UIType.GunBulletNumber, weaponDatas[currWeaponID].data.currentBulletNumber, weaponDatas[currWeaponID].data.reserveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoLad)
            return;
        if (Input.GetMouseButton(0) && isShot&&weaponDatas[currWeaponID].data.currentBulletNumber>0)
        {
            //Player.Instance.TriggerAnimator(Defines.ShotAnimationClip);
            Player.Instance.SetShotAnimator(Defines.ShotAnimationClip, true);
            isShot = false;
            weaponDatas[currWeaponID].data.currentBulletNumber--;
            StartCoroutine(ReadyShot(weaponDatas[currWeaponID].data.ShootSpeed));
            AudioManager.Instance.PlayOneShotMusicFX(weaponDatas[currWeaponID].data.MusicName);
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag(Defines.EnemyTag))
                {
                    hit.transform.GetComponent<EnemyManager>().Damage(weaponDatas[currWeaponID].data.Hurt);
                }
                ParticleSystem fx = Instantiate(InputFx, hit.point, hit.transform.rotation).GetComponent<ParticleSystem>();
                fx.Play();
            }
        }
        else if(Input.GetMouseButtonUp(0))
        { 
            Player.Instance.SetShotAnimator(Defines.ShotAnimationClip, false);
        }
        if (Input.GetKey(KeyCode.R)&& !isRoLad)
        {
            isRoLad = true;
            Player.Instance.TriggerAnimator(Defines.ReLoadClip);
            StartCoroutine(ReLoad());
        }
    }

    IEnumerator ReLoad() 
    {
        yield return new WaitForSeconds(1.2f);
        isRoLad=false;

        int value= weaponDatas[currWeaponID].data.reserveNumber % weaponDatas[currWeaponID].data.maxBulletNumber;
        weaponDatas[currWeaponID].data.reserveNumber -= value;
        UIManager.Instance.SetUIPlane(UIType.GunBulletNumber, value, weaponDatas[currWeaponID].data.reserveNumber);
    }


    IEnumerator ReadyShot(float value) 
    {
        yield return new WaitForSeconds(value);
        isShot = true;
    }

}
