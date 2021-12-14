using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    // Daño, ratio para el daño(cada cuanto los dañamos)
    // Una lista de tipo IRecibeDaño
    public int daño;
    public float ratioDaño;

    private List<IRecibeDaño> cosasADañar = new List<IRecibeDaño>();

    private void Start()
    {
        StartCoroutine(RepartirDaño());
    }


    IEnumerator RepartirDaño()
    {
        // Cada ratioDaño(tiempo) hacemos daño a los objetos
        // que esten en la lista

        while (true)
        {
            for (int i = 0; i < cosasADañar.Count; i++)
            {
                cosasADañar[i].RecibirDaño(daño);
            }
            yield return new WaitForSeconds(ratioDaño);
        }
    }
    
    //Detectar la colision y son de tipo IRecibeDaño, meterlos a la lista

    private void OnCollisionEnter(Collision other)
    {
        // Tenemos que comprobar si es de tipo IRecibeDaño
        if (other.gameObject.GetComponent<IRecibeDaño>() != null)
        {
            cosasADañar.Add(other.gameObject.GetComponent<IRecibeDaño>());
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<IRecibeDaño>() != null)
        {
            cosasADañar.Remove(other.gameObject.GetComponent<IRecibeDaño>());
        }
        
    }
}// Fin de la Clase principal
