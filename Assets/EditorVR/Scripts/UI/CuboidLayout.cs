﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.EditorVR.Utilities;

public class CuboidLayout : UIBehaviour
{
	static readonly Vector2 kCuboidPivot = new Vector2(0.5f, 0.5f);
	const float kLayerHeight = 0.004f;
	const float kExtraSpace = 0.00055f; // To avoid Z-fighting

	[SerializeField]
	RectTransform[] m_TargetTransforms;

	[SerializeField]
	RectTransform[] m_TargetHighlightTransforms;

	[Header("Prefab Templates")]
	[SerializeField]
	GameObject m_CubePrefab;

	[SerializeField]
	GameObject m_HighlightCubePrefab;

	Transform[] m_CubeTransforms;
	Transform[] m_HighlightCubeTransforms;

	protected override void Awake()
	{
		m_CubeTransforms = new Transform[m_TargetTransforms.Length];
		for (var i = 0; i < m_CubeTransforms.Length; i++)
		{
			m_CubeTransforms[i] = U.Object.Instantiate(m_CubePrefab, m_TargetTransforms[i], false).transform;
		}

		m_HighlightCubeTransforms = new Transform[m_TargetHighlightTransforms.Length];
		for (var i = 0; i < m_TargetHighlightTransforms.Length; i++)
		{
			m_HighlightCubeTransforms[i] = U.Object.Instantiate(m_HighlightCubePrefab, m_TargetHighlightTransforms[i], false).transform;
		}

		UpdateObjects();
	}

	protected override void OnRectTransformDimensionsChange()
	{
		UpdateObjects();
	}

	/// <summary>
	/// Set a new material on all backing cubes (used for instanced version of the material)
	/// </summary>
	/// <param name="backingCubeMaterial">New material to use</param>
	public void SetMaterials(Material backingCubeMaterial, Material[] highlightMaterials)
	{
		if (m_CubeTransforms != null)
		{
			foreach (var cube in m_CubeTransforms)
			{
				cube.GetComponent<Renderer>().sharedMaterial = backingCubeMaterial;
			}

			// These are most likely WorkspaceButtons, so the material cloning that is done there will get stomped by this
			foreach (var hightlight in m_HighlightCubeTransforms)
			{
				foreach (var child in hightlight.GetComponentsInChildren<Renderer>())
				{
					if(child.transform != hightlight)
						child.sharedMaterials = highlightMaterials;
				}
			}
		}
	}

	public void UpdateObjects()
	{
		if (m_CubeTransforms == null)
			return;

		// Update standard objects
		const float kStandardObjectSideScalePadding = 0.005f;
		for (var i = 0; i < m_CubeTransforms.Length; i++)
		{
			var rectSize = m_TargetTransforms[i].rect.size.Abs();
			// Scale pivot by rect size to get correct xy local position
			var pivotOffset =  Vector2.Scale(rectSize, kCuboidPivot - m_TargetTransforms[i].pivot);

			// Add space for target transform
			var localPosition = m_TargetTransforms[i].localPosition;
			m_TargetTransforms[i].localPosition = new Vector3(localPosition.x, localPosition.y, -kLayerHeight);

			//Offset by 0.5 * height to account for pivot in center
			const float zOffset = kLayerHeight * 0.5f + kExtraSpace;
			m_CubeTransforms[i].localPosition = new Vector3(pivotOffset.x, pivotOffset.y, zOffset);
			m_CubeTransforms[i].localScale = new Vector3(rectSize.x + kStandardObjectSideScalePadding, rectSize.y, kLayerHeight);
		}

		// Update highlight objects
		for (var i = 0; i < m_HighlightCubeTransforms.Length; i++)
		{
			var rectSize = m_TargetHighlightTransforms[i].rect.size.Abs();
			// Scale pivot by rect size to get correct xy local position
			var pivotOffset = Vector2.Scale(rectSize, kCuboidPivot - m_TargetHighlightTransforms[i].pivot);

			// Add space for target transform
			var localPosition = m_TargetHighlightTransforms[i].localPosition;
			m_TargetHighlightTransforms[i].localPosition = new Vector3(localPosition.x, localPosition.y, -kLayerHeight);

			//Offset by 0.5 * height to account for pivot in center
			const float zOffset = kLayerHeight * 0.5f + kExtraSpace;
			m_HighlightCubeTransforms[i].localPosition = new Vector3(pivotOffset.x, pivotOffset.y, zOffset);
			m_HighlightCubeTransforms[i].localScale = new Vector3(rectSize.x, rectSize.y, kLayerHeight);
		}
	}
}