using Duly.Test;
using UnityEngine;

public class TestDuly : MyFunction
{
    // Use this for initialization
    private void Start()
    {
        Debug.Log("Executing !");
        Debug.Log("Result => " + return1);
        Execute();
        Debug.Log("Result after execution => " + return1);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}