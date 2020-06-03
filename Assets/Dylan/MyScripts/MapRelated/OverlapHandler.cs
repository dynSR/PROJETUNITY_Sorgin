using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlapHandler : MonoBehaviour
{
    [SerializeField] private string erasingAMarkerWwiseEventSoundName;
    [SerializeField] private float valueToDivideBy = 2f;

    CursorHandler _cursor;

    public Sprite[] sprites;

    [HideInInspector]
    public RectTransform rectTransform;

    void Start()
    {
        _cursor = GameObject.Find("MapCursor").GetComponent<CursorHandler>();
        rectTransform = GetComponent<RectTransform>();
        GetComponent<Image>().sprite = sprites[0];
    }

    void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            HighlightOverlappedObject();

            if (ConnectedController.s_Singleton.PS4ControllerIsConnected)
            {
                if (Input.GetButtonDown("PS4_X"))
                {
                    Debug.Log("Cross pressed");
                    EraseAMarker();
                }
            }
            else if (ConnectedController.s_Singleton.XboxControllerIsConnected)
            {
                if (Input.GetButtonDown("XBOX_A"))
                {
                    Debug.Log("A pressed");
                    EraseAMarker();
                }
            }
        }
    }

    //Summary : Permet de vérifier si deux rect transform se supperposent, ici c'est celui du curseur et d'un marqueur x...
    public bool CheckIfTwoRectsOverlap(RectTransform rectTransform01, RectTransform rectTransform02)
    {
        Rect rect1 = new Rect(rectTransform01.localPosition.x, rectTransform01.localPosition.y, rectTransform01.rect.width / valueToDivideBy, rectTransform01.rect.height / valueToDivideBy);
        Rect rect2 = new Rect(rectTransform02.localPosition.x, rectTransform02.localPosition.y, rectTransform02.rect.width / valueToDivideBy, rectTransform02.rect.height / valueToDivideBy);

        return rect1.Overlaps(rect2);
    }

    private void HighlightOverlappedObject()
    {
        if (CheckIfTwoRectsOverlap(_cursor.GetComponent<RectTransform>(), rectTransform))
        {
            GetComponent<Image>().sprite = sprites[1];
        }
        else if(!CheckIfTwoRectsOverlap(_cursor.GetComponent<RectTransform>(), rectTransform) || !MapHandler.s_Singleton.mapIsDisplayed)
        {
            GetComponent<Image>().sprite = sprites[0];
        }

    }

    //Summary : Permet d'effacer un marqueur (jalon de repère) à la position du curseur
    public void EraseAMarker()
    {
        Debug.Log("Trying to erase a marker");

        //Si Le RectTransform attaché au Curseur est par-dessus le RectTransform attaché à l'objet utilisant ce script alors...
        if (CheckIfTwoRectsOverlap(_cursor.GetComponent<RectTransform>(), rectTransform))
        {
            _cursor.markersPlaced.Remove(gameObject);

            //Cette objet est détruit...
            Destroy(gameObject);

            //Un son de destruction se joue.
            AkSoundEngine.PostEvent(erasingAMarkerWwiseEventSoundName, this.gameObject);
            return;
        }
    }
}
