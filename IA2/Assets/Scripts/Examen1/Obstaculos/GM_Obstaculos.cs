using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_Obstaculos : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Obstaculo;
    void Start()
    {
        for (int i = 0; i < 7; i++)
            Instantiate(Obstaculo, new Vector3(Random.Range(-10, 10), Random.Range(-7, 7), 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
