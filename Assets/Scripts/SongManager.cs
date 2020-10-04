using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SongPart
{
    none,
    titleLoop,
    gameIntro,
    gameLoop,
    slalomIntro,
    slalomLoop,
    slalomOutro,
    deathLoop
}


public class SongManager : MonoBehaviour
{
    public float timeBetweenBeats;

    public AudioClip titleLoop;
    public AudioClip gameIntro;
    public AudioClip gameLoop;
    public AudioClip slalomIntro;
    public AudioClip slalomLoop;
    public AudioClip slalomOutro;
    public AudioClip deathLoop;

    public bool beatDebug;

    public AudioSource player1;
    public AudioSource player2;

    public SpawnController sC;

    [SerializeField]
    public SongPart queued;

    bool created;
    bool fadeToggle;

    AudioClip currentClip;

    private void Start()
    {

        SongManager[] songManagers = FindObjectsOfType<SongManager>();
        for(int i = 0; i < songManagers.Length; i++)
        {
            if(songManagers[i] != this)
            {
                if(created == false)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
        }

        sC = FindObjectOfType<SpawnController>();
        Beat();
    }

    private void Update()
    {
        if (fadeToggle)
        {
            player1.volume = 1f;
            if (player1.volume > 0.9f)
            {
                player2.volume = 0f;
            }
        } else
        {
            player2.volume = 1f;
            if (player2.volume > 0.9f)
            {
                player1.volume = 0f;
            }
        }
    }

    public void ChangeClip(AudioClip clip)
    {
        fadeToggle = !fadeToggle;
        currentClip = clip;
        Invoke("Beat", clip.length);

        if (fadeToggle)
        {
            player1.clip = clip;
            player1.time = 0;
            player2.time = 0;
            player1.Play();
        } else
        {
            player2.clip = clip;
            player1.time = 0;
            player2.time = 0;
            player2.Play();
        }
    }

    void Beat()
    {
        sC = FindObjectOfType<SpawnController>();
        sC.Beat();
        beatDebug = !beatDebug;
        if (queued != SongPart.none)
        {
            switch (queued)
            {
                case SongPart.titleLoop:
                    ChangeClip(titleLoop);
                    queued = SongPart.none;
                    break;
                case SongPart.gameIntro:
                    ChangeClip(gameIntro);
                    Invoke("EndGameIntro", gameIntro.length);
                    break;
                case SongPart.gameLoop:
                    ChangeClip(gameLoop);
                    queued = SongPart.none;
                    break;
                case SongPart.slalomIntro:
                    ChangeClip(slalomIntro);
                    Invoke("EndSlalomIntro", slalomIntro.length);
                    queued = SongPart.none;
                    break;
                case SongPart.slalomLoop:
                    ChangeClip(slalomLoop);
                    queued = SongPart.none;
                    break;
                case SongPart.slalomOutro:
                    ChangeClip(slalomOutro);
                    Invoke("EndSlalomOutro", slalomOutro.length);
                    queued = SongPart.none;
                    break;
                case SongPart.deathLoop:
                    ChangeClip(deathLoop);
                    queued = SongPart.none;
                    break;
            }
            queued = SongPart.none;
        }
        else
        {
            Invoke("Beat", (currentClip.length >= 6f ? currentClip.length/4 : (currentClip.length > 3f ? currentClip.length/2 : currentClip.length)));
        }
    }

    void EndSlalomIntro()
    {
        ChangeClip(slalomLoop);
    }

    void EndSlalomOutro()
    {
        ChangeClip(gameLoop);
    }

    void EndGameIntro()
    {
        ChangeClip(gameLoop);
    }
}
