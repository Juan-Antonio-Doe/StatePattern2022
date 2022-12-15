using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class Pursue : State {
    
    public Pursue(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player) {
        currentState = STATE.Pursue;
        agent.speed = 5f;
        agent.isStopped = false;
    }

    public override void Enter() {
        //anim.SetBool("isIdle", true);
        anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update() {
        agent.SetDestination(player.position);

        if (agent.hasPath) {
            if (CanAttackPlayer()) {
                nextState = new Attack(npc, agent, anim, player);
                stage = STAGES.Exit;
            }
            else if (!CanSeePlayer()) {
                nextState = new Patrol(npc, agent, anim, player);
                stage = STAGES.Exit;
            }
        }

        //base.Update();
    }

    public override void Exit() {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}
