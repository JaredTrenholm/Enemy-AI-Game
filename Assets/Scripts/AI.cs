using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{

    public bool PickRandomPatrol;
    public static int PatrolAmount = 4;
    public Vector3[] PatrolPos = new Vector3[PatrolAmount];
    public Material Patrolling;
    public Material Searching;
    public Material Chasing;
    public Material Attacking;
    public Material Retreating;
    public Material SpottedText;
    public GameObject Player;
    public NavMeshAgent agent;
    public Sight sight;

    private enum State
    {
        patrolling,
        chasing,
        searching,
        attacking,
        retreating
    }

    private float Waiting = 0f;
    private float WaitTarget = 0.5f;
    private float TimeBeforeSwitching = 0f;
    private float TimeTarget = 1f;
    private Vector3 PlayerLastPos;
    private int PatrolTarget = 0;
    private State state;
    private Vector3 PreviousPos;
    private float TimeStuck = 0f;
    private float StuckTarget = 5f;
    private float distance;


    private void Awake()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
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
                    if (System.Math.Abs(distance) < 3)
                    {
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
                this.gameObject.GetComponent<MeshRenderer>().material = Searching;
                break;
            case State.patrolling:
                this.gameObject.GetComponent<MeshRenderer>().material = Patrolling;
                break;
            case State.attacking:
                this.gameObject.GetComponent<MeshRenderer>().material = Attacking;
                break;
            case State.chasing:
                this.gameObject.GetComponent<MeshRenderer>().material = Chasing;
                break;
            case State.retreating:
                this.gameObject.GetComponent<MeshRenderer>().material = Retreating;
                break;
        }

    }
}