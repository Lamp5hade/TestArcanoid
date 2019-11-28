using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class BrickBehaviour : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private int _hP = 1;

     private Animator anim;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        switch (_hP)        //установка цвета от количества хп
        {
            case 1:
                spriteRenderer.color = Color.blue;
                break;
            case 2:
                spriteRenderer.color = Color.yellow;
                break;
            case 3:
                spriteRenderer.color = Color.red;
                break;
            case 4:
                spriteRenderer.color = Color.green;
                break;
            case 5:
                spriteRenderer.color = Color.cyan;
                break;
            case 6:
                spriteRenderer.color = Color.magenta;
                break;
            default:
                spriteRenderer.color = Color.white;
                break;

        }
            

        if (_hP <= 0)
            DestroyBrick();
    }

    public void Hit()
    {
        --_hP;
    }

    void DestroyBrick()     //если хп 0 или меньше, играем анимацию и уничтожаем сферу
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;

        StartCoroutine(DestroingAnimation());
    }

    IEnumerator DestroingAnimation()
    {
        anim?.SetTrigger("Destroy");

        yield return new WaitForSeconds(0.9f);

        Destroy(gameObject);
    }
}
