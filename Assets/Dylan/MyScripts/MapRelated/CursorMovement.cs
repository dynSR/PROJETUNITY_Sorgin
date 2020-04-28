using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMovement : MonoBehaviour
{
    [SerializeField] private float cursorSensitivity = 0.75f;
    [SerializeField] private float smoothValue = 0.75f;
    [SerializeField] private RectTransform _canvasRect;

    [SerializeField] GameObject[] markers;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        CursorMovements();

        //Press X
        if (Input.GetButtonDown("PS4_X"))
        {

        }
        //Press O
        if (Input.GetButtonDown("PS4_O"))
        {

        }
        //Press Square
        if (Input.GetButtonDown("PS4_Square"))
        {

        }
        //Press Triangle
        if (Input.GetButtonDown("PS4_Triangle"))
        {

        }
    }

    //Summary : Gestion des déplacements du curseur à l'intérieur de l'affichage de la carte
    private void CursorMovements()
    {
        float velY = Input.GetAxis("PS4_LStick_Vertical");
        float velX = Input.GetAxis("PS4_LStick_Horizontal");

        Vector2 position = rectTransform.anchoredPosition;

        position.y += velY * cursorSensitivity;
        position.x += velX * cursorSensitivity;

        //Bouge le curseur à la nouvelle position donnée, modifiée par les inputs + assurance du fait que le curseur reste toujours affiché entièrement à l'écran dans le rectTransform de l'UI Manager.
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(Mathf.Clamp(position.x, _canvasRect.rect.xMin - rectTransform.rect.xMin, _canvasRect.rect.xMax - rectTransform.rect.xMax), Mathf.Clamp(position.y, _canvasRect.rect.yMin - rectTransform.rect.yMin, _canvasRect.rect.yMax - rectTransform.rect.yMax)), 1);
    }

    //Summary : Permet d'instancier un marqueur (jalon de repère) à une position donnée
    private void InstantiateAMarker(GameObject markerToInstantiate, Transform positionToInstantiate)
    {
        Instantiate(markerToInstantiate, positionToInstantiate);
    }
}
