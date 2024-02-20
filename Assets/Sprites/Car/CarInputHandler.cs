using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    CarController  carController;
    private bool braking = false;
    private bool turbo = false;
    // Start is called before the first frame update
    void Awake()
    {
        carController = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        if(Input.GetKey(KeyCode.Space)){
            Debug.Log("Brake");
            braking = true;
        }
        else braking = false;
        if(Input.GetKey(KeyCode.LeftShift)){
            Debug.Log("Turbo");
            turbo = true;
        }
        else turbo = false;
        carController.SetInputVector(inputVector, braking, turbo);
        
        
    }
}
