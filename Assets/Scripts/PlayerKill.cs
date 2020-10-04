using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerKill : MonoBehaviour
{
    public LoopController lC;
    public SpawnController sC;
    public PlayerController pC;
    public CinemachineVirtualCamera vc;
    public ScoreHandler sH;
    public GoPastTest gPT;
    public Material gridM;

    public bool started;

    public TMP_Text scoreShowText;

    public List<TMP_Text> deathScreen = new List<TMP_Text>();
    public TMP_Text deathScore;

    public float throwForce;
    public Material blockerMaterial;
    public Material pointMaterial;
    public Material distortionMaterial;

    public AudioClip impactSound;

    public FadeCamera fC;

    bool dead;
    bool red;

    bool canRestart;
    bool restarting;

    void CanRestart()
    {
        canRestart = true;
    }

    private void Update()
    {
        if(!dead && started) { 
            sH.score += Time.deltaTime;
        }

        RaycastHit hit;
        Debug.DrawLine(transform.position, transform.position + (transform.forward*3f), Color.red);
        if(Physics.Raycast(transform.position, transform.forward, out hit, 3f))
        {
            GameObject other = hit.transform.gameObject;
            if(other.tag == "Point" && !dead)
            {
                Destroy(other.gameObject);
                sH.score += 1;
            }
            if (other.tag == "Blocker" && !dead)
            {
                /*
                other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                other.gameObject.GetComponent<Rigidbody>().useGravity = true;
                other.gameObject.GetComponent<Collider>().isTrigger = false;
                other.transform.parent = null;
                other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0.2f, 1f) * throwForce, ForceMode.Impulse);
                */
                pC.dead = true;
                lC.speedMult = 0f;
                sC.stopDebug = true;
                gPT.dead = true;
                gameObject.GetComponent<AudioSource>().Play();
                distortionMaterial.SetFloat("OffsetVal", 1000f);
                distortionMaterial.SetFloat("AlphaVal", 1f);
                FindObjectOfType<SongManager>().queued = SongPart.deathLoop;
                dead = true;
                red = true;
                deathScore.text = (sH.score).ToString("F2") + "s";
                Invoke("CanRestart", 2f);
                vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 2f;
            }
        }
        if (dead)
        {
            scoreShowText.color = Color.Lerp(scoreShowText.color, new Color(1, 1, 1, 0), 4f * Time.deltaTime);
            blockerMaterial.SetColor("ColorVariable", Color.Lerp(blockerMaterial.GetColor("ColorVariable"), new Color(blockerMaterial.GetColor("ColorVariable").r, blockerMaterial.GetColor("ColorVariable").g, blockerMaterial.GetColor("ColorVariable").b, 0), 8f * Time.deltaTime));
            pointMaterial.SetColor("ColorVariable", Color.Lerp(pointMaterial.GetColor("ColorVariable"), new Color(pointMaterial.GetColor("ColorVariable").r, pointMaterial.GetColor("ColorVariable").g, pointMaterial.GetColor("ColorVariable").b, 0), 8f * Time.deltaTime));
            distortionMaterial.SetFloat("OffsetVal", Mathf.Lerp(distortionMaterial.GetFloat("OffsetVal"), 0f, 1f * Time.deltaTime));
            distortionMaterial.SetFloat("AlphaVal", Mathf.Lerp(distortionMaterial.GetFloat("AlphaVal"), 0f, 1f * Time.deltaTime));
            if (canRestart)
            {
                foreach (TMP_Text text in deathScreen)
                {
                    text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 1f), 4f * Time.deltaTime);
                }
                if((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !restarting)
                {
                    fC.Reverse();
                    restarting = true;
                    Invoke("RestartNow", 1f);
                }
            }
        } else
        {
            blockerMaterial.SetColor("ColorVariable", Color.Lerp(blockerMaterial.GetColor("ColorVariable"), new Color(blockerMaterial.GetColor("ColorVariable").r, blockerMaterial.GetColor("ColorVariable").g, blockerMaterial.GetColor("ColorVariable").b, 1), 20f * Time.deltaTime));
            pointMaterial.SetColor("ColorVariable", Color.Lerp(pointMaterial.GetColor("ColorVariable"), new Color(pointMaterial.GetColor("ColorVariable").r, pointMaterial.GetColor("ColorVariable").g, pointMaterial.GetColor("ColorVariable").b, 1), 20f * Time.deltaTime));
            distortionMaterial.SetFloat("OffsetVal", Mathf.Lerp(distortionMaterial.GetFloat("OffsetVal"), 0f, 100f * Time.deltaTime));
        }
            
        if(red == true)
        {
            gridM.SetFloat("DeathAmount", Mathf.Lerp(gridM.GetFloat("DeathAmount"), 1f, 1f * Time.deltaTime));
        } else
        {
            gridM.SetFloat("DeathAmount", 0f);
        }
        if (dead)
        {
            vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain, 0f, 0.4f * Time.deltaTime);
        }
    }

    void RestartNow()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
