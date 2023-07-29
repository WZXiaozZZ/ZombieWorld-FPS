using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunState : EnemyFSMState
{
    public EnemyRunState()
    {
        stateID = Enemy_StateID.Run;
        AddTransition(Enemy_Transition.Idle, Enemy_StateID.Idle);
        AddTransition(Enemy_Transition.Attack, Enemy_StateID.Attack);
        AddTransition(Enemy_Transition.Patrol, Enemy_StateID.Patrol);
    }

    public override void DoBeforeEntering()
    {
        system.manager.ChangeSpeed(stateID);
        system.manager.animator.SetBool(Defines.RunAnimationClip, true);
    }
    public override void DoBeforeLeaving()
    {
        system.manager.animator.SetBool(Defines.RunAnimationClip, false);
    }

    public override void StateUpdate()
    {
        if ((Player.Instance.transform.position - system.manager.transform.position).magnitude < 4f)
        {
            system.PerformTransition(Enemy_Transition.Attack);
        }
        system.manager.Move(Player.Instance.transform.position);
    }

}
