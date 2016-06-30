using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{

    private BezierCurve curve;
    private Transform handleTransform;
    public Vector3[] p;
    public Vector3 point;

    private void OnSceneGUI()
    {

        p = new Vector3[100];
        curve = target as BezierCurve;

        // HERE: Adicionei esta chamada a funcao para regenerate a spline. Assim quando mudas no inspector ele regenera a curve
        curve.GenerateCurve();

        for (int i = 0; i < curve.numberOfPoints; i++)
        {
            p[i] = ShowPoint(i);
        }
        Handles.color = Color.white;
        for (int k = 0; k < curve.numberOfPoints; k++)
        {
            if (k == curve.numberOfPoints - 1)
            {
                Handles.DrawLine(p[k], p[0]);
            }
            else
            {
                Handles.DrawLine(p[k], p[k + 1]);
            }
        }
    }

    //private Vector3 ShowPoint (int index) {
    //	Vector3 point = handleTransform.TransformPoint(curve.points[index]);
    //	EditorGUI.BeginChangeCheck();
    //	point = Handles.DoPositionHandle(point, handleRotation);
    //	if (EditorGUI.EndChangeCheck()) {
    //		Undo.RecordObject(curve, "Move Point");
    //	    EditorUtility.SetDirty(curve);
    //		curve.points[index] = handleTransform.InverseTransformPoint(point);
    //	}
    //	return point;
    //}

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = curve.points[index];
        return point;
    }
}