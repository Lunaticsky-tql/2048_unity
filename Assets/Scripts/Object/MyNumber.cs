using System;
using System.Collections;
using System.Collections.Generic;
using Enum;
using UnityEngine;
using UnityEngine.UI;

public class MyNumber : MonoBehaviour
{
    private Image bg;
    private Text number_text;
    public NumberStatus status;

    private MyGrid inGrid;
    private float spawnScaleTime = 1;
    private float mergeSpanTime = 1;
    private float mergeShrinkTime = 1;
    private float movePosTime = 1;
    private bool isSpawning = false;
    private bool isMerging = false;
    private bool isMoving = false;
    private bool isDestroyAfterMove = false;
    private Vector3 startMovePos;
    public Color[] bg_colors;
    public AudioClip mergeSound;
    

    private void Awake()
    {
        bg = transform.GetComponent<Image>();
        number_text = transform.Find("number_text").GetComponent<Text>();
    }

    public void Init(MyGrid myGrid)
    {
        //associate grid and number
        myGrid.SetMyNumber(this);
        SetGrid(myGrid);
        //randomly set number text, 2 for 2/3, 4 for 1/3
        int num = UnityEngine.Random.Range(1, 4);
        if (num == 1)
        {
            SetNumber(4);
        }
        else
        {
            SetNumber(2);
        }

        status = NumberStatus.CanMerge;
        //play spawn animation
        PlaySpawnAnimation();
    }

    public void SetGrid(MyGrid myGrid)
    {
        inGrid = myGrid;
    }

    public void SetNumber(int number)
    {
        number_text.text = number.ToString();
        this.bg.color=this.bg_colors[(int)Mathf.Log(number,2)-1];
    }

    public int GetNumber()
    {
        return int.Parse(number_text.text);
    }

    public MyGrid GetGrid()
    {
        return inGrid;
    }

    public void MoveToGrid(MyGrid targetGrid)
    {
        transform.SetParent(targetGrid.transform);
        
        startMovePos= transform.localPosition;
        movePosTime = 0;
        isMoving = true;
        
        //remove number from old grid
        GetGrid().SetMyNumber(null);
        // set number to new grid
        targetGrid.SetMyNumber(this);
        SetGrid(targetGrid);
    }
    
    public void DesroyAfterMove(MyGrid targetGrid)
    {
        transform.SetParent(targetGrid.transform);
        startMovePos= transform.localPosition;
        movePosTime = 0;
        isMoving = true;
        isDestroyAfterMove=true;
        
    }

    public bool CanMerge(MyNumber targetNumber)
    {
        //judge if this number can merge with the target number
        if (this.GetNumber() == targetNumber.GetNumber() && targetNumber.status == NumberStatus.CanMerge)
        {
            return true;
        }

        return false;
    }

    public void Merge()
    {
        //update score
        GamePanel gamePanel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
        gamePanel.AddScore(this.GetNumber());
        //update number
        int number = this.GetNumber() * 2;
        if(number==2048)
        {
            gamePanel.GameWin(); 
        }
        this.SetNumber(number);
        //prevent merging again
        status = NumberStatus.CanNotMerge;
        PlayMergeAnimation();
        //play merge sound
        AudioManager.Instance.PlaySound(mergeSound);
    }

    public void ResetStatus()
    {
        status = NumberStatus.CanMerge;
    }

    public void PlaySpawnAnimation()
    {
        spawnScaleTime = 0;
        isSpawning = true;
    }
    public void PlayMergeAnimation()
    {
        mergeSpanTime = 0;
        mergeShrinkTime = 0;
        isMerging = true;
    }
    


    public void Update()
    {
        if (isSpawning)
        {
            if (spawnScaleTime < 1)
            {
                spawnScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, spawnScaleTime);
            }
            else
            {
                isSpawning = false;
            }
        }

        if (isMerging)
        {
            if (mergeSpanTime < 1 && mergeShrinkTime == 0)
            {
                mergeSpanTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, mergeSpanTime);
            }

            if (mergeSpanTime >= 1 && mergeShrinkTime < 1)
            {
                mergeShrinkTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, mergeShrinkTime);
            }
            if(mergeShrinkTime >= 1&&mergeSpanTime >= 1)
            {
                isMerging = false;
            }
        }

        if (isMoving)
        {
            if(movePosTime < 1)
            {
                movePosTime += Time.deltaTime * 5;
                transform.localPosition = Vector3.Lerp(startMovePos, Vector3.zero, movePosTime);
            }
            if(movePosTime>=1)
            {
                isMoving = false;
                if (isDestroyAfterMove)
                {
                    GameObject.Destroy(this.gameObject);
                }
            }
        }
        
    }

    


}