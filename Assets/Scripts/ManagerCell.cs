using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCell : Singleton<ManagerCell>
{
    private readonly Queue<GameObject> _queueDestroyCell = new Queue<GameObject>();

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
            ActivateCellFrom(_queueDestroyCell);
            yield return new WaitUntil(() => _queueDestroyCell.Count < 1);
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