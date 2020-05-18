using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnimator;
    [HideInInspector] public float _durationOfEffectSinceLaunched = 0;

    [Header("PLAYER MODELS")]
    //public Transform playerCharacter;
    public GameObject defaultCharacterModel;
    public GameObject catCharacterModel;
    public GameObject mouseCharacterModel;
    

    [Header("TRANSFORMATIONS STATES")]
    public bool playerIsTranformedInMouse = false;
    public bool playerIsTranformedInCat = false;
    public bool playerIsInHumanForm = true;

    [Header("PLAYER STATES")]
    public bool OnWall;
    public bool OnArmoire;
    public bool CanPickObject;

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
        #region Transformation Duration
        if (playerIsTranformedInCat || playerIsTranformedInMouse)
        {
            _durationOfEffectSinceLaunched -= Time.deltaTime;

            if (_durationOfEffectSinceLaunched <= 1.5f)
            {
                Debug.Log("End of transformation");
                playerAnimator.SetBool("EndOfTransformation", true);
            }
            if (_durationOfEffectSinceLaunched <= 0)
            {
                Debug.Log("Transformation into human");
                if (playerIsTranformedInCat)
                {
                    HumanTransformation(catCharacterModel, defaultCharacterModel);
                }
                else if (playerIsTranformedInMouse)
                {
                    HumanTransformation(mouseCharacterModel, defaultCharacterModel);
                }
            }
        }
        #endregion
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
}
