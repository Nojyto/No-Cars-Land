using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    private float shakeIntensity = 1f;
    private float shakeTime = 0.2f;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbcmp;

    private void Awake(){
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    private void Start(){
        StopShake();
    }
    public void ShakeCamera(){
        CinemachineBasicMultiChannelPerlin _cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = shakeIntensity;
        timer = shakeTime;
    }
    //     public void ShakeCamera(float carSpeed, float maxSpeed){
    //     float speedMultiplier = Mathf.Clamp01(carSpeed / maxSpeed); // maxSpeed should be defined in your CarController script
    //     float adjustedShakeIntensity = shakeIntensity * speedMultiplier;

    //     CinemachineBasicMultiChannelPerlin _cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    //     _cbmcp.m_AmplitudeGain = adjustedShakeIntensity;
    //     timer = shakeTime;
    // }
    public void StopShake(){
        CinemachineBasicMultiChannelPerlin _cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0f;
        timer = 0;
    }
    void Update(){
        if(timer>0){
            timer -= Time.deltaTime;
            if(timer<=0){
                StopShake();
            }
        }
    }
}
