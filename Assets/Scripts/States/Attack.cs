using UnityEngine;
using UnityEngine.AI;

public class Attack : State {

    float rotationSpeed = 2.0f;
    AudioSource shoot;

    public Attack(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player) {
        currentState = STATE.Attack;
        shoot = npc.GetComponent<AudioSource>();
    }

    public override void Enter() {
        anim.SetTrigger("isShooting");
        agent.isStopped = true;
        shoot.Play();
        base.Enter();
    }

    public override void Update() {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0; // Evitamos que el enemigo se incline hacia arriba o hacia abajo.

        // Hacemos una rotación con Quaternion porque la rotación debe ser con respecto hacia donde está mirando el enemigo.
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, // Suavizamos la rotación.
            Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime); // LookRotation indica hacia donde mirar.

        if (!CanAttackPlayer()) {
            nextState = new Pursue(npc, agent, anim, player);
            stage = STAGES.Exit;
        }
        else if (!CanSeePlayer()) {
            nextState = new Patrol(npc, agent, anim, player);
            stage = STAGES.Exit;
        }

        //base.Update();
    }

    public override void Exit() {
        shoot.Stop();
        anim.ResetTrigger("isShooting");
        base.Exit();
    }
}