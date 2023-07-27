using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemy_StateID
{
    NullStateID = 0,
    Idle,
    Patrol,
    Attack,
    Run,
}

public enum Enemy_Transition
{
    NullTransition = 0,
    Idle,
    Patrol,
    Attack,
    Run,
}


public class EnemyFSMSystem
{

    private List<EnemyFSMState> states;

    public EnemyManager manager;
    private Enemy_StateID currentStateID;
    public Enemy_StateID CurrentStateID { get { return currentStateID; } }
    private EnemyFSMState currentState;
    public EnemyFSMState CurrentState { get { return currentState; } }

    public EnemyFSMSystem()
    {
        states = new List<EnemyFSMState>();

    }



    /// <summary>
    /// 添加状态
    /// </summary>
    public void AddState(EnemyFSMState s)
    {
        // Check for Null reference before deleting
        if (s == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        // 第一个状态默认为初始状态
        if (states.Count == 0)
        {
            states.Add(s);
            currentState = s;
            currentStateID = s.ID;
            return;
        }

        // 如果有重复状态则报错
        foreach (EnemyFSMState state in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                               " because state has already been added");
                return;
            }
        }
        s.system = this;
        states.Add(s);
    }

    /// <summary>
    /// 删除状态
    /// </summary>
    public void DeleteState(Enemy_StateID id)
    {
        // 检测为空，报错
        if (id == Enemy_StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
            return;
        }

        // 搜索到就删除
        foreach (EnemyFSMState state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
                       ". It was not on the list of states");
    }

    /// <summary>
    /// 执行转换，必须通过转换改变状态
    /// </summary>
    public void PerformTransition(Enemy_Transition trans)
    {
        if (trans == Enemy_Transition.NullTransition)
        {
            Debug.LogError("FSM ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        // Check if the currentState has the transition passed as argument
        Enemy_StateID id = currentState.GetOutputState(trans);
        if (id == Enemy_StateID.NullStateID)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
                           " for transition " + trans.ToString());
            return;
        }

        // Update the currentStateID and currentState		
        currentStateID = id;
        foreach (EnemyFSMState state in states)
        {
            if (state.ID == currentStateID)
            {
                // Do the post processing of the state before setting the new one
                currentState.DoBeforeLeaving();

                currentState = state;

                // Reset the state to its desired condition before it can reason or act
                currentState.DoBeforeEntering();
                break;
            }
        }

    }
}
