using System.Collections;
using UnityEngine;

public class MoveCommand : ICommand
{
    private class CoroutineRunner : MonoBehaviour { }
    private CoroutineRunner coroutineRunnerWorker;
    private GameObject coroutineWorkerInstance;
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
        // Gameobject is not instantiated so it's not in the scene. It's just an object in memory.
        coroutineWorkerInstance = new GameObject();
        coroutineWorkerInstance.isStatic = true;
        coroutineRunnerWorker = coroutineWorkerInstance.AddComponent<CoroutineRunner>();
        return coroutineRunnerWorker;
    }
    // this does not seem right, but this is the only way I know of to run a coroutine from within this class without branching out to another one.

    private Vector3 endingLocation;
    private GameObject entity;
    private Vector3 direction;
    private bool isMoving = false;

    public delegate void ContinueExecution();
    public static ContinueExecution onEndOfCommand;

    public MoveCommand(GameObject entity, Vector3 direction)
    {
        this.entity = entity;
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
            Vector3 distanceFrom = CheckDistanceFromEndLocation(entity.transform.position, endingLocation);
            Vector3 newPos = Vector3.Lerp(entity.transform.position, endingLocation, 2f / distanceFrom.magnitude * Time.deltaTime);
            entity.transform.position = newPos;

            if (distanceFrom.magnitude <= 0.1f)
            {
                entity.transform.position = endingLocation;
                isMoving = false;
                onEndOfCommand.Invoke();
                GameObject.Destroy(coroutineWorkerInstance);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public Vector3 CheckDistanceFromEndLocation(Vector3 currentLocation, Vector3 endLocation)
    {
        return endLocation - currentLocation;
    }

}
