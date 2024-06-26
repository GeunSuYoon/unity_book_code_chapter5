using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
	// Money objs
	public long	money;
	public long	moneyIncreaseAmount;
	
	public Text	textMoney;

	public GameObject	prefabMoney;

	// Upgrade panel
	public long	moneyIncreaseLevel;
	public long	moneyIncreasePrice;
	public Text	textPrice;

	public Button buttonPrice;

	// Recruit panel
	public int		employeeCount;
	public Text		textRecruit;

	public Button 	buttonRecruit;

	public int		width;
	public float	space;

	public GameObject	prefabEmployee;

	public Text			textPerson;

	// Floor
	public float	spaceFloor;
	public int		floorCapacity;
	public int		currentFloor;

	public GameObject	prefabFloor;

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

		UpdateRecruitPanelText();
		ButtonRecruitActiveCheck();

		CreateFloor();
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

		if (employeeCount == 0)
			textPerson.text = "0명";
		else
			textPerson.text = employeeCount + "명";
	}

	void	UpdatePanelText()
	{
		textPrice.text = "Lv." + moneyIncreaseLevel + " 단가 상승\n\n";
		textPrice.text += "외주 당 단가>\n";
		textPrice.text += moneyIncreaseAmount.ToString("###,###") + " 원\n";
		textPrice.text += "업그레이드 가격>\n";
		textPrice.text += moneyIncreasePrice.ToString("###,###") + "원";
	}

	void	UpdateRecruitPanelText()
	{
		textRecruit.text = "Lv." + employeeCount + " 직원 고용\n\n";
		textRecruit.text += "직원 1초 당 단가>\n";
		textRecruit.text += AutoWork.autoMoneyIncreaseAmount.ToString("###,###") + " 원\n";
		textRecruit.text += "업그레이드 가격>\n";
		textRecruit.text += AutoWork.autoIncreasePrice.ToString("###,###") + "원";
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

	void	ButtonActiveCheck()
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

	void	ButtonRecruitActiveCheck()
	{
		if (money >= AutoWork.autoIncreasePrice)
		{
			buttonRecruit.interactable = true;
		}
		else
		{
			buttonRecruit.interactable = false;
		}
	}

	void	CreateEmployee()
	{
		Vector2	bossSpot = GameObject.Find("Boss").transform.position;
		float	spotX = bossSpot.x + (employeeCount % width) * space;
		float	spotY = bossSpot.y - (employeeCount / width) * space;

		Instantiate(prefabEmployee, new Vector2(spotX, spotY), Quaternion.identity);
	}
	
	public void	Recruit()
	{
		if (money >= AutoWork.autoIncreasePrice)
		{
			money -= AutoWork.autoIncreasePrice;
			employeeCount += 1;
			AutoWork.autoMoneyIncreaseAmount += moneyIncreaseLevel * 100;
			AutoWork.autoIncreasePrice += moneyIncreaseLevel * 500;

			CreateEmployee();
		}
	}

	void	CreateFloor()
	{
		Vector2	bgPosition = GameObject.Find("Background").transform.position;

		float	nextFloor = (employeeCount + 1) / floorCapacity;

		float	spotX = bgPosition.x;
		float	spotY = bgPosition.y - spaceFloor * nextFloor;

		if (nextFloor >= currentFloor)
		{
			Instantiate(prefabFloor, new Vector2(spotX, spotY), Quaternion.identity);
			currentFloor += 1;
		}
	}
}
