using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class aStarAgent : SteeringBehavior
{
    // Start is called before the first frame update
    public bool Selected = false;
    private SpriteRenderer color;

    public List<Vector3> Path = null;
    int i_currentWaypoint = 0;

    public PathfindingTest Pathfinding;
    ClassGrid s_Grid;

    public float f_NearArea;

    void Start()
    {
        color = GetComponent<SpriteRenderer>();
        s_Grid = Pathfinding.myTest;
    }

    // Update is called once per frame
    void Update()
    {
        if(Pathfinding.b_PathR == true)
        {
            Path = s_Grid.ConvertBacktrackToWorldPos(Pathfinding.Pathfinding_result);
            Pathfinding.b_PathR = false;

        }
            
    }
   

    private void FixedUpdate()
    {
        Vector3 v3SteeringForce = Vector3.zero;

        if (Path != null && Selected == true)
        {
            float f_Distance = (Path[i_currentWaypoint] - transform.position).magnitude;
            Debug.Log("fDistance to Point is: " + f_Distance);

            if (f_NearArea > f_Distance && i_currentWaypoint != Path.Count - 1)
            {

                i_currentWaypoint++;
                i_currentWaypoint = math.min(i_currentWaypoint, Path.Count - 1);
            }

            v3SteeringForce = i_currentWaypoint == Path.Count - 1 ? Seek(Path[i_currentWaypoint]) : Arrive(Path[i_currentWaypoint]);


            r_myRigidbody.AddForce(v3SteeringForce, ForceMode.Acceleration);  //Aceleración ignora la masa

            //Clamp es para que no exceda la velocidad máxima
            r_myRigidbody.velocity = Vector3.ClampMagnitude(r_myRigidbody.velocity, f_MaxSpeed);

        }
    }

    
    private void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Selected = true;
            Debug.Log("A*Agent is now Selected");
            color.color = Color.cyan;
            r_myRigidbody.isKinematic = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Selected = false;
            Debug.Log("A*Agent isnt selected");
            color.color = Color.white;
            r_myRigidbody.isKinematic = true;
        }


    }




    }
