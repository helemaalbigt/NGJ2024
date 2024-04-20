using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPlayerMovement : MonoBehaviour {
    public float speed = 1f;
    public float mouseSensitivity = 100f;
    public Transform camera;
    public Transform rightHand;
    
    private float _xRotation = 0f;
    void FixedUpdate()
    {
#if UNITY_EDITOR
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        if(transform != null)
            transform.Rotate(Vector3.up * mouseX);
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        camera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) {
            movement += transform.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            movement -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A)) {
            movement -= transform.right;
        }
        if (Input.GetKey(KeyCode.D)) {
            movement += transform.right;
        }
        
        transform.Translate(movement * Time.unscaledDeltaTime * speed);
        rightHand.LookAt(camera.position + camera.forward * 1.5f);
#endif
    }
}
