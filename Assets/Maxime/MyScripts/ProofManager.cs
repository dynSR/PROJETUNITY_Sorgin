using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProofManager : MonoBehaviour
{
    public static ProofManager singleton;

    private bool objectMode;

    public GameObject[] proofDocList;
    public GameObject[] proofObjsList;

    private int activeDocProof;
    private int activeObjProof;

    public float rotationSpeed;

    bool canSwitchMode = true;
    private bool dpadXIsPressed = false;
    private float dpadX;

    private void Awake()
    {
        if (singleton)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        activeDocProof = 0;
        activeObjProof = 0;
        objectMode = false;
        UIManager_AvantProces.singleton.UIUpdateActualDoc(activeDocProof + 1, proofDocList.Length);
    }

    // Update is called once per frame
    void Update()
    {
        CheckDpadXValue();

        if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_L1") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_LB"))
        {
            int myUpdateDir = -1;
            PreviousProof(myUpdateDir);
        }

        if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_R1") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_RB"))
        {
            int myUpdateDir = 1;
            NextProof(myUpdateDir);
        }

        if(canSwitchMode && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetAxis("PS4_DPadHorizontal") >= 0.75f || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetAxis("XBOX_DPadHorizontal") >= 0.75f))
        {
            SwitchMode();
            canSwitchMode = false;
        }

        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            RotateActiveObj();
        }
    }


    //Passe a la preuve suivante
    private void NextProof(int updateDirection)
    {
        if (objectMode)
        {
            proofObjsList[activeObjProof].gameObject.transform.rotation = Quaternion.identity;
            if (activeObjProof != proofObjsList.Length - 1)
            {
                activeObjProof += 1;
                ProofObjDisplayUpdate(activeObjProof, updateDirection);
            }
            else
            {
                activeObjProof = 0;
                ProofObjDisplayUpdate(activeObjProof, updateDirection);
            }

            UIManager_AvantProces.singleton.UIUpdateActualDoc(activeObjProof + 1, proofObjsList.Length);
        }
        else
        {
            if (activeDocProof != proofDocList.Length - 1)
            {
                activeDocProof += 1;
                ProofDocDisplayUpdate(activeDocProof, updateDirection);
            }
            else
            {
                activeDocProof = 0;
                ProofDocDisplayUpdate(activeDocProof, updateDirection);
            }

            UIManager_AvantProces.singleton.UIUpdateActualDoc(activeDocProof + 1, proofDocList.Length);
        }
    }

    //Passe a la preuve precedente
    private void PreviousProof(int updateDirection)
    {
        if (objectMode)
        {
            proofObjsList[activeObjProof].gameObject.transform.rotation = Quaternion.identity;
            if (activeObjProof != 0)
            {
                activeObjProof -= 1;
                ProofObjDisplayUpdate(activeObjProof, updateDirection);
            }
            else
            {
                activeObjProof = proofObjsList.Length - 1;
                ProofObjDisplayUpdate(activeObjProof, updateDirection);
            }

            UIManager_AvantProces.singleton.UIUpdateActualDoc(activeObjProof + 1, proofObjsList.Length);
        }
        else
        {
            if (activeDocProof != 0)
            {
                activeDocProof -= 1;
                ProofDocDisplayUpdate(activeDocProof, updateDirection);
            }
            else
            {
                activeDocProof = proofDocList.Length - 1;
                ProofDocDisplayUpdate(activeDocProof, updateDirection);
            }

            UIManager_AvantProces.singleton.UIUpdateActualDoc(activeDocProof + 1, proofDocList.Length);
        }
    }

    //affichage des docs
    //newActiveProofIndex est le nouveau doc actif
    //updateDirection 1 = bouton R1 (suivant), -1 = bouton L1 (precedent)
    private void ProofDocDisplayUpdate(int newActiveProofIndex, int newUpdateDirection)
    {
        proofDocList[newActiveProofIndex].SetActive(true);
        Debug.Log("Active : " + newActiveProofIndex);

        if (newActiveProofIndex == 0 && newUpdateDirection == 1)
        {
            proofDocList[proofDocList.Length - 1].SetActive(false);
            return;
        }
        else if (newActiveProofIndex == proofDocList.Length - 1 && newUpdateDirection == -1)
        {
            proofDocList[0].SetActive(false);
            return;
        }
        else
        {
            proofDocList[newActiveProofIndex - newUpdateDirection].SetActive(false);
        }
    }

    //affichage des objets
    //newActiveProofIndex est le nouvel objet actif
    //updateDirection 1 = bouton R1 (suivant), -1 = bouton L1 (precedent)
    private void ProofObjDisplayUpdate(int newActiveProofIndex, int newUpdateDirection)
    {
        proofObjsList[newActiveProofIndex].SetActive(true);
        Debug.Log("Active : " + newActiveProofIndex);

        if (newActiveProofIndex == 0 && newUpdateDirection == 1)
        {
            proofObjsList[proofObjsList.Length - 1].SetActive(false);
            return;
        }
        else if (newActiveProofIndex == proofObjsList.Length - 1 && newUpdateDirection == -1)
        {
            proofObjsList[0].SetActive(false);
            return;
        }
        else
        {
            proofObjsList[newActiveProofIndex - newUpdateDirection].SetActive(false);
        }
    }

    //switch entre l'affichage des docs et des objets
    private void SwitchMode()
    {
        objectMode = !objectMode;
        if (objectMode)
        {
            proofDocList[activeDocProof].SetActive(false);
            proofObjsList[activeObjProof].SetActive(true);
            UIManager_AvantProces.singleton.UIUpdateActualDoc(activeObjProof + 1, proofObjsList.Length);
        }
        else
        {
            proofObjsList[activeObjProof].gameObject.transform.rotation = Quaternion.identity;
            proofObjsList[activeObjProof].SetActive(false);
            proofDocList[activeDocProof].SetActive(true);
            UIManager_AvantProces.singleton.UIUpdateActualDoc(activeDocProof + 1, proofDocList.Length);
        }
    }

    //Summary : Permet de vérifier si la flèche du haut est pressée par le joueur...
    void CheckDpadXValue()
    {
        if (ConnectedController.s_Singleton.PS4ControllerIsConnected)
            dpadX = Input.GetAxis("PS4_DPadHorizontal");

        else if (ConnectedController.s_Singleton.XboxControllerIsConnected)
            dpadX = Input.GetAxis("XBOX_DPadHorizontal");

        //Si la valeur de cette input dépasse une certaine valeur alors...
        if (dpadX >= 0.75f)
        {
            //La touche est pressée.
            dpadXIsPressed = true;
        }
        else
        {
            //Sinon, elle n'est pas pressée par le joueur...
            dpadXIsPressed = false;

            //Alors lejoueur peut de nouveau appuyer sur cette input pour moddifier l'état d'affichage de la carte.
            canSwitchMode = true;
        }
    }

    //gère la rotation de l'objet
    private void RotateActiveObj()
    {
        if (objectMode)
        {
            //float xAxis = Input.GetAxis("PS4_LStick_Horizontal");
            //float yAxis = Input.GetAxis("PS4_LStick_Vertical");
            float xAxis = Input.GetAxis("Vertical");
            float yAxis = Input.GetAxis("Horizontal");
            
            if(xAxis != 0 || yAxis != 0)
            {
                Quaternion newYRotation = Quaternion.AngleAxis(yAxis * -rotationSpeed, Vector3.up);
                Quaternion newXrotation = Quaternion.AngleAxis(xAxis * -rotationSpeed, Vector3.left);
                proofObjsList[activeObjProof].gameObject.transform.rotation = newXrotation * newYRotation * proofObjsList[activeObjProof].gameObject.transform.rotation;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                proofObjsList[activeObjProof].gameObject.transform.rotation = Quaternion.identity;
            }
        }
    }
}
