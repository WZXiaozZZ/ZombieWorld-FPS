using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    CharacterController playerController;


    Vector3 direction;


    public float walkSpeed = 1;
    public float runSpeed = 2;
    private float currSpeed;
    public float jumpPower = 5;
    public float gravity = 7f;


    public float mousespeed = 5f;


    public float minmouseY = -45f;
    public float maxmouseY = 45f;


    float RotationY = 0f;
    float RotationX = 0f;
    private bool isRun;

    public Transform agretctCamera;

    [SerializeField]private Animator animator;
    private static Player _instance;
    public static Player Instance { get { return _instance; } }
    [SerializeField]private int maxHP=100;
    [SerializeField]private int hp;
    public int HP { get { return hp; } }
    public CanvasGroup bloodImage;
    [SerializeField] private Slider hpSlider;
    private bool isDeath;
    public void Awake()
    {
        _instance = this;
        hp = maxHP;
    }

    // Use this for initialization
    void Start()
    {
        playerController = this.GetComponent<CharacterController>();
        currSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath)
            return;
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");
        
        if (playerController.isGrounded)
        {
            direction = new Vector3(_horizontal, 0, _vertical);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRun = true;
            }
            else
                isRun = false;
            if (_horizontal != 0 || _vertical != 0)
            {
                if (!isRun)
                {
                    animator.SetBool("walk", true);
                    animator.SetBool("run", false);
                    AudioManager.Instance.PlayPlayerMusic(MusicName.Walk);
                    currSpeed = walkSpeed;
                }
                else
                {
                    animator.SetBool("run", true);
                    AudioManager.Instance.PlayPlayerMusic(MusicName.Run);
                    currSpeed = runSpeed;
                }
            }
            else
            {
                AudioManager.Instance.StopPlayerMusic();
                animator.SetBool("walk", false);
                animator.SetBool("run", false);
            }
            if (Input.GetKeyDown(KeyCode.Space))
                direction.y = jumpPower;
        }
        
        direction.y -= gravity * Time.deltaTime;
        playerController.Move(playerController.transform.TransformDirection(direction * Time.deltaTime * currSpeed));

        RotationX += agretctCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mousespeed;
        RotationY -= Input.GetAxis("Mouse Y") * mousespeed;
        RotationY = Mathf.Clamp(RotationY, minmouseY, maxmouseY);
        this.transform.eulerAngles = new Vector3(0, RotationX, 0);
        agretctCamera.transform.eulerAngles = new Vector3(RotationY, RotationX, 0);
    }

 
 

    public void TriggerAnimator(string name) 
    {
        animator.SetTrigger(name);
    }
    public void SetShotAnimator(string name,bool isTure) 
    {
        animator.SetBool(name, isTure);
    }

    public void Damage(int value) 
    {
        hp -= value;
        bloodImage.alpha= (1.0f -(float)((float)hp / (float)maxHP))/1.2f;
        hpSlider.value= (float)((float)hp / (float)maxHP) ;
        if (hp <= 0)
        {
            Death();
        }
    }
    public void Death() 
    {
        isDeath = true;
        animator.SetBool("death", true);
        
    }

}
