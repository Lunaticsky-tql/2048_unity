using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    public MyNumber number;

    public bool IsHaveNumber()
    {
        return number != null;
    }
    public MyNumber GetNumber()
    {
        return number;
    }
    public void SetNumber(MyNumber number)
    {
        this.number = number;
    }


}
