using Duly.Moreorless;
using UnityEngine;

public class TestDuly : Play
{
    // Use this for initialization
    private void Start()
    {
        Debug.Log("Executing !");
        Debug.Log("Result => " + result);
        Execute();
        Debug.Log("Result after execution => " + result);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}