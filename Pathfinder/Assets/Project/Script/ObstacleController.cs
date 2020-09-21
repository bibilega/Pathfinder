using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public List<Obstacle> Obstacles = new List<Obstacle>();

    [SerializeField] private GameObject _obstaclePrototype;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (!hit.transform.tag.Equals("Ground"))
                    return;

                var newPosition = new Vector3((int)hit.point.x, 0.5f, (int)hit.point.z);
                var newObstacle = Instantiate(_obstaclePrototype, newPosition, _obstaclePrototype.transform.rotation, transform);
                Obstacles.Add(newObstacle.GetComponent<Obstacle>());
                FieldController.Instance.UpdateObstacles();
            }
        }

        if(Input.GetKeyDown(KeyCode.Mouse2))
        {
            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (!hit.transform.tag.Equals("Obstacle"))
                    return;


                var obstacle = hit.transform.GetComponent<Obstacle>();
                FieldController.Instance.RemoveObstacle(obstacle);
                Destroy(hit.transform.gameObject);
            }
        }
    }
}
