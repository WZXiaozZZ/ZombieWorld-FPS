using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public MusicType[] MusicType;
    private Dictionary<MusicName, MusicType> keyValuePairs = new Dictionary<MusicName, MusicType>();
    private float currSpeed = 2;
    [SerializeField]private float runSpeed = 4;
    public float RunSpeed { get { return runSpeed; } }
    [SerializeField]private float walkSpeed = 2;
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
    [SerializeField] private float startRunDistance=10;
    private int hp=100;
    public int HP { get { return hp; } }
    private bool isDeath;
    public bool isAttacked = false;
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
        foreach (var item in MusicType)
        {
            keyValuePairs.Add(item.musicName, item);
        }
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
        if (isDeath)
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

    public void Death() 
    {
        animator.SetTrigger("death");
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isDeath = true;
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
            AudioSource.clip = keyValuePairs[musicName].musckClip;
            AudioSource.Play();
        }
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
