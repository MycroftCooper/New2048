using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static GameController_Mode1;

public class Map:MonoBehaviour
{
    private int sideLength;
    public int blockCounter;
    private Block [][] blocks;
    private static Vector3[,] screenPosition=new Vector3[4, 4]
    {
        { new Vector3(-2.0185f, 1.8505f,0f), new Vector3(-0.663f,1.8505f,0f), new Vector3(0.69f,1.8505f,0f),new Vector3(2.026f,1.8505f,0f) },
        { new Vector3(-2.0185f, 0.495f,0f),  new Vector3(-0.663f,0.495f,0f),  new Vector3(0.69f,0.495f,0f), new Vector3(2.026f,0.495f,0f)  },
        { new Vector3(-2.0185f,-0.851f,0f),  new Vector3(-0.663f,-0.851f,0f), new Vector3(0.69f,-0.851f,0f),new Vector3(2.026f,-0.851f,0f) },
        { new Vector3(-2.0185f, -2.2f,0f),   new Vector3(-0.663f,-2.2f,0f),   new Vector3(0.69f,-2.2f,0f),  new Vector3(2.026f,-2.2f,0f)   }
    };
    public int Score { get; set; }
    public int BestScore { get; set; }

    public void InitMap(int sideLength)
    {
        this.sideLength = sideLength;
        blocks = new Block[sideLength][];
        for (int i = 0; i < sideLength; i++)
        {
            blocks[i] = new Block[sideLength];
            for (int j = 0; j < sideLength; j++)
            {
                blocks[i][j] = new Block(new Vector2Int(i, j), screenPosition[i, j]);
            }
        }
        blockCounter = 0;
        transform.position = originalPosition;
    }
    public void DestroyMap()
    {
        foreach(Block[] i in blocks)
        {
            foreach(Block j in i)
            {
                if(!j.isEmpty())
                {
                    GameObject.Destroy(j.Entity.gameObject);
                }
            }
        }
    }

    public void addNewNumber()
    {
        //在哪里生成
        int putWhere = Random.Range(0, 15 - blockCounter);
        int counter = 0;
        for (int i = 0; i < sideLength; i++)
        {
            for (int j = 0; j < sideLength; j++)
            {
                if (!blocks[i][j].isEmpty()) continue;
                if (counter == putWhere)
                {
                    blocks[i][j].Entity = EntityControllerMode1.createNewEntity(blocks[i][j]);
                    blockCounter++;
                    return;
                }
                else counter++;
            }
        }
    }
    private void mergeNumber(Block blockA,Block blockB)
    {
        blockA.Entity.isChanged = true;
        Score += blockB.Entity.Num;
        GameObject.Destroy(blockB.Entity.gameObject);
        blockCounter--;
        blockB.Entity = blockA.Entity;
        blockA.Entity = null;
        blockB.Entity.doEntityMove(blockB);
        blockB.Entity.Num *= 2;
    }
    private void moveNumber(Block blockA, Block blockB)//AtoB
    {
        blockB.Entity = blockA.Entity;
        blockA.Entity = null;
        blockB.Entity.doEntityMove(blockB);
    }

    public bool isNowMoving()
    {
        if (transform.localPosition != originalPosition)
            return true;
        for (int i = 0; i < sideLength;i++)
        {
            for(int j=0;j<sideLength;j++)
            {
                if (!blocks[i][j].isEmpty() &&
                    !blocks[i][j].Entity.isOnPosition( blocks[i][j].ScreenPosition)
                    )
                {
                    return true;
                } 
            }
        }
        return false;
    }
    public bool moveUp()
    {
        bool isMoved = false;
        for (int row = 1; row < sideLength; row++)
        {
            for (int col = 0; col < sideLength; col++)
            {
                if (blocks[row][col].isEmpty()) continue;
                int nextRow = row - 1;
                while (blocks[nextRow][col].isEmpty() && nextRow != 0)
                    nextRow--;
                if (!blocks[nextRow][col].isEmpty())
                {
                    if (blocks[row][col].Entity.Num == blocks[nextRow][col].Entity.Num && !blocks[nextRow][col].Entity.isChanged)
                    {
                        mergeNumber(blocks[row][col], blocks[nextRow][col]);
                        isMoved = true;
                        continue; 
                    }
                    else
                    {
                        if (nextRow == (row - 1)) continue;
                        nextRow++;
                    }
                }
                moveNumber(blocks[row][col], blocks[nextRow][col]);
                isMoved = true;
            }
        }
        return isMoved;
    }
    public bool moveDown()
    {
        bool isMoved = false;
        for (int row = sideLength-2; row >=0; row--)
        {
            for (int col = 0; col < sideLength; col++)
            {
                if (blocks[row][col].isEmpty()) continue;
                int nextRow = row + 1;
                while (blocks[nextRow][col].isEmpty() && nextRow != sideLength-1)
                    nextRow++;
                if (!blocks[nextRow][col].isEmpty())
                {
                    if (blocks[row][col].Entity.Num == blocks[nextRow][col].Entity.Num && !blocks[nextRow][col].Entity.isChanged)
                    {
                        mergeNumber(blocks[row][col], blocks[nextRow][col]);
                        isMoved = true;
                        continue;
                    }
                    else
                    {
                        if (nextRow == (row + 1)) continue;
                        nextRow--;
                    }
                }
                moveNumber(blocks[row][col], blocks[nextRow][col]);
                isMoved = true;
            }
        }
        return isMoved;
    }
    public bool moveLeft()
    {
        bool isMoved = false;
        for (int row = 0; row < sideLength; row++)
        {
            for (int col = 1; col < sideLength; col++)
            {
                if (blocks[row][col].isEmpty()) continue;
                int nextCol = col - 1;
                while (blocks[row][nextCol].isEmpty() && nextCol != 0)
                    nextCol--;
                if (!blocks[row][nextCol].isEmpty())
                {
                    if (blocks[row][col].Entity.Num == blocks[row][nextCol].Entity.Num && !blocks[row][nextCol].Entity.isChanged)
                    {
                        mergeNumber(blocks[row][col], blocks[row][nextCol]);
                        isMoved = true;
                        continue;
                    }
                    else
                    {
                        if (nextCol == (col - 1)) continue;
                        nextCol++;
                    }
                }
                moveNumber(blocks[row][col], blocks[row][nextCol]);
                isMoved = true;
            }
        }
        return isMoved;
    }
    public bool moveRight()
    {
        bool isMoved = false;
        for (int row = 0; row < sideLength; row++)
        {
            for (int col = sideLength - 2; col >=0; col--)
            {
                if (blocks[row][col].isEmpty()) continue;
                int nextCol = col + 1;
                while (blocks[row][nextCol].isEmpty() && nextCol != sideLength - 1)
                    nextCol++;
                if (!blocks[row][nextCol].isEmpty())
                {
                    if (blocks[row][col].Entity.Num == blocks[row][nextCol].Entity.Num && !blocks[row][nextCol].Entity.isChanged)
                    {
                        mergeNumber(blocks[row][col], blocks[row][nextCol]);
                        isMoved = true;
                        continue;
                    }
                    else
                    {
                        if (nextCol == (col + 1)) continue;
                        nextCol--;
                    }
                }
                moveNumber(blocks[row][col], blocks[row][nextCol]);
                isMoved = true;
            }
        }
        return isMoved;
    }

    static Vector3 originalPosition = new Vector3(0f, 0f, -1);
    static Vector3 upPosition = new Vector3(0f, 1f, -1);
    static Vector3 downPosition = new Vector3(0f, -1f, -1);
    static Vector3 leftPosition = new Vector3(-1f, 0f, -1);
    static Vector3 rightPosition = new Vector3(1f, 0f, -1);
    public void moveMap(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Up:
                StartCoroutine (moveMap(upPosition));
                break;
            case MoveDirection.Down:
                StartCoroutine(moveMap(downPosition));
                break;
            case MoveDirection.Left:
                StartCoroutine(moveMap(leftPosition));
                break;
            case MoveDirection.Right:
                StartCoroutine(moveMap(rightPosition));
                break;
        }
    }
    private IEnumerator moveMap(Vector3 targetPosition)
    {
        while (isNowMoving())
        {
            yield return 0;
        }
        yield return StartCoroutine(TransfomTools.moveToPosition(transform, targetPosition, 20));
        StartCoroutine(TransfomTools.moveToPosition(transform,originalPosition,20));
    }

    public bool isLose()
    {
        if (blockCounter != (int)Mathf.Pow((float)sideLength, 2f)) return false;
        for(int i=0;i<sideLength;i++)
        {
            for(int j=0;j<sideLength-1;j++)
            {
                if (blocks[i][j].Entity.Num == blocks[i][j + 1].Entity.Num) return false;
                if(blocks[j][i].Entity.Num == blocks[j+1][i].Entity.Num) return false;
            }
        }
        return true;
    }

    public int[][] saveMap()
    {
        int[][] save = new int[sideLength][];
        for (int i = 0; i < sideLength; i++)
        {
            save[i] = new int[sideLength];
            for (int j = 0; j < sideLength; j++)
            {
                if (blocks[i][j].isEmpty()) save[i][j] = 0;
                else save[i][j] = blocks[i][j].Entity.Num;
            }
        }
        return save;
    }
    public void loadMap(int[][] save)
    {
        InitMap(4);
        for (int i = 0; i < sideLength; i++)
        {
            for (int j = 0; j < sideLength; j++)
            {
                if (save[i][j] == 0) 
                    blocks[i][j].Entity = null;
                else
                {
                    blocks[i][j].Entity = EntityControllerMode1.createNewEntity(blocks[i][j], save[i][j]);
                    blockCounter++;
                }
            }
        }
    }
}
