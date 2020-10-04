using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public Transform counter;

    public LoopController lC;
    public SpawnController sC;
    public PlayerController pC;
    public PlayerKill pK;

    public List<TMP_Text> menuItems = new List<TMP_Text>();

    bool begun;

    private void Start()
    {
        FindObjectOfType<SongManager>().queued = SongPart.titleLoop;
    }

    private void Update()
    {
        if (!begun)
        {
            if((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
            {
                begun = true;
                sC.startDebug = true;
                counter.parent.GetComponent<Animator>().SetTrigger("In");
                pC.playing = true;
                pK.started = true;
                Invoke("GameIntro", 1f);
                
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        } else
        {
            foreach (TMP_Text item in menuItems)
            {
                item.color = Color.Lerp(item.color, new Color(1, 1, 1, 0), 4f * Time.deltaTime);
            }
        }
    }

    void GameIntro()
    {
        FindObjectOfType<SongManager>().queued = SongPart.gameIntro;
    }
}
