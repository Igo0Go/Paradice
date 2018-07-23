using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SubRun))]
public class EDit : Editor
{
	static bool visualizePath = false;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		visualizePath = EditorGUILayout.Toggle("Visualize Path", visualizePath);
	}

	[DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
	static void DrawLinesForNavAgent(UnityEngine.AI.NavMeshPath agent, GizmoType gizmoType)
	{
		if (!visualizePath || agent == null)
			return;

		Gizmos.color = Color.red;
		Vector3[] points = agent.corners;
		for (int i = 0; i < points.Length - 1; i++)
		{
			Gizmos.DrawLine(points[i], points[i + 1]);
		}
	}   
}