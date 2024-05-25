using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

	public long	money;
	public long	moneyIncreaseAmount;
	
	public Text	textMoney;

	public GameObject	prefabMoney;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		ShowInfo();
        MoneyIncrease();
    }

	void	MoneyIncrease()
	{
		if (Input.GetMouseButtonDown(0)) // 마우스 버튼을 눌렀을 때
		{
			if (EventSystem.current.IsPointerOverGameObject() == false) // UI 위에 있지 않을 때
			{
				money += moneyIncreaseAmount;
				Vector2	mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Instantiate(prefabMoney, mousePosition, Quaternion.identity);	
			}
		}
	}

	void	ShowInfo()
	{
		if (money == 0)
			textMoney.text = "0원";
		else
			textMoney.text = money.ToString("###,###") + "원";
	}
}
