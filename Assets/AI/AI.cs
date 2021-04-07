using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    enum State
    {
        patrolling,
        chasing,
        searching,
        attacking,
        retreating
    }

    private float TimeBeforeSwitching = 0f;
    private float TimeTarget = 1f;
    private Vector3 PlayerLastPos;
    public bool PickRandomPatrol;
    public static int PatrolAmount = 4;
    public Vector3[] PatrolPos = new Vector3[PatrolAmount];
    private int PatrolTarget = 0;
    private State state;

    private float distance;

    public GameObject Player;
    public NavMeshAgent agent;

    public Sight sight;

    private Vector3 PreviousPos;
    private float TimeStuck = 0f;
    private float StuckTarget = 5f;

    public GameObject Patrolling;
    public GameObject Searching;
    public GameObject Chasing;
    public GameObject Attacking;
    public GameObject Retreating;
    private float Waiting = 0f;
    private float WaitTarget = 0.5f;


    private void Awake()
    {
        PreviousPos = gameObject.transform.position;
        if (PickRandomPatrol == true)
        {
            System.Random rand = new System.Random();
            PatrolTarget = rand.Next(0, PatrolAmount);
            if (PatrolTarget > PatrolAmount - 1)
            {
                PatrolTarget = PatrolAmount - 1;
                agent.SetDestination(PatrolPos[PatrolTarget]);
            }
        }
    }
    private void Update()
    {
        agent.speed = 3.5f;
        CheckTarget();
        IfStill();
        MoveToPatrol();
        CheckStateSwitch();

        Debug.Log("Target: " + PatrolTarget + "        Enemy Name: " + this.gameObject.name);




    }

    private void CheckTarget()
    {
        if (state == State.attacking)
        {
            Player.gameObject.GetComponent<PlayerMain>().Kill();
        }

        if (sight.CanSee == true)
        {

            PlayerLastPos = Player.gameObject.transform.position;
            ChangeState(State.chasing);
            agent.SetDestination(PlayerLastPos);
        }
    }

    private void IfStill()
    {
        if (PatrolTarget > PatrolAmount - 1)
        {
            PatrolTarget = PatrolAmount - 1;
        }
        if (PreviousPos == this.gameObject.transform.position)
        {
            if (TimeStuck >= StuckTarget)
            {
                ChangeState(State.patrolling);
                agent.SetDestination(PatrolPos[PatrolTarget]);
            }
            else
            {
                TimeStuck = TimeStuck + Time.deltaTime;
            }
        }
        else
        {
            TimeStuck = 0f;
        }
        PreviousPos = this.gameObject.transform.position;
    }

    private void MoveToPatrol()
    {
        switch (state)
        {
            case State.patrolling:
                distance = (this.gameObject.transform.position.x - PatrolPos[PatrolTarget].x) + (this.gameObject.transform.position.z - PatrolPos[PatrolTarget].z);
                if (System.Math.Abs(distance) < 3)
                {
                    if (PickRandomPatrol == false)
                    {
                        PatrolTarget = PatrolTarget + 1;
                        if (PatrolTarget == PatrolAmount)
                        {
                            PatrolTarget = 0;
                            PickRandomPatrol = !PickRandomPatrol;
                        }
                    }
                    else
                    {
                        System.Random rand = new System.Random();
                        PatrolTarget = rand.Next(0, PatrolAmount);
                        if (PatrolTarget > PatrolAmount - 1)
                        {
                            PatrolTarget = PatrolAmount - 1;
                        }
                    }
                }
                agent.SetDestination(PatrolPos[PatrolTarget]);
                break;
            case State.retreating:
                distance = (this.gameObject.transform.position.x - PatrolPos[PatrolTarget].x) + (this.gameObject.transform.position.z - PatrolPos[PatrolTarget].z);
                if (System.Math.Abs(distance) < 3)
                {
                    ChangeState(State.patrolling);
                    PatrolTarget = PatrolTarget + 1;
                    if (PatrolTarget == PatrolAmount)
                    {
                        PatrolTarget = 0;
                    }
                }
                agent.SetDestination(PatrolPos[PatrolTarget]);
                break;
        }
    }

    private void CheckStateSwitch()
    {
        if (TimeBeforeSwitching >= TimeTarget)
        {

            if (sight.CanSee == true)
            {
                if ((Player.transform.position.x < this.gameObject.transform.position.x + 2) && (Player.transform.position.x > this.gameObject.transform.position.x - 2) && (Player.transform.position.y < this.gameObject.transform.position.y + 2) && (Player.transform.position.y > this.gameObject.transform.position.y - 2) && (Player.transform.position.z < this.gameObject.transform.position.z + 2) && (Player.transform.position.z > this.gameObject.transform.position.z - 2))
                {
                    ChangeState(State.attacking);
                }
                else
                {
                    ChangeState(State.chasing);
                }


            }
            else
            {
                if (state == State.chasing) ChangeState(State.searching);
            }
            switch (state)
            {
                case State.searching:
                    distance = (this.gameObject.transform.position.x - PlayerLastPos.x) + (this.gameObject.transform.position.z - PlayerLastPos.z);
                    if (System.Math.Abs(distance) < 3) { 
                        if (Waiting >= WaitTarget)
                        {
                            ChangeState(State.retreating);
                        }
                        else
                        {
                            Waiting = Waiting + Time.deltaTime;
                        }
                    }
                    else
                    {
                    }
                    break;
                case State.patrolling:

                    break;
                case State.attacking:
                    agent.SetDestination(this.gameObject.transform.position);
                    Player.gameObject.GetComponent<PlayerMain>().Kill();

                    if (((sight.CanSee == false) && (state == State.attacking) && ((Player.transform.position.x < this.gameObject.transform.position.x + 2) && (Player.transform.position.x > this.gameObject.transform.position.x - 2) && (Player.transform.position.y < this.gameObject.transform.position.y + 2) && (Player.transform.position.y > this.gameObject.transform.position.y - 2) && (Player.transform.position.z < this.gameObject.transform.position.z + 2) && (Player.transform.position.z > this.gameObject.transform.position.z - 2))))
                    {
                        PlayerLastPos = Player.transform.position;
                        ChangeState(State.searching);
                        agent.SetDestination(PlayerLastPos);
                    }

                    break;
                case State.chasing:
                    agent.SetDestination(PlayerLastPos);
                    break;
                case State.retreating:

                    break;
            }

        }
        else
        {
            TimeBeforeSwitching = TimeBeforeSwitching + Time.deltaTime;
        }
    }

    private void ChangeState(State stateTarget)
    {
        state = stateTarget;
        switch (state)
        {
            case State.searching:
                Searching.SetActive(true);
                Patrolling.SetActive(false);
                Chasing.SetActive(false);
                Attacking.SetActive(false);
                Retreating.SetActive(false);
                break;
            case State.patrolling:
                Searching.SetActive(false);
                Patrolling.SetActive(true);
                Chasing.SetActive(false);
                Attacking.SetActive(false);
                Retreating.SetActive(false);
                break;
            case State.attacking:
                Searching.SetActive(false);
                Patrolling.SetActive(false);
                Chasing.SetActive(false);
                Attacking.SetActive(true);
                Retreating.SetActive(false);
                break;
            case State.chasing:
                Searching.SetActive(false);
                Patrolling.SetActive(false);
                Chasing.SetActive(true);
                Attacking.SetActive(false);
                Retreating.SetActive(false);
                break;
            case State.retreating:
                Searching.SetActive(false);
                Patrolling.SetActive(false);
                Chasing.SetActive(false);
                Attacking.SetActive(false);
                Retreating.SetActive(true);
                break;
        }

    }
}