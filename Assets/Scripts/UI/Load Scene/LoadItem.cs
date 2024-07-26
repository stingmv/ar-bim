using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadItem : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private List<RectTransform> items;
    [SerializeField] private List<Image> itemsImages;
    [SerializeField] private float _speed = .5f;
    [SerializeField] private float _radiusOrbit = 1;
    [SerializeField] private GameObject centerOrbit;
    [SerializeField] private Gradient _gradient;
    private float _initialTime;
    private float _angleDistribution;
    
    IEnumerator Start()
    {
        _angleDistribution = 360f / items.Count;
        for (int i = 0; i < items.Count; i++)
        {
            float angleRadians = Mathf.Deg2Rad * (_angleDistribution + i * _angleDistribution);
            items[i].position = centerOrbit.transform.position + new Vector3(Mathf.Sin(angleRadians) * _radiusOrbit, Mathf.Cos(angleRadians) * _radiusOrbit, 0); 
        }
        _initialTime = 0;
        // yield return new WaitForSeconds(_initDelay);
        while (true)
        {
            // Debug.Log(Time.unscaledTime);
            yield return null;
            _initialTime = (_initialTime + Time.deltaTime * _speed) % 1;
            for (int i = 0; i < items.Count; i++)
            {
                var indexToEvaluate = (_initialTime + i * (1f / items.Count)) % 1;
                itemsImages[i].color = _gradient.Evaluate(indexToEvaluate);
                items[i].localScale = Vector3.one * _animationCurve.Evaluate(indexToEvaluate);
                // yield return null;
            }
        }
    }

   
}
