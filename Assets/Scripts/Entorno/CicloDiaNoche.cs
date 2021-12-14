using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    //Tiempo entre 0.0 y 1.0 e indicará la hora del dia
    [Range(0.0f, 1.0f)] 
    public float tiempo;
    // Duracion en tiempo real y en segundos de un dia completo
    public float duracionDiaCompleto;
    public float horaComienzo = 0.4f;
    // Variable que indica cuanto tenemos que sumar a tiempo por cada frame
    // para que el dia dure en tiempo real->duracionDiaCompleto
    public float ratioTiempo;
    //Vector que marca la posicion del mediodia
    public Vector3 mediodia;

    [Header("Sol")] 
    public Light sol;
    //Variar el color de la luz del sol segun la hora del dia
    public Gradient colorSol;
    //Variar la intensidad del sol segun la hora del dia
    public AnimationCurve intensidadSol;
 
    [Header("Luna")] 
    public Light luna;
    //Variar el color de la luz de la luna segun la hora del dia
    public Gradient colorLuna;
    //Variar la intensidad de la luna segun la hora del dia
    public AnimationCurve intensidadLuna;

    [Header("Otras iluminaciones")] 
    public AnimationCurve multiGlobalIntensidad;

    public AnimationCurve multiGlobalReflejos;

    private void Start()
    {
        //Calcular nuestro tiempoRate
        ratioTiempo = 1.0f / duracionDiaCompleto;
        tiempo = horaComienzo;
    }

    private void Update()
    {
        //Incrementar el tiempo
        tiempo += ratioTiempo * Time.deltaTime;
        
        //Cotrolar que no pase de 1
        if (tiempo >= 1.0f)
        {
            //Reiniciar el dia
            tiempo = 0.0f;
        }
        
        //Rotar el sol y la luna
        sol.transform.eulerAngles = (tiempo - 0.25f) * mediodia * 4.0f;
        luna.transform.eulerAngles = (tiempo - 0.75f) * mediodia * 4.0f;
        //Establecer sus intensidades
        sol.intensity = intensidadSol.Evaluate(tiempo);
        luna.intensity = intensidadLuna.Evaluate(tiempo);
        //Establecer el color
        sol.color = colorSol.Evaluate(tiempo);
        luna.color = colorLuna.Evaluate(tiempo);
        //Activar/desactivar sol
        if (sol.intensity == 0 && sol.gameObject.activeInHierarchy )
        {
            sol.gameObject.SetActive(false);
        }else if (sol.intensity > 0 && !sol.gameObject.activeInHierarchy )
        {
            sol.gameObject.SetActive(true);
        }
        //Activar/desactivar luna
        if (luna.intensity == 0 && luna.gameObject.activeInHierarchy )
        {
            luna.gameObject.SetActive(false);
        }else if (luna.intensity > 0 && !luna.gameObject.activeInHierarchy )
        {
            luna.gameObject.SetActive(true);
        }
        
        //Cambiar los reflejos e instesida global
        RenderSettings.ambientIntensity = multiGlobalIntensidad.Evaluate(tiempo);
        RenderSettings.reflectionIntensity = multiGlobalReflejos.Evaluate(tiempo);
    }
}//Fin de la clase principal
