using Project.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour { 

    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    public Transform playerBody;

    [Header("Class References")]
    [SerializeField]
    private NetworkIdentity NetworkIdentity;

    // Start is called before the first frame update
    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    public void Update()
    {
        if (NetworkIdentity.IsControlling())
        {
            CheckCamera();
        }
        else
        {
            this.transform.gameObject.SetActive(false);
        }
    }

    private void CheckCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}



