using UnityEngine;
using UnityEngine.UI;

namespace UI.Accordion
{
	///Credit ChoMPHi
	///Sourced from - http://forum.unity3d.com/threads/accordion-type-layout.271818/

	[RequireComponent(typeof(VerticalLayoutGroup), typeof(ContentSizeFitter), typeof(ToggleGroup))]
	[AddComponentMenu("UI/Extensions/Accordion/Accordion Group")]
	public class Accordion : MonoBehaviour
	{
		private float _heightParent;
		public bool ExpandVertical { get; private set; } = true;

		public enum TransitionEnum
		{
			Instant,
			Tween
		}

		[SerializeField] private TransitionEnum _transition = TransitionEnum.Instant;
		[SerializeField] private float _transitionDuration = 0.3f;
		public float timeScale;
		
		public TransitionEnum Transition => _transition;

		public float GetHeight()
		{
			_heightParent = transform.parent.GetComponent<RectTransform>().rect.height;
			// Debug.Log(_heightParent);
			return _heightParent;
		}
		
		public float TransitionDuration => _transitionDuration;

		private void Update()
		{
			Time.timeScale = timeScale;
		}

		private void Awake()
		{
			ExpandVertical = !GetComponent<HorizontalLayoutGroup>();
		}

#if UNITY_EDITOR

		private void OnValidate()
		{
			if (!GetComponent<HorizontalLayoutGroup>() && !GetComponent<VerticalLayoutGroup>())
			{
				Debug.LogError("Accordion requires either a Horizontal or Vertical Layout group to place children");
			}
		}
#endif
	}
}
