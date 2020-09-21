using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float _speed = 5;
    private Transform _transform;
    private Coroutine _moveCoroutine;

    private void Start()
    {
        _transform = transform;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.tag.Equals("Obstacle"))
                    return;

                MoveToPoint(hit.point);
            }
        }
    }

    public void MoveToPoint(Vector3 target)
    {
        if(_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        var fieldController = FieldController.Instance;
        Cell start = fieldController.GetCellByPosition(_transform.position);
        Cell finish = fieldController.GetCellByPosition(target);
        List<Cell> path = Pathfinder.FindPath(start, finish);

        _moveCoroutine = StartCoroutine(Move(path));
    }

    IEnumerator Move(List<Cell> path)
    {
        while (path.Count > 0)
        {
            Cell currentTargetCell = path[path.Count-1];
            Vector3 newPosition = new Vector3(currentTargetCell.Position.X, _transform.position.y, currentTargetCell.Position.Z);
            _transform.position = Vector3.MoveTowards(_transform.position, newPosition, Time.deltaTime*_speed);
            if (_transform.position == newPosition)
                path.Remove(currentTargetCell);
            yield return new WaitForFixedUpdate();
        }
    }

}
