//
// Diego Quintero Martinez --- IAVideojuegos --- UCQ --- Examen1 --- IDVMI 
//
// Clase GM_Obstaculo
//
// GameManager, Nos ayuda a generar los obstaculos de manera aleatoria dentro de la escena. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_Obstaculos : MonoBehaviour
{
    // Obstaculo
    public GameObject Obstaculo;
    void Start()
    {
        // For que instanciara los obstaculos en posiciones aleatorias dentro de un area delimitada. 
        for (int i = 0; i < 7; i++)
            Instantiate(Obstaculo, new Vector3(Random.Range(-10, 10), Random.Range(-7, 7), 0), Quaternion.identity);
    }

   
}
