using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JugadorController : MonoBehaviour
{
    //Mirar
    [Header("Mirar")]
    public Transform contenedorCamara;

    public float maximoMirarAbajo;

    public float maximoMirarArriba;

    private float rotacionActualCamara;

    public float sensibilidad;

    private Vector2 deltaRaton;

    [Header("Movimiento")] 
    public float velocidadMovimiento;

    private Vector2 inputActualMovimiento;

    private Rigidbody rigidbody;

    public float fuerzaSalto;
    public LayerMask capasSuelo;

    [HideInInspector] public bool puedeMirar = true;

    public void ActivarCursor(bool activar)
    {
        Cursor.lockState = activar ? CursorLockMode.None : CursorLockMode.Locked;
        puedeMirar = !activar;
    }
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(puedeMirar == true)
            GirarVista();
    }

    private void FixedUpdate()
    {
        Mover();
    }

    // Una funcion publica a la que nuestro InputSystem pueda acceder
    // para enviarnos el valor del movimiento del raton (Vector2)

    public void CaputarMovimientoRaton(InputAction.CallbackContext contexto)
    {
        deltaRaton = contexto.ReadValue<Vector2>();
    }

    public void CapturarTeclasMovimiento(InputAction.CallbackContext contexto)
    {
        // Se esta apretando algun boton de movimiento?
        if (contexto.phase == InputActionPhase.Performed)
        {
            inputActualMovimiento = contexto.ReadValue<Vector2>();
        }else if (contexto.phase == InputActionPhase.Canceled)
        {
            inputActualMovimiento = Vector2.zero;
        }
    }
    //Se llamará cuando se presione la tecla espcio
    public void CapturarTeclaSalto(InputAction.CallbackContext contexto)
    {
        //Tenemos que comprobar que se acaba de presionar la tecla 
        if (contexto.phase == InputActionPhase.Started)
        {
            //Tenemos que comprobar si esta en el suelo
            if (TocaTierra())
            {
                //Saltamos
                rigidbody.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            }
        }
    }

    
    private void GirarVista()
    {
        //Girar la camara arriba y abajo en base al eje y de deltaRaton
        rotacionActualCamara += deltaRaton.y * sensibilidad;
        rotacionActualCamara = Mathf.Clamp(rotacionActualCamara, maximoMirarAbajo, maximoMirarArriba);
        contenedorCamara.localEulerAngles = new Vector3(-rotacionActualCamara, 0, 0);
        //Girar la camara de lado
        transform.eulerAngles += new Vector3(0, deltaRaton.x * sensibilidad, 0);

    }
    
    //Funcion mover
    private void Mover()
    {
        // Calcular el movimiento  realtivo a donde estemos mirando
        Vector3 dir = transform.forward * inputActualMovimiento.y
                      + transform.right * inputActualMovimiento.x;
        dir *= velocidadMovimiento;
        //Controlar la Y par que se puede saltar
        dir.y = rigidbody.velocity.y;
        rigidbody.velocity = dir;

    }

    private bool TocaTierra()
    {
        //4 rayos hacia abajo
        Ray[] rayos = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f)
                + (Vector3.up * 0.01f),
                Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f)
                                       + (Vector3.up * 0.01f),
                Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f)
                                       + (Vector3.up * 0.01f),
                Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f)
                                       + (Vector3.up * 0.01f),
                Vector3.down)
        };
        //Disparar cada rayo una distancia predeterminada ( muy pequeña)
        // y si existe colision con algo que este dentro de nuestras capasSuelo
        // ->>>>true. Caso contrario -->> false
        for (int i = 0; i < rayos.Length; i++)
        {
            // Tirame el rayo  rayo[i]  a una distancia 0.1f, y si choca
            // contra algo que tenga asociado uno de los layer
            // que esten en nuestra lista de layers capasSuelo
            // --> true, de lo contrario-->false
            if (Physics.Raycast(rayos[i], 0.1f, capasSuelo))
            {
                return true;
            }
        }

        return false;
    }
    
   

}// Cierre de la clase
