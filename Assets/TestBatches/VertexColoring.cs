using UnityEngine;
using System.Collections;

public class VertexColoring : MonoBehaviour {

	public Material targetMaterial;

	void Start () {

		var meshRenderer = GetComponent<MeshRenderer>();
		var meshFilter = GetComponent<MeshFilter>();
		var mesh = meshFilter.sharedMesh;
		var colorsPerMaterial = GetColors(meshRenderer.sharedMaterials);
		var newColors = new Color[mesh.vertexCount];

		SetToSingleMaterial(meshRenderer);

		for (int i = 0; i < mesh.subMeshCount; i++) {
			var triangles = mesh.GetTriangles(i);

			Debug.Log(triangles.Length);

			for (int vi = 0; vi < triangles.Length; vi++) {
				var vertex = mesh.vertices[triangles[vi]];
			}

			Debug.Log("---");
		}

	}

	Color[] GetColors(Material[] materials) {
		var colors = new Color[materials.Length];

		for (int i = 0; i < materials.Length; i++) {
			colors[i] = materials[i].color;
		}

		return colors;
	}

	void SetToSingleMaterial(MeshRenderer meshRenderer) {
		var newMaterials = meshRenderer.sharedMaterials;

		for (int i = 0; i < newMaterials.Length; i++) {
			newMaterials[i] = targetMaterial;
		}

		meshRenderer.sharedMaterials = newMaterials;
	}

}
