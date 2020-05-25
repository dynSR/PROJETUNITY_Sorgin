using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
public class ArmoireAnimation : MonoBehaviour
{

    GameObject playerCharacter;

    public GameObject PlayerPositionHolder;
    public Animator Anim;
    public CinemachineVirtualCamera CamArmoire;
    public float Speed;

    public GameObject Light;
    public GameObject Text;

    [SerializeField] private GameObject interactionPopup;



    bool Lerp;
    bool Inside;
    bool Animating;

    bool InTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            if (!Inside)
            {
                Text.SetActive(InTrigger);
                if (playerCharacter != null)
                {
                    playerCharacter.transform.Find("View").gameObject.SetActive(true);
                    Light.SetActive(false);

                }
            }
            else
            {
                Text.SetActive(false);
                if (playerCharacter != null)
                {
                    playerCharacter.transform.Find("View").gameObject.SetActive(false);
                    Light.SetActive(true);
                }
            }

            if (InTrigger)
            {
                if ((ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_X") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_A")) && !Animating && !Inside)
                {
                    Animating = true;
                    LaunchAnim();
                    Player.s_Singleton.inWardrobe = true;
                    interactionPopup.SetActive(false);
                }

                if ((ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_X") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_A")) && !Animating && Inside)
                {
                    Animating = true;
                    OutAnim();
                }
            }

            if (Lerp)
            {
                playerCharacter.transform.position = Vector3.MoveTowards(playerCharacter.transform.position, PlayerPositionHolder.transform.position, Time.deltaTime * Speed);
                playerCharacter.transform.rotation = Quaternion.RotateTowards(playerCharacter.transform.rotation, PlayerPositionHolder.transform.rotation, Speed * Time.deltaTime * 100);
            }
        }  
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InTrigger = true;
            playerCharacter = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InTrigger = false;
    }

    void LaunchAnim()
    {
        playerCharacter.GetComponent<MoveScript>().enabled = false;
        Lerp = true;
        Invoke("Animate", 0.2f);
        Invoke("CameraOn", 1.5f);
    }

    void OutAnim()
    {
        Invoke("AnimateOut", 0.2f);
        Invoke("CameraOff", 1.5f);
    }

    void Animate()
    {
        CamArmoire.Priority = 12;

        //Player.transform.parent = PlayerPositionHolder.transform;
        Anim.SetBool("Enter", true);
        playerCharacter.GetComponentInChildren<Animator>().SetBool("Enter", true);
        //Director.Play();
    }
    void AnimateOut()
    {
        CamArmoire.Priority = 8;

        Anim.SetBool("Enter", false);
        playerCharacter.GetComponentInChildren<Animator>().SetBool("Enter", false);
    }

    void CameraOff()
    {
        playerCharacter.transform.parent = null;
        playerCharacter.GetComponent<MoveScript>().enabled = true;
        Animating = false;
        Inside = false;
        Lerp = false;
        Player.s_Singleton.inWardrobe = false;
        interactionPopup.SetActive(true);

    }

    void CameraOn()
    {
        Animating = false;
        Inside = true;

    }
}
