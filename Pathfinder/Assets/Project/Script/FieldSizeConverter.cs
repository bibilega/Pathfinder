using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSizeConverter : MonoBehaviour
{
    void Start()
    {
        var size = GetComponent<MeshFilter>().sharedMesh.bounds.size;

        var startX = transform.position.x - size.x / 2f +1;
        var startZ = transform.position.z + size.z / 2f -1;
        var startFieldPoit = new Int2((int)startX, (int)startZ);

        var finishX = transform.position.x + size.x / 2f -1;
        var finishZ = transform.position.z - size.z / 2f +1;
        var lastFieldPoint = new Int2((int)finishX, (int)finishZ);

        FieldController.Instance.Initialized(startFieldPoit, lastFieldPoint);
    }

    [ContextMenu("CreatePlane")]
    public void CreatePlane()
    {
        var width = 30;
        var heigh = 30;

        var gameObject = new GameObject("Ground");
        var meshFilter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        var meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        var mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(-width/2, 0, heigh/2),
            new Vector3(width/2,0,heigh/2),
              new Vector3(width/2,0,-heigh/2),
            new Vector3(-width/2,0,-heigh/2),
        };


        mesh.uv = new Vector2[]
     {
               new Vector2(0,1),
               new Vector2(1,1),
               new Vector2(0,0),
               new Vector2(1,0)
     };


        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        meshFilter.mesh = mesh;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
    
    
}
