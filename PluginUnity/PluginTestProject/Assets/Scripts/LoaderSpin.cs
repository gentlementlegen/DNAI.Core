using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderSpin : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        while (true)
        {
            transform.Rotate(0f, 0f, -45f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
