//using System;
using UnityEngine;
using UnityEngine.AI;

internal class Flee : State {
    private int currentIndex { get; set; } = -1;
    //private float fearTime { get; set; } = 5f;

    public Flee(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player) {
        currentState = STATE.Flee;
        agent.speed = 5f;
        agent.isStopped = false;
    }

    public override void Enter() {
        //anim.SetBool("isIdle", true);
        agent.ResetPath();

        float lastDist = Mathf.Infinity;

        for (int i = 0; i < GameEnviroment.Singleton.Safepoints.Count; i++) {

            float dist = Vector3.Distance(npc.transform.position, GameEnviroment.Singleton.Safepoints[i].transform.position);

            if (dist < lastDist) {
                currentIndex = i - 1;
                lastDist = dist;
            }
        }
        
        anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update() {

        if (agent.remainingDistance < 1) {
            if (IsPlayerTooClose()) {
                if (currentIndex >= GameEnviroment.Singleton.Safepoints.Count - 1)
                    currentIndex = 0;
                else
                    currentIndex++;

                agent.SetDestination(GameEnviroment.Singleton.Safepoints[currentIndex].transform.position);
            }
            else {
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