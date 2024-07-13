using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float rotateSpeed = 5f;
    [SerializeField]
    Item type;
    [SerializeField]
    AudioClip collectSound;
    [SerializeField]
    float value;

    void Collect()
    {
        //play audio
        //add value according to type
        if (collectSound != null)
            SoundManager.instance.PlaySound(collectSound);

        switch (type)
        {
            case Item.Coin: HUD_Manager.instance.AddCoins(); break;
            case Item.Gem: HUD_Manager.instance.AddGem(); break;
            default: Debug.Log("COLLECTED: " + type); break;
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boat")
            Collect();

        if (other.tag == "Obstacle" && other.GetComponent<Obstacle>() != null)
            Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.fixedDeltaTime);
    }

}


public enum Item
{
    Coin, 
    Shield,
    Health,
    Junk,
    Gem
}
