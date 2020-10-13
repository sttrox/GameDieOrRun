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
            if (_queueDestroyCell.Count > 0)
            {
                S_Cell cell = null;
                if (_queueDestroyCell.Dequeue()?.TryGetComponent(out cell) == true)
                    cell.Activate();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}