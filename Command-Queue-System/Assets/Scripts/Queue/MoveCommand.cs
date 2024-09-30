using System.Collections;
using UnityEngine;

public class MoveCommand : ICommand
{
    private class CoroutineRunner : MonoBehaviour { }
    private CoroutineRunner coroutineRunnerWorker;
    private GameObject coroutineWorkerInstance;

    // this does not seem right, but this is the only way I could find to run a coroutine from within this class without branching out to another one.
    private CoroutineRunner coroutineRunner
    {
        get
        {
            if (coroutineRunnerWorker != null)
            {
                return coroutineRunnerWorker;
            }
            return InitCoroutineRunner();
        }

        set { }
    }

    private CoroutineRunner InitCoroutineRunner()
    {
        coroutineWorkerInstance = new GameObject();
        coroutineWorkerInstance.isStatic = true;
        coroutineRunnerWorker = coroutineWorkerInstance.AddComponent<CoroutineRunner>();
        return coroutineRunnerWorker;
    }

    private Vector3 endingLocation;
    private GameObject entity;
    private Vector3 direction;
    private bool isMoving = false;

    public delegate void ContinueExecution();
    public static ContinueExecution onEndOfCommand;

    public MoveCommand(GameObject entity, Vector3 direction)
    {
        this.entity = entity; // The player
        this.direction = direction;
    }

    public void Execute()
    {
        endingLocation = entity.transform.position + (direction * 2);
        isMoving = true;
        coroutineRunner.StartCoroutine(SmoothMove());
    }

    public IEnumerator SmoothMove()
    {
        while (isMoving)
        {
            // Slowly move to end position
            Vector3 distanceFrom = CheckDistanceFromEndLocation(entity.transform.position, endingLocation);
            Vector3 newPos = Vector3.Lerp(entity.transform.position, endingLocation, 2f / distanceFrom.magnitude * Time.deltaTime);
            entity.transform.position = newPos;

            // If the player is close enough to the end, snap to the tile position
            if (distanceFrom.magnitude <= 0.1f)
            {
                entity.transform.position = endingLocation;
                isMoving = false;
                onEndOfCommand.Invoke();
                GameObject.Destroy(coroutineWorkerInstance);
                // Destroy the gameObject instance that was used to run the coroutine
            }

            yield return new WaitForEndOfFrame();
        }

    }

    public Vector3 CheckDistanceFromEndLocation(Vector3 currentLocation, Vector3 endLocation)
    {
        return endLocation - currentLocation;
    }

}
