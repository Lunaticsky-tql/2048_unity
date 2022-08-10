using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyNumber : MonoBehaviour
{
    private Image bg;
    private Text number_text;

    private MyGrid inGrid;

    private void Awake()
    {
        bg = transform.GetComponent<Image>();
        number_text = transform.Find("number_text").GetComponent<Text>();
    }
    public void Init( MyGrid myGrid ) {
        //associate grid and number
        myGrid.SetNumber(this);
        SetGrid(myGrid);
        //randomly set number text, 2 for 2/3, 4 for 1/3
        int num = UnityEngine.Random.Range(1, 4);
        Debug.Log("num = " + num);
        if(num==1) {
            SetNumber(4);
        } else {
            SetNumber(2);
        }
    }
    public void SetGrid( MyGrid myGrid )
    {
        inGrid = myGrid;
    }
    public void SetNumber( int number )
    {
        number_text.text = number.ToString();
    }
    public int GetNumber()
    {
        return int.Parse(number_text.text);
        
    }
    public MyGrid GetGrid()
    {
        return inGrid;
    } 
}