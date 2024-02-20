using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRenderer : MonoBehaviour
{
    CarController carController;
    TrailRenderer trailRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        carController = GetComponentInParent<CarController>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(carController.IsTireScreeching(out float lateralVelocity, out bool isBraking)){
            trailRenderer.emitting = true;
        }
        else trailRenderer.emitting = false;
    }
}
