using System;
using UnityEngine;
using UnityEngine.AI;

internal class Patrol : State {

    private int currentIndex { get; set; } = -1;
    
    public Patrol(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player) {
        currentState = STATE.Patrol;
        agent.speed = 2f;
        agent.isStopped = false; // El agente se mueve.
    }

    public override void Enter() {
        float lastDist = Mathf.Infinity;

        for (int i = 0; i < GameEnviroment.Singleton.Checkpoints.Count; i++) {

            float dist = Vector3.Distance(npc.transform.position, GameEnviroment.Singleton.Checkpoints[i].transform.position);

            if (dist < lastDist) {
                currentIndex = i - 1;
                lastDist = dist;
            }
        }

        anim.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update() {
        if (agent.remainingDistance < 1) {
            if (currentIndex >= GameEnviroment.Singleton.Checkpoints.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;
            //currentIndex = (currentIndex >= GameEnviroment.Singleton.Checkpoints.Count - 1) ? (currentIndex = 0) : currentIndex++;

            agent.SetDestination(GameEnviroment.Singleton.Checkpoints[currentIndex].transform.position);
        }

        if (CanSeePlayer()) {
            nextState = new Pursue(npc, agent, anim, player);
            stage = STAGES.Exit;
        }
        else if (IsPlayerTooClose()) {
            nextState = new Flee(npc, agent, anim, player);
            stage = STAGES.Exit;
        }
        
        //base.Update();
    }

    public override void Exit() {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}