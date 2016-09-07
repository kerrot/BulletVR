using UnityEngine;
using System.Linq;
using System.Collections;

public class LineDrawControl : MonoBehaviour {
    public Vector3[] points;
    [SerializeField]
    private Material lineMaterial;

    void OnRenderObject()
    {
        if (points.Length > 0)
        {
            lineMaterial.SetPass(0);
            //GL.PushMatrix();
            //GL.MultMatrix(transform.localToWorldMatrix);
            GL.Begin(GL.LINES);
            points.ToList().ForEach(p =>
            {
                GL.Vertex3(p.x, p.y, p.z);
            });
            GL.End();
            //GL.PopMatrix();
        }
    }
}
