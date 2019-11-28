using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Bonus
{
    SpeedPlus = 0,
    SpeedMinus = 1,
    Platform = 2,
    Ball = 3
}

public class GameManager : MonoBehaviour
{
    private static int CurrentLevel = 1;

    //[SerializeField] private int _ballHP;
    [SerializeField] private bool _isBonus = false;

    [Range(0, 25)]                                                  //Шансы выпадения разных бонусов
    [SerializeField] private int _speedPlusBonusChance = 5;
    [Range(0, 25)]
    [SerializeField] private int _speedMinusBonusChance = 5;
    [Range(0, 25)]
    [SerializeField] private int _platformBonusChance = 5;
    [Range(0, 25)]
    [SerializeField] private int _ballBonusChance = 5;

    [SerializeField] private GameObject bonus;
    private GameObject ball;

    private void Awake()
    {
        ball = GameObject.FindGameObjectWithTag("Player");
        ball.GetComponent<BallPhysics>().HitEvent += HitBrick;      //Подписываемся на события физики шара
        ball.GetComponent<BallPhysics>().HitEvent += DeathField;
    }
    
    public void HitBrick(RaycastHit hit)
    {
        if (hit.collider.tag == "Brick")
        {
            hit.collider.GetComponent<BrickBehaviour>().Hit();

            if (_isBonus)                   //рассчет вероятности если на уровне включены бонусы
            {
                int chance = Random.Range(1, 101);

                if (chance >= 1 && chance < _speedPlusBonusChance) SpawnBonus(hit.transform, Bonus.SpeedPlus);
                else if (chance >= 25 && chance < 25 + _speedMinusBonusChance) SpawnBonus(hit.transform, Bonus.SpeedMinus);
                else if (chance >= 50 && chance < 50 + _platformBonusChance) SpawnBonus(hit.transform, Bonus.Platform);
                else if (chance >= 75 && chance < 75 + _ballBonusChance) SpawnBonus(hit.transform, Bonus.Ball);
            }
        }

        if (GameObject.FindGameObjectWithTag("Brick") == null)  //условие перехода на следующий уровень или завершения игры
        {
            if (CurrentLevel <= 3) 
                Application.LoadLevel(CurrentLevel++);
            else
                Application.Quit();
        }
    }

    void DeathField(RaycastHit hit)
    {
        if (hit.collider.tag == "DeathField")
        {
            Respawn();
        }
    }

    void Respawn()                      //если шар падает, его можно снова запустить
    {
        GameObject platform = GameObject.FindGameObjectWithTag("Platform");
        platform.GetComponent<PlatformController>().IsStriked = false;
        ball.SetActive(false);
        platform.transform.GetChild(0).gameObject.SetActive(true);
    }

    

    void SpawnBonus(Transform spawnTransform, Bonus bonus)      //выпадение бонуса из кирпича при ударе
    {
        GameObject go = Instantiate(this.bonus, spawnTransform.position, new Quaternion());
        go.GetComponent<BonusBehaviour>().SetType(bonus);
    }
}
