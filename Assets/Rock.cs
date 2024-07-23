using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Obstacle
{
    public AudioClip[] hitSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Boat>() != null)
        {
            if (!other.GetComponent<Boat>().isProtected())
            {
                HUD_Manager.instance.LoseGame();
            }
            else
            {
                SoundManager.instance.PlayRandom(hitSound);
                Destroy(this.gameObject);
            }

            //player can resume if they pay gems.
        }
    }
}
