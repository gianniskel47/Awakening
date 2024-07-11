using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePartLoader : MonoBehaviour
{
    public SO_Vect2 soVect2;
    public float loadRange;

    private bool isLoaded= false;

    void Update()
    {
        DistanceCheck();
    }

    private void DistanceCheck()
    {
        if(Vector3.Distance(soVect2.playerPos, transform.position) < loadRange)
        {
            LoadScene();
        }
        else
        {
            UnloadScene();
        }
    }

    private void LoadScene()
    {
        if (!isLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }

    private void UnloadScene()
    {
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }
}
