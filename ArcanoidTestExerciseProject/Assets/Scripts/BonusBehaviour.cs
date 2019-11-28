using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BallPhysics))]
public class BonusBehaviour : MonoBehaviour
{
    [SerializeField] private float _speedFactor = 2f;
    [SerializeField] private float _scaleFactor = 2f;
    [SerializeField] private float _bonusTime = 3f;

    private Bonus bonus;

    private SpriteRenderer spriteRenderer;
    private BallPhysics bonusPhysics;

    private GameObject newBall;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        bonusPhysics = gameObject.GetComponent<BallPhysics>();
    }

    void Start()
    {
        switch (bonus)      //установка цвета спрайта бонуса, в зависимости от типа
        {
            case Bonus.SpeedPlus:
                spriteRenderer.color = Color.yellow;
                break;
            case Bonus.SpeedMinus:
                spriteRenderer.color = Color.black;
                break;
            case Bonus.Ball:
                spriteRenderer.color = Color.green;
                break;
            case Bonus.Platform:
                spriteRenderer.color = Color.red;
                break;

        }
        bonusPhysics.HitEvent += BonusApply;
        bonusPhysics.HitEvent += DeathField;
        bonusPhysics.SetVelocity(9.8f, new Vector2(0, -1));
    }

    public void SetType(Bonus bonus)
    {
        this.bonus = bonus;
    }

    void BonusApply(RaycastHit hit)
    {
        if(hit.collider.tag == "Platform")      //если бонус пойман платформой, выполняется его применение
        {
            bonusPhysics.enabled = false;
            spriteRenderer.enabled = false;
            switch (bonus)
            {
                case Bonus.SpeedPlus:
                    {
                        StartCoroutine(SpeedPlus());
                        break;
                    }
                case Bonus.SpeedMinus:
                    {
                        StartCoroutine(SpeedMinus());
                        break;
                    }
                case Bonus.Ball:
                    {
                        StartCoroutine(Ball());
                        break;
                    }
                case Bonus.Platform:
                    {
                        StartCoroutine(Platform(hit));
                        break;
                    }

            }
        }
    }

    void DeathField(RaycastHit hit)
    {
        if(hit.collider.tag == "DeathField")
        {
            Destroy(gameObject);
        }
    }

    void BallDeathField(RaycastHit hit)
    {
        Destroy(newBall);
    }

    IEnumerator SpeedPlus()     //бонус ускорения
    {
        Debug.Log(_bonusTime);
        BallPhysics bf;
        bf = GameObject.FindGameObjectWithTag("Player").GetComponent<BallPhysics>();
        bf.SetVelocity(bf.GetVelocity() * _speedFactor);

        yield return new WaitForSeconds(_bonusTime);
        
        bf.SetVelocity(bf.GetVelocity() / _speedFactor);
        Destroy(gameObject);
    }

    IEnumerator SpeedMinus()    //"бонус" замедления
    {
        Debug.Log(_bonusTime);
        BallPhysics bf;
        bf = GameObject.FindGameObjectWithTag("Player").GetComponent<BallPhysics>();
        bf.SetVelocity(bf.GetVelocity() / _speedFactor);

        yield return new WaitForSeconds(_bonusTime);
        
        bf.SetVelocity(bf.GetVelocity() * _speedFactor);
        Destroy(gameObject);
    }

    IEnumerator Ball()      //бонусный шар
    {
        Debug.Log(_bonusTime);
        GameObject b;
        b = GameObject.FindGameObjectWithTag("Player");
        newBall = Instantiate(b, b.transform.position, new Quaternion());
        BallPhysics bf = newBall.GetComponent<BallPhysics>();
        bf.SetVelocity(b.GetComponent<BallPhysics>().GetVelocity().magnitude, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)));
        bf.HitEvent += Camera.main.GetComponent<GameManager>().HitBrick;
        bf.HitEvent += BallDeathField;

        yield return new WaitForSeconds(_bonusTime);

        Destroy(newBall);
        Destroy(gameObject);
    }

    IEnumerator Platform(RaycastHit hit)        //расширение платформы
    {
        Debug.Log(_bonusTime);
        hit.transform.localScale = new Vector3(hit.transform.localScale.x * _scaleFactor, hit.transform.localScale.y);
        hit.transform.GetChild(0).localScale = new Vector3(hit.transform.GetChild(0).localScale.x / _scaleFactor, hit.transform.GetChild(0).localScale.y);

        yield return new WaitForSeconds(_bonusTime);
        
        hit.transform.localScale = new Vector3(hit.transform.localScale.x / _scaleFactor, hit.transform.localScale.y);
        hit.transform.GetChild(0).localScale = new Vector3(hit.transform.GetChild(0).localScale.x * _scaleFactor, hit.transform.GetChild(0).localScale.y);
        Destroy(gameObject);
    }
}
