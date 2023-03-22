using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToTurnOn = new List<GameObject>();
    public string deathText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            for (int i = 0; i < objectsToTurnOn.Count; i++)
            {
                objectsToTurnOn[i].SetActive(true);
            }
            collision.GetComponent<CharacterController2D>().OnDeath();
        }
        NarrationBox.narrationBoxSingleton.TextBox.SetActive(true);
        NarrationBox.narrationBoxSingleton.alreadyActivated = true;
        StartCoroutine(NarrationBox.narrationBoxSingleton.ShowText(deathText));
    }
}
