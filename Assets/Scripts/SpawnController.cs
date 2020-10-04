using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

public class SpawnController : MonoBehaviour
{
    public List<GameObject> patterns = new List<GameObject>();

    public int slalemLeftId;
    public int slalemRightId;

    public float minSpawnTimeHard = 3f;
    public float maxSpawnTimeHard = 5f;

    public float minSpawnTimeEasy = 7f;
    public float maxSpawnTimeEasy = 10f;

    public float minSpawnTime;
    public float maxSpawnTime;

    public Transform spawnTransform;
    public Transform eventSpawnTransform;
    public PlayerController pC;

    public float timeBetweenBeat = .729f;

    public GameObject eventViewPrefab;

    public GameObject pointPrefab;

    public bool startDebug;
    public bool stopDebug;

    public bool spawn;

    bool slalemLeft = true;

    bool pointSpawn;

    bool waitingForSpawn;

    public void StartSpawning()
    {
        spawn = true;
        minSpawnTime = (PlayerPrefs.GetInt("Mode") == 1) ? minSpawnTimeEasy : minSpawnTimeHard;
        maxSpawnTime = (PlayerPrefs.GetInt("Mode") == 1) ? maxSpawnTimeEasy : maxSpawnTimeHard;
        Spawn();
        StartCoroutine("Slalom");
    }

    

    IEnumerator Slalom()
    {
        yield return new WaitForSeconds(Random.Range(20f, 30f));
        spawn = false;
        yield return new WaitForSeconds(3f);
        GameObject eventView = Instantiate(eventViewPrefab, eventSpawnTransform.position, eventSpawnTransform.rotation, null);
        Destroy(eventView.GetComponent<ScoreHandler>());
        eventView.transform.parent = transform;
        eventView.transform.GetChild(0).GetComponent<TMP_Text>().text = "Slalom";
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < Random.Range(8, 14); i++)
        {
            GameObject chosenPattern = null;
            if (slalemLeft)
            {
                chosenPattern = patterns[slalemLeftId];
            } else
            {
                chosenPattern = patterns[slalemRightId];
            }
            slalemLeft = !slalemLeft;
            GameObject spawnedPattern = Instantiate(chosenPattern, spawnTransform.position, spawnTransform.rotation, null);
            spawnedPattern.transform.parent = transform;
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
        yield return new WaitForSeconds(2f);
        GameObject eventView2 = Instantiate(eventViewPrefab, eventSpawnTransform.position, eventSpawnTransform.rotation, null);
        Destroy(eventView2.GetComponent<ScoreHandler>());
        eventView2.transform.parent = transform;
        eventView2.transform.GetChild(0).GetComponent<TMP_Text>().text = "Done";
        eventView2.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(5f);
        spawn = true;
        StartSpawning();
    }

    public void Beat()
    {
        if (waitingForSpawn)
        {
            StartSpawning();
            waitingForSpawn = false;
        }
    }

    private void Update()
    {
        if (startDebug)
        {
            startDebug = false;
            waitingForSpawn = true;
        }
        if (stopDebug)
        {
            stopDebug = false;
            spawn = false;
        }

        
    }

    void Spawn()
    {
        if (!pointSpawn)
        {
            GameObject chosenPattern = patterns[Random.Range(0, patterns.Count - 1)];
            GameObject spawnedPattern = Instantiate(chosenPattern, spawnTransform.position, spawnTransform.rotation, null);
            spawnedPattern.transform.parent = transform;
        } else
        {
            //GameObject newPoint = Instantiate(pointPrefab, new Vector3(pC.laneXs[Random.Range(0, pC.laneXs.Count)], spawnTransform.position.y, spawnTransform.position.z), spawnTransform.rotation, null);
            //newPoint.transform.parent = transform;
        }


        pointSpawn = false;
        if (spawn)
        {
            Invoke("Spawn", Random.Range(minSpawnTime, maxSpawnTime));
        }
    }
}
