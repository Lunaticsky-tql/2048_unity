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
    public MyNumber GetMyNumber()
    {
        return number;
    }
    public void SetMyNumber(MyNumber number)
    {
        this.number = number;
    }

}
