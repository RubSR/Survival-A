using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Inventario : MonoBehaviour
{
    //Necesitamos un array para manejar
    // los slots de la interfaz ( parte visual)
    public ItemSlotUI[] uiSlots;
    // Array para manejar los items de cada slot
    public ItemSlot[] slots;
    //Menejar la ventana de inventario
    public GameObject ventanaInventario;
    // Transform para saber en que posicion hacer drop
    public Transform dropPosicion;

    [Header("Item Seleccionado")] 
    private ItemSlot itemSeleccionado;

    private int indiceSeleccionado;
    public TextMeshProUGUI itemNombre;
    public TextMeshProUGUI itemDescripcion;
    public TextMeshProUGUI itemStatNombre;
    public TextMeshProUGUI itemStatCantidad;
    public GameObject usarButton;
    public GameObject dropButton;
    public GameObject equiparButton;
    public GameObject desequiparButton;
    
    //Guardar quien de nuestros Objetos
    // esta equipado
    private int itemEquipadoIndex;
    
    //Componentes
    //Para manejar la aparicion del mouse
    private JugadorController controller;
    //Para utilizar el boton usar
    private NecesidadJugador necesidad;

    [Header("Eventos")] 
    public UnityEvent abrirInventario;

    public UnityEvent cerrarInventario;
    
    //Singleton
    //Public->quiero acceder desde fuera
    // statica-> no va cambiar
    // Nombre de la clase
    // instancia->nombre
    public static Inventario instancia;

    private void Awake()
    {
        //Inicializamos el singleton
        instancia = this;
        controller = GetComponent<JugadorController>();
        necesidad = GetComponent<NecesidadJugador>();
    }

    private void Start()
    {
        //Invetario y esconderlo
        ventanaInventario.SetActive(false);
        slots = new ItemSlot[uiSlots.Length];
        
        //Inicializar slots[]
        for (int x = 0; x < slots.Length; x++)
        {
            slots[x] = new ItemSlot();
            uiSlots[x].indice = x;
            uiSlots[x].Clear();
        }
        LimpiarInfoItemSeleccionado();
    }
    
    
    //Abrir/cerrar inventario
    public void ActivarDesactivarInventario()
    {
        if (ventanaInventario.activeInHierarchy)
        {
            ventanaInventario.SetActive(false);
            abrirInventario.Invoke();
            controller.ActivarCursor(false);
        }
        else
        {
            ventanaInventario.SetActive(true);
            abrirInventario.Invoke();
            LimpiarInfoItemSeleccionado();
            controller.ActivarCursor(true);
        }
    }
    
    //Metodo que devuelve true o false en base a si esta activado o no el inventario
    public bool EstaAbierto()
    {
        return ventanaInventario.activeInHierarchy;
    }
    
    //Añadir Item
    public void AddItem(ItemData item)
    {
        //Comprobar si puede hacer stacks
        if (item.puedeStackear)
        {
            ItemSlot itemAStackear = DameElStack(item);
            if (itemAStackear != null)
            {
                itemAStackear.cantidad++;
                UpdateInterfazInventario();
                return;
            }
        }
        //Buscar slot vacio
        ItemSlot slotVacio = GetSlotVacio();
        if (slotVacio != null)
        {
            slotVacio.item = item;
            slotVacio.cantidad = 1;
            UpdateInterfazInventario();
            return;
        }
        
        //Si no puede stackear y no hay sitio lo tiro
        TirarItem(item);


    }//Fin del metodo add
    
    //Actualizar la interfaz
    private void UpdateInterfazInventario()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                uiSlots[i].Set(slots[i]);   
            }
            else
            {
                uiSlots[i].Clear();
            }

        }
    }

    private ItemSlot DameElStack(ItemData item)
    {
        //Recorre nuestro aray de slots para ver
        // si hay un slot donde puede stackear
        // y si lo hay lo devuelvo
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].cantidad < item.maxStack)
            {
                return slots[i];
            }
        }

        return null;
    }

    //Si hay hueco nos devuelve un slot vacio
    // Si no hay devuelve null
    private ItemSlot GetSlotVacio()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }

        return null;

    }

    private void TirarItem(ItemData item)
    {
        //Instanciando su dropPrefab
        Instantiate(item.prefab, dropPosicion.position,
                Quaternion.Euler(Vector3.one * Random.value * 360.0f));

    }
    
    //Seleccionar item-> cuando hagamos click en un slot
    public void SeleccionarItem(int indice)
    {
        if (slots[indice].item == null)
        {
            return ;
        }

        itemSeleccionado = slots[indice];
        indiceSeleccionado = indice;
        // Vamos a pintar la info en la parte derecha
        itemNombre.text = itemSeleccionado.item.nombreObjeto;
        itemDescripcion.text = itemSeleccionado.item.descripcion;
        
        //Pintamos los stats ( si tienen)
        itemStatNombre.text = string.Empty;
        itemStatCantidad.text = string.Empty;

        for (int i = 0; i < itemSeleccionado.item.stats.Length; i++)
        {
            itemStatNombre.text += 
                itemSeleccionado.item.stats[i].tipo.ToString() + "\n";

            itemStatCantidad.text +=
                itemSeleccionado.item.stats[i].valor.ToString() + "\n";
        }
        
        //Mostrar los botones adecuados.
        dropButton.SetActive(true);
        usarButton.SetActive(itemSeleccionado.item.tipo == TipoItem.Consumible);
        equiparButton.SetActive(itemSeleccionado.item.tipo == TipoItem.Equipable
                && !uiSlots[indice].equipado);
        desequiparButton.SetActive(itemSeleccionado.item.tipo == TipoItem.Equipable
                                && uiSlots[indice].equipado);

    }
    
    //Limpie el objeto seleccionado
    private void LimpiarInfoItemSeleccionado()
    {
        itemSeleccionado = null;
        itemNombre.text = string.Empty;
        itemDescripcion.text = string.Empty;
        itemStatCantidad.text = string.Empty;
        itemStatNombre.text = string.Empty;
         
        usarButton.SetActive(false);
        dropButton.SetActive(false);
        equiparButton.SetActive(false);
        desequiparButton.SetActive(false);

    }
    
    //Metodo de captura de InputSystem para abrir/cerrar Inventario

    public void TabInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ActivarDesactivarInventario();
        }
    }


    // LLama a la funcion tirarItem->Instanciaba el prefab en la escena
    // Borrar el item del inventario
    public void DropButton()
    {
        TirarItem(itemSeleccionado.item);
        BorrarItemSeleccionado();
    }

    private void BorrarItemSeleccionado()
    {
        itemSeleccionado.cantidad--;

        if (itemSeleccionado.cantidad == 0)
        {
            if (uiSlots[indiceSeleccionado].equipado == true)
            {
                Desequipar(indiceSeleccionado);
            }

            itemSeleccionado.item = null;
            LimpiarInfoItemSeleccionado();
        }
        
        UpdateInterfazInventario();
    }

    private void Desequipar(int indice)
    {
        //Poner la propiedad equipado del item a false
        uiSlots[indice].equipado = false;
        EquipoManager.instancia.Desequipar();
        UpdateInterfazInventario();

        if (indiceSeleccionado == indice)
        {
            SeleccionarItem(indice);
        }

    }
        
    //Boton Usar
    public void BotonUsar()
    {
        //Comprobar que se de tipo consumible
        if (itemSeleccionado.item.tipo == TipoItem.Consumible)
        {
            for (int i = 0; i < itemSeleccionado.item.stats.Length; i++)
            {
                switch (itemSeleccionado.item.stats[i].tipo)
                {
                    case TipoConsumible.Vida:
                        necesidad.Curar(itemSeleccionado.item.stats[i].valor);
                        break;
                    case TipoConsumible.Hambre:
                        necesidad.Comer(itemSeleccionado.item.stats[i].valor);
                        break;
                    case TipoConsumible.Sed:
                        necesidad.Beber(itemSeleccionado.item.stats[i].valor);
                        break;
                    case TipoConsumible.Sueño:
                        necesidad.Dormir(itemSeleccionado.item.stats[i].valor);
                        break;
                    
                }
                
            }
        }
        BorrarItemSeleccionado();
    }

    public void BotonEquipar()
    {
        if (uiSlots[itemEquipadoIndex].equipado)
        {
            Desequipar(itemEquipadoIndex);
        }

        uiSlots[indiceSeleccionado].equipado = true;
        itemEquipadoIndex = indiceSeleccionado;
        EquipoManager.instancia.Equipar(itemSeleccionado.item);
        UpdateInterfazInventario();
        SeleccionarItem(indiceSeleccionado);
    }

    public void BotonDesequipar()
    {
        Desequipar(itemEquipadoIndex);
    }
    
    
    
        
}//Fin  de la clase principal

//Guarda la informacion sobre un objeto que
// este en uno de nuestros slots del inventario
// y la cantidad
public class ItemSlot
{
    public ItemData item;
    public int cantidad;
}

