using UnityEngine;
using System.Collections;

public class WaveMesh : MonoBehaviour 
{	
	private Vector3[] meshVertices;
	private Vector3[] origVerticesPos; 

	public float windSpeed = 30;
	public float windScale = 0.1f;
	public float windRoughness = 10;

	private Mesh mesh;

	float move;

	public Vector3 affectedVertices = new Vector3(1,1,0);
	public bool useAlternativeMethod = false;
	
	void Start () {
		
		MeshFilter myMF = this.GetComponent("MeshFilter") as MeshFilter;
		mesh = myMF.mesh;
		meshVertices = mesh.vertices;
		origVerticesPos = mesh.vertices; // I changed this from origVerticesPos = meshVertices;
	}
	
	void Update () 
	{
		int size = meshVertices.Length;
		for(int i = 0;i < size;i++) 
		{
			Vector3 nV_ = origVerticesPos[i];

			if(useAlternativeMethod)
			{
				nV_.x += Mathf.Sin((nV_.y + nV_.x) * windRoughness + Time.time * windSpeed) * windScale;
			}
			else
			{
			if(affectedVertices.x != 0)
				nV_.x += Mathf.Cos(nV_.x * windRoughness + Time.time * windSpeed) * windScale;

			if(affectedVertices.y != 0)
				nV_.y += Mathf.Sin(nV_.y * windRoughness + Time.time * windSpeed) * windScale;
			}

			meshVertices[i] = nV_;
		}

		mesh.vertices = meshVertices;
		mesh.RecalculateNormals();
	}
}