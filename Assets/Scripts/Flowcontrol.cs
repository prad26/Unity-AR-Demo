using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowcontrol : MonoBehaviour
{

    public List<Material> pipes;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in pipes)
        {
            item.SetFloat("Vector1_54C31050",0f);    
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        float flowAmount = Mathf.Lerp(0, 1, Time.deltaTime);

        foreach (var item in pipes)
        {
            item.SetFloat("Vector1_54C31050",flowAmount);    
        }
    }
}
