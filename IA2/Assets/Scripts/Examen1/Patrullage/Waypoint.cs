using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Start is called before the first frame update

    public Patrullage sPatrullage;
    void Start()
    {
        sPatrullage = FindObjectOfType<Patrullage>();
        transform.position = new Vector3(transform.position.x,transform.position.y,0);
        sPatrullage.Waypoints.Add(transform.position);

    }

    private void OnMouseDown()
    {
        sPatrullage.Waypoints.Remove(transform.position);
        Destroy(gameObject);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (sPatrullage.Waypoints[sPatrullage.iTargetWaypoint] == transform.position)
            {
                sPatrullage.iTargetWaypoint= sPatrullage.iTargetWaypoint+1;
                Debug.Log("a");
            }
                
        }
    }


}
