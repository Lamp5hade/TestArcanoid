using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private float _platformSpeed = 5f;
    [SerializeField] private float _ballSpeed = 5f;
    [SerializeField] private GameObject ball;

    public bool IsStriked { get; set; } = false;

    private BallPhysics ballPhysics;

    private Vector2 direction;
    private Vector2 ballDestination;
    private Transform spawn;

    private Camera mainCamera;

    private InputHandler inputHandler;

    private void Awake()
    {
        inputHandler = InputHandler.GetInstance();

        if(ball == null)
            ball = GameObject.FindGameObjectWithTag("Player");
        if (ball.GetComponent<BallPhysics>() == null)
            ball.AddComponent<BallPhysics>();
        ballPhysics = ball.GetComponent<BallPhysics>();

        mainCamera = Camera.main;
        spawn = transform.GetChild(0);
    }

    void Start()
    {
        ball.SetActive(false);

        inputHandler.SetCommand((int)GameInputs.ButtonD, new MoveRightCommand(this));       //привязка кнопок к командам
        inputHandler.SetCommand((int)GameInputs.ButtonA, new MoveLeftCommand(this));
        inputHandler.SetCommand((int)GameInputs.ButtonSpace, new StrikeCommand(this));
        inputHandler.SetCommand((int)GameInputs.ButtonLeftMouse, new StrikeCommand(this));
    }

    void Update()
    {
        if (!IsStriked)
        {
            ballDestination = mainCamera.ScreenToWorldPoint(Input.mousePosition) - spawn.position;      //если шар еще не отпущен, направление выбирается к курсору мыши
        }

        if (Input.GetKey(KeyCode.D)) inputHandler.PressButton((int)GameInputs.ButtonD);
        else if (Input.GetKeyUp(KeyCode.D)) inputHandler.PressUndoButton((int)GameInputs.ButtonD);

        else if (Input.GetKey(KeyCode.A)) inputHandler.PressButton((int)GameInputs.ButtonA);
        else if (Input.GetKeyUp(KeyCode.A)) inputHandler.PressUndoButton((int)GameInputs.ButtonA);

        else if (Input.GetKey(KeyCode.Space)) inputHandler.PressButton((int)GameInputs.ButtonSpace);
        else if (Input.GetKeyUp(KeyCode.Space)) inputHandler.PressUndoButton((int)GameInputs.ButtonSpace);

        else if (Input.GetMouseButton(0)) inputHandler.PressButton((int)GameInputs.ButtonLeftMouse);
        else if (Input.GetMouseButtonUp(0)) inputHandler.PressUndoButton((int)GameInputs.ButtonLeftMouse);
    }

    void LateUpdate()
    {
        if (direction != Vector2.zero)      //движение платформы
        {
            transform.Translate(direction * _platformSpeed * Time.deltaTime);
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8.5f + transform.localScale.x, 8.5f - transform.localScale.x), transform.position.y);
        }
    }

    public void MoveRight()
    {
        direction = new Vector2(1, 0);
    }

    public void MoveLeft()
    {
        direction = new Vector2(-1, 0);
    }

    public void Strike()        //выстрел шариком
    {
        if (!IsStriked)
        {
            ball.transform.position = spawn.position;
            spawn.gameObject.SetActive(false);
            ball.SetActive(true);
            IsStriked = true;
            ballPhysics.SetVelocity(_ballSpeed, ballDestination);
        }
    }

    public void Stop()
    {
        direction = new Vector2(0, 0);
    }
}
