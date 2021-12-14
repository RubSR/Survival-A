using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herramienta : Equipo
{
    public float ratioAtaque;
    private bool atacando;
    public float distanciaAtaque;

    [Header("Recoleccion")] 
    public bool puedeRecolectar;

    [Header("Combate")] 
    public bool puedeCombatir;

    public int damage;
    
    //Componentes
    private Animator anim;
    private Camera cam;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }

    public override void Atacar()
    {
        
    }
}
