using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class HMesh 
{
	public static Vector3 NearestPointOnMesh(Vector3 targetPoint, Mesh mesh)
	{
		int[] nearestTriangle;
		bool pointIsInsideTriangle;

		return NearestPointOnMesh(targetPoint, mesh.vertices, mesh.triangles, out nearestTriangle, out pointIsInsideTriangle);
	}

	public static Vector3 NearestPointOnMesh(Vector3 targetPoint, Mesh mesh, out int[] nearestTriangle)
	{
		bool pointIsInsideTriangle;

		return NearestPointOnMesh(targetPoint, mesh.vertices, mesh.triangles, out nearestTriangle, out pointIsInsideTriangle);
	}

	public static Vector3 NearestPointOnMesh(Vector3 targetPoint, Mesh mesh, out int[] nearestTriangle, out bool pointIsInsideTriangle)
	{
		return NearestPointOnMesh(targetPoint, mesh.vertices, mesh.triangles, out nearestTriangle, out pointIsInsideTriangle);
	}

	public static Vector3 NearestPointOnMesh(Vector3 targetPoint, Vector3[] vertices, int[] triangles)
	{
		int[] nearestTriangle;
		
		return NearestPointOnMesh(targetPoint, vertices, triangles, out nearestTriangle);
	}

	public static Vector3 NearestPointOnMesh(Vector3 targetPoint, Vector3[] vertices, int[] triangles, out int[] nearestTriangle)
	{
		bool pointIsInsideTriangle;

		return NearestPointOnMesh(targetPoint, vertices, triangles, out nearestTriangle, out pointIsInsideTriangle);
	}

	private struct PointOnTriangle
	{
		public int[] triangleVertices;
		public Vector3 position;
		public bool isInsideTriangle;

		public PointOnTriangle(int[] triangleVertices, Vector3 position, bool isInsideTriangle)
		{
			this.triangleVertices = triangleVertices;
			this.position = position;
			this.isInsideTriangle = isInsideTriangle;
		}
	}

	private static List<PointOnTriangle> nearestPointsOnTriangles;

	public static Vector3 NearestPointOnMesh(Vector3 targetPoint, Vector3[] vertices, int[] triangles, out int[] nearestTriangle, out bool pointIsInsideTriangle)
	{
		pointIsInsideTriangle = false;;

		//List<PointOnTriangle> nearestPointsOnTriangles = new List<PointOnTriangle>();
		if (nearestPointsOnTriangles == null)
		{
			nearestPointsOnTriangles = new List<PointOnTriangle>();
		}
		else
		{
			nearestPointsOnTriangles.Clear();
		}

		// Find nearest points to to every triangle on the mesh.
		{
			int triAIndex = 0;
			
			for (int i = 0; triangles.Length / 3 > i; i++)
			{
				triAIndex = i * 3;
				
				int vertexAIndex = triangles[triAIndex];
				int vertexBIndex = triangles[triAIndex + 1];
				int vertexCIndex = triangles[triAIndex + 2];
				
				Vector3 vertexA = vertices[vertexAIndex];
				Vector3 vertexB = vertices[vertexBIndex];
				Vector3 vertexC = vertices[vertexCIndex];

				int[] npvs = { vertexAIndex, vertexBIndex, vertexCIndex };
				bool isInsideTriangle;
				Vector3 position = NearestPointOnTriangle(targetPoint, vertexA, vertexB, vertexC, out isInsideTriangle);

				nearestPointsOnTriangles.Add(new PointOnTriangle(npvs, position, isInsideTriangle));
			}
		}
		
		Vector3 nearestPointOnMesh = Vector3.zero;
		nearestTriangle = null;

		// Find a nearest point from from all the points.
		{
			float minDistance = float.MaxValue;

			foreach (PointOnTriangle point in nearestPointsOnTriangles)
			{
				float targetDistance = Vector3.SqrMagnitude(point.position - targetPoint);
				
				if (targetDistance < minDistance)
				{
					minDistance = targetDistance;

					nearestPointOnMesh = point.position;
					nearestTriangle = point.triangleVertices;
					pointIsInsideTriangle = point.isInsideTriangle;
				}
			}
		}
		
		return nearestPointOnMesh;
	}

	public static Vector3 NearestPointOnTriangle(Vector3 targetPoint, Vector3 vertexA, Vector3 vertexB, Vector3 vertexC)
	{
		bool pointIsInsideTriangle = false;

		return NearestPointOnTriangle(targetPoint, vertexA, vertexB, vertexC, out pointIsInsideTriangle);
	}
	
	public static Vector3 NearestPointOnTriangle(Vector3 targetPoint, Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, out bool pointIsInsideTriangle) 
	{
		Vector3 edge1 = vertexB - vertexA;
		Vector3 edge2 = vertexC - vertexA;
		Vector3 edge3 = vertexC - vertexB;
		float edge1Len = edge1.magnitude;
		float edge2Len = edge2.magnitude;
		float edge3Len = edge3.magnitude;
		
		Vector3 ptLineA = targetPoint - vertexA;
		Vector3 ptLineB = targetPoint - vertexB;
		Vector3 ptLineC = targetPoint - vertexC;
		Vector3 xAxis = edge1 / edge1Len;
		Vector3 zAxis = Vector3.Cross(edge1, edge2).normalized;
		Vector3 yAxis = Vector3.Cross(zAxis, xAxis);
		
		Vector3 edge1Cross = Vector3.Cross(edge1, ptLineA);
		Vector3 edge2Cross = Vector3.Cross(edge2, -ptLineC);
		Vector3 edge3Cross = Vector3.Cross(edge3, ptLineB);

		bool edge1On = Vector3.Dot(edge1Cross, zAxis) > 0f;
		bool edge2On = Vector3.Dot(edge2Cross, zAxis) > 0f;
		bool edge3On = Vector3.Dot(edge3Cross, zAxis) > 0f;

		pointIsInsideTriangle = false;

		// If searching for a point also inside the triangle (not only on edges).
		//	If the point is inside the triangle then return its coordinate.
		if (edge1On && edge2On && edge3On) 
		{	
			pointIsInsideTriangle = true;

			float xExtent = Vector3.Dot(ptLineA, xAxis);
			float yExtent = Vector3.Dot(ptLineA, yAxis);
			return vertexA + xAxis * xExtent + yAxis * yExtent;
		}
		
		//	Otherwise, the nearest point is somewhere along one of the edges.
		Vector3 edge1Norm = xAxis;
		Vector3 edge2Norm = edge2.normalized;
		Vector3 edge3Norm = edge3.normalized;
		
		float edge1Ext = Mathf.Clamp(Vector3.Dot(edge1Norm, ptLineA), 0f, edge1Len);
		float edge2Ext = Mathf.Clamp(Vector3.Dot(edge2Norm, ptLineA), 0f, edge2Len);
		float edge3Ext = Mathf.Clamp(Vector3.Dot(edge3Norm, ptLineB), 0f, edge3Len);
		
		Vector3 edge1Pt = vertexA + edge1Ext * edge1Norm;
		Vector3 edge2Pt = vertexA + edge2Ext * edge2Norm;
		Vector3 edge3Pt = vertexB + edge3Ext * edge3Norm;
		
		float sqDist1 = (targetPoint - edge1Pt).sqrMagnitude;
		float sqDist2 = (targetPoint - edge2Pt).sqrMagnitude;
		float sqDist3 = (targetPoint - edge3Pt).sqrMagnitude;
		
		if (sqDist1 < sqDist2) 
		{
			if (sqDist1 < sqDist3) 
			{
				return edge1Pt;
			} 
			else 
			{
				return edge3Pt;
			}
		} 
		else if (sqDist2 < sqDist3) 
		{
			return edge2Pt;
		} 
		else 
		{
			return edge3Pt;
		}
	}

	public static bool isPointIsOnVertex(Vector3 point, Vector3[] vertices) 
	{

		foreach (Vector3 vertex in vertices)
		{
			if (Vector3.Distance(point, vertex) <= 0.001f)
			{
				return true;
			}
		}

		return false;
	}

	public static Vector3[] GetNearestEdgeToAPoint(Vector3 targetPoint, Vector3 vertexA, Vector3 vertexB, Vector3 vertexC)
	{
		Vector3 edge1 = vertexB - vertexA;
		Vector3 edge2 = vertexC - vertexA;
		Vector3 edge3 = vertexC - vertexB;
		float edge1Len = edge1.magnitude;
		float edge2Len = edge2.magnitude;
		float edge3Len = edge3.magnitude;
		
		Vector3 ptLineA = targetPoint - vertexA;
		Vector3 ptLineB = targetPoint - vertexB;
		Vector3 ptLineC = targetPoint - vertexC;
		Vector3 xAxis = edge1 / edge1Len;
		Vector3 zAxis = Vector3.Cross(edge1, edge2).normalized;
		Vector3 yAxis = Vector3.Cross(zAxis, xAxis);

		Vector3 edge1Norm = xAxis;
		Vector3 edge2Norm = edge2.normalized;
		Vector3 edge3Norm = edge3.normalized;
		
		float edge1Ext = Mathf.Clamp(Vector3.Dot(edge1Norm, ptLineA), 0f, edge1Len);
		float edge2Ext = Mathf.Clamp(Vector3.Dot(edge2Norm, ptLineA), 0f, edge2Len);
		float edge3Ext = Mathf.Clamp(Vector3.Dot(edge3Norm, ptLineB), 0f, edge3Len);

		Vector3 edge1Pt = vertexA + edge1Ext * edge1Norm;
		Vector3 edge2Pt = vertexA + edge2Ext * edge2Norm;
		Vector3 edge3Pt = vertexB + edge3Ext * edge3Norm;
		
		float sqDist1 = (targetPoint - edge1Pt).sqrMagnitude;
		float sqDist2 = (targetPoint - edge2Pt).sqrMagnitude;
		float sqDist3 = (targetPoint - edge3Pt).sqrMagnitude;

		Vector3[] edge = new Vector3[2];

		if (sqDist1 < sqDist2) 
		{
			if (sqDist1 < sqDist3) 
			{
				//edge1
				edge[0] = vertexA;
				edge[1] = vertexB;
			} 
			else 
			{
				//edge3
				edge[0] = vertexB;
				edge[1] = vertexC;
			}
		} 
		else if (sqDist2 < sqDist3) 
		{
			//edge2
			edge[0] = vertexA;
			edge[1] = vertexC;
		} 
		else 
		{
			//edge3
			edge[0] = vertexB;
			edge[1] = vertexC;
		}

		return edge;
	}

}
