using UnityEngine;
using Unity.Cinemachine;
using StarterAssets;

public class CharacterAiming : MonoBehaviour
{
    private float turnSpeed;
    //private float defaultRecoil;
    //private float aimRecoil;
    public Transform cameraLookAt;
    public AxisState xAxis;
    public AxisState yAxis;
    public InputAxis Axis;
    //public bool isAiming;

    private Camera mainCamera;
    private Animator animator;
    //private int isAimingParam = Animator.StringToHash("IsAiming");

    private StarterAssetsInputs m_Input;
    private void Awake()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        m_Input = GetComponent<StarterAssetsInputs>();
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (DataManager.HasInstance)
        {
            turnSpeed = DataManager.Instance.GlobalConfig.turnSpeed;
            //defaultRecoil = DataManager.Instance.GlobalConfig.defaultRecoil;
            //aimRecoil = DataManager.Instance.GlobalConfig.aimRecoil;
        }
    }

    private void Update()
    {
        //var weapon = activeWeapon.GetActiveWeapon();
        //if (weapon)
        //{
        //    if (activeWeapon.canFire)
        //    {
        //        isAiming = Input.GetMouseButton(1);
        //        animator.SetBool(isAimingParam, isAiming);
        //        weapon.weaponRecoil.recoilModifier = isAiming ? aimRecoil : defaultRecoil;
        //    }
        //}
    }

    void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);
        Axis.UpdateRecentering(Time.fixedDeltaTime, true);
        //Debug.Log($"Axis : {Axis.Value}");
        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        //cameraLookAt.eulerAngles = new Vector3(m_Input.look.x, m_Input.look.y, 0);
        
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        //Debug.Log(yawCamera);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}
