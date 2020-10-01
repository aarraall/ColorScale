using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public Camera cam;
    public CinemachineVirtualCamera cmVirtual;
    public GameObject target;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    

    private void Update()
    {
        if (target)
        {         
            cmVirtual.LookAt = target.transform;
            cmVirtual.Follow = target.transform;
        }
    }


}
