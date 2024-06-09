using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool isShot = true;
    public GameObject InputFx;
    private bool isRoLad;
    private bool isBulletNull;
    [SerializeField] private GunManager gunManager;
    private void Start()
    {
        UIManager.Instance.SetUIPlane(UIType.GunBulletNumber, weaponDatas[currWeaponID].data.currentBulletNumber, weaponDatas[currWeaponID].data.reserveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoLad)
            return;
        if (Input.GetKey(KeyCode.R) && !isRoLad)
        {
            isRoLad = true;
            Player.Instance.TriggerAnimator(Defines.ReLoadClip);
            StartCoroutine(ReLoad());
        }

        if (Input.GetMouseButton(0) && isShot && weaponDatas[currWeaponID].data.currentBulletNumber > 0)
        {
            //Player.Instance.TriggerAnimator(Defines.ShotAnimationClip);
            Player.Instance.SetShotAnimator(Defines.ShotAnimationClip, true);
            isShot = false;
            weaponDatas[currWeaponID].data.currentBulletNumber--;
            if (weaponDatas[currWeaponID].data.currentBulletNumber == 0)
                isBulletNull = true;
            StartCoroutine(ReadyShot(weaponDatas[currWeaponID].data.ShootSpeed));
            AudioManager.Instance.PlayOneShotMusicFX(weaponDatas[currWeaponID].data.MusicName);
            UIManager.Instance.SetUIPlane(UIType.GunBulletNumber, weaponDatas[currWeaponID].data.currentBulletNumber, weaponDatas[currWeaponID].data.reserveNumber);
            float range = Random.Range(-weaponDatas[currWeaponID].data.RecoilAmount, weaponDatas[currWeaponID].data.RecoilAmount);
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2+ range, Screen.height / 2+ range));
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
            gunManager.Shot();
        }
        else if (Input.GetMouseButtonUp(0)|| isBulletNull)
        {
            Player.Instance.SetShotAnimator(Defines.ShotAnimationClip, false);
        }


    }


    // 后座力相关参数
    private Vector3 currentRecoil = Vector3.zero;
    private Vector3 targetRecoil = Vector3.zero;
    private Vector3 recoilVelocity = Vector3.zero;
    public float recoilSmoothTime = 0.1f; // 后座力平滑时间
    public float recoilAmount = 2f; // 后座力强度


    IEnumerator ReLoad()
    {
        yield return new WaitForSeconds(1.2f);
        isRoLad = false;
        int currentNumber = weaponDatas[currWeaponID].data.currentBulletNumber;
        int value= weaponDatas[currWeaponID].data.reserveNumber+ weaponDatas[currWeaponID].data.currentBulletNumber- weaponDatas[currWeaponID].data.maxBulletNumber;
        if (value <= 0)
        {
            weaponDatas[currWeaponID].data.currentBulletNumber = weaponDatas[currWeaponID].data.reserveNumber;
            weaponDatas[currWeaponID].data.reserveNumber = 0;
        }
        else if (value >= weaponDatas[currWeaponID].data.maxBulletNumber)
        {
            weaponDatas[currWeaponID].data.currentBulletNumber = weaponDatas[currWeaponID].data.maxBulletNumber;
            weaponDatas[currWeaponID].data.reserveNumber -= (weaponDatas[currWeaponID].data.maxBulletNumber- currentNumber);
        }
        isBulletNull = false;
        UIManager.Instance.SetUIPlane(UIType.GunBulletNumber, weaponDatas[currWeaponID].data.currentBulletNumber, weaponDatas[currWeaponID].data.reserveNumber);
    }


    IEnumerator ReadyShot(float value)
    {
        yield return new WaitForSeconds(value);
        isShot = true;
    }

}
