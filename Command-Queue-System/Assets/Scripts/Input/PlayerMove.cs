
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public List<ICommand> commandQueue;
    public bool isBusy;
    public int commandIndex;

    public delegate void AddIcon(Vector2 direction);
    public static AddIcon addIcon;

    public delegate void RemoveIcon();
    public static RemoveIcon removeIcon;

    private void Start()
    {
        MoveCommand.onEndOfCommand = ContinueExecution;
        commandQueue = new List<ICommand>();
    }

    public void InitMoveCommand(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && ctx.ReadValue<Vector2>() != null)
        {
            // Add command into queue using the direction of the input
            Vector2 direction = ctx.ReadValue<Vector2>();
            AddCommand(new MoveCommand(gameObject, direction), direction);
            return;
        }

    }

    public void InitAttackCommand(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // Add attack command into queue
            AddCommand(new AttackCommand(), Vector2.zero);
            return;
        }
    }

    public void RemoveCommand(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // Remove last command from the list
            removeIcon.Invoke();
            commandQueue.Remove(commandQueue.Last());
            return;
        }
    }

    public void AddCommand(ICommand command, Vector2 direction)
    {
        addIcon.Invoke(direction);
        commandQueue.Add(command);
    }

    public void StartExecution()
    {
        if (commandQueue.Count == 0) return;
        commandQueue[0].Execute();
        isBusy = true;
    }

    public void ContinueExecution()
    {
        commandIndex++;
        if (!BoundaryHandler.IsPlayerPositionValid(transform.position))
        {
            BoundaryHandler.OutOfBounds();
            return;
            // if the player ends the movement method not on a tile, the player loses the game
        }
        if (commandIndex == commandQueue.Count)
        {
            EndExecution();
            return;
        }
        commandQueue[commandIndex].Execute();
    }
    public void EndExecution()
    {
        if (GameMode.isOnWinTile)
        {
            BoundaryHandler.EndOnWinTile();
            return;
        }
        BoundaryHandler.EndOnNormalTile();
    }

}
