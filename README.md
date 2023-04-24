# IA para videojuegos
## 2021.3.16f1 LTS
## 
## Tarea 1P1: Wander
#### Consiste en mantener su direccion, pero realizando cambios menores cada frame. Asi la fuerza de giro toma cambios aleatorios menores y reales, no se ve brusco el movimiento.
## 
## Examen 1 
## Sigilo 
#### Agente que se mueve al dar click en pantalla, y guardia que cuenta con 3 estados, si lo detecta  por un determinado tiempo lo persigue por 5 segundos o si lo alcanza lo destruye.
#### ~ Click Izquierdo para mover al infiltrador.
#### ~ Espacio para reaparecer.  
## Obstaculos
#### Agente que se mueve al dar click en pantalla, si en su camino encuentra obstaculos, los rodeara. Los obstaculos se generan aleatoriamente en la pantalla de juego
#### ~ Click Izquierdo para mover al agente.
## Patrullage
#### Agente que se movera entre n puntos en escena, de manera ciclica. Se pueden agregar nuevos puntos o eliminar. 
#### ~ Click derecho para agregar punto
#### ~ Click izquierdo sobre el punto que se desea eliminar, para eliminarlo.
##
##
### Cada comportamiento se encuentra en su escena individual, todas las escenas para este examen estan en  la carpeta examen 1, con sus respectivos nombres. Tambien los scripts estan ordenados de manera similar,  en la carpeta scripts/examen y ahi estan las carpetas de scripts con su respectivo nombre.
##
## Tarea 1P2 : BreadthFirstSearch 
### Para probar el comportamiento se tiene que abrir la escena "Pathfinding" que esta en: Assets/Scenes/Parcial2/Pathfinding. Desde el objeto grid que esta en escena se puede modificar los puntos de inicio y finales, aunque por defecto deberian de estar en los requeridos por la tarea. 
### Los scripts necesarios para esta tarea se encuentran en: Assets/Scripts/Parcial2/Tarea1/. 
##
## Examen 2 
#### Se genera una grid, por el cual usaremos algoritmo de Pathfinding y un agente AStar para recorrer de nodo de inicio a final con la mejor ruta, ademas de que se mostrara informacion visual como la ruta, la lista abierta, la lista cerrada y sus costos.
#### ~ Click Derecho sobre el agente para seleccionarlo.
#### ~ Click Izquierdo sobre cualquier nodo para seleccionarlo como nodo inicial.
#### ~ Click Derecho sobre cualquier nodo para seleccionarlo como nodo final.
#### ~ Espacio para iniciar el pathfinding, ya que se haya seleccionado a los nodos de inicio y final.
#### ~ El boton de arriba a la derecha reiniciara, para poder probar de nuevo el algoritmo.
#### ~ El agente solo se movera al estar seleccionado. 
### La escena dedicada al examen esta en Assets/Scenes/Parcial2/ y se llama Examen_2do_Parcial
### Equipo con : Erick Aaron Hernandez Ibarra
##
## Examen 3 
#### Se agrego el StateMachine en el ejercicio del guardia e infiltrador, se aplico la logica a 3D y se agregaron tanto animaciones como ambientacion.
#### ~ Click Izquierdo sobre el suelo para que el personaje camine hacia el lugar.
#### ~ El Guardia cuenta con 3 estados: Patrol, Alert y Atack. Indicados por el color verde, amarillo y rojo, respectivamente en la luz del guardia. 
### La escena dedicada al examen esta en Assets/Scenes/Parcial3/ y se llama Examen3.
### Equipo con : Erick Aaron Hernandez Ibarra y Javier Alfredo Gonzales Rubio

