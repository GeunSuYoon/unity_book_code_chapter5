using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWork : MonoBehaviour
{
	public static long	autoMoneyIncreaseAmount = 10;
	public static long	autoIncreasePrice = 1000;
	public static float	waitSecond = 1f;

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(Work());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	IEnumerator	Work()
	{
		while (true)
		{
			// GameManager	gm = GameObject.Find("GameManager").GetComponent<GameManager>();
			// gm.money += autoMoneyIncreaseAmount;

			yield return new WaitForSeconds(waitSecond);
		}
	}

}
