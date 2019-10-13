using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathParseTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		print("Starting");
		string test = "2+2";
		print($"2+2 = ${MathParser.parse(test).ToString()}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
