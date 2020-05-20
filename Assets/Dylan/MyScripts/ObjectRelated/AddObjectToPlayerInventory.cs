using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddObjectToPlayerInventory : MonoBehaviour
{
    public Object objectPickedup;
    public bool canBePickuped = false;
    private Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canBePickuped && GameManager.s_Singleton.gameState == GameState.PlayMode && !MapHandler.s_Singleton.mapIsDisplayed && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_X") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_A")))
        {
            AddTheObjectToTheInventory(objectPickedup);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Can pick the object");
            //transform.GetChild(0).gameObject.SetActive(true);
            canBePickuped = true;
            Player.s_Singleton.canPickObject = true;
            myAnimator.SetBool("PlayerIsInTrigger", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Cannot pick the object");
            //transform.GetChild(0).gameObject.SetActive(false);
            canBePickuped = false;
            Player.s_Singleton.canPickObject = false;
            myAnimator.SetBool("PlayerIsInTrigger", false);
        }
    }

    //Fonction permettant de ramasser un objet et de le stocker dans l'inventaire d'objets du joueur
    public void AddTheObjectToTheInventory(Object objToAddToInventory)
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
                //Si un des compartiments de sort est vide et le joueur n'a pas encore atteint la limite d'objet ramassable...
                if (PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<ObjectCompartment>().MyCompartmentObject == null)
                {
                    for (int x = 0; x < ObjectDataBase.s_Singleton.objectsInTheGame.Count; x++)
                    {
                        if (ObjectDataBase.s_Singleton.objectsInTheGame[x] == objectPickedup)
                        {
                            PlayerObjectsInventory.s_Singleton.numberOfObjectInInventory++;

                            //Activation du component image + changement de son sprite du compartiment de sort dans lequel l'objet ramassé a été ajouté
                            PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<ObjectCompartment>().MyCompartmentObject = ObjectDataBase.s_Singleton.objectsInTheGame[x];
                            PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<Image>().enabled = true;
                            PlayerObjectsInventory.s_Singleton.objectsCompartments[i].GetComponent<Image>().sprite = ObjectDataBase.s_Singleton.objectsInTheGame[x].MyObjectIcon;
                            Player.s_Singleton.canPickObject = false;
                            Destroy(this.gameObject);
                            return;
                        }
                    }
                }
            }
        }
    }
}
