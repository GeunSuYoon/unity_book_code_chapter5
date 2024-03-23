using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public long money;
    public long moneyIncreaseAmount;
    public long moneyIncreaseLevel;
    public long moneyIncreasePrice;
    public int employeeCount = 1;
    public long employeeIncreaseAmount;
    public long employeePrice;

    public GameObject prefabMoney;

    public Text textMoney;
    public Text textEmployee;

    public Button buttonPrice;
    public Button buttonRecruit;

    public GameObject panelPrice;
    public GameObject panelRecruit;

    public GameObject prefabEmployee;
    public GameObject prefabFloor;

    public Vector2[] line;
    public int width;
    public float space;

    public float spaceFloor;
    public int floorCapacity;   //바닥 하나 당 수용 가능 인원
    private float currentFloor = 1;

    private void Awake()
    {
        gm = this;
    }

    // Use this for initialization
    void Start ()
    {
        string path = Application.persistentDataPath + "/save.xml";
        if (System.IO.File.Exists(path))
        {
            Load();
            FillEmployee();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        ShowInfo();
        MoneyIncrease();

        ButtonActiveCheck();
        ButtonRecruitActiveCheck();

        UpdateUpgradePanel();
        UpdateRecruitPanelText();

        CreateFloor();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void ShowInfo()
    {
        if (money != 0)
            textMoney.text = money.ToString("###,###") + " 원";
        else
            textMoney.text = "0 원";

        if (employeeCount != 0)
            textEmployee.text = employeeCount.ToString("###,###") + " 명";
        else
            textEmployee.text = "0 명";
    }

    void MoneyIncrease()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                //소지금 표시 프리팹 생성
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject obj = Instantiate(prefabMoney, mousePosition, Quaternion.identity);

                //소지금 증가
                money += moneyIncreaseAmount;
            }
        }
        
    }

    public void PanelSwitch(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void ButtonActiveCheck()
    {
        if(money >= moneyIncreasePrice)
        {
            buttonPrice.interactable = true;
        }
        else
        {
            buttonPrice.interactable = false;
        }
    }
    public void ButtonRecruitActiveCheck()
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

    void UpdateUpgradePanel()
    {

        if (panelPrice.activeSelf == true)
        {
            Text textPrice = panelPrice.transform.Find("Text").GetComponent<Text>();

            textPrice.text = "Lv." + moneyIncreaseLevel + " 단가 상승\n";
            textPrice.text += "외주 당 단가>\n";
            textPrice.text += moneyIncreaseAmount.ToString("###,###") + " 원\n";
            textPrice.text += "업그레이드 가격>\n";
            textPrice.text += moneyIncreasePrice.ToString("###,###") + " 원";
        }
        
    }

    void UpdateRecruitPanelText()
    {
        if (panelRecruit.activeSelf == true)
        {
            Text textRecruit = panelRecruit.transform.Find("Text").GetComponent<Text>();

            textRecruit.text = "Lv." + employeeCount + " 신규 고용\n";
            textRecruit.text += "직원 외주 당 단가>\n";
            textRecruit.text += employeeIncreaseAmount.ToString("###,###") + " 원\n";
            textRecruit.text += "업그레이드 가격>\n";
            textRecruit.text += employeePrice.ToString("###,###") + " 원";
        }
    }

    public void UpgradePrice()
    {
        if(money >= moneyIncreasePrice)
        {
            money -= moneyIncreasePrice;
            moneyIncreaseLevel += 1;
            moneyIncreaseAmount += moneyIncreaseLevel * 100;
            moneyIncreasePrice += moneyIncreaseLevel * 500;
        }
    }

    public void Recruit()
    {
        if (money >= AutoWork.autoIncreasePrice)
        {
            money -= AutoWork.autoIncreasePrice;
            employeeCount += 1;
            AutoWork.autoMoneyIncreaseAmount += moneyIncreaseLevel * 5;
            AutoWork.autoIncreasePrice += employeeCount * 500;
            CreateEmployee();
            //CreateFloor();
        }
    }

    void CreateEmployee()
    {
        //float spotX = line[employee % line.Length].x;
        //float spotY = line[employee % line.Length].y + -space * (employee / line.Length);

        //GameObject obj = Instantiate(prefabDesk, new Vector2(spotX, spotY), Quaternion.identity);

        Vector2 bossSpot = GameObject.Find("Boss").transform.position;
        float spotX = bossSpot.x + (employeeCount % width) * space;
        float spotY = bossSpot.y - (employeeCount / width) * space;
        Instantiate(prefabEmployee, new Vector2(spotX, spotY), Quaternion.identity);

    }

    void CreateFloor()
    {
        //float capacity = (employeeCount + 1) / floorCapacity;
        //if (capacity >= currentFloor)
        //{
        //    GameObject obj = Instantiate(prefabFloor, new Vector2(0, -spaceFloor * capacity), Quaternion.identity);
        //    currentFloor += 1;
        //    Camera.main.GetComponent<CameraDrag>().limitMinY -= spaceFloor;
        //}
        Vector2 bgPosition = GameObject.Find("Background").transform.position;
        float nextFloor = (employeeCount + 1) / floorCapacity;

        float spotX = bgPosition.x;
        float spotY = bgPosition.y;

        spotY -= spaceFloor * nextFloor;

        if(nextFloor >= currentFloor)
        {
            Instantiate(prefabFloor, new Vector2(spotX, spotY), Quaternion.identity);
            currentFloor += 1;
            Camera.main.GetComponent<CameraDrag>().limitMinY -= spaceFloor;
        }
    }

    void FillEmployee()
    {
        GameObject[] employees = GameObject.FindGameObjectsWithTag("Employee");
        if (employeeCount != employees.Length)
        {
            for (int i = employees.Length; i <= employeeCount; i++)
            {
                Vector2 bossSpot = GameObject.Find("Boss").transform.position;
                float spotX = bossSpot.x + (i % width) * space;
                float spotY = bossSpot.y - (i / width) * space;
                GameObject obj = Instantiate(prefabEmployee, new Vector2(spotX, spotY), Quaternion.identity);
            }
        }
    }



    void Save()
    {
        SaveData saveData = new SaveData();
        saveData.money = money;
        saveData.moneyIncreaseAmount = moneyIncreaseAmount;
        saveData.moneyIncreaseLevel = moneyIncreaseLevel;
        saveData.moneyIncreasePrice = moneyIncreasePrice;
        saveData.employeeCount = employeeCount;
        saveData.autoMoneyIncreaseAmount = AutoWork.autoMoneyIncreaseAmount;
        saveData.autoIncreasePrice = AutoWork.autoIncreasePrice;
        string path = Application.persistentDataPath + "/save.xml";
        XmlManager.XmlSave<SaveData>(saveData, path);
    }
    void Load()
    {
        SaveData saveData = new SaveData();
        string path = Application.persistentDataPath + "/save.xml";
        saveData = XmlManager.XmlLoad<SaveData>(path);
        money = saveData.money;
        moneyIncreaseAmount = saveData.moneyIncreaseAmount;
        moneyIncreaseLevel = saveData.moneyIncreaseLevel;
        moneyIncreasePrice = saveData.moneyIncreasePrice;
        employeeCount = saveData.employeeCount;
        AutoWork.autoMoneyIncreaseAmount = saveData.autoMoneyIncreaseAmount;
        AutoWork.autoIncreasePrice = saveData.autoIncreasePrice;
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < line.Length; i++)
        {
            Gizmos.DrawSphere(line[i], 0.3f);
        }
    }
}

[System.Serializable]
public class SaveData
{
    public long money;
    public long moneyIncreaseAmount;
    public long moneyIncreaseLevel;
    public long moneyIncreasePrice;
    public int employeeCount;

    public long autoMoneyIncreaseAmount;
    public long autoIncreasePrice;
}