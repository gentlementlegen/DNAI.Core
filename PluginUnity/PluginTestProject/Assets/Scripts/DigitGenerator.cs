using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DNAI.DigitRecognizer;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public class DigitGenerator : DigitRecognizer {

    [SerializeField]
    private Texture2D digitImages;

    [SerializeField]
    private int countWidth;

    [SerializeField]
    private int countHeight;

    [SerializeField]
    private Image displayedImageUI;

    [SerializeField]
    private Text displayedTextUI;

    private System.Random random = new System.Random((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

    public void NewImage()
    {
        int id = random.Next(0, countWidth * countHeight);

        int xOffset = (id % countWidth) * 28;
        int yOffset = (id / countWidth) * 28;

        displayedImageUI.sprite = Sprite.Create(digitImages, new Rect { x = xOffset, y = yOffset, width = 28, height = 28 }, new Vector2(0, 0));

        pixels = (DenseMatrix)Matrix<double>.Build.Dense(28, 28);

        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                Color pix = digitImages.GetPixel(x + xOffset, y + yOffset);
                double val = 1.0 - ((pix.r + pix.g + pix.b) / 3.0);

                pixels[y, x] = val;

                Debug.Log($"Pix[{y}, {x}] = ({pix.r}, {pix.g}, {pix.b}) = {val}");
            }
        }

        ExecuterecognizeDigit();

        Debug.Log($"Result: {result}");
        Debug.Log($"Max Output: {maxOut}");
        Debug.Log($"Outputs: {results}");

        displayedTextUI.text = result.ToString();
    }
}
