using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameInputs
{
    ButtonD = 0,
    ButtonA = 1,
    ButtonSpace = 2,
    ButtonLeftMouse = 2
}

public class InputHandler       //реализация паттерна "Команда" для расширения настроек управления
{
    private ICommand[] buttons; 

    #region Singleton
    private static InputHandler instance;

    private InputHandler()
    {
        buttons = new ICommand[3];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = new NoCommand();
        }
    }

    public static InputHandler GetInstance()
    {
        if (instance == null)
            instance = new InputHandler();
        return instance;
    }
    #endregion 

    public void SetCommand(int number, ICommand com)
    {
        buttons[number] = com;
    }

    public void PressButton(int number)
    {
        buttons[number].Execute();
    }

    public void PressUndoButton(int number)
    {
        buttons[number].Undo();
    }
}

public interface ICommand
{
    void Execute();
    void Undo();
}

internal class MoveRightCommand : ICommand
{
    PlatformController platformController;

    public MoveRightCommand(PlatformController pf)
    {
        platformController = pf;
    }

    public void Execute()
    {
        platformController.MoveRight();
    }

    public void Undo()
    {
        platformController.Stop();
    }
}

internal class MoveLeftCommand : ICommand
{
    PlatformController platformController;

    public MoveLeftCommand(PlatformController pf)
    {
        platformController = pf;
    }

    public void Execute()
    {
        platformController.MoveLeft();
    }

    public void Undo()
    {
        platformController.Stop();
    }
}

internal class StrikeCommand : ICommand
{
    PlatformController platformController;

    public StrikeCommand(PlatformController pf)
    {
        platformController = pf;
    }

    public void Execute()
    {
    }

    public void Undo()
    {
        platformController.Strike();
    }
}

internal class NoCommand : ICommand
{
    public void Execute() { }
    public void Undo() { }
}
