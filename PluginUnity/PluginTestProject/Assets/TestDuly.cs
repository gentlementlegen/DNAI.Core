using Duly.MoreOrLess;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestDuly : Play
{
    public Text TextNumber;

    public int Tries = 10;
    public int MysteryNumber = 42;

    public void Play()
    {
        Debug.Log("Has to find " + MysteryNumber);
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        lastResult = COMPARISON.NONE;
        for (int i = Tries; i > 0 && result != MysteryNumber; i--)
        {
            Execute();
            lastResult = result < MysteryNumber ? COMPARISON.MORE : COMPARISON.LESS;
            Debug.Log($"Result = {result}; MysteryNumber = {MysteryNumber}; lastResult = {lastResult}");
            TextNumber.text = result.ToString();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnMysteryNumberChanged(string nbr)
    {
        if (string.IsNullOrEmpty(nbr))
            return;
        MysteryNumber = Convert.ToInt32(nbr);
    }
}