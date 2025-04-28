using UnityEngine;

public class RotatingLightEffect : MonoBehaviour
{
    public Transform target; // Nút hoặc đối tượng mà ánh sáng xoay quanh
    public float radius = 5f; // Bán kính quỹ đạo xoay
    public float speed = 20f; // Tốc độ xoay

    private float angle = 0f;

    void Update()
    {
        if (target != null)
        {
            // Cập nhật góc xoay (tăng góc theo tốc độ mỗi frame)
            angle += speed * Time.deltaTime;

            // Tính toán vị trí mới của ánh sáng theo quỹ đạo tròn
            float x = target.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float z = target.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            // Cập nhật vị trí ánh sáng
            transform.position = new Vector3(x, transform.position.y, z);
        }
    }
}
