using System;
using System.Collections;
using UI.Accordion.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Accordion
{
	[RequireComponent(typeof(RectTransform), typeof(LayoutElement))]
	[AddComponentMenu("UI/Extensions/Accordion/Accordion Element")]
	public class AccordionElement : Toggle
	{

		[SerializeField] private float m_MinHeight = 50f;
		[SerializeField] private LayoutElement _bodyElement;
		[SerializeField] private Image _iconElement;
		private Accordion _accordion;
		private RectTransform _rectTransform;
		private LayoutElement _layoutElement;
		private Quaternion _quaternionFinal;
		protected override void Awake()
		{
			base.Awake();
			// transition = Transition.None;
			// toggleTransition = ToggleTransition.None;
			_accordion = gameObject.GetComponentInParent<Accordion>();
			_rectTransform = transform as RectTransform;
			_layoutElement = gameObject.GetComponent<LayoutElement>();
			onValueChanged.AddListener(OnValueChanged);
		}

		protected override void Start()
		{
			base.Start();
			_layoutElement.preferredHeight = m_MinHeight;
			_bodyElement.preferredHeight = 0;

		}
#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();
			_accordion = gameObject.GetComponentInParent<Accordion>();

			if (group == null)
			{
				ToggleGroup tg = GetComponentInParent<ToggleGroup>();
				
				if (tg != null)
				{
					group = tg;
				}
			}
		}
#endif

		public void OnValueChanged(bool state)
		{
			if (_layoutElement == null)
				return;
			var transition = (_accordion != null) ? _accordion.Transition : Accordion.TransitionEnum.Instant;

			if (transition == Accordion.TransitionEnum.Instant && _accordion != null)
			{
				if (state)
				{
					if (_accordion.ExpandVertical)
					{
					}
				}
				else
				{
					if (_accordion.ExpandVertical)
					{
					}
				}
			}
			else if (transition == Accordion.TransitionEnum.Tween)
			{
				if (state)
				{
					if (_accordion.ExpandVertical)
					{
						TweenHeight(_rectTransform.rect.height, _accordion.GetHeight() * .7f);
						_quaternionFinal = Quaternion.Euler(new Vector3(0,0,180));
					}
				}
				else
				{
					if (_accordion.ExpandVertical)
					{
						TweenHeight(this._rectTransform.rect.height, this.m_MinHeight);
						_quaternionFinal = Quaternion.Euler(new Vector3(0,0,0));
					}
				}
			}
		}

		private void TweenHeight(float initialHeight, float finalHeight)
		{
			StopAllCoroutines();
			StartCoroutine(StartTween(initialHeight, finalHeight));
		}
		IEnumerator StartTween(float initialHeight, float finalHeight )
		{
			float elapsedTime = 0.0f;
			var initialQuaternion = _iconElement.transform.rotation;
			while (elapsedTime < _accordion.TransitionDuration)
			{
				elapsedTime += Time.deltaTime;
				var percentage = Mathf.Clamp01 (elapsedTime / _accordion.TransitionDuration);
				var actualTween = Mathf.Lerp(initialHeight, finalHeight, percentage);
				var rotation = Quaternion.Lerp(initialQuaternion, _quaternionFinal, percentage);
				_iconElement.transform.rotation = rotation;
				_layoutElement.preferredHeight = actualTween;
				_bodyElement.preferredHeight = actualTween  - m_MinHeight;

				yield return null;
			}
		}
	}
}