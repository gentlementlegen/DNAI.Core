using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DNAI.DigitRecognizer;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public class DigitGenerator : DigitRecognizer {

    private static readonly int NB_SAMPLE = 10;
    private static readonly int SAMPLE_SIZE = NB_SAMPLE * 10;

    private Texture2D[] digitImages = new Texture2D[SAMPLE_SIZE];

    [SerializeField]
    private int countWidth;

    [SerializeField]
    private int countHeight;

    [SerializeField]
    private Image displayedImageUI;

    [SerializeField]
    private Text displayedTextUI;

    [SerializeField]
    private Text displayedStats;

    private Texture2D send;

    private System.Random random = new System.Random((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

    private int GoodPredict { get; set; } = 0;

    private int BadPredict { get; set; } = 0;

    private bool AIRunning = false;

    public void Start()
    {
        send = new Texture2D(28, 28, TextureFormat.RGBA32, false);
        displayedImageUI.sprite = Sprite.Create(send, new Rect(0, 0, 28, 28), Vector2.zero);

        for (int i = 0; i < SAMPLE_SIZE; i++)
        {
            digitImages[i] = Resources.Load($"mnist/{i % 10}/img_ ({i / NB_SAMPLE + 1})", typeof(Texture2D)) as Texture2D;
        }
        displayedStats.text = "";
    }

    public void NewImage()
    {
        int id = random.Next(0, countWidth * countHeight);

        RecognizeImage(id);
    }

    public void FullEvaluation()
    {
        if (AIRunning)
        {
            AIRunning = false;
        }
        else
        {
            StartCoroutine(RunEvaluate());
        }
    }

    public void RecognizeImage(int id)
    {
        Debug.Log($"Index: {id}");

        //int xOffset = (id % countWidth) * 28;
        //int yOffset = (id / countWidth) * 28;

        pixels = (DenseMatrix)Matrix<double>.Build.Dense(28, 28);

        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                Color pix = digitImages[id].GetPixel(x, y);
                float val = /*1.0f - */((pix.r + pix.g + pix.b) / 3.0f);
                float inv_val = 1 - val;

                pixels[27 - y, x] = val;
                send.SetPixel(x, y, new Color(inv_val, inv_val, inv_val));
            }
        }

        ExecuterecognizeDigit();

        send.Apply();

        Debug.Log($"Result: {result}");
        Debug.Log($"Max Output: {maxOut}");
        Debug.Log($"Outputs: {results}");

        displayedTextUI.text = result.ToString();
    }

    public IEnumerator RunEvaluate()
    {
        AIRunning = true;
        for (int id = 0; id < SAMPLE_SIZE && AIRunning; id++)
        {
            RecognizeImage(id);

            if (result == id % NB_SAMPLE)
            {
                GoodPredict++;
            }
            else
            {
                BadPredict++;
            }

            displayedStats.text = $"Good: {GoodPredict} | Bad: {BadPredict}";

            yield return new WaitForSeconds(0);
        }
    }
}
