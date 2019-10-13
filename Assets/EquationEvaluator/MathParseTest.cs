using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathParseTest : MonoBehaviour
{
	private bool tested;
    // Start is called before the first frame update
    void Start()
    {
		tested = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!tested)
		{
			print("Starting");
			string test = "8*(2*z+5)-37/x+neg4.2*(90)/(3.14^(2))";
			print($"2+2 = {MathParser.parse(test).ToString()}");
			print($"2+2 = {MathParser.parse(test).eval(1,1)}");
			tested = true;
		}
	}
}
