using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DuplicationButtons : MonoBehaviour, ISubmitHandler
{
    public Object objectFound;

    public void OnSubmit(BaseEventData eventData)
    {
        for (int i = 0; i < PlayerObjectsInventory.s_Singleton.objectsCompartments.Count; i++)
        {
            if (PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<ObjectCompartment>().MyCompartmentObject == null)
            {
                PlayerObjectsInventory.s_Singleton.numberOfObjectInInventory++;

                //Activation du component image + changement de son sprite du compartiment de sort dans lequel l'objet ramassé a été ajouté
                PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<ObjectCompartment>().MyCompartmentObject = objectFound;
                PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<Image>().enabled = true;
                PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<Image>().sprite = objectFound.MyObjectIcon;
                PlayerSpellsInventory.s_Singleton.UseTheSpellInTheSpellCompartment();
                return;
            }
        }
    }
}
