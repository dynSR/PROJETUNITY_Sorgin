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
        UIManagerGlobalAvProces.singleton.UIUpdateActualDoc(activeDocProof + 1, proofDocList.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("PS4_L1") || Input.GetButtonDown("XBOX_LB"))
        {
            int myUpdateDir = -1;
            PreviousProof(myUpdateDir);
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("PS4_R1") || Input.GetButtonDown("XBOX_RB"))
        {
            int myUpdateDir = 1;
            NextProof(myUpdateDir);
        }

        if(Input.GetKeyDown(KeyCode.T) || Input.GetAxis("PS4_DPadHorizontal") > 0 || Input.GetAxis("XBOX_DPadHorizontal") > 0)
        {
            SwitchMode();
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

            UIManagerGlobalAvProces.singleton.UIUpdateActualDoc(activeObjProof + 1, proofObjsList.Length);
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

            UIManagerGlobalAvProces.singleton.UIUpdateActualDoc(activeDocProof + 1, proofDocList.Length);
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
                return;
            }
            else
            {
                activeObjProof = proofObjsList.Length - 1;
                ProofObjDisplayUpdate(activeObjProof, updateDirection);
            }
        }
        else
        {
            if (activeDocProof != 0)
            {
                activeDocProof -= 1;
                ProofDocDisplayUpdate(activeDocProof, updateDirection);
                return;
            }
            else
            {
                activeDocProof = proofDocList.Length - 1;
                ProofDocDisplayUpdate(activeDocProof, updateDirection);
            }
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
            UIManagerGlobalAvProces.singleton.UIUpdateActualDoc(activeObjProof + 1, proofObjsList.Length);
        }
        else
        {
            proofObjsList[activeObjProof].gameObject.transform.rotation = Quaternion.identity;
            proofObjsList[activeObjProof].SetActive(false);
            proofDocList[activeDocProof].SetActive(true);
            UIManagerGlobalAvProces.singleton.UIUpdateActualDoc(activeDocProof + 1, proofDocList.Length);
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
