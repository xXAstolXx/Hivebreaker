using UnityEngine;

public class MovingDamageArea : MonoBehaviour
{
    private float speed = 2.0f;

    private float returnValue = 1.0f;

    private float amplitude = 2.0f;

    private void Awake()
    {
        Move();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        speed -= returnValue * Time.deltaTime;
        if (speed <= -amplitude || speed >= amplitude) 
        {
            returnValue *= -1;
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
