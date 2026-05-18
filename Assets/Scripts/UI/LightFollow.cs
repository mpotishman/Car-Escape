using UnityEngine;

public class LightFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float swaySpeed = 1.5f;
    [SerializeField] private float swayAmount = 0.3f;

    private Vector3 offset;

    private void Start()
    {
        if (target == null)
        {
            return;
        }

        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPos = target.position + offset;
        targetPos.x += Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        targetPos.z += Mathf.Cos(Time.time * swaySpeed * 0.7f) * swayAmount;

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
