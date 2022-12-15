using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State {

    public enum STATE {
        Idle,
        Patrol,
        Pursue,
        Attack,
        Flee,
        Sleep
    }
    
    public enum STAGES {
        Enter,
        Update,
        Exit
    }

    public STATE currentState;
    protected STAGES stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected NavMeshAgent agent;
    protected State nextState;

    float visDist = 10.0f;
    float visAngle = 30.0f;
    float shootDist = 5.0f;
    float fearDistance = 3.0f;

    public State(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) {
        this.npc = npc;
        this.agent = agent;
        this.anim = anim;
        stage = STAGES.Enter;
        this.player = player;
    }

    public virtual void Enter() {
        stage = STAGES.Update;
    }

    public virtual void Update() {
        stage = STAGES.Update;
    }

    public virtual void Exit() {
        stage = STAGES.Exit;
    }

    // Este método nos sirve para cambiar entre los diferentes métodos que cambian el estado.
    public State Process() {
        if (stage == STAGES.Enter) Enter();
        if (stage == STAGES.Update) Update();
        if (stage == STAGES.Exit) {
            Exit();
            return nextState; // Nos devuelve el estado que tocaría a continuación.
        }
        Debug.Log($"Current This type: {this.GetType()}");
        return this;    // Esto nos devolvería el mismo estado en el que nos encontramos si no nos...
    }

    public bool CanSeePlayer() {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        // Si el ángulo es menor que el ángulo de visión y la distancia es menor que la distancia de visión vemos al jugador.
        if (direction.magnitude < visDist && angle < visAngle) {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer() {
        Vector3 direction = player.position - npc.transform.position;
        // Si la distancia es menor que la distancia de disparo podemos atacar al jugador.
        if (direction.magnitude < shootDist) {
            return true;
        }
        return false;
    }

    public bool IsPlayerTooClose() {
        Vector3 direction = player.position - npc.transform.position;
        // Si la distancia es menor que la distancia de miedo el jugador está demasiado cerca.
        if (direction.magnitude < fearDistance) {
            return true;
        }
        return false;
    }
}
