using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleModel : MonoBehaviour
{
    public List<GameObject> models;
    
    public void toggle()
    {
        if(models[0].activeInHierarchy)
        {
            models[0].SetActive(false);
            models[1].SetActive(true);
        }
        else
        {
            models[0].SetActive(true);
            models[1].SetActive(false);
        }
            
    }
}
