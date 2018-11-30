using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DNAI.DigitRecognizer;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Threading.Tasks;

public class DigitGenerator : DigitRecognizer
{

    private static readonly int NB_SAMPLE = 10;
    private static readonly int SAMPLE_SIZE = NB_SAMPLE * 10;

    private Texture2D[] digitImages = new Texture2D[SAMPLE_SIZE];

    [SerializeField]
    private Image displayedImageUI;

    [SerializeField]
    private Text displayedTextUI;

    [SerializeField]
    private Image loadImage;

    private Texture2D send;

    private System.Random random = new System.Random((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

    private Task RecognitionTask { get; set; }

    private Action OnResultChanged = null;

    public void Start()
    {
        send = new Texture2D(28, 28, TextureFormat.RGBA32, false);
        displayedImageUI.sprite = Sprite.Create(send, new Rect(0, 0, 28, 28), Vector2.zero);

        for (int i = 0; i < SAMPLE_SIZE; i++)
        {
            digitImages[i] = Resources.Load($"mnist/{i % 10}/img_ ({i / NB_SAMPLE + 1})", typeof(Texture2D)) as Texture2D;
        }
        loadImage.enabled = false;
    }

    public void Update()
    {
        if (OnResultChanged != null)
        {
            OnResultChanged();
            OnResultChanged = null;
        }
    }

    public void NewImage()
    {
        int id = random.Next(0, SAMPLE_SIZE);

        RecognizeImage(digitImages[id]);
    }

    public void OnImageRecognized()
    {
        displayedTextUI.text = result.ToString();
        loadImage.enabled = false;
    }

    private void RecognizeImage(Texture2D image)
    {
        if (RecognitionTask == null || RecognitionTask.IsCompleted)
        {
            pixels = (DenseMatrix)Matrix<double>.Build.Dense(28, 28);

            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    Color pix = image.GetPixel(x, y);
                    float val = ((pix.r + pix.g + pix.b) / 3.0f);
                    float inv_val = 1 - val;

                    pixels[27 - y, x] = val;
                    send.SetPixel(x, y, new Color(inv_val, inv_val, inv_val));
                }
            }

            send.Apply();
            displayedTextUI.text = "";
            loadImage.enabled = true;

            RecognitionTask = Task.Run(() =>
            {
                ExecuterecognizeDigit();

                OnResultChanged = () => OnImageRecognized();
            }).ContinueWith(t =>
            {
                Debug.Log($"Task status = {t.Status} : {t.Exception.InnerException.Message}");
            });
        }
    }
}
