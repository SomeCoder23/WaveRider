using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    float timer;

    void Start()
    {
        this.gameObject.SetActive(false);
    }


    public void Activate(float time)
    {
        timer = time;
        this.gameObject.SetActive(true);
        StartCoroutine(ActivateShield());
    }

    IEnumerator ActivateShield()
    {
        float elapsedTime = 0;
        float countdownValue = 0.1f;
        while(elapsedTime < timer)
        {
            elapsedTime += countdownValue;
            HUD_Manager.instance.UpdateShieldCountdown(countdownValue);
            yield return new WaitForSeconds(countdownValue);
        }

        gameObject.SetActive(false);
        Boat.instance.DisableShield();
        StopCoroutine(ActivateShield());
    }

}
