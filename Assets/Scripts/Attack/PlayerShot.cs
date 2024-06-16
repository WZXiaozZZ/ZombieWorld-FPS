using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum WeaponName
{
    Rifle01,
    Rifle02,
    HandsGun,
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
    [SerializeField] private GunManager[] gunManagers;
    public GunManager GunManager { get { return gunManagers[currWeaponID]; } }
    [SerializeField] private Animator zhunxinAnim;
    private bool isAuto;
    private int autoLevel = 0;
    private float autoTime = 20f;
    private float autoTimer = 0;
    private int weaponHaveNumber = 0;
    [SerializeField] private GameObject[] gunModel;
    private void Start()
    {
        UIManager.Instance.SetUIPlane(UIType.GunBulletNumber, gunManagers[currWeaponID].CurrentBullet, gunManagers[currWeaponID].ReserveBullet);
    }
    float shotTimer = 0;
    // Update is called once per frame
    void Update()
    {
        if (isRoLad || GameManager.Instance.IsStopGame || Time.timeScale == 0 || Player.Instance.IsDeath)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha1)&&weaponHaveNumber>=2)
        {
            gunModel[currWeaponID].SetActive(false);
            currWeaponID = 2;
            gunModel[currWeaponID].SetActive(true);
            StartCoroutine( ChangeWeapon());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponHaveNumber >= 1)
        {
            gunModel[currWeaponID].SetActive(false);
            currWeaponID = 1;
            gunModel[currWeaponID].SetActive(true);
            StartCoroutine( ChangeWeapon());
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gunModel[currWeaponID].SetActive(false);
            currWeaponID = 0;
            gunModel[currWeaponID].SetActive(true);
            StartCoroutine(ChangeWeapon());
        }
        if (Input.GetKey(KeyCode.R) && !isRoLad)
        {
            zhunxinAnim.SetBool("stop", true);
            isRoLad = true;
            Player.Instance.TriggerAnimator(Defines.ReLoadClip);
            StartCoroutine(ReLoad());
        }

        if (Input.GetMouseButton(0) && isShot && gunManagers[currWeaponID].CurrentBullet > 0)
        {
            if (shotTimer > 7)
                shotTimer = 7;
            gunManagers[currWeaponID].Shot();
            //Player.Instance.TriggerAnimator(Defines.ShotAnimationClip);
            Player.Instance.SetShotAnimator(Defines.ShotAnimationClip, true);
            isShot = false;
            if (gunManagers[currWeaponID].CurrentBullet == 0)
                isBulletNull = true;
            StartCoroutine(ReadyShot(weaponDatas[currWeaponID].data.ShootSpeed));
            AudioManager.Instance.PlayOneShotMusicFX(weaponDatas[currWeaponID].data.MusicName);
            UIManager.Instance.SetUIPlane(UIType.GunBulletNumber, gunManagers[currWeaponID].CurrentBullet, gunManagers[currWeaponID].ReserveBullet);
            float range = Random.Range(-weaponDatas[currWeaponID].data.RecoilAmount * shotTimer, weaponDatas[currWeaponID].data.RecoilAmount * shotTimer);
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2 + range, Screen.height / 2 + range));
            RaycastHit hit;
            zhunxinAnim.SetBool("stop", false);
            int damage = 0;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag(Defines.EnemyTag))
                {
                    damage = weaponDatas[currWeaponID].data.Hurt + (GameManager.Instance.Attribute.AtkLevel * 2);
                    hit.transform.GetComponent<EnemyManager>().Damage(damage);
                }
                if (hit.transform.CompareTag("Boss"))
                {
                    damage = weaponDatas[currWeaponID].data.Hurt + (GameManager.Instance.Attribute.AtkLevel * 2);
                    hit.transform.GetComponent<Boss>().Damage(damage);
                }
                ParticleSystem fx = Instantiate(InputFx, hit.point, hit.transform.rotation).GetComponent<ParticleSystem>();
                fx.Play();
                fx.transform.LookAt(transform.position);
                if (GameManager.Instance.Attribute.LifestealLevel > 0)
                {
                    Player.Instance.AddHP(damage * GameManager.Instance.Attribute.LifestealLevel / 20);
                }
            }
            shotTimer++;
        }
        else if (Input.GetMouseButtonUp(0) || isBulletNull)
        {
            shotTimer = 0;
            zhunxinAnim.SetBool("stop", true);
            Player.Instance.SetShotAnimator(Defines.ShotAnimationClip, false);
        }
        if (autoLevel > 0)
        {
            autoTimer += Time.deltaTime;
            if (autoTimer >= autoTime)
            {
                autoTimer = 0;
                gunManagers[currWeaponID].AddBullet(2);
                UIManager.Instance.SetUIPlane(UIType.GunBulletNumber, gunManagers[currWeaponID].CurrentBullet, gunManagers[currWeaponID].ReserveBullet);
            }
        }
    }


    public void AddWeapon()
    {
        weaponHaveNumber++;
        if (weaponHaveNumber == 1)
        {
            gunModel[currWeaponID].SetActive(false);
            currWeaponID = 1;
            gunModel[currWeaponID].SetActive(true);
            StartCoroutine(ChangeWeapon());

        }
        if (weaponHaveNumber == 2)
        {
            gunModel[currWeaponID].SetActive(false);
            currWeaponID = 2;
            gunModel[currWeaponID].SetActive(true);
            StartCoroutine(ChangeWeapon());
        }
    }

    IEnumerator ChangeWeapon() 
    {
        isRoLad = true;
        Player.Instance.ChangeGun(gunManagers[currWeaponID]);
        yield return new WaitForSeconds(1.5f);
        isRoLad = false;
    }

    public void SetAutoLevel(int level)
    {
        autoLevel = level;
        autoTime -= (((float)autoLevel) / 2.5f);
        autoTime = Mathf.Clamp(autoTime, 1, 20);
    }

    // 后座力相关参数
    private Vector3 currentRecoil = Vector3.zero;
    private Vector3 targetRecoil = Vector3.zero;
    private Vector3 recoilVelocity = Vector3.zero;
    public float recoilSmoothTime = 0.1f; // 后座力平滑时间
    public float recoilAmount = 2f; // 后座力强度


    IEnumerator ReLoad()
    {
        AudioManager.Instance.PlayOneShotMusicFX(weaponDatas[currWeaponID].data.ReLoadMusic);

        yield return new WaitForSeconds(weaponDatas[currWeaponID].data.ReLoadTime);
        isRoLad = false;
        gunManagers[currWeaponID].ReLoadBullet();
        isBulletNull = false;
        UIManager.Instance.SetUIPlane(UIType.GunBulletNumber, gunManagers[currWeaponID].CurrentBullet, gunManagers[currWeaponID].ReserveBullet);
    }


    IEnumerator ReadyShot(float value)
    {
        yield return new WaitForSeconds(value * (100 + GameManager.Instance.Attribute.RateOfFireLevel) / 100);
        isShot = true;
    }

}
