using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public MusicType[] MusicType;
    private Dictionary<MusicName, MusicType> keyValuePairs = new Dictionary<MusicName, MusicType>();
    private float currSpeed = 2;
    [SerializeField] private float runSpeed = 4;
    public float RunSpeed { get { return runSpeed; } }
    [SerializeField] private float walkSpeed = 2;
    public float WalkSpeed { get { return walkSpeed; } }
    private EnemyFSMSystem fsmSystem;
    [SerializeField] private Transform patrolTramsform;
    private Vector3[] patrolPos;
    private bool isGround;
    public bool IsGround { get { return isGround; } }
    [SerializeField] private float traceDistance = 5f;
    private float hurtTimer = 0.3f;
    private int currPatrolPoint = 0;
    public Animator animator;
    private AudioSource AudioSource;
    [SerializeField] private float startRunDistance = 10;
    [SerializeField]private int hp = 100;
    public int HP { get { return hp; } }
    private bool isDeath;
    public bool isAttacked = false;
    private bool isStop;
    [SerializeField] private int score = 1;
    public int exp { get { return score; } }
    [SerializeField] private float attackRange = 3f;
    public float AttackRange { get { return attackRange; } }
    [SerializeField] private float canDamageRange = 4f;
    public float CanDamageRange { get { return canDamageRange; } }
    [SerializeField] private int atk=30;
    public int Atk { get { return atk; } }
    public void Awake()
    {
        currSpeed = walkSpeed;
        patrolPos = new Vector3[patrolTramsform.childCount];
        for (int i = 0; i < patrolPos.Length; i++)
        {
            patrolPos[i] = patrolTramsform.GetChild(i).position;
        }
        AudioSource = GetComponent<AudioSource>();
        fsmSystem = new EnemyFSMSystem();
        fsmSystem.manager = this;
        EnemyFSMState patorlState = new EnemyPatrolState();
        fsmSystem.AddState(patorlState);
        patorlState.system = fsmSystem;
        EnemyFSMState runState = new EnemyRunState();
        fsmSystem.AddState(runState);
        runState.system = fsmSystem;
        EnemyFSMState attackState = new EnemyAttackState();
        fsmSystem.AddState(attackState);
        attackState.system = fsmSystem;
        foreach (var item in MusicType)
        {
            keyValuePairs.Add(item.musicName, item);
        }
    }

    public void AddAttribute(int hp,float speed,int atk) 
    {
        runSpeed += speed;
        this.hp += hp;
        this.atk += atk;
    }

    public void ChangeSpeed(Enemy_StateID _StateID)
    {
        switch (_StateID)
        {
            case Enemy_StateID.NullStateID:
                break;
            case Enemy_StateID.Idle:
                break;
            case Enemy_StateID.Patrol:
                currSpeed = walkSpeed;
                break;
            case Enemy_StateID.Attack:
                currSpeed = 0;
                break;
            case Enemy_StateID.Run:
                currSpeed = runSpeed;
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isDeath || isStop)
            return;
        if (fsmSystem != null)
        {
            fsmSystem.CurrentState.StateUpdate();
        }
    }
    public void Damage(int value)
    {
        hp -= value;
        isAttacked = true;
        if (hp <= 0)
            Death();
    }

    public void IsStop(bool value)
    {
        isStop = value;
    }

    public void Death()
    {
        animator.SetTrigger("death");
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isDeath = true;
        GameManager.Instance.RemoveEnemyManager(this);
        Destroy(gameObject, 10f);
    }
    public bool Patrol()
    {
        if ((transform.position - patrolPos[currPatrolPoint]).magnitude < 0.5f)
        {
            currPatrolPoint++;
            if (currPatrolPoint == patrolPos.Length)
            {
                currPatrolPoint = 0;
            }
        }
        Move(patrolPos[currPatrolPoint]);
        if ((Player.Instance.transform.position - transform.position).magnitude < startRunDistance)
        {
            return true;
        }
        return false;
    }

    public void PlayMusic(MusicName musicName)
    {
        if (!AudioSource.isPlaying)
        {
            AudioSource.clip = keyValuePairs[musicName].musicClip;
            AudioSource.Play();
        }
    }
    public void ShotMusic(MusicName musicName)
    {
        AudioSource.PlayOneShot( keyValuePairs[musicName].musicClip);
    }
    public void Move(Vector3 value)
    {
        value.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, value, currSpeed * Time.fixedDeltaTime);
        Vector3 targetDirection = value - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }

}
