using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAgentFSM : StateMachine
{
    [HideInInspector]
    public PatrolState patrolState;
    [HideInInspector]
    public AlertState alertState;
    [HideInInspector]
    public AttackState attackState;

    // M�scara de capa para solo hacer el Raycast contra los objetos en esta capa.
    private LayerMask WallLayerMask;
    // private LayerMask InfiltratorLayerMask;

    // Todas las variables que son necesarias para modificarse en el editor
    // public 

    public NavMeshAgent navMeshAgent;
    
    // Posici�n donde el agente va a estar parado mientras est� en el estado de Patrol, y a la cual
    // debe regresar en el estado de Alert para volver al estado de Patrol.
    public Vector3 v3AgentPatrollingPosition;

    public Animator animator;
    public Animator InfAnimator;
    public Light lt;

    // Cono de visi�n.
    // Estas variables pueden cambiar mucho seg�n el modo de juego (2d, 3d, etc.)
    // Variables que necesitamos para definir el cono de visi�n.
    [Range(0.01f, 1000.0f)]
    public float fVisionDist = 10.0f;
    [Range(0.0f, 360.0f)]
    public float fVisionAngle = 90.0f;

    // Hacia d�nde est� viendo el agente Patrullero.
    // public Vector3 v3AgentFacingDirection;  // _sm.transform.forward

    // Una 
    public Transform v3TargetTransform;
    public Vector3 v3LastKnownTargetPos;
    


    [Range(0, 360.0f)]
    public float fRotateAngle = 45f;
    public float FRotateAngle 
    {
        get { return fRotateAngle; }
    }

    [Range(0.1f, 120.0f)]
    public float fTimeToRotate = 2.0f;
    public float FTimeToRotate
    {
        // OJO: Aqu� la hab�a puesto con F may�scula, lo cual es un error que causa un stack
        // overflow inmediato.
        get { return fTimeToRotate; }
    }

    [Range(0.01f, 1000f)]
    public float fAlertVisionDist = 20.0f;
    public float FAlertVisionDist
    {
        get { return fAlertVisionDist; }
    }

    [Range(0.0f, 359.0f)]
    public float fAlertVisionAngle = 90.0f;
    public float FAlertVisionAngle
    {
        get { return fAlertVisionAngle; }
    }

    [Range(0.1f, 120.0f)]
    public float fTimeToGoFromAlertToAttack = 2.0f;
    public float FTimeToGoFromAlertToAttack
    {
        get { return fTimeToGoFromAlertToAttack; }
    }


    [Range(0.1f, 120.0f)]
    public float fTimeBeforeCheckingTargetsLastKnownPosition = 2.0f;
    public float FTimeBeforeCheckingTargetsLastKnownPosition
    {
        get { return fTimeBeforeCheckingTargetsLastKnownPosition; }
    }


    // Qu� tanto tiempo va a perseguir el agente patrullero al agente infiltrador durante el 
    // estado de Ataque.
    [Range(1.0f, 25.0f)]
    public float fMaxChasingTime = 5.0f;
    public float FMaxChasingTime
    {
        get { return fMaxChasingTime; }
    }

    public bool CheckFieldOfVision(float in_fVisionDist, float in_fVisionAngle, out Vector3 v3TargetPos)
    {
        lt.spotAngle = fVisionAngle;
        lt.range = fVisionDist;
        v3TargetPos = Vector3.zero;
        // La comprobaci�n de dos chequeos, uno similar al chequeo del �rea de un c�rculo.
        // y otro que es respecto al �ngulo de ese c�rculo.

        //OJO: Cu�l de estas dos comprobaciones deber�a realizarse primero en t�rminos de 
        // desempe�o (performance).
        Vector3 v3AgentToTarget = (v3TargetTransform.position - transform.position);

        // Profiling o benchmarking
        float fAgentToTargetDist = v3AgentToTarget.magnitude;
        if (fAgentToTargetDist > in_fVisionDist)
        {
            // Nos salimos porque no est� en el rango de visi�n.
            return false;
        }

        if (Vector3.Angle(v3AgentToTarget, transform.forward) > in_fVisionAngle * 0.5)
        {
            // Nos salimos porque no se est� dentro del �ngulo que define al cono.
            return false;
        }

        // si el raycast choca primero contra una wall que contra el Target, entonces no  
        // puede ver al Target tal cual, porque hay una pared de por medio.
        if (Physics.Raycast(transform.position, v3AgentToTarget.normalized, 
            v3AgentToTarget.magnitude, WallLayerMask))
        {
            return false;
        }

        v3TargetPos = v3TargetTransform.position;
        return true;
    }

    private Vector3 PointForAngle(float fAngle, float fDistance)
    {
        float fAngleRads = Mathf.Rad2Deg * fAngle;
        return transform.TransformDirection(
            new Vector2(Mathf.Cos(fAngleRads), Mathf.Sin(fAngleRads))
            * fVisionDist);
    }

    // 
    private void Awake()
    {
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);

        WallLayerMask = LayerMask.GetMask("Wall");
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Esto es para que la posici�n en la que el patrullero inicie en la escena sea su posici�n
        // a la que deba regresar para volver al estado Patrol. S
        // Si ustedes no desearan este comportamiento, por favor cambien esta l�nea.
        v3AgentPatrollingPosition = transform.position;
    }

    protected override BaseState GetInitialState()
    { 
        // Seg�n definimos para este agente, el primer estado debe ser el de patrullar.
        return patrolState;
    }

    



}
