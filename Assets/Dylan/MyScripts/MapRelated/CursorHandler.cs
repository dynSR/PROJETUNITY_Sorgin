using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private float cursorSensitivity = 0.75f;
    [SerializeField] private RectTransform mapWindow;
    [SerializeField] private string instantiatingAMarkerWwiseEventSoundName;

    [SerializeField] private GameObject[] markers;
    public List<GameObject> markersPlaced;

    int SavedNumber;

    private string velYAxisName;
    private string velXAxisName;

    private RectTransform myRectTransform;

    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();

        if (PlayerPrefs.GetInt("FirstMapSave") ==0)
        {
            PlayerPrefs.SetInt("NumberOfSaved", 0);
            PlayerPrefs.SetInt("FirstMapSave", 1);
        }

       Load();
    }

    void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            CursorMovements();

            #region Croix/A
            //Press X
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_X") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_A"))
            {
                InstantiateAMarker(markers[0], this.transform.position);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                InstantiateAMarker(markers[0], this.transform.position);
            }
            #endregion

            #region Square/X
            //Press Square
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Square") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_X"))
            {
                InstantiateAMarker(markers[1], this.transform.position);
            }
            #endregion

            #region Triangle/Y
            //Press Triangle
            if (ConnectedController.s_Singleton.PS4ControllerIsConnected && Input.GetButtonDown("PS4_Triangle") || ConnectedController.s_Singleton.XboxControllerIsConnected && Input.GetButtonDown("XBOX_Y"))
            {
                InstantiateAMarker(markers[2], this.transform.position);
            }
            #endregion

            //Debug clavier du reset des marqueurs au cas où !
            if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerPrefs.DeleteAll();
                StartCoroutine(ResetMarkers());
            }
        }
    }

    //Summary : Gestion des déplacements du curseur à l'intérieur de l'affichage de la carte
    private void CursorMovements()
    {
        if (ConnectedController.s_Singleton.PS4ControllerIsConnected)
        {
            velYAxisName = "PS4_LStick_Vertical";
            velXAxisName = "PS4_LStick_Horizontal";
        }
        else if(ConnectedController.s_Singleton.XboxControllerIsConnected)
        {
            velYAxisName = "XBOX_LStick_Vertical";
            velXAxisName = "XBOX_LStick_Horizontal";
        }

        float velY = Input.GetAxisRaw(velYAxisName);
        float velX = Input.GetAxisRaw(velXAxisName);
        
        Vector2 position = myRectTransform.anchoredPosition;

        position.y += velY * cursorSensitivity;
        position.x += velX * cursorSensitivity;

        //Bouge le curseur à la nouvelle position donnée, modifiée par les inputs + assurance du fait que le curseur reste toujours affiché entièrement à l'écran dans le rectTransform de l'UI Manager.
        myRectTransform.anchoredPosition = Vector2.Lerp(myRectTransform.anchoredPosition, new Vector2(Mathf.Clamp(position.x, mapWindow.rect.xMin - myRectTransform.rect.xMin, mapWindow.rect.xMax - myRectTransform.rect.xMax), Mathf.Clamp(position.y, mapWindow.rect.yMin - myRectTransform.rect.yMin, mapWindow.rect.yMax - myRectTransform.rect.yMax)), 1);
    }

    //Summary : Permet d'instancier un marqueur (jalon de repère) à une position donnée
    private void InstantiateAMarker(GameObject markerToInstantiate, Vector3 positionToInstantiate)
    {
        //Crée un marqueur à l'endroit du curseur
        GameObject instantiatedObj = Instantiate(markerToInstantiate, positionToInstantiate, Quaternion.identity, mapWindow.transform) as GameObject;
        SetParent(instantiatedObj, mapWindow.transform);

        markersPlaced.Add(instantiatedObj);

        //Joue un son de création
        AkSoundEngine.PostEvent(instantiatingAMarkerWwiseEventSoundName, this.gameObject);
    }

    void SetParent(GameObject objToSet, Transform parent)
    {
        //Debug.Log("Setting the Parent of the instantiated object");
        objToSet.transform.SetParent(parent);
    }

    public void Save()
    {
        for (int i = 0; i < markersPlaced.Count; i++)
        {
            PlayerPrefs.SetFloat("xPos" + i, markersPlaced[i].transform.GetComponent<RectTransform>().position.x);
            PlayerPrefs.SetFloat("yPos" + i, markersPlaced[i].transform.GetComponent<RectTransform>().position.y);

            if(markersPlaced[i].CompareTag("EnnemyMap"))
            {
                PlayerPrefs.SetInt("Marker" + i, 0);
            }
            if (markersPlaced[i].CompareTag("WayMap"))
            {
                PlayerPrefs.SetInt("Marker" + i, 1);
            }
            if (markersPlaced[i].CompareTag("ObjectMap"))
            {
                PlayerPrefs.SetInt("Marker" + i, 2);
            }
            SavedNumber++;
        }
        PlayerPrefs.SetInt("NumberOfSaved", SavedNumber);
        Debug.Log("Save finished");
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if(PlayerPrefs.GetInt("NumberOfSaved") > 0)
        {
            for (int i = 0; i < PlayerPrefs.GetInt("NumberOfSaved"); i++)
            {
                InstantiateAMarker(markers[PlayerPrefs.GetInt("Marker" + i)], new Vector3(PlayerPrefs.GetFloat("xPos" +i), PlayerPrefs.GetFloat("yPos" +i), 0));
                Debug.Log("Instantiated");
            }
        }
    }


    IEnumerator ResetMarkers()
    {
        int removedItem = 0;

        do
        {
            for (int i = 0; i < markersPlaced.Count; i++)
            {
                removedItem++;
                Debug.Log(removedItem);
                Destroy(markersPlaced[i]);
            }
            yield return new WaitForEndOfFrame();
        } while (removedItem != markersPlaced.Count);

        markersPlaced.Clear();
    }

    //

    //

    ///Cf : le script OverlapHandler 

    //GameObject GetTheOverlappedRect(GameObject overlappedRect)
    //{
    //    return overlappedRect;
    //}

    //bool CheckIfTwoRectsOverlap(RectTransform rectTransform01, RectTransform rectTransform02)
    //{
    //    Rect rect1 = new Rect(rectTransform01.sizeDelta.x / 2, rectTransform01.sizeDelta.y / 2, rectTransform01.rect.width / 2, rectTransform01.rect.height / 2);
    //    Rect rect2 = new Rect(rectTransform02.sizeDelta.x / 2, rectTransform02.sizeDelta.y / 2, rectTransform02.rect.width / 2, rectTransform02.rect.height / 2);

    //    if (!rect1.Overlaps(rect2)) return false;
    //    else
    //    {
    //        debugObjectOverlapped = GetTheOverlappedRect(rectTransform02.gameObject);
    //        return true;
    //    }
    //}

    ////Summary : Permet d'effacer un marqueur (jalon de repère) à la position du curseur
    //private void EraseAMarker()
    //{
    //    Debug.Log("Trying to erase a marker");

    //    if (CheckIfTwoRectsOverlap(this.rectTransform, obj01.GetComponent<RectTransform>()))
    //    {
    //        Debug.Log(markers[0].GetComponent<RectTransform>().name);
    //        Debug.Log("Erasing the landmark overlapped by the cursor" + markers[0].name);
    //        Destroy(debugObjectOverlapped.gameObject);
    //    }

    //    else if (CheckIfTwoRectsOverlap(this.rectTransform, obj02.GetComponent<RectTransform>()))
    //    {
    //        Debug.Log(markers[0].GetComponent<RectTransform>().name);
    //        Debug.Log("Erasing the landmark overlapped by the cursor " + markers[1].name);
    //        Destroy(debugObjectOverlapped.gameObject);
    //    }
    //}
}
