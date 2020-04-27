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

    public void ProofDisplayUpdate(int currentProofIndex)
    {
        int roundedAxis = Mathf.RoundToInt(1 * Mathf.Sign(Input.GetAxis("Horizontal")));
        Debug.Log("Rounded axis : " + roundedAxis);
        proofManager.proofList[currentProofIndex].SetActive(true);
        Debug.Log("Active : " + currentProofIndex);

        if (currentProofIndex == 0 && roundedAxis == 1)
        {
            proofManager.proofList[proofManager.proofList.Length-1].SetActive(false);
            Debug.Log("Unactive : " + (proofManager.proofList.Length - 1));
            return;
        }
        else if(currentProofIndex == proofManager.proofList.Length-1 && roundedAxis == -1)
        {
            proofManager.proofList[0].SetActive(false);
            Debug.Log("Unactive : " + 0);
            return;
        }
        else
        {
            proofManager.proofList[currentProofIndex - roundedAxis].SetActive(false);
            Debug.Log("Unactive : " + (currentProofIndex - roundedAxis));
        }

    }
}
