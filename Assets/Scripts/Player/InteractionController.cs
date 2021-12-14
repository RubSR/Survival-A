using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    //ratio de disparo de los raycast(Cada cuanto tiempo lanzo uno)
    public float ratioCheck = 0.05f;
    //saber el momento en el que se lanzo el ultimo raycast
    private float ultimoCheck;
    // Distancia maximo del raycast
    public float maxDistancia;
    //Layer
    public LayerMask layer;

    private GameObject objetoInteractuableActual;
    private IInteractuable interActual;

    public TextMeshProUGUI mensajeUWU;

    private void Update()
    {
        //Si ha pasado mas de 0.05f desde el ultimo check
        if (Time.time - ultimoCheck > ratioCheck)
        {
            ultimoCheck = Time.time;
            //Me creo un rayo, desde el centro de la pantalla
            // y en direccion a mi frente
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(
            Screen.width/2, Screen.height/2, 0));

            RaycastHit hit;
            
            //hemos chocado con algo que pertenezca a nuestro layer?
            if (Physics.Raycast(ray, out hit, maxDistancia, layer))
            {
                //No es el mismo objeto de antes?
                if (hit.collider.gameObject != objetoInteractuableActual)
                {
                    objetoInteractuableActual = hit.collider.gameObject;
                    interActual = hit.collider.GetComponent<IInteractuable>();
                    MostratMensaje();
                }
            }
            else
            {
                objetoInteractuableActual = null;
                interActual = null;
                mensajeUWU.gameObject.SetActive(false);

            }
        }
    }//Fin update

    void MostratMensaje()
    {
        //Activamos el texto mensaje
        mensajeUWU.gameObject.SetActive(true);
        //seteamos el texto que tiene que mostrar
        mensajeUWU.text = string.Format("<b>[E]</b> {0}", interActual.GetMensaje());
    }
    
    //SE llama cuando apretamos la E - Manejado por Input System
    public void PresionarBotonE(InputAction.CallbackContext context)
    {
        //Se acaba de presionar y existe un objetoActualInteractuable

        if (context.phase == InputActionPhase.Started && interActual != null)
        {
            interActual.Interactuar();
            objetoInteractuableActual = null;
            interActual = null;
            mensajeUWU.gameObject.SetActive(false);
        }
    }
    
    
    
    
    
    
} // Fin de la clase principal

public interface IInteractuable
{
    //Haremos que al apuntar al objeto dentro del mundo
    // aparezca un mensaje tipo : [E] para recoger piedra
    string GetMensaje();
    // Implementaremos la logica de interactuar
    void Interactuar();
}
