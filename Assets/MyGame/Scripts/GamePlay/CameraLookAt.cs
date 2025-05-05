using UnityEngine;
using Unity.Cinemachine;
using StarterAssets;

public class CharacterAiming : MonoBehaviour
{
    private float turnSpeed;
    public Transform cameraLookAt;
    public AxisState xAxis;
    public AxisState yAxis;
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (DataManager.HasInstance)
        {
            turnSpeed = DataManager.Instance.GlobalConfig.turnSpeed;
        }
    }
    void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        //xAxis.m_MaxSpeed = 0;
        //yAxis.m_MaxSpeed = 0;
        yAxis.Update(Time.fixedDeltaTime);
        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}
