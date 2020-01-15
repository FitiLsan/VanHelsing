using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	[SerializeField] private Transform _canvasQuBoard;

	//картинки состояний квеста (квест взят или он выполнен и его можно сдать)
	[SerializeField] private GameObject CanTakeIcon;
	[SerializeField] private GameObject QuCompleteIcon;

	[SerializeField]  private bool _QuCanTake;
	[SerializeField] private bool _QuComplete;

	//если квест у нпс был взят ,выполнен и сдан , то билборды над ним исчезают
	[SerializeField] private bool _AllQuComplete;

	

	 
	void Update()
	{
		_canvasQuBoard.LookAt(Camera.main.transform);
		QuCanTake();
		QuCanComplete();
		MisComplete();
	}
	/// <summary>
	/// метод, который включает воскл. знак над нпс и выключает остальные 
	/// </summary>
	void QuCanTake()
	{
		if (_QuCanTake)
		{
			ActivGameObj(CanTakeIcon);
			DeactiveGameObj(QuCompleteIcon,  _QuComplete,_AllQuComplete);
		}
	}
	/// <summary>
	/// метод, который включает вопросительный знак над нпс и выключает остальные 
	/// </summary>
	void QuCanComplete()
	{
		if (_QuComplete == true)
		{
			ActivGameObj(QuCompleteIcon);
			DeactiveGameObj(CanTakeIcon,   _QuCanTake,_AllQuComplete);
		}
	}
	/// <summary>
	/// метод, который выключает все картинки над нпс
	/// </summary>
	void MisComplete()
	{
		if (_AllQuComplete == true)
		{
			DeactiveGameObj(CanTakeIcon, QuCompleteIcon,   _QuComplete,   _QuCanTake);
		}
	}

	/// <summary>
	/// метод, который включает какой-либо геймобжект
	/// </summary>
	/// <param name="gameObject"></param>
	void ActivGameObj(GameObject gameObject)
		{
			gameObject.SetActive(true);

		}
	/// <summary>
	/// метод выключающий геймобжекты и делающий булевые переменные фолс
	/// </summary>
	/// <param name="gameObj"></param>
	/// <param name="onlyfalse1"></param>
    void DeactiveGameObj(GameObject gameObj,   bool onlyfalse1,bool onlyfalse2)
		{

			gameObj.SetActive(false);
		    onlyfalse2 = false;
			onlyfalse1 = false;
		}
	void DeactiveGameObj(GameObject gameObj,GameObject gameObj1,  bool onlyfalse,   bool onlyfalse1)
	{
		gameObj.SetActive(false);
		gameObj1.SetActive(false);
		onlyfalse = false;
		onlyfalse1 = false;
	}
}
 
