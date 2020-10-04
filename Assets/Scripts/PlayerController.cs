using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int currentLane;
    public List<float> laneXs = new List<float>();

    int lastLane = 0;

    public AnimationCurve laneChangeAnimation;
    public float animationLength;

    public float animationPos = 1f;
    public bool dead;

    public bool playing;

    public TMP_Text bubble1;
    public TMP_Text bubble2;
    public TMP_Text desc;

    bool canMove = true;

    private void Start()
    {
        lastLane = currentLane;
        if(PlayerPrefs.GetInt("Mode") == 0)
        {
            PlayerPrefs.SetInt("Mode", 1);
        }
    }

    void MoveAgain()
    {
        canMove = true;
    }

    private void Update()
    {
        float add = (animationLength * Time.deltaTime);
        print(add);
        animationPos += add;
        animationPos = Mathf.Clamp(animationPos, 0f, 1f);

        transform.position = new Vector3(Mathf.Lerp(laneXs[lastLane], laneXs[currentLane], laneChangeAnimation.Evaluate(animationPos)), transform.position.y, transform.position.z);


        if (true)
        {
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !dead)
            {
                lastLane = currentLane;
                currentLane -= 1;
                currentLane = Mathf.Clamp(currentLane, 0, laneXs.Count - 1);
                animationPos = 0f;
            }

            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !dead)
            {
                lastLane = currentLane;
                currentLane += 1;
                currentLane = Mathf.Clamp(currentLane, 0, laneXs.Count - 1);
                animationPos = 0f;
            }
        }

        if (!playing)
        {
            bubble1.fontSize = (PlayerPrefs.GetInt("Mode") == 1 ? 17.49f : 13.86f);
            bubble1.fontStyle = (PlayerPrefs.GetInt("Mode") == 1 ? FontStyles.Bold : FontStyles.Normal);
            bubble1.color = (PlayerPrefs.GetInt("Mode") == 1 ? new Color(1, 1, 1, 1) : new Color(0.53f, 0.53f, 0.53f, 1));
            bubble2.fontSize = (PlayerPrefs.GetInt("Mode") == 2 ? 17.49f : 13.86f);
            bubble2.fontStyle = (PlayerPrefs.GetInt("Mode") == 2 ? FontStyles.Bold : FontStyles.Normal);
            bubble2.color = (PlayerPrefs.GetInt("Mode") == 2 ? new Color(1, 1, 1, 1) : new Color(0.53f, 0.53f, 0.53f, 1));
            desc.text = (PlayerPrefs.GetInt("Mode") == 1 ? "Easy: Bigger spaces between rows" : "Hard: Smaller spaces between rows");

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayerPrefs.SetInt("Mode", 1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerPrefs.SetInt("Mode", 2);
            }
        }
    }
}
