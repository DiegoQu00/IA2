using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infiltrador : MonoBehaviour
{

    public GameObject goObjective; //Objetivo a perseguir o evadir
    public Rigidbody rbPlayer;

    public Rigidbody myRigidbody;

    public float fMaxSpeed = 4f;
    public float fArriveRadius = 2f;
    public float fMaxForce = 6f;

    public Vector3 TargetPosition;
    

    enum SteeringTarget { mouse }
    [SerializeField] SteeringTarget currentTarget = SteeringTarget.mouse;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentTarget)
        {
            case SteeringTarget.mouse:
                if (Input.GetMouseButtonDown(0))
                {
                    TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                break;

            
        }

        
    }

    

    private void FixedUpdate()
    {
        //Vector3 TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TargetPosition.z = 0.0f;    //Le pone la z de la camara

        Vector3 v3SteeringForce = Vector3.zero;

            


        v3SteeringForce = Arrive(TargetPosition);
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

    

}
