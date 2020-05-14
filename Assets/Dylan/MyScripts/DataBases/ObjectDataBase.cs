using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDataBase : MonoBehaviour
{
    public List<Object> objectsInTheGame;

    public static ObjectDataBase s_Singleton;

    #region Singleton
    private void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            s_Singleton = this;
        }
    }
    #endregion
}
