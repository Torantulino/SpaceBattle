using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHP : MonoBehaviour
{

	Image bar;

	void Start()
	{
		bar = gameObject.GetComponent<Image>();
	}

	public void SetHP(float value)
	{
		bar.fillAmount = value;
	}

	public void IncrementHP(float value)
	{
		bar.fillAmount += value;
	}
}