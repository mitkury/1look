using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Circle : MonoBehaviour {

	public int segments = 32;
	public float radius = 0.5f;
	public float border = 0.2f;

	Mesh mesh;
	MeshRenderer meshRenderer;
	float _completenes = 1;
	List<Vector3> fullCircleVertices = new List<Vector3>();
	float segmentAngle;
	bool created;

	void Start () {
		CreateCircleBorder();
	}

	Vector3 GetDirection(float angle) {
		return Quaternion.AngleAxis(angle, -Vector3.forward) * Vector3.up;
	}

	void CreateCircleBorder() {
		mesh = GetComponent<MeshFilter>().mesh;
		meshRenderer = GetComponent<MeshRenderer>();

		segmentAngle = 360f / (float)segments;
		fullCircleVertices.Clear();

		var origin = Vector3.zero;
		var vertices = new Vector3[segments * 4];
		var triangles = new List<int>();
		var directions = new Vector3[segments];

		for (int d = 0; d < segments; d++) {
			directions[d] = GetDirection(segmentAngle * d);
		}
		
		for (int s = 0; s < segments; s++) {
			
			var direction = directions[s];
			var nextDirection = s + 1 < segments ? directions[s + 1] : directions[0];
			var vA = s * 4;
			var vB = vA + 1;
			var vC = vA + 2;
			var vD = vA + 3;

			/*
			 * 
			 *   vB    vC
			 *
			 * 
			 * vA    vD
			 * 
			 */
			vertices[vA] = origin + direction * (1 - border) * radius;
			vertices[vB] = origin + direction * radius;
			vertices[vC] = origin + nextDirection * radius;
			vertices[vD] = origin + nextDirection * (1 - border) * radius;

			// Buffer positions of vertices.
			fullCircleVertices.Add(vertices[vA]);
			fullCircleVertices.Add(vertices[vB]);
			fullCircleVertices.Add(vertices[vC]);
			fullCircleVertices.Add(vertices[vD]);

			// Triangle 1
			triangles.Add(vA);
			triangles.Add(vB);
			triangles.Add(vC);
	
			// Triangle 2
			triangles.Add(vA);
			triangles.Add(vC);
			triangles.Add(vD);
		}

		var uvs = new Vector2[vertices.Length];
		for (var i = 0; i < uvs.Length; i++) {
			uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
		}
		
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles.ToArray();
		mesh.uv = uvs;
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		created = true;
	}

	/*
	void CreateCircle() {
		var segments = 32;
		var radius = 0.5f;
		var segmentAngle = 360f / (float)segments;
		var origin = Vector3.zero;
		var vertices = new Vector3[segments * 3];
		var triangles = new int[segments * 3];

		var directions = new Vector3[segments];
		for (int d = 0; d < segments; d++) {
			directions[d] = Quaternion.AngleAxis(segmentAngle * d, -Vector3.forward) * Vector3.up;
		}

		//vertices[0] = origin;

		for (int s = 0; s < segments; s++) {

			var direction = directions[s];
			var nextDirection = s + 1 < segments ? directions[s + 1] : directions[0];

			var vA = s * 3;
			var vB = vA + 1;
			var vC = vA + 2;

			vertices[vA] = origin;
			vertices[vB] = origin + direction * radius;
			vertices[vC] = origin + nextDirection * radius;

			triangles[vA] = vA;
			triangles[vB] = vB;
			triangles[vC] = vC;
		}

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}
	*/

	public float completenes {
		get {
			return _completenes;
		}
		set {
			if (!created) {
				CreateCircleBorder();
			}

			if (_completenes == value)
				return;

			// Set NaN to 1.
			_completenes = float.IsNaN(value) ? 1f : value;
			// Set values more than 1 to 1.
			_completenes = _completenes > 1f ? 1f : _completenes;
			// Set zero and less than zero values to near zero.
			_completenes = _completenes <= 0f ? 0.00001f : _completenes;

			var targetAngle = _completenes * 360f;
			var targetSegment = (int) Mathf.Ceil(targetAngle / segmentAngle) - 1;
			var targetDirection = GetDirection(targetAngle);
			var vertices = new Vector3[segments * 4];
			var origin = Vector3.zero;

			//Debug.Log("target angle: "+targetAngle+"deg, target segment: "+targetSegment);

			var tvA = targetSegment * 4;
			var tvB = tvA + 1;
			var tvC = tvA + 2;
			var tvD = tvA + 3;

			for (int s = 0; s < segments; s++) {
				var vA = s * 4;
				var vB = vA + 1;
				var vC = vA + 2;
				var vD = vA + 3;

				if (s < targetSegment) {
					vertices[vA] = fullCircleVertices[vA];
					vertices[vB] = fullCircleVertices[vB];
					vertices[vC] = fullCircleVertices[vC];
					vertices[vD] = fullCircleVertices[vD];
				} else {
					vertices[vA] = fullCircleVertices[tvA];
					vertices[vB] = fullCircleVertices[tvB];
					vertices[vC] = origin + targetDirection * radius;
					vertices[vD] = origin + targetDirection * (1 - border) * radius;
				}
			}

			mesh.vertices = vertices;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}
	}
}
