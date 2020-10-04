using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopController : MonoBehaviour
{
    public Transform loop;
    public Transform stars;

    public float speedMult = 1f;
    public float loopSpeed = 4f;
    public float starsSpeed = 1f;

    public float accelerateSpeed = 2f;

    public bool spin;

    public float currentSpeed = 0f;

    private void Update()
    {
        loop.Rotate(0f, 0f, -loopSpeed * Time.deltaTime * currentSpeed);
        stars.Rotate(starsSpeed * Time.deltaTime * currentSpeed, 0f, 0f);

        if (spin)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speedMult, accelerateSpeed * Time.deltaTime);
        } else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, accelerateSpeed * Time.deltaTime);
        }
    }
}
