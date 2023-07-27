using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSMState
{
    protected Dictionary<Enemy_Transition, Enemy_StateID> map = new Dictionary<Enemy_Transition, Enemy_StateID>();
    protected Enemy_StateID stateID;
    public EnemyFSMSystem system;
    public Enemy_StateID ID { get { return stateID; } }

    public void AddTransition(Enemy_Transition trans, Enemy_StateID id)
    {
        // Check if anyone of the args is invalid
        if (trans == Enemy_Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if (id == Enemy_StateID.NullStateID)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }

        // Since this is a Deterministic FSM,
        //   check if the current transition was already inside the map
        if (map.ContainsKey(trans))
        {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                           "Impossible to assign to another state");
            return;
        }

        map.Add(trans, id);
    }

    /// <summary>
    /// This method deletes a pair transition-state from this state's map.
    /// If the transition was not inside the state's map, an ERROR message is printed.
    /// </summary>
    public void DeleteTransition(Enemy_Transition trans)
    {
        // Check for NullTransition
        if (trans == Enemy_Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }

        // Check if the pair is inside the map before deleting
        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
                       " was not on the state's transition list");
    }

    /// <summary>
    /// This method returns the new state the FSM should be if
    ///    this state receives a transition and 
    /// </summary>
    public Enemy_StateID GetOutputState(Enemy_Transition trans)
    {
        // Check if the map has this transition
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return Enemy_StateID.NullStateID;
    }

    /// <summary>
    /// 进入状态时执行
    /// </summary>
    public virtual void DoBeforeEntering()
    {
    }

    /// <summary>
    ///离开状态时执行
    /// </summary>
    public virtual void DoBeforeLeaving() { }

    /// <summary>
    /// 在此状态执行
    /// </summary>
    public virtual void StateUpdate()
    {

    }
}
