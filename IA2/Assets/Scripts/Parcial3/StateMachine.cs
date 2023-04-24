using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

 These StateMachine and BaseState classes are based on Mina Pecheux's work posted at:
    https://medium.com/c-sharp-progarmming/make-a-basic-fsm-in-unity-c-f7d9db965134
    https://youtu.be/-VkezxxjsSE

 */


public class StateMachine : MonoBehaviour
{
    // Referencia al estado actual de la m�quina.
    BaseState currentState;

    // Funci�n get para que otros scripts puedan saber en qu� estado se encuentra la m�quina de estados.
    public BaseState CurrentState
    {
        get { return currentState; }
    }

    //
    // List<BaseState> availableStates;

    public void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
    }

    // Los estados de la m�quina tienen dos Updates, uno para la l�gica de juego (UpdateLogic),
    // y otro para las f�sicas (UpdatePhysics), �sto es para ser congruentes con los
    // Update y FixedUpdate de Unity.
    public void Update()
    {
        // Si hay un estado actual (es decir, no es nulo)
        if (currentState != null)
            // Entonces, que actualice la l�gica de juego, inputs, y otras cosas necesarias del juego.
            currentState.UpdateLogic();
    }

    public void FixedUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState newState)
    {
        Debug.Log("Changing from state: " + currentState.name + " to: " + newState.name);
        // Primero, que el estado actual haga la limpieza que requiera.
        currentState.Exit();
        // Despu�s, asignamos el nuevo estado como el estado actual de la m�quina.
        currentState = newState;
        // Finalmente, que el nuevo estado haga las inicializaciones que requiera en su enter.
        currentState.Enter();
    }

    // Como las m�quinas de estados que vamos a usar deben heredar de esta clase y hacer un
    // override de esta funci�n para inicial en el estado que deseen.
    // OJO: esta funci�n es protected para que solo esta clase y sus hijas puedan accederla.
    protected virtual BaseState GetInitialState()
    {
        // por defecto regresa null.
        return null;
    }

    private void OnGUI()
    {
        string text = currentState != null ? currentState.name : "No current State asigned";
        GUILayout.Label($"<size=40>{text}</size>");
    }
}
