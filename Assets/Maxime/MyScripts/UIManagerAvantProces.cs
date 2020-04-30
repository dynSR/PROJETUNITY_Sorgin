using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerAvantProces : MonoBehaviour
{
    public static UIManagerAvantProces singleton;

    private ProofManager proofManager;

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

    void Start()
    {
        proofManager = ProofManager.singleton;
    }

    void Update()
    {
        
    }

    //affichage des docs
    //newActiveProofIndex est le nouveau doc actif
    //updateDirection 1 = bouton R1 (suivant), -1 = bouton L1 (precedent)
    public void ProofDocDisplayUpdate(int newActiveProofIndex, int updateDirection)
    {
        proofManager.proofDocList[newActiveProofIndex].SetActive(true);
        Debug.Log("Active : " + newActiveProofIndex);

        if (newActiveProofIndex == 0 && updateDirection == 1)
        {
            proofManager.proofDocList[proofManager.proofDocList.Length-1].SetActive(false);
            return;
        }
        else if(newActiveProofIndex == proofManager.proofDocList.Length-1 && updateDirection == -1)
        {
            proofManager.proofDocList[0].SetActive(false);
            return;
        }
        else
        {
            proofManager.proofDocList[newActiveProofIndex - updateDirection].SetActive(false);
        }
    }
}
