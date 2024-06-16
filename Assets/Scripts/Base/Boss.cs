using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public float stopDistance = 5.0f;
    public float attackDistance = 2.0f;
    public float rotationSpeed = 5.0f;
    public int damageAmount = 20;
    public Animator animator;
    private Transform model;
    private bool isAttacking = false;
    [SerializeField] private float hp = 1000;
    [SerializeField] private float walkSpeed = 5;
    private bool isAttackOver = true;
    [SerializeField] private float waitTime = 5f;
    private void Start()
    {
        model = GetComponentInChildren<Transform>();
        hp*= Mathf.Clamp(CreateEnemyManager.Instance.UpgradesLevel/2f,0,10000);
        StartCoroutine(StartMove());
    }
    bool isStart;
    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(waitTime);
        isStart = true;
        GameManager.Instance.BossShowTimeOver();
    }


    void Update()
    {
        if (!isStart||isDeath||GameManager.Instance.IsStopGame)
            return;
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (distanceToPlayer > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, Time.deltaTime * walkSpeed);
            Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            isAttacking = false;
        }
        else if (!isAttacking && isAttackOver)
        {
            isAttackOver = false;
            if (!isAttacking)
            {
                animator.SetBool("attack", true);
                isAttacking = true;
            }

            Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Check if player is within attack distance
            if (distanceToPlayer <= attackDistance)
            {
                // Apply damage to player (assuming player has a method TakeDamage)
                Player.Instance.Damage(damageAmount);
            }
            StartCoroutine(WaitAttackOver());
        }
    }

    IEnumerator WaitAttackOver()
    {
        yield return new WaitForSeconds(2);
        isAttacking = false;
        isAttackOver = false;
        animator.SetBool("attack", false);
    }
    bool isDeath;
    public void Damage(int value)
    {
        hp -= value;
        if (hp <= 0)
            Death();
    }

    public void Death()
    {
        isDeath = true;
        animator.SetTrigger("death");
        GameManager.Instance.Upgrades();
        Destroy(gameObject.transform.parent.gameObject, 5f);
    }


}

