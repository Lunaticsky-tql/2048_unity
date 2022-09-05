using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GamePanel : MonoBehaviour
{
    #region UI tools

    public Text scoreText;
    public Text bestScoreText;
    public Button regretButton;
    public Button restartButton;

    public Button quitButton;
    public Transform gridParent;
    public readonly Dictionary<int, int> GridConfig = new Dictionary<int, int>() { { 4, 193 }, { 5, 152 }, { 6, 127 } };
    public readonly Dictionary<int, int> SpaceConfig = new Dictionary<int, int>() { { 4, 15 }, { 5, 15 }, { 6, 12 } };

    public readonly Dictionary<int, int> FontSizeConfig = new Dictionary<int, int>()
        { { 4, 80 }, { 5, 60 }, { 6, 40 } };

    public bool isNeedToCreateNum = false;
    private int _row;
    private int _column;
    public WinPanel winPanel;
    public LosePanel losePanel;

    #endregion

    #region Attributes & Assets

    public GameObject gridPrefab;
    public GameObject numberPrefab;
    public AudioClip bgClip;
    public MyGrid[][] grids = null;
    public List<MyGrid> canCreateNumberGrid = new List<MyGrid>();
    private Vector3 _pointerDownPos, _pointerUpPos;
    private int currentScore = 0;
    public StepModel lastStepModel;
    public String BestScoreKey;
    public int gridSize;

    #endregion

    #region Game Logic

    public void InitGrid()
    {

        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = gridSize;
        gridLayoutGroup.cellSize = new Vector2(GridConfig[gridSize], GridConfig[gridSize]);
        gridLayoutGroup.spacing = new Vector2(SpaceConfig[gridSize], SpaceConfig[gridSize]);
        gridLayoutGroup.padding = new RectOffset(SpaceConfig[gridSize], SpaceConfig[gridSize], SpaceConfig[gridSize],
            SpaceConfig[gridSize]);
        grids = new MyGrid[gridSize][];
        _row = gridSize;
        _column = gridSize;
        //set the font size of number prefab
        Text numberText = numberPrefab.GetComponentInChildren<Text>();
        numberText.fontSize = FontSizeConfig[gridSize];
        //create the grid
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                if (grids[i] == null)
                {
                    grids[i] = new MyGrid[_column];
                }

                grids[i][j] = CreateGrid();
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

        GameObject gameObject = GameObject.Instantiate(numberPrefab.gameObject, canCreateNumberGrid[index].transform);
        gameObject.GetComponent<MyNumber>().Init(canCreateNumberGrid[index]);
    }

    private void CreateNumber(MyGrid grid, int number)
    {
        GameObject gameObject = GameObject.Instantiate(numberPrefab.gameObject, grid.transform);
        gameObject.GetComponent<MyNumber>().Init(grid);
        gameObject.GetComponent<MyNumber>().SetNumber(number);
    }

    public void MoveNumber(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.UP:
                for (int j = 0; j < _column; j++)
                {
                    for (int i = 1; i < _row; i++)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            MyNumber number = grids[i][j].GetMyNumber();
                            for (int m = i - 1; m >= 0; m--)
                            {
                                if (grids[m][j].IsHaveNumber())
                                {
                                    HandleMerge(number, grids[m][j].GetMyNumber());
                                    break;
                                }
                                else
                                {
                                    number.MoveToGrid(grids[m][j]);
                                    isNeedToCreateNum = true;
                                }
                            }
                        }
                    }
                }

                break;
            case MoveType.DOWN:
                for (int j = 0; j < _column; j++)
                {
                    for (int i = _row - 2; i >= 0; i--)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            MyNumber number = grids[i][j].GetMyNumber();
                            for (int m = i + 1; m < _row; m++)
                            {
                                if (grids[m][j].IsHaveNumber())
                                {
                                    HandleMerge(number, grids[m][j].GetMyNumber());
                                    break;
                                }
                                else
                                {
                                    number.MoveToGrid(grids[m][j]);
                                    isNeedToCreateNum = true;
                                }
                            }
                        }
                    }
                }

                break;
            case MoveType.LEFT:
                for (int i = 0; i < _row; i++)
                {
                    for (int j = 1; j < _column; j++)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            MyNumber number = grids[i][j].GetMyNumber();
                            for (int m = j - 1; m >= 0; m--)
                            {
                                if (grids[i][m].IsHaveNumber())
                                {
                                    HandleMerge(number, grids[i][m].GetMyNumber());
                                    break;
                                }
                                else
                                {
                                    number.MoveToGrid(grids[i][m]);
                                    isNeedToCreateNum = true;
                                }
                            }
                        }
                    }
                }

                break;
            case MoveType.RIGHT:
                for (int i = 0; i < _row; i++)
                {
                    for (int j = _column - 2; j >= 0; j--)
                    {
                        if (grids[i][j].IsHaveNumber())
                        {
                            MyNumber number = grids[i][j].GetMyNumber();
                            for (int m = j + 1; m < _column; m++)
                            {
                                if (grids[i][m].IsHaveNumber())
                                {
                                    HandleMerge(number, grids[i][m].GetMyNumber());
                                    break;
                                }
                                else
                                {
                                    number.MoveToGrid(grids[i][m]);
                                    isNeedToCreateNum = true;
                                }
                            }
                        }
                    }
                }

                break;
        }
    }

    public void HandleMerge(MyNumber current, MyNumber target)
    {
        if (current.CanMerge(target))
        {
            target.Merge();
            // destroy the current number
            current.GetGrid().SetMyNumber(null);

            current.DesroyAfterMove(target.GetGrid());
            isNeedToCreateNum = true;
        }
    }

    public void ResetNumberStatus()
    {
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                if (grids[i][j].IsHaveNumber())
                {
                    grids[i][j].GetMyNumber().ResetStatus();
                }
            }
        }
    }

    public bool IsGameOver()
    {
        //if there is blank grid, the game is not over
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                if (!grids[i][j].IsHaveNumber())
                {
                    return false;
                }
            }
        }

        // if there is no blank grid, check if there is any number can merge
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                if (i > 0 && grids[i][j].GetMyNumber().CanMerge(grids[i - 1][j].GetMyNumber()))
                {
                    return false;
                }

                if (i < _row - 1 && grids[i][j].GetMyNumber().CanMerge(grids[i + 1][j].GetMyNumber()))
                {
                    return false;
                }

                if (j > 0 && grids[i][j].GetMyNumber().CanMerge(grids[i][j - 1].GetMyNumber()))
                {
                    return false;
                }

                if (j < _column - 1 && grids[i][j].GetMyNumber().CanMerge(grids[i][j + 1].GetMyNumber()))
                {
                    return false;
                }
            }
        }

        return true;
    }

    #endregion


    #region Input Handler

    public void OnRegretButtonClick()
    {
        BackToLastStep();
        //set the button interactable to false
        regretButton.interactable = false;
    }

    public void OnRestartButtonClick()
    {
        RestartGame();
    }

    public void OnQuitButtonClick()
    {
        ExitGame();
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

        //save the current status
        lastStepModel.UpdateData(this.currentScore, PlayerPrefs.GetInt(BestScoreKey, 0), grids);
        regretButton.interactable = true;
        MoveType moveType = CaculateMoveType();
        Debug.Log("move type:" + moveType);
        //move number
        MoveNumber(moveType);
        //create new number
        if (isNeedToCreateNum)
        {
            CreateNumber();
        }

        ResetNumberStatus();
        isNeedToCreateNum = false;

        //check game over
        if (IsGameOver())
        {
            GameOver();
        }
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

    #region ScoreManager

    public void AddScore(int score)
    {
        currentScore += score;
        UpdateScoreText();
        if (currentScore > PlayerPrefs.GetInt(BestScoreKey, 0))
        {
            Debug.Log("New Best Score!");
            PlayerPrefs.SetInt(BestScoreKey, currentScore);
            UpdateBestScoreText(currentScore);
        }
        
    }


    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        scoreText.text = currentScore.ToString();
    }

    public void UpdateBestScoreText(int bestScore)
    {
        bestScoreText.text = bestScore.ToString();
    }

    public void InitPanelInformation()
    {
        //get the grid size
        gridSize = PlayerPrefs.GetInt(Const.GameModel, 4);
        //set the best score mode
        BestScoreKey = Const.BestScore + gridSize;
        Debug.Log("BestScoreKey:" + BestScoreKey);
        UpdateBestScoreText(PlayerPrefs.GetInt(BestScoreKey, 0));
        UpdateScoreText();
        //init the last step model
        lastStepModel = new StepModel();
        regretButton.interactable = false;
    }

    #endregion


    #region Game Process

    public void GameWin()
    {
        Debug.Log("You Win!");
        winPanel.Show();
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        losePanel.Show();
    }

    public void RestartGame()
    {
        //clear score
        ResetScore();
        //clear grid
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                MyNumber number = grids[i][j].GetMyNumber();
                if (number != null)
                {
                    GameObject.Destroy(number.gameObject);
                    grids[i][j].SetMyNumber(null);
                }
            }
        }

        //create new number
        CreateNumber();
    }

    public void BackToLastStep()
    {
        //reset score
        currentScore = lastStepModel.score;
        UpdateScoreText();
        //reset best score
        PlayerPrefs.SetInt(BestScoreKey, lastStepModel.bestScore);
        UpdateBestScoreText(lastStepModel.bestScore);
        //reset grid
        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                //clear grid
                MyNumber number = grids[i][j].GetMyNumber();
                if (number != null)
                {
                    GameObject.Destroy(number.gameObject);
                    grids[i][j].SetMyNumber(null);
                }

                //create new number according to last step model
                if (lastStepModel.numbers[i][j] != 0)
                {
                    CreateNumber(grids[i][j], lastStepModel.numbers[i][j]);
                }
            }
        }

        //reset regret button
        regretButton.interactable = false;
    }

    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
    

    #endregion


    private void Awake()
    {
        AudioManager.Instance.PlayMusic(bgClip);
        InitPanelInformation();
        InitGrid();
        CreateNumber();
    }
}