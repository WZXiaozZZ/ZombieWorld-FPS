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
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && isShot)
        {
            //Player.Instance.TriggerAnimator(Defines.ShotAnimationClip);
            Player.Instance.SetShotAnimator(Defines.ShotAnimationClip, true);
            isShot = false;
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
    }

    IEnumerator ReadyShot(float value) 
    {
        yield return new WaitForSeconds(value);
        isShot = true;
    }

}
