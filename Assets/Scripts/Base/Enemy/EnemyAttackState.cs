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
        system.manager.animator.SetBool(Defines.AttackAnimationClip, false);
    }

    public override void StateUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > 1.2f&& !isAttack)
        {
            if ((Player.Instance.transform.position - system.manager.transform.position).magnitude < 6f)
            {
                isAttack = true;
                Player.Instance.Damage(30);
            }
            
        }//system.manager.PlayMusic(MusicName.Walk);
        if(timer>2f)
            system.PerformTransition(Enemy_Transition.Run);

    }

}
