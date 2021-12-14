using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipoManager : MonoBehaviour
{
    // Traernos el equipo actual
    public Equipo equipoActual;
    public Transform equipoParent;

    private JugadorController controller;
    
    // Singleton
    public static EquipoManager instancia;

    private void Awake()
    {
        instancia = this;
        controller = GetComponent<JugadorController>();
    }
    
    //Ataque normal
    public void AtacarInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && equipoActual != null &&
            controller.puedeMirar == true)
            {
                //equipoActual.Atacar();
            }
    }
    
    //Ataque alternativo
    public void AtacarAltInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && equipoActual != null &&
        controller.puedeMirar == true)
        {
            //equipoActual.AtacarAlt();
        }
    }

    public void Equipar(ItemData item)
    {
        Desequipar();
        equipoActual = Instantiate(item.equipPrefab, equipoParent).GetComponent<Equipo>();

    }

    public void Desequipar()
    {
        if (equipoActual != null)
        {
            Destroy(equipoActual.gameObject);
            equipoActual = null;
        }
        
    }
}
