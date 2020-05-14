using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddObjectToPlayerInventory : MonoBehaviour
{
    public Object _object;
    public bool canBePickup = false;

    private void Start()
    {
        _object = this.GetComponent<Object>();
    }

    private void Update()
    {
        if (canBePickup && GameManager.s_Singleton.gameState == GameState.PlayMode && !MapHandler.s_Singleton.mapIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_X") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_A")))
        {
            AddTheObjectToTheInventory();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Can pick the object");
            transform.GetChild(0).gameObject.SetActive(true);
            canBePickup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Cannot pick the object");
            transform.GetChild(0).gameObject.SetActive(false);
            canBePickup = false;
        }
    }

    //Fonction attachée au clique sur "Oui" de la fenêtre popup de validation d'achat...
    public void AddTheObjectToTheInventory()
    {
        if (PlayerObjectsInventory.s_Singleton.numberOfObjectInInventory == 3)
        {
            StartCoroutine(UIManager.s_Singleton.FadeInAndOutObjectFeedBack(UIManager.s_Singleton.cantPickAnObjectFeedback));
            return;
        }
        else
        {
            for (int i = 0; i < PlayerObjectsInventory.s_Singleton.objectsCompartments.Count; i++)
            {
                //Si un des compartiments de sort est vide et le joueur n'a pas encore atteint la limite de sort achetable...
                if (PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<ObjectCompartment>().MyCompartmentObject == null)
                {
                    PlayerObjectsInventory.s_Singleton.numberOfObjectInInventory++;

                    //Activation du component image + changement de son sprite du compartiment de sort dans lequel le sort acheté a été ajouté
                    PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<ObjectCompartment>().MyCompartmentObject = _object;
                    PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<Image>().enabled = true;
                    PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<Image>().sprite = _object.MyObjectIcon;
                    Destroy(gameObject);

                    return;
                }
            }
        }
    }
}
