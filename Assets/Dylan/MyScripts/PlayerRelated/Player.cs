using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnimator;
    [HideInInspector] public float _durationOfEffectSinceLaunched = 0;
    //En public pour debug
    public float spellDurationOfEffect;

    [Header("PLAYER MODELS")]
    public GameObject defaultCharacterModel;
    public GameObject defaultCharacterModelClone;
    public GameObject catCharacterModel;
    public GameObject mouseCharacterModel;

    [Header("CLONE PARAMETERS")]
    public Transform posToInstantiateTheClone;

    [Header("TRANSFORMATIONS STATES")]
    public bool playerIsTranformedInMouse = false;
    public bool playerIsTranformedInCat = false;
    public bool playerIsInHumanForm = true;

    [Header("PLAYER STATES")]
    public bool onWall;
    public bool inWardrobe;
    public bool canPickObject;
    public bool isDead;
    //public bool LookAtMap;
    public bool isUsingASpell = false;
    public bool isTryingToClone = false;
    public bool isAiming = false;
    public bool hasATarget = false;

    public OppeningDoor doorNearPlayerCharacter;
    //[HideInInspector]
    public List<Transform> playerTargets = new List<Transform>();
    public Transform actualPlayerTarget;
    public GameObject detectionRadar;
    public List<Transform> objectsFound = new List<Transform>();

    public static Player s_Singleton;

    #region Singleton
    private void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            s_Singleton = this;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            #region Square/X
            if (isUsingASpell && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X")))
            {
                Debug.Log("Player Script");
                if(PlayerSpellsInventory.s_Singleton.spellsCompartments[0].MyCompartmentSpell.spellType == Spell.SpellType.Etourdissement && isAiming)
                {
                    StunPlayerTarget();
                    Debug.Log("Stun");
                }
            }
            #endregion

            #region Transformation Duration
            if (playerIsTranformedInCat || playerIsTranformedInMouse)
            {
                if (_durationOfEffectSinceLaunched == 0)
                    _durationOfEffectSinceLaunched = spellDurationOfEffect;
                else
                    _durationOfEffectSinceLaunched -= Time.deltaTime;

                Debug.Log(_durationOfEffectSinceLaunched);

                if (_durationOfEffectSinceLaunched <= 0)
                {
                    Debug.Log("Transformation into human");
                    if (playerIsTranformedInCat)
                    {
                        Debug.Log("FROM CAT TO HUMAN");
                        HumanTransformation(catCharacterModel, defaultCharacterModel);
                    }
                    else if (playerIsTranformedInMouse)
                    {
                        Debug.Log("FROM MOUSE TO HUMAN");
                        HumanTransformation(mouseCharacterModel, defaultCharacterModel);
                    }
                }
            }
            #endregion
        }
    }

    public float SetSpellDuration(float value)
    {
        spellDurationOfEffect = value;
        Debug.Log("Spell duration of effect equals :  " + spellDurationOfEffect);
        return spellDurationOfEffect;
    }

    private void HumanTransformation(GameObject objToDisactive, GameObject objToActive)
    {
        Debug.Log("Trying to transform the player character in a human...");

        objToDisactive.SetActive(false);
        objToActive.SetActive(true);

        // Suppression - Instance
        //GameObject activeCharacterModel = activePlayerCharacter.GetChild(0).gameObject;
        //Destroy(activeCharacterModel);

        //GameObject modelToSwitchTo = Instantiate(newPlayerCharacterModel, activePlayerCharacter) as GameObject;
        //modelToSwitchTo.transform.SetParent(activePlayerCharacter);

        Player.s_Singleton.playerIsTranformedInMouse = false;
        Player.s_Singleton.playerIsTranformedInCat = false;
        Player.s_Singleton.playerIsInHumanForm = true;
        Player.s_Singleton.playerAnimator.SetBool("EndOfTransformation", false);
    }


    void StunPlayerTarget()
    {
        PlayerSpellsInventory.s_Singleton.spellsCompartments[0].MyCompartmentSpell.Stun(actualPlayerTarget);
        Debug.Log("Cible disponible, possibiilité de lancer le sort");
    }
}
