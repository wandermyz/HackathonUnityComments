﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.Experimental.EditorVR.Helpers
{
	public class UITransformCopy : MonoBehaviour
	{
		static readonly Vector2 kTransformPivot = new Vector2(0.5f, 0.5f);

		Transform m_TargetTransform;

		[SerializeField]
		RectTransform m_SourceRectTransform;

		[SerializeField]
		float m_XPositionPadding = 0.005f;

		[SerializeField]
		float m_YPositionPadding = 0f;

		[SerializeField]
		float m_ZPositionPadding = 0.00055f;

		[SerializeField]
		float m_XScalePadding = 0.01f;

		[SerializeField]
		float m_YScalePadding = 0f;

		[SerializeField]
		bool m_ParentUnderSource = true;

		void Awake()
		{
			m_TargetTransform = transform;

			if (m_ParentUnderSource)
				m_TargetTransform.SetParent(m_SourceRectTransform, false);

			DriveTransformWithRectTransform();
		}

		void Update()
		{
			if (m_SourceRectTransform.hasChanged)
				DriveTransformWithRectTransform();
		}

		void DriveTransformWithRectTransform()
		{
			if (!m_SourceRectTransform || !m_TargetTransform || !gameObject.activeInHierarchy)
				return;

			// Drive transform with source RectTransform
			var rectSize = m_SourceRectTransform.rect.size.Abs();
			// Scale pivot by rect size to get correct xy local position
			var pivotOffset = Vector2.Scale(rectSize, kTransformPivot - m_SourceRectTransform.pivot);

			// Add space for object
			var localPosition = m_SourceRectTransform.localPosition;
			m_SourceRectTransform.localPosition = new Vector3(localPosition.x, localPosition.y, -m_ZPositionPadding);

			//Offset by 0.5 * height to account for pivot in center
			m_TargetTransform.localPosition = new Vector3(pivotOffset.x + m_XPositionPadding, pivotOffset.y + m_YPositionPadding, m_ZPositionPadding);
			m_TargetTransform.localScale = new Vector3(rectSize.x + m_XScalePadding, rectSize.y + m_YScalePadding, transform.localScale.z);
		}
	}
}