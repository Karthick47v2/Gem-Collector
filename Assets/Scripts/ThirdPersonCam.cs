using UnityEngine;

public class ThirdPersonCam : MonoBehaviour{
    private float xRotation = 0f;
    private float yRotation = 0f;

    public Transform playerBody;
    public Vector3 playerBodyOffset;

    public float minXRotation = -90f;
    public float maxXRotation = 90f;
    public float mouseSensivity = 100f;

    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update(){
        MoveCamera();
    }

    private void LateUpdate(){
        FollowTarget();
    }

    private void FollowTarget(){
        if(playerBody != null){
            var targetPos = playerBody.position + playerBodyOffset;
            transform.position = targetPos;
        }
    }

    private void MoveCamera(){
        var mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}