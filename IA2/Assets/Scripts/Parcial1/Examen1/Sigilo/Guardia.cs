//
// Diego Quintero Martinez --- IAVideojuegos --- UCQ --- Examen1 --- IDVMI 
//
// Clase Guardia
//
// Agente Guardia que vigilara una zona, cuenta con un campo de vision para detectar al agente Infiltrador. 
// Modo Normal/Default:  Cada cierto tiempo girara 45 grados.
//
// Si el agente infiltador entra en el campo de vision del guardia, entrara a modo alerta.
// Modo Alerta : Ampliara un poco su campo de vision(solo hacia lo ancho),e ira hacia la ultima posicion donde se vio al infiltrador. 
//
// Si el guardia detecta por mas de 1 segundo total al infiltrador, entrara en modo Ataque; 
// Modo Ataque: Perseguira por 5 segundos al Infiltrador, al tocar al infiltrador lo destruye. Si destruye al infiltrador o terminan los 5 segundos, volvera a su posicion inicial.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Guardia : MonoBehaviour
{
    // Declaracion de variables

    // Variables tipo componentes que se usaran en el script. 
    [Header("Components")]
    public Rigidbody r_myRigidbody = null;
    public Rigidbody r_objRigidbody = null;
    public Transform t_infiltrator = null;
    public Transform t_guard = null;
    private SpriteRenderer color;

    // Variables necesarias para la rotacion del guardia 
    [Header("Rotation")]
    [Range(0f, 360f)]
    public float f_visionAngleBar = 30f;
    public float f_visionAngle = 30f;
    public float f_visionDistance = 10f;
    public float f_rotationAngle = 45.0f;
    public float f_rotationAux = 0;

    // Variables de tiempo. 
    [Header("Timers")]
    public float f_rotateTime = 0;
    public float f_alertTime = 0;
    public float f_atackTime = 0;

    // Variables necesarias para el movimiento del guardia.
    [Header("Movement")]
    public float f_MaxSpeed = 4f;
    public float f_ArriveRadius = 2f;
    public float f_MaxForce = 6f;
    public float f_PredictionsSteps = 2f;
    public Vector3 v3_TargetPosition;
    private Vector2 v2_infiltratorVector;

    // Objetivo del guardia.
    enum SteeringTarget {  infiltrator }
    [SerializeField] SteeringTarget currentTarget = SteeringTarget.infiltrator;


    //Booleanas
    private bool b_once = true;
    private bool b_detected = false;
    private bool b_alertState;
    private bool b_atackState;
    public bool b_UseArrive;

    


    //Se inicializa al momento de dar play o al generarse el objeto. 
    private void Start()
    {
        r_myRigidbody = GetComponent<Rigidbody>();
        f_visionAngle = f_visionAngleBar;
        color = GetComponent<SpriteRenderer>();
        color.color = Color.cyan;
       

    }

    //Se actualiza cada frame
    private void Update()
    {
        // Inicio de tiempo para los giros.
        f_rotateTime += Time.deltaTime;
        
        // Si detecta al objetivo aumenta el angulo de vision, si no se mantiene normal.
        f_visionAngleBar = b_detected ? f_visionAngle + 20 : f_visionAngle;

        
        // Si pasaron 5 segundos, llama a la funcion que gira el objeto.
        if (f_rotateTime >= 5)
        {
            Normal_State();
        }

        
        // Si detecta al infiltrador  entra en modo alerta, cambia su color y empieza a contar cuanto tiempo se mantiene "detectado"
        if (b_detected == true)
        {
            Alert_State();
            color.color = Color.yellow;
            f_alertTime += Time.deltaTime;

        }
        // Si no lo detecta mantiene apagado el estado de alerta
        else
            b_alertState = false;

        // Si fue detectado mas de 1 segundo, entra en modo de ataque, cambia su color y comienza el timepo de ataque. 
        if(f_alertTime >= 1)
        {
            b_atackState = true;
            color.color = Color.red;
            f_atackTime += Time.deltaTime;
        }

        // PUNTA-COLA 
        v2_infiltratorVector = t_infiltrator.position - t_guard.position;

        // Deteccion de infiltrator en la vision del guard
        if (Vector3.Angle(v2_infiltratorVector.normalized, t_guard.right) < f_visionAngleBar * 0.5f)
        {
            if(v2_infiltratorVector.magnitude < f_visionDistance)
                b_detected=true;
            
        }

        // Apagar booleana
        else
            b_detected = false;

        // Si lo sigue por mas de 6 segundos, termina el ataque. 
        if (f_atackTime >= 6)
        {
            Atack_State();

        }

        // Reactivar el infiltrator
        if (Input.GetKeyDown("space"))
        {
            //Posicion de spawn
            if (r_objRigidbody.gameObject.activeSelf == false)
                r_objRigidbody.gameObject.transform.position = new Vector3(0, 3, 0);

            // Activar el infiltrator
            r_objRigidbody.gameObject.SetActive(true);
        }


    }


    // Si hay colision con el infiltrator, entra esta funcion
    private void OnCollisionEnter(Collision collision)
    {

        // Si esta en modo ataque
        if(b_atackState == true)
        {

            // Desactivar Infiltrator
            r_objRigidbody.gameObject.SetActive(false);
            
            // Regresar a 0,0 el guardia 
            r_objRigidbody.transform.position = Vector3.zero;

           
                
            // Desactivar modo ataque
            b_atackState = false;

        }
    }

    // Corre a un rate estable, a diferencia del update que corre cada frame variando entre una y otra computadora. 
    private void FixedUpdate()
    {
        // Si esta en modo alerta, posicion objetivo es la del infiltrator. 
        if (b_alertState == true)
        {
            v3_TargetPosition = t_infiltrator.position;
            //Desactivar giro.
            f_rotateTime = 0;

        }


        
       
        v3_TargetPosition.z = 0.0f;    

        Vector3 v3SteeringForce = Vector3.zero;




        v3SteeringForce = b_atackState ? Pursuit(r_objRigidbody) :Arrive(v3_TargetPosition);
        //DrawGizmos

        r_myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);  //Aceleración ignora la masa

        //Clamp es para que no exceda la velocidad máxima
        r_myRigidbody.velocity = Vector3.ClampMagnitude(r_myRigidbody.velocity, f_MaxSpeed);


    }

    // Funcion de movimiento Arrive, se mueve hacia un punto objetivo. Nos devuelve la SteeringForce necesaria para el movimiento, de acuerdo a el objetivo deseado.
    public Vector3 Arrive(Vector3 in_v3v3_TargetPosition)
    {

        Vector3 v3Diff = in_v3v3_TargetPosition - transform.position;
        float fDistance = v3Diff.magnitude;
        float fDesiredMagnitude = f_MaxSpeed;

        if (fDistance < f_ArriveRadius)
        {

            fDesiredMagnitude = Mathf.InverseLerp(0.0f, f_ArriveRadius, fDistance);
        }

        Vector3 v3DesiredVelocity = v3Diff.normalized * fDesiredMagnitude;

        Vector3 v3SteeringForce = v3DesiredVelocity - r_myRigidbody.velocity;



        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, f_MaxForce);
        return v3SteeringForce;
    }

    // Funcion Arrive, necesaria para el funcionamiento del Seek.
    private float ArriveFunction(Vector3 in_v3DesiredDirection)
    {
        float fDistance = in_v3DesiredDirection.magnitude;
        float fDesiredMagnitude = f_MaxSpeed;

        if (fDistance < f_ArriveRadius)
        {
            fDesiredMagnitude = Mathf.InverseLerp(0f, f_ArriveRadius, fDistance);
        }

        return fDesiredMagnitude;
    }

    // Funcion Seek, necesaria para el funcionamiento del pursuit. Se mueve hacia el objetivo.
    public Vector3 Seek(Vector3 in_v3v3_TargetPosition)
    {
        //PUNTA-COLA
        Vector3 v3DesiredDirection = in_v3v3_TargetPosition - transform.position;
        float fDesiredMagnitude = f_MaxSpeed;

        if (b_UseArrive)
        {
            fDesiredMagnitude = ArriveFunction(v3DesiredDirection);
        }

        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * f_MaxSpeed;

        Vector3 v3SteeringForce = v3DesiredVelocity - r_myRigidbody.velocity;

        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, f_MaxForce);

        return v3SteeringForce;
    }

    // Funcion Pursuit, se mueve hacia el objetivo prediciendo n pasos, asi puede alcanzarlo mas facilmente, en vez de solo seguir sus pasos.
    Vector3 Pursuit(Rigidbody in_target)
    {
        Vector3 v3v3_TargetPosition = in_target.transform.position;
        v3v3_TargetPosition += in_target.velocity * Time.fixedDeltaTime * f_PredictionsSteps;

        return Seek(v3v3_TargetPosition);
    }

    // Funcion para mostrar Gizmos
    private void OnDrawGizmos()
    {
        // Desactivar si esta atacando
        if(b_atackState==false)
        {
            // Si el angulo de vision es menor a 0, salir. 
            if (f_visionAngle <= 0f) return;

        // Campo de vision del guardia 
        float f_halfVisionAngle = f_visionAngleBar * 0.5f;

        Vector2 v_p1, v_p2;

        v_p1 = PointForAngle(f_halfVisionAngle, f_visionDistance);
        v_p2 = PointForAngle(-f_halfVisionAngle, f_visionDistance);

        // Si detecta al infiltrado, cambiar el color del campo de vision. 
        Gizmos.color = b_detected ? Color.red: Color.green;
        Gizmos.DrawLine(t_guard.position, (Vector2)t_guard.position + v_p1);
        Gizmos.DrawLine(t_guard.position, (Vector2)t_guard.position + v_p2);
        Gizmos.DrawLine((Vector2)t_guard.position + v_p1,(Vector2)t_guard.position + v_p2);
        }
        

        
    }

    // Creacion del angulo para la vision
    Vector3 PointForAngle(float angle, float distance)
    {
        return t_guard.TransformDirection(
            new Vector2(Mathf.Cos(angle*Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)))
            *distance;


    }

    // Funcion para rotar el guardia
    private void Normal_State()
    {
        // Guardar valor de rotacion inicial
        if (b_once == true)
        {
            f_rotationAux = transform.eulerAngles.z;
            b_once = false;
        }
        // Rotar poco a poco de posicion inicial a posicion inicial + la rotacion adicional
        transform.rotation = Quaternion.Euler(0,0,Mathf.Lerp(f_rotationAux, f_rotationAux + f_rotationAngle, f_rotateTime-5.0f));

        // Salir de la funcion, tiempo necesario para que rote por completo
        if (f_rotateTime >= 6f)
        {
            f_rotateTime = 0;
            b_once = true;
        }
    }

    // Funcion para cuando se detecta por mas de 1 segundo al objetivo.
    private void Alert_State()
    {
        b_alertState = true;
    }

    //Funcion para cuando termine de atacar, si toco al infiltrador o termino su tiempo.
    private void Atack_State()
    {
        b_atackState = false;
        v3_TargetPosition = Vector3.zero;
        f_atackTime = 0;
        f_alertTime = 0;
        color.color = Color.cyan;
        
    }
    
}
