using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Spell_", order = 1)]

public class Spell : ScriptableObject
{
    //Variables des sorts contenus dans le jeu
    public enum SpellType { Etourdissement, Crochetage, Radar, Duplication, Clone, TransformationEnChat, TransformationEnSouris };
    public SpellType spellType;
   
    [SerializeField] private string spellName;
    [SerializeField] private Sprite spellIcon;
    [SerializeField] private float spellDurationOfEffect;
    [SerializeField] private int spellValue;
    [TextArea(1, 5)]
    [SerializeField] private string spellEffectDescription;


    public string MySpellName { get => spellName; }
    public float MySpellDurationOfEffect { get => spellDurationOfEffect; }
    public int MySpellValue { get => spellValue; }
    public Sprite MySpellIcon { get => spellIcon; set => spellIcon = value; }
    public string MySpellEffectDescription { get => spellEffectDescription; set => spellEffectDescription = value; }

    public void UseTheSpell()
    {
        switch (spellType)
        {
            case SpellType.Etourdissement:
                if (Player.s_Singleton.isAiming ==false)
                {
                    ActivateAimMode();
                }
                if (Player.s_Singleton.isAiming && Player.s_Singleton.Target != null)
                {
                    Stun(Player.s_Singleton.Target);
                }
                break;
            case SpellType.Crochetage:
                LockPicking();
                break;
            case SpellType.Radar:
                Detection();
                break;
            case SpellType.Duplication:
                Duplication();
                break;
            case SpellType.Clone:
                PreviewClonage(Player.s_Singleton.posToInstantiateTheClone);
                break;
            case SpellType.TransformationEnSouris:
                if (Player.s_Singleton.playerIsInHumanForm)
                {
                    MouseTransformation(Player.s_Singleton.defaultCharacterModel, Player.s_Singleton.mouseCharacterModel);
                }
                else if (Player.s_Singleton.playerIsTranformedInCat)
                {
                    MouseTransformation(Player.s_Singleton.catCharacterModel, Player.s_Singleton.mouseCharacterModel);
                }
                break;
            case SpellType.TransformationEnChat:
                if (Player.s_Singleton.playerIsInHumanForm)
                {
                    CatTransformation(Player.s_Singleton.defaultCharacterModel, Player.s_Singleton.catCharacterModel);
                }
                else if (Player.s_Singleton.playerIsTranformedInMouse)
                {
                    CatTransformation(Player.s_Singleton.mouseCharacterModel, Player.s_Singleton.catCharacterModel);
                }
                break;
            default:
                break;
        }
    }

    #region Transformations
    private void CatTransformation(GameObject objToDisactive, GameObject objToActive)
    {
        Debug.Log("Trying to transform the player character in a cat...");

        objToDisactive.SetActive(false);
        objToActive.SetActive(true);

        Player.s_Singleton.playerIsTranformedInCat = true;
        Player.s_Singleton.playerIsTranformedInMouse = false;
        Player.s_Singleton.playerIsInHumanForm = false;

        PlayerSpellsInventory.s_Singleton.UseTheSpellInTheSpellCompartment();
    }

    private void MouseTransformation(GameObject objToDisactive, GameObject objToActive)
    {
        Debug.Log("Trying to transform the player character in a mouse...");

        objToDisactive.SetActive(false);
        objToActive.SetActive(true);

        Player.s_Singleton.playerIsTranformedInMouse = true;
        Player.s_Singleton.playerIsTranformedInCat = false;
        Player.s_Singleton.playerIsInHumanForm = false;

        PlayerSpellsInventory.s_Singleton.UseTheSpellInTheSpellCompartment();
    }
    #endregion

    #region Clonage
    private void PreviewClonage(Transform posToSpawnClone)
    {
        if (!Player.s_Singleton.playerIsInHumanForm)
        {
            Debug.Log("Impossible de cloner");
            PlayerSpellsInventory.s_Singleton.CantUseASpell();
            return;
        }
        else
        {
            //Previsualisation
            Player.s_Singleton.isTryingToClone = true;
            posToSpawnClone.gameObject.SetActive(true);
            //PlayerSpellsInventory.s_Singleton.DeactivateSpellActivationFeedback();
        }
    }

    public void Clonage(GameObject objToClone, Transform posToSpawnClone)
    {
        GameObject objToInstantiate = Instantiate(objToClone, posToSpawnClone.position, Quaternion.identity) as GameObject;
        objToInstantiate.transform.rotation = posToSpawnClone.rotation;

        PlayerSpellsInventory.s_Singleton.UseTheSpellInTheSpellCompartment();
    }
    #endregion

    #region Duplication
    private void Duplication()
    {
        //Refs
        UIManager uiManagerReference = UIManager.s_Singleton;
        PlayerObjectsInventory playerObjInventoryRef = PlayerObjectsInventory.s_Singleton;

        Debug.Log("Dupplication");
        if (!Player.s_Singleton.playerIsInHumanForm)
        {
            Debug.Log("Impossible de cloner");
            PlayerSpellsInventory.s_Singleton.CantUseASpell();
            return;
        }
        else
        {
            for (int i = 0; i < playerObjInventoryRef.objectsCompartments.Count; i++)
            {
                if (playerObjInventoryRef.objectsCompartments[i].MyCompartmentObject != null)
                {
                    //Open the Window
                    uiManagerReference.DisplayDuplicationPopup();
                    
                    //Attribution de l'objet trouvé 
                    uiManagerReference.duplicationButtonLayout.transform.GetChild(i).GetComponent<DuplicationButtons>().objectFound = playerObjInventoryRef.objectsCompartments[i].MyCompartmentObject;

                    uiManagerReference.duplicationButtonLayout.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = uiManagerReference.duplicationButtonLayout.transform.GetChild(i).GetComponent<DuplicationButtons>().objectFound.MyObjectIcon;

                    //EventSystem.current.SetSelectedGameObject(uiManagerReference.duplicationButtonLayout.transform.GetChild(0).gameObject);
                }
                else if (playerObjInventoryRef.numberOfObjectInInventory == 0 || playerObjInventoryRef.numberOfObjectInInventory  == 3)
                {
                    Debug.Log("No object found, duplication is impossible");
                    PlayerSpellsInventory.s_Singleton.CantUseASpell();
                    PlayerSpellsInventory.s_Singleton.DeactivateSpellActivationFeedback();
                }
            }
        }
    }
    #endregion

    #region Stun
    private void ActivateAimMode()
    {
        Debug.Log("Input a mettre et stun a lancer selon les input et normalement c'est bon");
        Player player = Player.s_Singleton;
        player.FieldOfView.SetActive(true);
        player.isAiming = true;
    }

    private void DeactivateAimMode()
    {
        Player player = Player.s_Singleton;
        player.FieldOfView.SetActive(false);
        player.isAiming = false;
    }


    public void Stun(Transform target)
    {
        target.GetComponent<NavMeshAgent>().enabled = false;
        target.GetComponent<EnnemyView>().Stun(5);
        DeactivateAimMode();
        PlayerSpellsInventory.s_Singleton.UseTheSpellInTheSpellCompartment();

    }
    #endregion

    #region LockPicking
    private void LockPicking()
    {
        /*   Joueur près d'une porte fermée ?   */
        if (Player.s_Singleton.playerIsInHumanForm && Player.s_Singleton.doorNearPlayerCharacter != null && Player.s_Singleton.doorNearPlayerCharacter.doorType == DoorType.CommonDoor)
        {
            Player.s_Singleton.doorNearPlayerCharacter.UnlockDoor();
            PlayerSpellsInventory.s_Singleton.UseTheSpellInTheSpellCompartment();
        }
        else
        {
            PlayerSpellsInventory.s_Singleton.CantUseASpell();
            PlayerSpellsInventory.s_Singleton.DeactivateSpellActivationFeedback();
        }
        //Oui

        //Non
    }
    #endregion

    #region Detection
    private void Detection()
    {
        Player player = Player.s_Singleton;

        //Activer le gameObject contenant le trigger permettant de détecter les objets
        player.detectionRadar.SetActive(true);
        PlayerSpellsInventory.s_Singleton.UseTheSpellInTheSpellCompartment();

        // ! -- Done on enable sur le script "ObjectDetection" -- !
        // - Le faire grossir autour du joueur
        // - Ajouter tous les objets trouvés dans une liste
        // - Activer le script outline de tous ces objets trouvés pendant x secondes

        // - Désactiver l'outline de tous ces objets à la fin du timer
        // - Clear la liste au cas où ? 


        // ! -- Pas encore fait -- !
        // - Les afficher sur la carte
    }
    #endregion

}
