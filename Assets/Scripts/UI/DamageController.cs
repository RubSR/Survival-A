using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageController : MonoBehaviour
{
    // El componente Image del indicador
    public Image imagen;
    public float velocidad;

    private Coroutine fadeAway;

    

    //Funciona que se quede eschando al evento
    public void Flash()
    {
        //Si hay uno fadeAway ejecutandose, lo paro 
        if (fadeAway != null)
        {
            StopCoroutine(fadeAway);
        }
        //Resetear la imagen
        imagen.enabled = true;
        imagen.color = Color.white; // El alfa lo pone al maximo
        fadeAway = StartCoroutine(Pepe());
            
        
    }
    
    //Hacemos un fadeAway pot velocidad
    IEnumerator Pepe()
    {
        float alfa = 1.0f;

        while (alfa > 0.0f)
        {
            // restar por frame una cantidad al alfa
            // en base a mi velocidad 
            alfa -= (1.0f / velocidad) * Time.deltaTime;
            imagen.color = new Color(1.0f, 1.0f, 1.0f, alfa);
            yield return null;
        }

        imagen.enabled = false;
    }


}
