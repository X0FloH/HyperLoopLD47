using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GoPastTest : MonoBehaviour
{
    public float normalCameraShakeAmplitude;
    public float goPastCameraShakeAmplitude;

    public CinemachineVirtualCamera vc;

    public float goPastLerpSpeed = 10f;
    public float returnLerpSpeed = 1f;

    public bool dead;

    public List<AudioClip> wooshes = new List<AudioClip>();

    List<Collider> colliders = new List<Collider>();

    bool startedGoPast;

    bool canWoosh = true;

    private void FixedUpdate()
    {
        if((colliders.Count > 0 || (vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain < goPastCameraShakeAmplitude-0.5f && startedGoPast)) && !dead)
        {
            startedGoPast = true;
            vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain, goPastCameraShakeAmplitude, goPastLerpSpeed * Time.deltaTime);
        } else
        {
            if (!dead)
            {
                startedGoPast = false;
                vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain, normalCameraShakeAmplitude, returnLerpSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Blocker")
        {
            colliders.Add(other);
            if (canWoosh)
            {
                GetComponent<AudioSource>().clip = wooshes[Random.Range(0, wooshes.Count)];
                GetComponent<AudioSource>().Play();
                canWoosh = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
        canWoosh = true;
    }
}
