using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    const string IDLE = "Idle";
    const string WALK = "Walk";
    const float MOVEMENT_SPEED_THRESHOLD = 0.05f;
    const float ARRIVAL_TOLERANCE = 0.05f;

    CustomActions input;
    NavMeshAgent agent;
    Animator animator;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;
    [SerializeField] float lookRotationSpeed = 8f;
    [SerializeField, Min(0f)] float clickEffectSurfaceOffset = 0.1f;

    private string currentAnimation;

    private bool alive = true;

    public bool Alive
    {
        get => alive;
        set => alive = value;
    }
    void Awake()
    {
        input = new CustomActions();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent != null)
            agent.updateRotation = false;

        AssignInputs();
    }

    public void SetActiveAnimator(Animator newAnimator)
    {
        if (newAnimator == null)
        {
            Debug.LogWarning("[PlayerController] SetActiveAnimator recibió un Animator nulo.");
            return;
        }
        animator = newAnimator;
    }

    void AssignInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
    }

    void ClickToMove()
    {
        if (!alive)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Camera.main == null)
        {
            Debug.LogError("No hay una cámara etiquetada como 'MainCamera'.");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, clickableLayers))
        {
            if (agent == null || !agent.enabled)
            {
                Debug.LogError("El NavMeshAgent no está configurado o habilitado.");
                return;
            }

            if (!NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                Debug.LogWarning("El punto clicado no está en el NavMesh.");
                return;
            }

            agent.destination = navHit.position;

            if (clickEffect != null)
            {
                Quaternion surfaceRotation =
                    Quaternion.FromToRotation(Vector3.up, hit.normal) *
                    clickEffect.transform.rotation;

                ParticleSystem effect = Instantiate(
                    clickEffect,
                    hit.point + hit.normal * clickEffectSurfaceOffset,
                    surfaceRotation);
                effect.Play();
            }
            else
            {
                Debug.LogWarning("El efecto de clic no está asignado.");
            }
        }
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        bool isMoving = IsMoving();
        FaceMovementDirection(isMoving);
        SetAnimations(isMoving);
    }

    bool IsMoving()
    {
        if (!alive || agent == null || !agent.enabled || !agent.isOnNavMesh || agent.pathPending)
            return false;

        bool hasArrived = agent.hasPath &&
                          agent.remainingDistance <= agent.stoppingDistance + ARRIVAL_TOLERANCE;
        if (hasArrived)
            return false;

        return agent.hasPath &&
               agent.desiredVelocity.sqrMagnitude > MOVEMENT_SPEED_THRESHOLD * MOVEMENT_SPEED_THRESHOLD;
    }

    void FaceMovementDirection(bool isMoving)
    {
        if (!isMoving)
            return;

        Vector3 direction = agent.desiredVelocity;
        direction.y = 0f;
        if (direction.sqrMagnitude <= Mathf.Epsilon)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            lookRotationSpeed * Time.deltaTime);
    }

    void SetAnimations(bool isMoving)
    {
        if (animator == null)
            return;

        string nextAnimation = isMoving ? WALK : IDLE;
        if (currentAnimation == nextAnimation)
            return;

        currentAnimation = nextAnimation;
        animator.Play(nextAnimation);
    }
}


    

