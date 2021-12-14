using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    //Icono del objeto
    public Image icono;

    public TextMeshProUGUI cantidadTexto;
    private ItemSlot slotActual;
    private Outline bordecito;

    public int indice;
    public bool equipado;
    
    //Traernos el Outline
    private void Awake()
    {
        bordecito = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        bordecito.enabled = equipado;
    }
    
    //Metodo para setear el itemSlot en su UI, cuadriotp de inventario
    public void Set(ItemSlot slot)
    {
        slotActual = slot;
        //Establecer texto cantidad, icono ....
        icono.gameObject.SetActive(true);
        icono.sprite = slot.item.icono;
        cantidadTexto.text = slot.cantidad > 1 ? slot.cantidad.ToString() : String.Empty;
        if (bordecito != null)
        {
            bordecito.enabled = equipado;
        }
    }
    
    //Lo contrario, borrar el contenido del slotUI
    public void Clear()
    {
        slotActual = null;
        icono.gameObject.SetActive(false);
        cantidadTexto.text = string.Empty;
    }
    
    //Metodo par el boton
    public void PulsarSlot()
    {
        Inventario.instancia.SeleccionarItem(indice);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
