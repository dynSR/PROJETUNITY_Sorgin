using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("PS4_L1"))
        {
            int myUpdateDir = -1;
            PreviousProof(myUpdateDir);
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("PS4_R1"))
        {
            int myUpdateDir = 1;
            NextProof(myUpdateDir);
        }

        if(Input.GetKeyDown(KeyCode.T) || Input.GetAxis("PS4_DPadHorizontal") > 0)
        {
            SwitchMode();
        }

        RotateActiveObj();
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
                return;
            }
            else
            {
                activeObjProof = 0;
                ProofObjDisplayUpdate(activeObjProof, updateDirection);
            }
        }
        else
        {
            if (activeDocProof != proofDocList.Length - 1)
            {
                activeDocProof += 1;
                UIManagerAvantProces.singleton.ProofDocDisplayUpdate(activeDocProof, updateDirection);
                return;
            }
            else
            {
                activeDocProof = 0;
                UIManagerAvantProces.singleton.ProofDocDisplayUpdate(activeDocProof, updateDirection);
            }
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
                UIManagerAvantProces.singleton.ProofDocDisplayUpdate(activeDocProof, updateDirection);
                return;
            }
            else
            {
                activeDocProof = proofDocList.Length - 1;
                UIManagerAvantProces.singleton.ProofDocDisplayUpdate(activeDocProof, updateDirection);
            }
        }
    }

    //affichage des objets
    //newActiveProofIndex est le nouvel objet actif
    //updateDirection 1 = bouton R1 (suivant), -1 = bouton L1 (precedent)
    private void ProofObjDisplayUpdate(int newActiveProofIndex, int updateDirection)
    {
        proofObjsList[newActiveProofIndex].SetActive(true);
        Debug.Log("Active : " + newActiveProofIndex);

        if (newActiveProofIndex == 0 && updateDirection == 1)
        {
            proofObjsList[proofObjsList.Length - 1].SetActive(false);
            return;
        }
        else if (newActiveProofIndex == proofObjsList.Length - 1 && updateDirection == -1)
        {
            proofObjsList[0].SetActive(false);
            return;
        }
        else
        {
            proofObjsList[newActiveProofIndex - updateDirection].SetActive(false);
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
        }
        else
        {
            proofObjsList[activeObjProof].gameObject.transform.rotation = Quaternion.identity;
            proofObjsList[activeObjProof].SetActive(false);
            proofDocList[activeDocProof].SetActive(true);
        }
    }

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
                proofObjsList[activeObjProof].gameObject.transform.Rotate(xAxis * rotationSpeed, yAxis * -rotationSpeed, 0f);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                proofObjsList[activeObjProof].gameObject.transform.rotation = Quaternion.identity;
            }
        }
    }
}
