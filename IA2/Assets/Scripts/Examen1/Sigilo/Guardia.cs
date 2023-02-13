using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardia : MonoBehaviour
{

    [Header("GO&Transforms")]
    public Rigidbody myRigidbody;
    public Rigidbody objRigidbody;
    public Transform t_player;
    public Transform t_head;
    private Vector2 v_playerVector;

    [Header("Rotation")]
    [Range(0f, 360f)]
    public float f_visionAngleBar = 30f;
    public float f_visionAngle = 30f;
    public float f_visionDistance = 10f;
    public float f_rotationAngle=45.0f;
    public float f_rotationAux=0;

    [Header("Timers")]
    public float f_rotateTime = 0;
    public float f_alertTime = 0;
    public float f_atackTime = 0;


    [Header("Movement")]
    public float fMaxSpeed = 4f;
    public float fArriveRadius = 2f;
    public float fMaxForce = 6f;
    public float fPredictionsSteps = 2f;
    public Vector3 TargetPosition;

    enum SteeringTarget {  player }
    [SerializeField] SteeringTarget currentTarget = SteeringTarget.player;



    private bool b_once = true;
    private bool b_detected = false;
    private bool b_alertState;
    public bool b_atackState;
    public bool bUseArrive;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        f_visionAngle = f_visionAngleBar;

    }

    private void Update()
    {
        f_rotateTime += Time.deltaTime;

        f_visionAngleBar = b_detected ? f_visionAngle + 20 : f_visionAngle;

        switch (currentTarget)
        {

            case SteeringTarget.player:
                
                break;
        }

        if (f_rotateTime >= 5)
        {
            Normal_State();
        }

        

        if (b_detected == true)
        {
            Alert_State();
            f_alertTime += Time.deltaTime;

        }
        else
            b_alertState = false;

        if(f_alertTime >= 1)
        {
            b_atackState = true;
            f_atackTime += Time.deltaTime;
        }
        
        v_playerVector = t_player.position - t_head.position;
        if (Vector3.Angle(v_playerVector.normalized, t_head.right) < f_visionAngleBar * 0.5f)
        {
            if(v_playerVector.magnitude < f_visionDistance)
                b_detected=true;
            
        }
        else
            b_detected = false;

        if (f_atackTime >= 6)
        {
            Atack_State();
        }

        if (Input.GetKeyDown("space"))
        {
            objRigidbody.gameObject.SetActive(true);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {

        
        if(b_atackState == true)
        {
            objRigidbody.gameObject.SetActive(false);
            b_atackState=false;
        }
    }

    private void FixedUpdate()
    {

        if (b_alertState == true)
        {
            TargetPosition = t_player.position;
            f_rotateTime = 0;

        }


        

        TargetPosition.z = 0.0f;    //Le pone la z de la camara

        Vector3 v3SteeringForce = Vector3.zero;




        v3SteeringForce = b_atackState ? Pursuit(objRigidbody) :Arrive(TargetPosition);
        //DrawGizmos

        myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);  //Aceleración ignora la masa

        //Clamp es para que no exceda la velocidad máxima
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, fMaxSpeed);


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
        }

        //Else, do not deaccelerate and just do Seek normally
        Vector3 v3DesiredVelocity = v3Diff.normalized * fDesiredMagnitude;

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;

        //Igual aquí, haces este normalized * maxSpeed para que la magnitud de la
        //fuerza nunca sea mayor que la maxSpeed

        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, fMaxForce);
        return v3SteeringForce;
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
    Vector3 Pursuit(Rigidbody in_target)
    {
        Vector3 v3TargetPosition = in_target.transform.position;
        v3TargetPosition += in_target.velocity * Time.fixedDeltaTime * fPredictionsSteps;

        return Seek(v3TargetPosition);
    }

    private void OnDrawGizmos()
    {
        if(b_atackState==false)
        {
            if (f_visionAngle <= 0f) return;

        float f_halfVisionAngle = f_visionAngleBar * 0.5f;

        Vector2 v_p1, v_p2;

        v_p1 = PointForAngle(f_halfVisionAngle, f_visionDistance);
        v_p2 = PointForAngle(-f_halfVisionAngle, f_visionDistance);

        Gizmos.color = b_detected ? Color.red: Color.green;
        Gizmos.DrawLine(t_head.position, (Vector2)t_head.position + v_p1);
        Gizmos.DrawLine(t_head.position, (Vector2)t_head.position + v_p2);
        Gizmos.DrawLine((Vector2)t_head.position + v_p1,(Vector2)t_head.position + v_p2);
        }
        

        
    }

    Vector3 PointForAngle(float angle, float distance)
    {
        return t_head.TransformDirection(
            new Vector2(Mathf.Cos(angle*Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)))
            *distance;


    }

    private void Normal_State()
    {
        if (b_once == true)
        {
            f_rotationAux = transform.eulerAngles.z;
            b_once = false;
        }
        transform.rotation = Quaternion.Euler(0,0,Mathf.Lerp(f_rotationAux, f_rotationAux + f_rotationAngle, f_rotateTime-5.0f));

        if (f_rotateTime >= 6f)
        {
            f_rotateTime = 0;
            b_once = true;
        }
    }

    private void Alert_State()
    {
        b_alertState = true;
    }

    private void Atack_State()
    {
        
        
        b_atackState = false;
        TargetPosition = Vector3.zero;
        f_atackTime = 0;
        f_alertTime = 0;
        
    }
    
}
