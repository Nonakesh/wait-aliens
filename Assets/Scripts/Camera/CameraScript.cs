using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform cameraObject;
    
    [SerializeField]
    private Vector3 minPosition;
    [SerializeField]
    private Vector3 maxPosition;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float boostedSpeed;
    
    [SerializeField]
    private float rotationSpeed;
    
    [SerializeField]
    private float scrollSpeed;
    
    [Header("Mouse")]
    [SerializeField]
    private bool mouseMovementEnabled;
    [SerializeField]
    private int mouseBorderWidth = 50;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 velocity = new Vector3(horizontal, 0, vertical);
        if(velocity == Vector3.zero && mouseMovementEnabled)
        {
            float mouseHorizontal = GetMouseHorizontal();
            float mouseVertical = GetMouseVertical();
            velocity = new Vector3(mouseHorizontal, 0, mouseVertical);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocity *= boostedSpeed;
        }
        else
        {
            velocity *= speed;
        }

        velocity *= Time.unscaledDeltaTime;
        velocity = transform.forward * velocity.z + transform.right * velocity.x;
        
        Vector3 total = ClampVector(transform.position + velocity, minPosition, maxPosition);
        transform.position = total;

        if (Input.GetMouseButton(2))
        {
            var rotation = Input.GetAxis("Mouse X");
            
            transform.rotation *= Quaternion.Euler(0, rotation * rotationSpeed * Time.unscaledDeltaTime, 0);
        }
    }

    private Vector3 ClampVector(Vector3 value, Vector3 min, Vector3 max)
    {
        value.x = Mathf.Clamp(value.x, min.x, max.x);
        value.y = Mathf.Clamp(value.y, min.y, max.y);
        value.z = Mathf.Clamp(value.z, min.z, max.z);
        return value;
    }

    private float GetMouseHorizontal()
    {
        Vector3 mouse = Input.mousePosition;

        if (mouse.x < mouseBorderWidth)
            return Mathf.Lerp(-1, 0, Mathf.InverseLerp(0, mouseBorderWidth, mouse.x));
        if (mouse.x > Screen.width - mouseBorderWidth)
            return Mathf.Lerp(0, 1, Mathf.InverseLerp(Screen.width - mouseBorderWidth, Screen.width, mouse.x));
        return 0;
    }

    private float GetMouseVertical()
    {
        Vector3 mouse = Input.mousePosition;

        if (mouse.y < mouseBorderWidth)
            return Mathf.Lerp(-1, 0, Mathf.InverseLerp(0, mouseBorderWidth, mouse.y));
        if (mouse.y > Screen.height - mouseBorderWidth)
            return Mathf.Lerp(0, 1, Mathf.InverseLerp(Screen.height - mouseBorderWidth, Screen.height, mouse.y));
        return 0;
    }
}
