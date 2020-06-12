using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ObjectDetection : MonoBehaviour
{
    Player player;
    bool isEnabled = false;
    public Vector3 scaleTo;
    public float scaleDuration = 1f;
    //Public pour debug
    public bool spellDurationOfEffectIsOver = false;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.s_Singleton;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Singleton.gameState == GameState.PlayMode)
        {
            if (isEnabled)
            {
                if (player._durationOfEffectSinceLaunched == 0)
                    player._durationOfEffectSinceLaunched = player.spellDurationOfEffect;
                else
                    player._durationOfEffectSinceLaunched -= Time.deltaTime;

                if (player._durationOfEffectSinceLaunched <= 0)
                {
                    spellDurationOfEffectIsOver = true;
                    //EndOfDetection();
                    StartCoroutine(ScaleOverSeconds(gameObject, new Vector3(0,0,0), scaleDuration));
                    
                }
            }
        }  
    }

    private void OnEnable()
    {
        spellDurationOfEffectIsOver = false;
        StartCoroutine(ScaleOverSeconds(gameObject, scaleTo, scaleDuration));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ObjectDetection"))
        {
            other.gameObject.GetComponentInParent<Outliner>().OutlineColor = new Color(255, 255, 255, 255);
            player.objectsFound.Add(other.gameObject.transform);

            Debug.Log(other.transform.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //List<Transform> removedObject = new List<Transform>();

        if (other.gameObject.CompareTag("ObjectDetection"))
        {
            other.gameObject.GetComponentInParent<Outliner>().OutlineColor = new Color(255, 255, 255, 0);
            player.objectsFound.Remove(other.gameObject.transform);

            Debug.Log(other.transform.name);
        }
    }

    void EndOfDetection()
    {
        for (int i = 0; i < player.objectsFound.Count; i++)
        {
            player.objectsFound[i].GetComponentInParent<Outliner>().OutlineColor = new Color(255, 255, 255, 0);
        }

        isEnabled = false;
        Player.s_Singleton.objectsFound.Clear();
    }

    public IEnumerator ScaleOverSeconds(GameObject objectToScale, Vector3 scaleTo, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingScale = objectToScale.transform.localScale;

        while (elapsedTime < seconds)
        {
            objectToScale.transform.localScale = Vector3.Lerp(startingScale, scaleTo, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if(!spellDurationOfEffectIsOver)
            isEnabled = true;
        else
        {
            EndOfDetection();
            gameObject.SetActive(false);
        }
            

        objectToScale.transform.localScale = scaleTo;
    }

}
