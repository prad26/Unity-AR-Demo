using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowcontrol : MonoBehaviour
{
    public List<Material> pipes;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitFor(1f));

        foreach (var item in pipes)
        {
            StartCoroutine(flow(item));
        }
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    public IEnumerator waitFor(float item)
    {
        yield return new WaitForSeconds(item);
    }
    
    public IEnumerator flow(Material item)
    {
        yield return new WaitForSeconds(0.5f);

        item.SetFloat("Vector1_4D290F82", Time.time);
        item.SetFloat("Boolean_CD861CCF", 1f);

    }
}
