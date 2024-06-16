using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyFSMState
{
    private float timer;
    bool isAttack;
    public EnemyAttackState()
    {
        stateID = Enemy_StateID.Attack;
        AddTransition(Enemy_Transition.Idle, Enemy_StateID.Idle);
        AddTransition(Enemy_Transition.Patrol, Enemy_StateID.Patrol);
        AddTransition(Enemy_Transition.Run, Enemy_StateID.Run);
    }

    public override void DoBeforeEntering()
    {
        system.manager.ChangeSpeed(stateID);
        system.manager.animator.SetBool(Defines.AttackAnimationClip, true);
    }
    public override void DoBeforeLeaving()
    {
        timer = 0;
        isAttack = false;
        isPlayAttackMusic = false;
        system.manager.animator.SetBool(Defines.AttackAnimationClip, false);
    }
    private bool isPlayAttackMusic;
    public override void StateUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > 1.2f&& !isAttack)
        {
            if (!isPlayAttackMusic)
            {
                system.manager.ShotMusic(MusicName.Attack);
                isPlayAttackMusic = true;
            }
            if ((Player.Instance.transform.position - system.manager.transform.position).magnitude < system.manager.CanDamageRange)
            {
                isAttack = true;
                Player.Instance.Damage(system.manager.Atk);
            }
            
        }//system.manager.PlayMusic(MusicName.Walk);
        if(timer>2f)
            system.PerformTransition(Enemy_Transition.Run);

    }

}
