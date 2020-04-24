using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerAvantProces : MonoBehaviour
{
    public static UIManagerAvantProces singleton;

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
        
    }

    void Update()
    {
        
    }

    public void ProofDisplayUpdate(int currentProofIndex)
    {
        ProofManager proofManager = ProofManager.singleton;
        int roundedAxis = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        proofManager.proofList[currentProofIndex].SetActive(true);
        if (currentProofIndex == 0 && roundedAxis == -1)
        {
            proofManager.proofList[proofManager.proofList.Length-1].SetActive(false);
        }
        else if(currentProofIndex == proofManager.proofList.Length-1 && roundedAxis == 1)
        {
            proofManager.proofList[0].SetActive(false);
        }
        else
        {
            proofManager.proofList[currentProofIndex - roundedAxis].SetActive(false);
        }
        
    }
}
