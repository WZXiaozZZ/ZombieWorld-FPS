using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyFSMState
{
    public EnemyPatrolState()
    {
        stateID = Enemy_StateID.Patrol;
        AddTransition(Enemy_Transition.Idle, Enemy_StateID.Idle);
        AddTransition(Enemy_Transition.Attack, Enemy_StateID.Attack);
        AddTransition(Enemy_Transition.Run, Enemy_StateID.Run);
    }

    public override void DoBeforeEntering()
    {
        system.manager.ChangeSpeed(stateID);
        system.manager.animator.SetBool(Defines.WalkAnimationClip,true);
    }
    public override void DoBeforeLeaving()
    {
        system.manager.animator.SetBool(Defines.WalkAnimationClip, false);
    }

    public override void StateUpdate()
    {
        system.manager.PlayMusic(MusicName.Walk);
        if (system.manager.Patrol()|| system.manager.isAttacked)
        {
            system.PerformTransition(Enemy_Transition.Run);
        }


    }



}
