using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel: MonoBehaviour
{
    #region UI tools

    public Text scoreText;
    public Text bestScoreText;
    public Button regretButton;
    public Button restartButton;

    public Button quitButton;

    // public WinPanel winPanel;
    // public LosePanel losePanel;
    public Transform gridParent;
    public readonly Dictionary<int, int> GridConfig = new Dictionary<int, int>() { { 4, 100 }, { 5, 80 }, { 6, 66 } };
    public readonly Dictionary<int,int> SpaceConfig= new Dictionary<int, int>() { { 4, 10 }, { 5, 8 }, { 6, 7 } };
    private int _row;
    private int _column;


    #endregion

    #region Attributes & Assets

    public GameObject gridPrefab;
    public GameObject numberPrefab;
    public MyGrid[][] grids = null;
    public List<MyGrid> canCreateNumberGrid = new List<MyGrid>();
    private Vector3 _pointerDownPos, _pointerUpPos;

    #endregion



    public void InitGrid()
    {
        //get the grid size
        int gridSize = PlayerPrefs.GetInt(Const.GameModel, 4);
        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = gridSize;
        gridLayoutGroup.cellSize = new Vector2(GridConfig[gridSize], GridConfig[gridSize]);
        gridLayoutGroup.spacing = new Vector2(SpaceConfig[gridSize], SpaceConfig[gridSize]);
        gridLayoutGroup.padding= new RectOffset(SpaceConfig[gridSize], 0, SpaceConfig[gridSize], 0);
        grids= new MyGrid[gridSize][];
        _row = gridSize;
        _column = gridSize;
        //create the grid
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                if(grids[i]==null)
                {
                    grids[i] = new MyGrid[_column];
                }
                grids[i][j]=CreateGrid();
            }
        }
    }

    public MyGrid CreateGrid()
    {
        GameObject gameObject = GameObject.Instantiate(gridPrefab, gridParent);
        return gameObject.GetComponent<MyGrid>();
    }
    public void CreateNumber()
    {
        
        canCreateNumberGrid.Clear();
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                if (!grids[i][j].IsHaveNumber())
                {  
                    //generate number in the "blank" grid
                    canCreateNumberGrid.Add(grids[i][j]);
                }
            }
        }
        if (canCreateNumberGrid.Count == 0)
        {
            return;
        }
        // random choose a grid to create number
        int index = Random.Range(0, canCreateNumberGrid.Count);

        GameObject gameObject= GameObject.Instantiate(numberPrefab.gameObject, canCreateNumberGrid[index].transform);
        gameObject.GetComponent<MyNumber>().Init(canCreateNumberGrid[index]);
         
    }

    #region Input Handler
    
    public void OnRegretButtonClick()
    {
    }

    public void OnRestartButtonClick()
    {
    }

    public void OnQuitButtonClick()
    {
    }
    
    public void OnPointerDown()
    {
        _pointerDownPos = Input.mousePosition;
    }

    public void OnPointerUp()
    {
        _pointerUpPos = Input.mousePosition;

        if (Vector3.Distance(_pointerDownPos, _pointerUpPos) < 100)
        {
            Debug.Log("Invalid input!");
            return;
        }
        MoveType moveType = CaculateMoveType();
        Debug.Log("move type:" + moveType);


    }
    public MoveType CaculateMoveType()
    {
        if (Mathf.Abs(_pointerUpPos.x - _pointerDownPos.x) > Mathf.Abs(_pointerDownPos.y - _pointerUpPos.y))
        {
            // left or right
            if (_pointerUpPos.x - _pointerDownPos.x > 0)
            {
                // right
                return MoveType.RIGHT;
            }
            else
            {
                // left
                return MoveType.LEFT;
            }
        }
        else
        {
            // up or down
            if (_pointerUpPos.y - _pointerDownPos.y > 0)
            {
                // up
                return MoveType.UP;
            }
            else
            {
                // down
                return MoveType.DOWN;
            }

        }
    }
    #endregion

    private void Awake()
    {
        InitGrid();
        CreateNumber();
    }
}