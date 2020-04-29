using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCursorOverlap : MonoBehaviour
{
    [SerializeField] private string erasingAMarkerWwiseEventSoundName;
    [SerializeField] private float valueToDivideBy = 2f;

    CursorMovement _cursor;
    RectTransform rectTransform;

    void Start()
    {
        _cursor = GameObject.Find("MapCursor").GetComponent<CursorMovement>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetButtonDown("PS4_O"))
        {
            Debug.Log("O pressed");
            EraseAMarker();
        }

        Debug.Log(CheckIfTwoRectsOverlap(_cursor.GetComponent<RectTransform>(), rectTransform));
    }

    //Summary : Permet de vérifier si deux rect transform se supperposent, ici c'est celui du curseur et d'un marqueur x...
    bool CheckIfTwoRectsOverlap(RectTransform rectTransform01, RectTransform rectTransform02)
    {
        Rect rect1 = new Rect(rectTransform01.localPosition.x, rectTransform01.localPosition.y, rectTransform01.rect.width / valueToDivideBy, rectTransform01.rect.height / valueToDivideBy);
        Rect rect2 = new Rect(rectTransform02.localPosition.x, rectTransform02.localPosition.y, rectTransform02.rect.width / valueToDivideBy, rectTransform02.rect.height / valueToDivideBy);

        return rect1.Overlaps(rect2);
    }

    //Summary : Permet d'effacer un marqueur (jalon de repère) à la position du curseur
    private void EraseAMarker()
    {
        Debug.Log("Trying to erase a marker");

        //Si Le RectTransform attaché au Curseur est par-dessus le RectTransform attaché à l'objet utilisant ce script alors...
        if (CheckIfTwoRectsOverlap(_cursor.GetComponent<RectTransform>(), rectTransform))
        {
            //Cette objet est détruit...
            Destroy(gameObject);

            //Un son de destruction se joue.
            AkSoundEngine.PostEvent(erasingAMarkerWwiseEventSoundName, this.gameObject);
            return;
        }
    }
}
