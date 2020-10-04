using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    public float score;
    public TMP_Text text;

    private void Update()
    {
        text.text = score.ToString("F2") + "s";
    }

}
