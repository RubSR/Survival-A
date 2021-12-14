using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NecesidadJugador : MonoBehaviour , IRecibeDaño
{
    public Necesidad barraVida;
    public Necesidad barraHambre;
    public Necesidad barraSed;
    public Necesidad barraDormir;

    public float sinComidaVidaQuePierdo;
    public float sinAguaVidaQuePierdo;

    public UnityEvent alRecibirDaño;

    private void Start()
    {
        barraDormir.valorActual = barraDormir.valorInicial;
        barraSed.valorActual = barraSed.valorInicial;
        barraHambre.valorActual = barraHambre.valorInicial;
        barraVida.valorActual = barraVida.valorInicial;
    }

    // Update is called once per frame
    void Update()
    {
        //Perdida de hambre y sed por tiempo
        barraHambre.Restar(barraHambre.valorPerdida * Time.deltaTime);
        barraSed.Restar(barraSed.valorPerdida * Time.deltaTime);
        barraDormir.Añadir(barraDormir.valorRegen * Time.deltaTime);
        
        //Si el hambre y/o la sed estan a cero, quitarnos vida
        if (barraHambre.valorActual == 0.0f)
        {
            barraVida.Restar(sinComidaVidaQuePierdo * Time.deltaTime);
        }

        if (barraSed.valorActual == 0.0f)
        {
            barraVida.Restar(sinAguaVidaQuePierdo * Time.deltaTime);
        }
        // La palmo
        if (barraVida.valorActual == 0.0f)
        {
            Morir();
        }
        
        
        
        //Actualizar la parte visual
        barraVida.barra.fillAmount = barraVida.Porcentaje();
        barraHambre.barra.fillAmount = barraHambre.Porcentaje();
        barraSed.barra.fillAmount = barraSed.Porcentaje();
        barraDormir.barra.fillAmount = barraDormir.Porcentaje();

    }//Fin del update de la clase principal
     //BLoque de acciones del jugador

     public void Curar(float cantidad)
     {
         barraVida.Añadir(cantidad);
     }

     public void Comer(float cantidad)
     {
         barraHambre.Añadir(cantidad);
     }

     public void Beber(float cantidad)
     {
         barraSed.Añadir(cantidad);
     }

     public void Dormir(float cantidad)
     {
         barraDormir.Restar(cantidad);
     }

     public void Morir()
     {
         Debug.Log("Estoy muerto");
     }
     
     //Recibir daño ->Interfaz IrecibeDaño
     public void RecibirDaño(int cantidadDaño)
     {
         barraVida.Restar(cantidadDaño);
         alRecibirDaño?.Invoke();
     }
     
    
    
}//Final de la clase principal


[System.Serializable]
public class Necesidad
{
    [HideInInspector]
    public float valorActual;
    public float valorMaximo;
    public float valorInicial;
    public float valorRegen;
    public float valorPerdida;
    public Image barra;
    
    //añadir
    public void Añadir(float cantidad)
    {
        valorActual = Mathf.Min(valorActual + cantidad, valorMaximo);
    }
    
    // Restar
    public void Restar(float cantidad)
    {
        valorActual = Mathf.Max(valorActual - cantidad, 0.0f);
    }
    
    // TRanformar los valores
    public float Porcentaje()
    {
        return valorActual / valorMaximo;
    }
}// Final clase Necesidad

// Interfaz
public interface IRecibeDaño
{
    void RecibirDaño(int cantidadDaño);
}




