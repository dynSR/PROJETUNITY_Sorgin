using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProofManager : MonoBehaviour
{
    public static ProofManager singleton;

    public GameObject[] proofList;
    private int activeProof;
    private float timerSwap = 0f;

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
        activeProof = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerSwap > 0)
        {
            timerSwap -= Time.deltaTime;
        }
        else
        {
            if (Input.GetAxis("Horizontal") < -0.1)
            {
                PreviousProof();
                Debug.Log(activeProof);
                timerSwap = 0.5f;
                return;
            }

            if (Input.GetAxis("Horizontal") > 0.1)
            {
                NextProof();
                Debug.Log(activeProof);
                timerSwap = 0.5f;
                return;
            }
        }
    }

    private void NextProof()
    {
        if(activeProof != proofList.Length-1)
        {
            activeProof += 1;
            UIManagerAvantProces.singleton.ProofDisplayUpdate(activeProof);
        }
        else
        {
            activeProof = 0;
            UIManagerAvantProces.singleton.ProofDisplayUpdate(activeProof);
        }
        
    }
    
    private void PreviousProof()
    {
        if (activeProof != 0)
        {
            activeProof -= 1;
            UIManagerAvantProces.singleton.ProofDisplayUpdate(activeProof);
        }
        else
        {
            activeProof = proofList.Length-1;
            UIManagerAvantProces.singleton.ProofDisplayUpdate(activeProof);
        }
    }
}
