using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Vector3 newTrackedBodyOffset;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            virtualCamera.GetComponentInChildren<CinemachineFramingTransposer>().m_TrackedObjectOffset = newTrackedBodyOffset;
        }
    }
}
