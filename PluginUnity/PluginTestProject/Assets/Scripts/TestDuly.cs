#define COMPILE

#if COMPILE
using Core.Plugin.Unity.Runtime;
using DNAI.MoreOrLess;
#endif
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using static DNAI.Vertex.DNAIBehaviour;

public class TestDuly
#if COMPILE
    : MoreOrLess
#endif
{
    public Text TextNumber;

    public int Tries = 10;
    public int MysteryNumber = 42;

    //Vertex v;

    public void Play()
    {
        Debug.Log("Has to find " + MysteryNumber);
#if COMPILE
        StartCoroutine(PlayRoutine());
#endif
    }

#if COMPILE
    private IEnumerator PlayRoutine()
    {
        ExecuteReset();
        lastResult = (int)COMPARISON.NONE;
        for (int i = Tries; i > 0 && result != MysteryNumber; i--)
        {
            ExecutePlay();
            lastResult = (int) (result < MysteryNumber ? COMPARISON.MORE : COMPARISON.LESS);
            Debug.Log($"Result = {result}; MysteryNumber = {MysteryNumber}; lastResult = {lastResult}");
            TextNumber.text = result.ToString();
            yield return new WaitForSeconds(0.5f);
        }
}
#endif

    public void OnMysteryNumberChanged(string nbr)
    {
        if (string.IsNullOrEmpty(nbr))
            return;
        MysteryNumber = Convert.ToInt32(nbr);
    }

#if COMPILE
    public void OutputChanged(EventOutputChange e)
    {
        Debug.Log("Output changed callback ! ==> " + e.Value + " (" + e.ValueType + ")");
    }
#endif
}