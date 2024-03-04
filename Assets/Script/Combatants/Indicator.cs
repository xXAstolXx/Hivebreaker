using UnityEngine;

public class Indicator : MonoBehaviour
{

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void RotateTowards(Vector3 position)
    {
        if (position == Vector3.zero)
        {
            transform.rotation.eulerAngles.Set(0.0f, 0.0f, 0.0f);
        }
        else
        {
            position.y = 1.0f;
            transform.LookAt(position);
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f);
        }
    }
}
