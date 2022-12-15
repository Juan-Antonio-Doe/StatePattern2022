//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State {
	public Idle(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player) {
        currentState = STATE.Idle;
    }

    public override void Enter() {
        //anim.SetBool("isIdle", true);
        anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update() {
        if (CanSeePlayer()) {
            nextState = new Pursue(npc, agent, anim, player);
            stage = STAGES.Exit;
        }
        else if (IsPlayerTooClose()) {
            nextState = new Flee(npc, agent, anim, player);
            stage = STAGES.Exit;
        }
        else if (Random.Range(0, 100) < 10) {   // Generamos una probabilidad de salida del estado Idle del 10%.
            nextState = new Patrol(npc, agent, anim, player);
            // El evento ahora será EXIT
            stage = STAGES.Exit;
        }
        
        //base.Update();
    }

    public override void Exit() {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}
