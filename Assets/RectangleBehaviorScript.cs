using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleBehaviorScript : MonoBehaviour
{
    public GameObject rect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleRect()
    {
        rect.GetComponent<MeshRenderer>().enabled = !rect.GetComponent<MeshRenderer>().enabled;
    }
}
