using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public bool IsSolid = true;

    private float _radius = 0f;

    private Vector2 velocity;

    private Camera mainCamera;
    
    private Ray ray;
    private RaycastHit hit;

    public delegate void HitHandler(RaycastHit hit);
    public event HitHandler HitEvent;

    void Start()
    {
        _radius = transform.localScale.x/2;
        mainCamera = Camera.main;
    }
    
    void FixedUpdate()
    {

        ray = mainCamera.ScreenPointToRay(mainCamera.WorldToScreenPoint(transform.position));       //проверяется пересечение сферы с коллайдерами
        Physics.SphereCast(ray, _radius, out hit);

        if (hit.collider != null)
        {
            HitEvent?.Invoke(hit);      //событие столкновения

            if (IsSolid)        //если сфера "твердая", рассчитываем направление отскока
            {
                Vector2 normal = new Vector2(Mathf.Round(hit.normal.x), Mathf.Round(hit.normal.y));
                SetVelocity(Vector2.Reflect(velocity, normal).normalized * velocity.magnitude);
            }
        }

        if (velocity.magnitude != 0)        //если вектор скорости не нулевой, двигаем сферу
            transform.Translate(velocity * Time.fixedDeltaTime);

    }

    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    public void SetVelocity(float x, float y)
    {
        velocity.x = x;
        velocity.y = y;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        this.velocity = direction.normalized * velocity;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }
    
}
