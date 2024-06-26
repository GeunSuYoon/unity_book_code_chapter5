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

	public long	moneyIncreaseLevel;
	public long	moneyIncreasePrice;
	public Text	textPrice;

	public Button buttonPrice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		ShowInfo();
        MoneyIncrease();
		UpdatePanelText();
		ButtonActiveCheck();
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

	void	UpdatePanelText()
	{
		textPrice.text = "Lv." + moneyIncreaseLevel + " 단가 상승\n\n";
		textPrice.text += "외주 당 단가>\n";
		textPrice.text += moneyIncreaseAmount.ToString("###,###") + " 원\n";
		textPrice.text += "업그레이드 가격>\n";
		textPrice.text += moneyIncreasePrice.ToString("###,###") + "원";
	}

	public void	UpgradePrice()
	{
		if (money >= moneyIncreasePrice)
		{
			money -= moneyIncreasePrice;
			moneyIncreaseLevel += 1;
			moneyIncreaseAmount += moneyIncreaseLevel * 100;
			moneyIncreasePrice += moneyIncreaseLevel * 500;
		}
	}

	void ButtonActiveCheck()
	{
		if (money >= moneyIncreasePrice)
		{
			buttonPrice.interactable = true;
		}
		else
		{
			buttonPrice.interactable = false;
		}
	}
}
