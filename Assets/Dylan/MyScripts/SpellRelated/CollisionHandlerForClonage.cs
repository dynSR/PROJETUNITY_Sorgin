using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionHandlerForClonage : MonoBehaviour
{
    public Collider[] hitColliders;
    public bool isColliding = false;

    //public float minDistance = 1.0f;
    //public float maxDistance = 4.0f;
    //public float smooth = 10.0f;
    //Vector3 dollyDir;
    //public Vector3 dollyDirAdjusted;
    //public float distance;
    //public float lerp;


    //private void Awake()
    //{
    //    dollyDir = transform.localPosition.normalized;
    //    distance = transform.localPosition.magnitude;
    //}

    private void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            if (Player.s_Singleton.isUsingASpell && Player.s_Singleton.isTryingToClone && (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X")))
            {
                ClonePlayerCharacter();
            }
                //Ajustement de la position de la position d'instance - Pas obligatoire donc commenté
                //Vector3 desiredObjPosition = transform.parent.TransformPoint(dollyDir * maxDistance);
                //RaycastHit hit;

                //if (Physics.Linecast(transform.parent.position, /*Player.s_Singleton.posToInstantiateTheClone.position*/desiredObjPosition, out hit))
                //{
                //    //transform.position = Vector3.Lerp(transform.parent.GetComponent<Collider>().ClosestPointOnBounds(transform.parent.localPosition), hit.collider.ClosestPointOnBounds(hit.collider.transform.localPosition), Time.deltaTime* smooth);
                //    distance = Mathf.Clamp((hit.distance * lerp), minDistance, maxDistance);
                //}
                //else
                //{
                //    distance = maxDistance;
                //    //transform.position = Player.s_Singleton.posToInstantiateTheClone.position;
                //}

                //transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);

                CollisionCheck();
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.black;
        //Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    void ClonePlayerCharacter()
    {
        if (/*PlayerSpellsInventory.s_Singleton.spellCompartmentIsActive &&*/ !isColliding)
        {
            PlayerSpellsInventory.s_Singleton.spellsCompartments[0].MyCompartmentSpell.Clonage(Player.s_Singleton.defaultCharacterModelClone, Player.s_Singleton.posToInstantiateTheClone);
            Player.s_Singleton.posToInstantiateTheClone.GetChild(0).gameObject.SetActive(false);
            Player.s_Singleton.isTryingToClone = false;

            Debug.Log(Player.s_Singleton.posToInstantiateTheClone.GetChild(0).gameObject.name);
            return;
        }
        else
            PlayerSpellsInventory.s_Singleton.CantUseASpell();
    }

    void CollisionCheck()
    {
        hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        int i = 0;
        Renderer playerCharacterRenderer = transform.GetChild(0).GetComponent<Renderer>();
        SkinnedMeshRenderer[] meshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

        if (i < hitColliders.Length)
        {
            i++;
            isColliding = true;

            if (transform.GetChild(0).gameObject.activeInHierarchy)
            {
                if (playerCharacterRenderer != null)
                {
                    playerCharacterRenderer.material.color = new Color(255, 0, 0, 100);
                }
                //else if (meshRenderers.Length > 0 && playerCharacterRenderer == null)
                //{
                //    for (int j = 0; j < meshRenderers.Length; j++)
                //    {
                //        meshRenderers[j].material.color = new Color(255, 0, 0, 100);
                //    }
                //}
            }  
        }
        else
        {
            isColliding = false;

            if (transform.GetChild(0).gameObject.activeInHierarchy )
            {
                if (playerCharacterRenderer != null)
                {
                    playerCharacterRenderer.material.color = new Color(255, 255, 255, 100);

                }
                //else if (meshRenderers.Length > 0 && playerCharacterRenderer == null)
                //{
                //    for (int j = 0; j < meshRenderers.Length; j++)
                //    {
                //        meshRenderers[j].material.color = new Color(255, 255, 255, 100);
                //    }
                //}
            }
        }
    }
}
