using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering_Behaviors : MonoBehaviour
{
    public GameObject goPlayer; //Objetivo a perseguir o evadir
    public Rigidbody rbPlayer;  //RB del objetivo

    public Rigidbody myRigidbody;

    //[SerializeField] private Vector2 currentPosition;     //trasnform.position
    //[SerializeField] private Vector2 currentVelocity;     //rigidbody.velocity
    //[SerializeField] private Vector2 targetPosition;      

    public float fMaxSpeed = 4f;
    public float fMaxForce = 6f;
    public float fTime = 1f;
    public float fPredictionsSteps = 2f;
    public float fArriveRadius = 2f;        //La distancia para desacelerar
    public bool bUseArrive;

    Vector3 v3TargetPosition = Vector3.zero;
    Vector3 v3SteeringForceAux = Vector3.zero;

    enum SteeringBehavior { Seek, Flee, Pursue, Evade, Wander, Arrive }
    [SerializeField] SteeringBehavior currentBehavior = SteeringBehavior.Seek;

    enum SteeringTarget { mouse, player }
    [SerializeField] SteeringTarget currentTarget = SteeringTarget.mouse;

    public Vector3 TargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();

        TargetPosition.z = 0.0f;
    }

    private void OnValidate()
    {
        if (currentBehavior == SteeringBehavior.Pursue || currentBehavior == SteeringBehavior.Evade)
        {
            //Buscamos y asignamos un target Rigibody
            SetPursueTarget();
        }
    }




    void Update()
    {
        switch (currentTarget)
        {
            case SteeringTarget.mouse:
                TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                break;

            case SteeringTarget.player:
                TargetPosition = goPlayer.transform.position;
                break;
        }
    }


    private void FixedUpdate()
    {
        //Vector3 TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TargetPosition.z = 0.0f;    //Le pone la z de la camara

        Vector3 v3SteeringForce = Vector3.zero;
        switch (currentBehavior)
        {
            case SteeringBehavior.Seek:
                v3SteeringForce = Seek(TargetPosition);
                break;

            case SteeringBehavior.Flee:
                v3SteeringForce = Flee(TargetPosition);
                break;

            case SteeringBehavior.Pursue:
                v3SteeringForce = Pursuit(rbPlayer);
                break;

            case SteeringBehavior.Evade:
                v3SteeringForce = Evade(rbPlayer);
                break;

            case SteeringBehavior.Wander:
                v3SteeringForce = Wander();
                break;

            case SteeringBehavior.Arrive:
                v3SteeringForce = Arrive(TargetPosition);
                v3TargetPosition = TargetPosition;      //Drawgizmos
                break;
        }

        v3SteeringForceAux = v3SteeringForce;           //DrawGizmos

        myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);  //Aceleración ignora la masa

        //Clamp es para que no exceda la velocidad máxima
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, fMaxSpeed);

    }

    private float ArriveFunction(Vector3 in_v3DesiredDirection)
    {
        float fDistance = in_v3DesiredDirection.magnitude;
        float fDesiredMagnitude = fMaxSpeed;

        if (fDistance < fArriveRadius)
        {
            fDesiredMagnitude = Mathf.InverseLerp(0f, fArriveRadius, fDistance);
        }

        return fDesiredMagnitude;
    }

    public Vector3 Seek(Vector3 in_v3TargetPosition)
    {
        //Dirección deseada es punta "a donde quiero llegar" - cola "donde estoy ahorita"
        Vector3 v3DesiredDirection = in_v3TargetPosition - transform.position;
        float fDesiredMagnitude = fMaxSpeed;

        if (bUseArrive)
        {
            fDesiredMagnitude = ArriveFunction(v3DesiredDirection);
        }

        //Normalized para que la magnitud de la fuerza nunca sea mayor que la maxSpeed
        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * fMaxSpeed;

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;

        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, fMaxForce);

        return v3SteeringForce;
    }

    public Vector3 Flee(Vector3 in_v3TargetPosition)
    {
        //Dirección deseada es punta "a donde quiero llegar" - cola "donde estoy ahorita"
        Vector3 v3DesiredDirection = transform.position - in_v3TargetPosition;

        //Normalized para que la magnitud de la fuerza nunca sea mayor que la maxSpeed
        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * fMaxSpeed;

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;

        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, fMaxForce);

        return v3SteeringForce;
    }

    Vector3 Pursuit(Rigidbody in_target)
    {
        Vector3 v3TargetPosition = in_target.transform.position;
        v3TargetPosition += in_target.velocity * Time.fixedDeltaTime * fPredictionsSteps;

        return Seek(v3TargetPosition);
    }

    void SetPursueTarget()
    {
        goPlayer = GameObject.Find("PursuitTarget");

        if (goPlayer == null)
        {
            Debug.LogError("No PursuitTarget gameObject found in scene...");
            return;
        }

        rbPlayer = goPlayer.GetComponent<Rigidbody>();
        if (rbPlayer == null)
        {
            Debug.LogError("No RigidBody present on gameObject PursuitTarget but it should..");
            return;
        }
    }

    //Vector2 Pursuit(Vector2 in_targetPosition)
    //{
    //    float fDistance = Vector2.Distance(in_targetPosition, currentPosition);
    //    fTime = fDistance / maxSpeed;

    //    Vector2 in_targetVelocity = goPlayer.GetComponent<Rigidbody2D>().velocity;
    //    in_targetPosition += in_targetVelocity * Time.deltaTime * fTime;

    //    return Seek(in_targetPosition);
    //}


    Vector2 Evade(Rigidbody in_target)
    {
        float fDistance = Vector2.Distance(transform.position, in_target.transform.position);
        fTime = fDistance / fMaxSpeed;

        Vector3 v3TargetPosition = in_target.transform.position;
        v3TargetPosition += in_target.velocity * Time.fixedDeltaTime * fTime;

        return Flee(v3TargetPosition);
    }

    Vector3 Arrive(Vector3 in_v3TargetPosition)
    {
        //Check if it's in the radius
        Vector3 v3Diff = in_v3TargetPosition - transform.position;
        float fDistance = v3Diff.magnitude;
        float fDesiredMagnitude = fMaxSpeed;
        
        if (fDistance < fArriveRadius)
        {
            //Entonces, estamos dentro del radio de desaceleración
            //y remplazamos la magnituid deseada por una interpolación entre 0 y el radio del arrive
            fDesiredMagnitude = Mathf.InverseLerp(0.0f, fArriveRadius, fDistance);

            print("Deaccelerating, inverse lerp is: " + fDesiredMagnitude);
        }

        //Else, do not deaccelerate and just do Seek normally
        Vector3 v3DesiredVelocity = v3Diff.normalized * fDesiredMagnitude;

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;

        //Igual aquí, haces este normalized * maxSpeed para que la magnitud de la
        //fuerza nunca sea mayor que la maxSpeed

        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, fMaxForce);
        return v3SteeringForce;
    }
    

    private void OnDrawGizmos()
    {
        if (currentBehavior == SteeringBehavior.Pursue ||
            currentBehavior == SteeringBehavior.Evade)
        {
            Gizmos.color = Color.yellow;

            Vector3 nextPosition = rbPlayer.transform.position + rbPlayer.velocity * Time.fixedDeltaTime * fPredictionsSteps;

            Gizmos.DrawSphere(nextPosition, 0.25f);
        }

        //Dibujamos una línea de la velocidad
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + myRigidbody.velocity);

        //Ahora dibujamos una línea de la fuerza
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + v3SteeringForceAux);

        //Dibujamos una esfera del radio en el que debe dejar de acelerar
        if (currentBehavior == SteeringBehavior.Arrive)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(v3TargetPosition, fArriveRadius);
        }

        if (currentBehavior == SteeringBehavior.Wander)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + v3SteeringForceAux, fCircleRadius);
        }
    }

    //TAREA PARCIAL 1
    //Referencias:
    /*
     https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-wander--gamedev-1624
     https://mrcalderon3d.wordpress.com/2013/09/10/steering-behaviorswander/
     */

    public float fCircleDistance = 5f;
    public Vector2 v2CircleCenter;

    public float fCircleRadius = 3f;    //Entre mayor sea el radio, mayor será la fuerza de desplzamiento
    private Vector2 v2Displacement;

    public float fAngleChange = 1f;
    private float fWanderAngle;          //Escalar que nos indica cuánto debe inclinarse la fuerza de desplazamiento

    //public Vector3 v3MoveVector;
    //public float fSpeed = 1f;

    private Vector3 v3WanderForce;      //La fuerza del vector resultante

    //public float fRotationSpeed = 1f;

    Vector2 Wander()
    {
        //--Calcular el centro del circulo
        v2CircleCenter = myRigidbody.velocity;   //Obtenemos la posición actual
        v2CircleCenter.Normalize();               //Cambiamos su longitud a 1 (apuntando a la misma direccion)
        v2CircleCenter *= fCircleDistance;        //Multiplicamos el vector por un escalar (circle distance = la distancia a la que estaremos del centro)

        //--Fuerza de desplazamiento
        v2Displacement = new Vector3(0, -1, 0);    //Inicializamos el vector
        v2Displacement *= fCircleRadius;          //Multiplicado por el radio del circulo (determina si gira a la derecha o a la izq)

        SetAngle(ref v2Displacement, fWanderAngle);                               //Buscamos la aleatoridad en el moviento

        fWanderAngle += (Random.value * fAngleChange) - (fAngleChange * 0.5f);  //Buscamos un angulo aleatorio (punta menos cola) [No entiendo por que multiplicar por 0.5]


        return v3WanderForce = v2CircleCenter + v2Displacement;                     //Se combinan los vectores para producir la fuerza de desplazamiento
    }

    void SetAngle(ref Vector2 vector, float angle)
    {
        /*
        La direccion de la fuerza se calcula en funcion de un objetivo
        
        La direccion de desplazamiento se calcula en funcion de punto aleatorio 
        en la circuferencia del circulo (por eso usamos coseno y seno)
        */

        float len = vector.magnitude;

        vector.x = Mathf.Cos(angle) * len;

        vector.y = Mathf.Sin(angle) * len;
    }


    /*
     Respecto al 0.5. 

    fWanderAngle += (Random.value * fAngleChange) - (fAngleChange * 0.5f);  //Buscamos un angulo aleatorio (punta menos cola) [No entiendo por que multiplicar por 0.5]

    Random.value nos va a dar un número entre 0 y 1.0f, y, al multiplicarlo por tu fAngleChange (que por ahora también es 1), nos da un valor entre 0 y fAngleChange.
    Después, a eso le restamos fAngleChange * 0.5, es decir, medio fAngleChange ¿Correcto?
    Vamos a los extremos del caso. Si random es 0:
    (0*fAngleChange) - (fAngleChange * 0.5f) -> 
    0 - (fAngleChange * 0.5f) ->
    - fAngleChange/2
    y así queda el valor. 

    Con el random = 1:
    (1*fAngleChange) - (fAngleChange * 0.5f) -> 
    fAngleChange - (fAngleChange * 0.5f) ->
    + fAngleChange/2

    Si te fijas, ambos tienen el mismo valor, pero uno es negativo y el otro es positivo, es decir, el rango posible de valores es [-fAngleChange/2, +fAngleChange/2], ¿Va?
    Lo que está haciendo esa parte del -(fAngleChange * 0.5f) es desplazar el rango posible de valores desde [0.0f, fAngleChange] hacia [-fAngleChange/2, +fAngleChange/2]. 
    
    ¿Por qué? porque el cambio de ángulo puede hacia la izquierda (valores negativos) o hacia la derecha (valores positivos).
     
     */

}