using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Player;
using GameDev;
using Unity.VisualScripting;
using Interactions;

namespace Enemy
{
    [AddComponentMenu("GameDev/Enemy/AI Controller")]

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class AIController : MonoBehaviour, IInteractable
    {
        public enum AIState
        {
            Idle,
            Patrol,
            Wander,
            Stun,
            Attack,
            Chase
        }
        #region Variables 
        //the current state - going to need states???
        [SerializeField] private AIState _state = AIState.Idle;
        //Nav Mesh Agent
        [SerializeField] private NavMeshAgent _agent;
        //Animator
        [SerializeField] private Animator _animator;
        //walk speed and a run/chase speed
        [SerializeField] private float _walkSpeed = 2f, _runSpeed = 5;
        //patrolPoints/wayPoints []array of locations
        [SerializeField] private Transform[] _wayPoints;
        //iteration of array
        [SerializeField] private int _currentWayPointIndex = 0;
        // move randomly???
        [SerializeField] private Vector3 _randomPosition;
        //where are you player??
        [SerializeField] private float OGdetectionRadius = 5f;
        [SerializeField] public float detectionRadius = 5f;
        //who are you player??
        [SerializeField] private LayerMask _playerLayer;
        //keep chasing distance
        [SerializeField] private float _chaseDistance = 15f;
        //attack distance
        [SerializeField] private float _attackDistance = 2.5f;
        [SerializeField] private float _attackDamage = 30f;
        //time between attacks
        //[SerializeField] private float _attackCooldown = 2f; // Not used

        //[SerializeField] private float _lastAttackTime = 0f; // Not used

        [SerializeField] private float _stunDuration = 3f;
        [SerializeField] float timer = 0;
        //bool idleChoice = false; // Not used
        bool _isStunned = false;
        public bool isPlayerCrouch;
        public bool isPlayerRun;
        [SerializeField] public CharacterController _characterController;
        [SerializeField] public bool _characterHealth;

        string currentAnim;
        #endregion
        #region Unity Event Functions
        private void Start()
        {
            OGdetectionRadius = detectionRadius;
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _state = AIState.Idle;
            TransitionToState(_state);

            _characterController = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        }
        private void Update()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                _animator.speed = 1;

                isPlayerCrouch = _characterController.GetComponent<Movement>().isCrouch;
                if (!isPlayerRun)
                {
                    if (isPlayerCrouch)
                    {
                        if (detectionRadius == OGdetectionRadius || detectionRadius == detectionRadius * 2)
                        {
                            detectionRadius = OGdetectionRadius;
                            // Debug.Log(detectionRadius);
                            detectionRadius = detectionRadius / 2;
                            // Debug.Log(detectionRadius);
                        }
                    }
                    else
                    {
                        detectionRadius = OGdetectionRadius;
                    }
                }

                isPlayerRun = _characterController.GetComponent<Movement>().isRun;
                if (!isPlayerCrouch)
                {
                    if (isPlayerRun)
                    {
                        if (detectionRadius == OGdetectionRadius || detectionRadius == detectionRadius / 2)
                        {
                            detectionRadius = OGdetectionRadius;
                            detectionRadius = detectionRadius * 2;
                        }
                    }
                    else
                    {
                        detectionRadius = OGdetectionRadius;
                    }
                }

                switch (_state)
                {
                    case AIState.Idle:
                        StartCoroutine(Idle());
                        break;
                    case AIState.Patrol:
                        Patrol();
                        break;
                    case AIState.Wander:
                        Wander();
                        break;
                    case AIState.Stun:
                        Stun();
                        break;
                    case AIState.Attack:
                        Attack();
                        break;
                    case AIState.Chase:
                        Chase();
                        break;
                }
            }
            else
            {
                _agent.isStopped = true;
                _agent.speed = 0;
                _animator.speed = 0;
            }
        }

        private void LateUpdate()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, GetPlayerPosition());
                if (distanceToPlayer <= _attackDistance)
                {
                    TransitionToState(AIState.Attack);
                }
                if (distanceToPlayer <= detectionRadius && distanceToPlayer > _attackDistance)
                {
                    TransitionToState(AIState.Chase);
                }
                if (distanceToPlayer > _chaseDistance && _state == AIState.Chase)
                {
                    TransitionToState(AIState.Idle);
                }
                switch (_state)
                {
                    case AIState.Idle:
                        _agent.isStopped = true;
                        _agent.speed = 0;
                        StartCoroutine(Idle());
                        break;
                    case AIState.Patrol:
                        _agent.isStopped = false;
                        _agent.speed = _walkSpeed;
                        Patrol();
                        break;
                    case AIState.Wander:
                        _agent.isStopped = false;
                        _agent.speed = _walkSpeed;
                        Wander();
                        break;
                    case AIState.Stun:
                        _agent.destination = this.transform.position;
                        _agent.isStopped = true;;
                        _agent.speed = 0;
                        Stun();
                        break;
                    case AIState.Attack:
                        _agent.isStopped = true;
                        _agent.speed = 0;
                        Attack();
                        break;
                    case AIState.Chase:
                        _agent.isStopped = false;
                        _agent.speed = _runSpeed;
                        Chase();
                        break;
                }
            }
        }
        #endregion

        #region States
        void TransitionToState(AIState newState)
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                _state = newState;
            }
        }
        IEnumerator Idle()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                if (_isStunned)
                {
                    yield return null;
                }
                PlayAnim("Idle");
                _agent.stoppingDistance = 2.5f;
                _agent.speed = 0;
                timer = Random.Range(3, 10f);
                yield return new WaitForSeconds(timer);
                if (_state == AIState.Idle)
                {
                    int choice = Random.Range(0, 2);
                    if (choice == 0)
                    {
                        _randomPosition = GetRandomPosition();
                        TransitionToState(AIState.Wander);

                    }
                }
                else
                {
                    TransitionToState(AIState.Patrol);
                }
            }
        }


        void Patrol()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                if (_isStunned)
                {
                    return;
                }
                _agent.speed = _walkSpeed;
                _agent.stoppingDistance = 0f;

                PlayAnim("Walk");
                if (_wayPoints.Length == 0)
                {
                    TransitionToState(AIState.Idle);
                }
                if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    int choice = Random.Range(0, 6);
                    if (choice == 0)
                    {
                        TransitionToState(AIState.Idle);
                    }
                    else
                    {
                        TransitionToNextWayPoint();
                    }
                }
            }
        }

        void TransitionToNextWayPoint()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                if (_isStunned)
                {
                    return;
                }
                _currentWayPointIndex = (_currentWayPointIndex + 1) % _wayPoints.Length;
                _agent.SetDestination(_wayPoints[_currentWayPointIndex].position);
                _agent.speed = _walkSpeed;
                PlayAnim("Walk");
            }
        }
        void Wander()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                if (_isStunned)
                {
                    return;
                }
                _agent.SetDestination(_randomPosition);
                _agent.stoppingDistance = 0f;

                while (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    GetRandomPosition();
                    _agent.SetDestination(_randomPosition);
                }
                _agent.speed = _walkSpeed;
                PlayAnim("Walk");
                //Debug.Log("1");
                if (_agent.remainingDistance <= 1f)
                {
                    //Debug.Log("2");
                    int choice = Random.Range(0, 10);
                    //Debug.Log("3: " + choice);

                    if (choice == 0)
                    {
                        //Debug.Log("4: Idle");
                        TransitionToState(AIState.Idle);
                    }
                    else
                    {
                        //Debug.Log("4: Wander");
                        _randomPosition = GetRandomPosition();
                        //Debug.Log("5: Wander");
                        TransitionToState(AIState.Wander);
                    }
                }
            }
        }
        Vector3 GetRandomPosition()
        {
            Vector3 finalPosition = Vector3.zero;
            Vector3 randomDirection = Random.insideUnitSphere * 10;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 10, 1))
            {
                finalPosition = hit.position;
                return finalPosition;

            }
            return this.transform.position;
        }
        void Chase()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                if (_isStunned)
                {
                    //Debug.Log("Return Coz Stun");
                    return;
                }
                _agent.SetDestination(GetPlayerPosition());
                //Debug.Log("Destination");
                PlayAnim("Run");
                //Debug.Log("Animation");
                _agent.stoppingDistance = 0.2f;
                //Debug.Log("stop dist");
                _agent.speed = _runSpeed;
                //Debug.Log("runSpeed");

                // Slow down as the enemy approaches the player
                float distanceToPlayer = Vector3.Distance(transform.position, GetPlayerPosition());
                if (distanceToPlayer <= _attackDistance)
                {
                    _characterHealth = _characterController.GetComponent<Health>().isHurt;
                    if (!_characterHealth)
                    {
                        _characterController.GetComponent<Health>().DamagePlayer(_attackDamage);
                        TransitionToState(AIState.Attack);
                    }
                }
            }
        }

        void Stun()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                _isStunned = true;
                // Disable enemy and stop movement
                //Debug.Log("Need to make sure they enemy doesn't keep moving...");
                _agent.isStopped = _isStunned;
                // Play the stun animation
                PlayAnim("Stun");
                // Start a coroutine to resume after the stun duration
                StartCoroutine(RecoverFromStun());
            }
        }

        IEnumerator RecoverFromStun()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                // Wait for the stun duration
                yield return new WaitForSeconds(_stunDuration);
                _isStunned = false;
                // Enable the enemy to move again
                _agent.isStopped = _isStunned;
                // Transition back to a previous state
                TransitionToState(AIState.Idle);
            }
        }

        void Attack()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                if (_isStunned)
                {
                    return;
                }
                // Stop enemy while attacking to prevent sliding past Player
                _agent.isStopped = true;
                _agent.speed = 0;
                _agent.stoppingDistance = 3f;
                // Trigger attack animation
                PlayAnim("Attack");

                // Once the attack is over, re-enable
                StartCoroutine(ResumeAfterAttack());
            }
        }

        // Coroutine to resume movement after the attack animation
        IEnumerator ResumeAfterAttack()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                // Wait for the attack animation duration or a fixed time
                yield return new WaitForSeconds(1f);

                // After the attack, re-enable movement
                _agent.isStopped = false;

                // If the player is still in range, transition to Chase, otherwise go idle or patrol
                float distanceToPlayer = Vector3.Distance(transform.position, GetPlayerPosition());
                if (distanceToPlayer > _attackDistance)
                {
                    TransitionToState(AIState.Chase);
                }
                else
                {
                    TransitionToState(AIState.Patrol);
                }
            }
        }

        #endregion
        void PlayAnim(string trigger)
        {
            currentAnim = trigger;
            if (_animator != null)
            {
                _animator.SetTrigger(trigger);
            }
        }
        private Vector3 GetPlayerPosition()
        {
            return _characterController.transform.position;
        }


        public void Interact()
        {
            Debug.Log("Yep I have run...");
            if (GameManager.instance.state == GameStates.Play)
            {
                if (!_isStunned)
                {
                    Debug.Log("Hit the enemy...");
                    Stun();
                    Debug.Log("Stun anim...");
                }
            }
        }


        public void OnTriggerEnter(Collider other)
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                if (other.tag == "NearDoor")
                {
                    // Debug.Log("In front door");
                    TransitionToState(AIState.Idle);
                    //nearDoor = true;
                    other.GetComponentInChildren<RayDoor>().CheckDoorForAI();
                }
            }
        }

        // public void OnTriggerExit(Collider other)
        // {
        //     if (GameManager.instance.state == GameStates.Play)
        //     {
        //         if (other.tag == "NearDoor")
        //         {
        //             Debug.Log("Left the door");
        //             // TransitionToState(AIState.Idle);
        //             //nearDoor = false;
        //         }
        //     }
        // }
    }
}

