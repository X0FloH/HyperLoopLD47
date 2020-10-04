using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Killer : MonoBehaviour
{
    public ScoreHandler sH;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Blocker")
        {
            Destroy(other.gameObject);
        }
        if(other.tag == "Score")
        {
            if (other.transform.parent.GetChild(0).GetComponent<TMP_Text>().text == "Slalom")
            {
                FindObjectOfType<SongManager>().queued = SongPart.slalomIntro;
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                if (other.transform.parent.GetChild(0).GetComponent<TMP_Text>().text == "Done")
                {
                    FindObjectOfType<SongManager>().queued = SongPart.slalomOutro;
                    Destroy(other.transform.parent.gameObject);
                } else
                {
                    sH.text.text = sH.score.ToString();
                }
            }
        }
    }
}
