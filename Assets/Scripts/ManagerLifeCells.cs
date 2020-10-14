using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLifeCells : Singleton<ManagerLifeCells>
{
    private readonly Queue<GameObject> _queueDestroyCell = new Queue<GameObject>();

    public bool isActivationCells = false;

    public void StepOn(ControllerColliderHit hit)
    {
        if (CheckCellByHexagon(hit))
        {
            var cell = GetCellByHexagon(hit);
            if (!_queueDestroyCell.Contains(cell))
                _queueDestroyCell.Enqueue(cell);
        }
    }

    private bool CheckCellByHexagon(ControllerColliderHit hit)
    {
        return GetCellByHexagon(hit)?.CompareTag("Cell_Floor") == true;
    }

    private GameObject GetCellByHexagon(ControllerColliderHit hit)
    {
        return hit?.rigidbody?.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ProcessWalking().GetEnumerator());
    }

    private IEnumerable ProcessWalking()
    {
        for (;;)
        {
            if (isActivationCells)
                ActivateCellFrom(_queueDestroyCell);
            yield return new WaitUntil(() => _queueDestroyCell.Count != 0);
        }
    }

    private void ActivateCellFrom(Queue<GameObject> queueCells)
    {
        while (queueCells.Count > 0)
        {
            Cell cell = null;
            var go = queueCells.Dequeue();
            if (go != null)
                if (go.TryGetComponent(out cell) == true)
                    cell.Activate();
        }
    }
}