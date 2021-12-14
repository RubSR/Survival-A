using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour , IInteractuable
{
   public ItemData item;
   public string GetMensaje()
   {
      return string.Format("Recoger {0}", item.nombreObjeto);
   }

   public void Interactuar()
   {
      Inventario.instancia.AddItem(item);
      Destroy(gameObject);
   }
}
