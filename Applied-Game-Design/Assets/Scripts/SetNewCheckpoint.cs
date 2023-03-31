using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetNewCheckpoint : MonoBehaviour
{
    [SerializeField] private CharacterController2D controller;
    private bool hasBeenReached = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!hasBeenReached && collision.CompareTag("Player"))
        {
            controller.startingPosition = this.transform;
            hasBeenReached = true;
        }
    }
}
