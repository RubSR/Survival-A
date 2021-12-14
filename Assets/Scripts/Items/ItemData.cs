using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoItem
{
    Recurso,
    Equipable,
    Consumible
}

[CreateAssetMenu(fileName = "Item", menuName = "Nuevo itemData del juego")]
public class ItemData : ScriptableObject
{
    [Header("Informacion")] public string nombreObjeto;
    public string descripcion;
    public TipoItem tipo;
    public Sprite icono;
    public GameObject prefab;

    [Header("Stacks")] 
    public bool puedeStackear;
    public int maxStack;

    [Header("Consumibles stats")] 
    public ItemDataConsumibles[] stats;

    [Header("Equipo")] 
    public GameObject equipPrefab;

}//Final de clase principal

public enum TipoConsumible
{
    Hambre,
    Sed,
    Vida,
    Sueño,
}
[System.Serializable]
public class ItemDataConsumibles
{
    public TipoConsumible tipo;
    public float valor;
}
