using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.InputSystem;

public enum PlayerOption
{
    NONE, //0
    X, // 1
    O // 2
}

public class TTT : MonoBehaviour
{
    public int Rows;
    public int Columns;
    [SerializeField] BoardView board;

    PlayerOption currentPlayer = PlayerOption.X;
    Cell[,] cells;

    // Start is called before the first frame update
    void Start()
    {
        cells = new Cell[Columns, Rows];

        board.InitializeBoard(Columns, Rows);

        for(int i = 0; i < Rows; i++)
        {
            for(int j = 0; j < Columns; j++)
            {
                cells[j, i] = new Cell();
                cells[j, i].current = PlayerOption.NONE;
            }
        }
        
    }

    public void OnMakeOptimalMove()
    {
        switch(currentPlayer)
        {
            case PlayerOption.X:
                if (WinAsX())
                {
                    break;
                }
                else if (BlockO())
                {
                    break;
                }
                else if (AttemptGoOnMiddle())
                {
                    break;
                }
                else if (FromCornerChooseAdjacentCellAsX())
                {
                    break;
                }
                else if (ChooseRandomCorner())
                {
                    break;
                }
                else
                {
                    RandomPossibleCell();
                    break;
                }
            case PlayerOption.O:
                if (WinAsO())
                {
                    break;
                }
                else if (BlockX())
                {
                    break;
                }
                else if (AttemptGoOnMiddle())
                {
                   break;
                }
                //else if (FromCornerChooseAdjacentCellAsO())
                //{
                    //break;
                //}
                else if (ChooseRandomCorner())
                {
                    break;
                }
                else
                {
                    RandomPossibleCell();
                    break;
                }
        }
    }

    public void OnReset()
    {
        board.BoardViewReset(Rows, Columns);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
                cells[j, i].current = PlayerOption.NONE;
        }
        currentPlayer = PlayerOption.X;
    }

    bool IsItEmptyBoard()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if(cells[j, i].current != PlayerOption.NONE)
                    return false;
            }
        }
        return true;
    }

    int RNG(int minInclusive, int maxExclusive)
    {
        int randonNumber = UnityEngine.Random.Range(minInclusive, maxExclusive);
        return randonNumber;
    }

    bool CheckIfAllCornersAreEmpty()
    {
        if(cells[0, 0].current == PlayerOption.NONE && cells[2, 0].current == PlayerOption.NONE && cells[0, 2].current == PlayerOption.NONE && cells[2, 2].current == PlayerOption.NONE)
            return true;
        return false;
    }

    bool ChooseRandomCorner()
    {
        if(cells[0, 0].current == PlayerOption.NONE || cells[2, 0].current == PlayerOption.NONE || cells[0, 2].current == PlayerOption.NONE || cells[2, 2].current == PlayerOption.NONE)
        {
            bool foundEmpty = false;
            Debug.Log("Random Corner");
            while (!foundEmpty)
            {
                int randonNumber = UnityEngine.Random.Range(0, 4);
                if (randonNumber == 0 && cells[0, 0].current == PlayerOption.NONE)
                {
                    foundEmpty = true;
                    ChooseSpace(0, 0);
                    return true;
                }
                else if (randonNumber == 1 && cells[2, 0].current == PlayerOption.NONE)
                {
                    foundEmpty = true;
                    ChooseSpace(2, 0);
                    return true;
                }
                else if (randonNumber == 2 && cells[0, 2].current == PlayerOption.NONE)
                {
                    foundEmpty = true;
                    ChooseSpace(0, 2);
                    return true;
                }
                else if (randonNumber == 3 && cells[2, 2].current == PlayerOption.NONE)
                {
                    foundEmpty = true;
                    ChooseSpace(2, 2);
                    return true;
                }
            }
        }
        return false;
    }

    bool AttemptGoOnMiddle()
    {
        if (cells[1, 1].current == PlayerOption.NONE)
        {
            //if (cells[0, 0].current == PlayerOption.X || cells[2, 0].current == PlayerOption.X || cells[0, 2].current == PlayerOption.X || cells[2, 2].current == PlayerOption.X)
            ChooseSpace(1, 1);
            return true;
        }
        return false;
    }

    void RandomPossibleCell()
    {
        int countOne = 0;
        int randomNumber = 0;
        int countTwo = 0;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (cells[j, i].current == PlayerOption.NONE)
                {
                    countOne++;
                }
            }
        }
        Debug.Log(countOne);    
        randomNumber = UnityEngine.Random.Range(0, countOne + 1);
        Debug.Log(randomNumber);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if(cells[j, i].current == PlayerOption.NONE)
                {
                    if (randomNumber == countTwo)
                    {
                        ChooseSpace(j, i);
                        Debug.Log("Random");
                        return;
                    }
                    countTwo++;
                }
            }
        }
    }

    bool BlockO()
    {
        Debug.Log("hi");
        for(int i = 0; i < 3; i++) // O O ! All across the board (horizontal, left-right read)
        {
            if(cells[0, i].current == PlayerOption.O && cells[1, i].current == PlayerOption.O)
            {
                if(cells[2, i].current != PlayerOption.X)
                {
                    ChooseSpace(2, i);
                    Debug.Log("1");
                    return true;
                }
            }
        }
        for(int i = 0; i < 3; i++) // ! O O All across the board (horizontal, left-right read)
        {
            if (cells[1,i].current == PlayerOption.O && cells[2, i].current == PlayerOption.O)
            {
                if (cells[0, i].current != PlayerOption.X)
                {
                    ChooseSpace(0, i);
                    Debug.Log("2");
                    return true;
                }
            }
        }
        for(int i = 0; i < 3; i++) // O O ! All across the board (vertical, top-bottom read)
        {
            if (cells[i,0].current == PlayerOption.O && cells[i,1].current == PlayerOption.O)
            {
                if (cells[i, 2].current != PlayerOption.X)
                {
                    ChooseSpace(i, 2);
                    Debug.Log("3");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // ! O O All across the board (vertical, top-bottom read)
        {
            if (cells[i, 1].current == PlayerOption.O && cells[i, 2].current == PlayerOption.O)
            {
                if (cells[i, 0].current != PlayerOption.X)
                {
                    ChooseSpace(i, 0);
                    Debug.Log("4");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // O ! O All across the board (horizontal, left-right read)
        {
            if (cells[0, i].current == PlayerOption.O && cells[2, i].current == PlayerOption.O)
            {
                if (cells[1, i].current != PlayerOption.X)
                {
                    ChooseSpace(1, i);
                    Debug.Log("9");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // O ! O All across the board (vertical, top-bottom read)
        {
            if (cells[i, 0].current == PlayerOption.O && cells[i, 2].current == PlayerOption.O)
            {
                if (cells[i, 1].current != PlayerOption.X)
                {
                    ChooseSpace(i, 1);
                    Debug.Log("10");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i += 2) // All diagonal cases
        {
            for (int x = 0; x < 3; x += 2)
            {
                if (cells[i, x].current == PlayerOption.O && cells[1, 1].current == PlayerOption.O)
                {
                    if(i == 2 && x == 2)
                    {
                        if (cells[i-2, x-2].current != PlayerOption.X)
                        {
                            ChooseSpace(i-2, x-2);
                            Debug.Log("5");
                            return true;
                        }
                    }
                    else if(i == 2 && x == 0)
                    {
                        if (cells[i-2, x+2].current != PlayerOption.X)
                        {
                            ChooseSpace(i-2, x+2);
                            Debug.Log("6");
                            return true;
                        }
                    }
                    else if(i == 0 && x == 2)
                    {
                        if (cells[i+2, x-2].current != PlayerOption.X)
                        {
                            ChooseSpace(i+2, x-2);
                            Debug.Log("7");
                            return true;
                        }
                    }
                    else if(i == 2 && x == 0)
                    {
                        if (cells[i-2, x+2].current != PlayerOption.X)
                        {
                            ChooseSpace(i-2, x+2);
                            Debug.Log("8");
                            return true;
                        }
                    }
                }
            }
        }
        Debug.Log("hi2");
        return false;
    }

    bool BlockX()
    {
        Debug.Log("hi");
        for (int i = 0; i < 3; i++) // X X ! All across the board (horizontal, left-right read)
        {
            if (cells[0, i].current == PlayerOption.X && cells[1, i].current == PlayerOption.X)
            {
                if (cells[2, i].current != PlayerOption.O)
                {
                    ChooseSpace(2, i);
                    Debug.Log("1");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // ! X X All across the board (horizontal, left-right read)
        {
            if (cells[1, i].current == PlayerOption.X && cells[2, i].current == PlayerOption.X)
            {
                if (cells[0, i].current != PlayerOption.O)
                {
                    ChooseSpace(0, i);
                    Debug.Log("2");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // X X ! All across the board (vertical, top-bottom read)
        {
            if (cells[i, 0].current == PlayerOption.X && cells[i, 1].current == PlayerOption.X)
            {
                if (cells[i, 2].current != PlayerOption.O)
                {
                    ChooseSpace(i, 2);
                    Debug.Log("3");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // ! X X All across the board (vertical, top-bottom read)
        {
            if (cells[i, 1].current == PlayerOption.X && cells[i, 2].current == PlayerOption.X)
            {
                if (cells[i, 0].current != PlayerOption.O)
                {
                    ChooseSpace(i, 0);
                    Debug.Log("4");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // X ! X All across the board (horizontal, left-right read)
        {
            if (cells[0, i].current == PlayerOption.X && cells[2, i].current == PlayerOption.X)
            {
                if (cells[1, i].current != PlayerOption.O)
                {
                    ChooseSpace(1, i);
                    Debug.Log("9");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // X ! X All across the board (vertical, top-bottom read)
        {
            if (cells[i, 0].current == PlayerOption.X && cells[i, 2].current == PlayerOption.X)
            {
                if (cells[i, 1].current != PlayerOption.O)
                {
                    ChooseSpace(i, 1);
                    Debug.Log("10");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i += 2) // All diagonal cases
        {
            for (int x = 0; x < 3; x += 2)
            {
                if (cells[i, x].current == PlayerOption.X && cells[1, 1].current == PlayerOption.X)
                {
                    if (i == 2 && x == 2)
                    {
                        if (cells[i - 2, x - 2].current != PlayerOption.O)
                        {
                            ChooseSpace(i - 2, x - 2);
                            Debug.Log("5");
                            return true;
                        }
                    }
                    else if (i == 2 && x == 0)
                    {
                        if (cells[i - 2, x + 2].current != PlayerOption.O)
                        {
                            ChooseSpace(i - 2, x + 2);
                            Debug.Log("6");
                            return true;
                        }
                    }
                    else if (i == 0 && x == 2)
                    {
                        if (cells[i + 2, x - 2].current != PlayerOption.O)
                        {
                            ChooseSpace(i + 2, x - 2);
                            Debug.Log("7");
                            return true;
                        }
                    }
                    else if (i == 2 && x == 0)
                    {
                        if (cells[i - 2, x + 2].current != PlayerOption.O)
                        {
                            ChooseSpace(i - 2, x + 2);
                            Debug.Log("8");
                            return true;
                        }
                    }
                }
            }
        }
        Debug.Log("hi2");
        return false;
    }

    bool WinAsX()
    {
        for (int i = 0; i < 3; i++) // X X ! All across the board (horizontal, left-right read)
        {
            if (cells[0, i].current == PlayerOption.X && cells[1, i].current == PlayerOption.X)
            {
                if (cells[2, i].current != PlayerOption.O)
                {
                    ChooseSpace(2, i);
                    Debug.Log("won 1");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // ! X X All across the board (horizontal, left-right read)
        {
            if (cells[1, i].current == PlayerOption.X && cells[2, i].current == PlayerOption.X)
            {
                if (cells[0, i].current != PlayerOption.O)
                {
                    ChooseSpace(0, i);
                    Debug.Log("won 2");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // X X ! All across the board (vertical, top-bottom read)
        {
            if (cells[i, 0].current == PlayerOption.X && cells[i, 1].current == PlayerOption.X)
            {
                if (cells[i, 2].current != PlayerOption.O)
                {
                    ChooseSpace(i, 2);
                    Debug.Log("won 3");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // ! X X All across the board (vertical, top-bottom read)
        {
            if (cells[i, 1].current == PlayerOption.X && cells[i, 2].current == PlayerOption.X)
            {
                if (cells[i, 0].current != PlayerOption.O)
                {
                    ChooseSpace(i, 0);
                    Debug.Log("won 4");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // X ! X All across the board (horizontal, left-right read)
        {
            if (cells[0, i].current == PlayerOption.X && cells[2, i].current == PlayerOption.X)
            {
                if (cells[1, i].current != PlayerOption.O)
                {
                    ChooseSpace(1, i);
                    Debug.Log("won 9");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // X ! X All across the board (vertical, top-bottom read)
        {
            if (cells[i, 0].current == PlayerOption.X && cells[i, 2].current == PlayerOption.X)
            {
                if (cells[i, 1].current != PlayerOption.O)
                {
                    ChooseSpace(i, 1);
                    Debug.Log("won 10");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i += 2) // All diagonal cases
        {
            for (int x = 0; x < 3; x += 2)
            {
                if (cells[i, x].current == PlayerOption.X && cells[1, 1].current == PlayerOption.X)
                {
                    if (i == 2 && x == 2)
                    {
                        if (cells[i - 2, x - 2].current != PlayerOption.O)
                        {
                            ChooseSpace(i - 2, x - 2);
                            Debug.Log("won 5");
                            return true;
                        }
                    }
                    else if (i == 2 && x == 0)
                    {
                        if (cells[i - 2, x + 2].current != PlayerOption.O)
                        {
                            ChooseSpace(i - 2, x + 2);
                            Debug.Log("won 6");
                            return true;
                        }
                    }
                    else if (i == 0 && x == 2)
                    {
                        if (cells[i + 2, x - 2].current != PlayerOption.O)
                        {
                            ChooseSpace(i + 2, x - 2);
                            Debug.Log("won 7");
                            return true;
                        }
                    }
                    else if (i == 2 && x == 0)
                    {
                        if (cells[i - 2, x + 2].current != PlayerOption.O)
                        {
                            ChooseSpace(i - 2, x + 2);
                            Debug.Log("won 8");
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    bool WinAsO()
    {
        for (int i = 0; i < 3; i++) // O O ! All across the board (horizontal, left-right read)
        {
            if (cells[0, i].current == PlayerOption.O && cells[1, i].current == PlayerOption.O)
            {
                if (cells[2, i].current != PlayerOption.X)
                {
                    ChooseSpace(2, i);
                    Debug.Log("won 1");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // ! O O All across the board (horizontal, left-right read)
        {
            if (cells[1, i].current == PlayerOption.O && cells[2, i].current == PlayerOption.O)
            {
                if (cells[0, i].current != PlayerOption.X)
                {
                    ChooseSpace(0, i);
                    Debug.Log("won 2");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // O O ! All across the board (vertical, top-bottom read)
        {
            if (cells[i, 0].current == PlayerOption.O && cells[i, 1].current == PlayerOption.O)
            {
                if (cells[i, 2].current != PlayerOption.X)
                {
                    ChooseSpace(i, 2);
                    Debug.Log("won 3");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // ! O O All across the board (vertical, top-bottom read)
        {
            if (cells[i, 1].current == PlayerOption.O && cells[i, 2].current == PlayerOption.O)
            {
                if (cells[i, 0].current != PlayerOption.X)
                {
                    ChooseSpace(i, 0);
                    Debug.Log("won 4");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // O ! O All across the board (horizontal, left-right read)
        {
            if (cells[0, i].current == PlayerOption.O && cells[2, i].current == PlayerOption.O)
            {
                if (cells[1, i].current != PlayerOption.X)
                {
                    ChooseSpace(1, i);
                    Debug.Log("won 9");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i++) // O ! O All across the board (vertical, top-bottom read)
        {
            if (cells[i, 0].current == PlayerOption.O && cells[i, 2].current == PlayerOption.O)
            {
                if (cells[i, 1].current != PlayerOption.X)
                {
                    ChooseSpace(i, 1);
                    Debug.Log("won 10");
                    return true;
                }
            }
        }
        for (int i = 0; i < 3; i += 2) // All diagonal cases
        {
            for (int x = 0; x < 3; x += 2)
            {
                if (cells[i, x].current == PlayerOption.O && cells[1, 1].current == PlayerOption.O)
                {
                    if (i == 2 && x == 2)
                    {
                        if (cells[i - 2, x - 2].current != PlayerOption.X)
                        {
                            ChooseSpace(i - 2, x - 2);
                            Debug.Log("won 5");
                            return true;
                        }
                    }
                    else if (i == 2 && x == 0)
                    {
                        if (cells[i - 2, x + 2].current != PlayerOption.X)
                        {
                            ChooseSpace(i - 2, x + 2);
                            Debug.Log("won 6");
                            return true;
                        }
                    }
                    else if (i == 0 && x == 2)
                    {
                        if (cells[i + 2, x - 2].current != PlayerOption.X)
                        {
                            ChooseSpace(i + 2, x - 2);
                            Debug.Log("won 7");
                            return true;
                        }
                    }
                    else if (i == 2 && x == 0)
                    {
                        if (cells[i - 2, x + 2].current != PlayerOption.X)
                        {
                            ChooseSpace(i - 2, x + 2);
                            Debug.Log("won 8");
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    bool FromCornerChooseAdjacentCellAsX()
    {
        if (cells[0, 0].current == PlayerOption.X || cells[2, 0].current == PlayerOption.X || cells[0, 2].current == PlayerOption.X || cells[2, 2].current == PlayerOption.X)
        {
            Debug.Log("Adjacent X");
            bool foundEmpty = false;
            while (!foundEmpty)
            {
                int randonNumber = UnityEngine.Random.Range(0, 4);
                if (randonNumber == 0 && cells[0, 0].current == PlayerOption.X && (cells[1, 0].current == PlayerOption.NONE || cells[0, 1].current == PlayerOption.NONE))
                {
                    int randomNumberTwo = UnityEngine.Random.Range(0, 2);
                    if (randomNumberTwo == 0 && cells[1, 0].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(1, 0);
                        return true;
                    }
                    else if (randomNumberTwo == 1 && cells[0, 1].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(0, 1);
                        return true;
                    }
                }
                else if (randonNumber == 1 && cells[2, 0].current == PlayerOption.X && (cells[1, 0].current == PlayerOption.NONE || cells[2, 1].current == PlayerOption.NONE))
                {
                    int randomNumberTwo = UnityEngine.Random.Range(0, 2);
                    if (randomNumberTwo == 0 && cells[1, 0].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(1, 0);
                        return true;
                    }
                    else if (randomNumberTwo == 1 && cells[2, 1].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(2, 1);
                        return true;
                    }
                }
                else if (randonNumber == 2 && cells[2, 2].current == PlayerOption.X && (cells[1, 2].current == PlayerOption.NONE || cells[2, 1].current == PlayerOption.NONE))
                {
                    int randomNumberTwo = UnityEngine.Random.Range(0, 2);
                    if (randomNumberTwo == 0 && cells[1, 2].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(1, 2);
                        return true;
                    }
                    else if (randomNumberTwo == 1 && cells[2, 1].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(2, 1);
                        return true;
                    }
                }
                else if (randonNumber == 3 && cells[0, 2].current == PlayerOption.X && (cells[1, 2].current == PlayerOption.NONE || cells[0, 1].current == PlayerOption.NONE))
                {
                    int randomNumberTwo = UnityEngine.Random.Range(0, 2);
                    if (randomNumberTwo == 0 && cells[1, 2].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(1, 2);
                        return true;
                    }
                    else if (randomNumberTwo == 1 && cells[0, 1].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(0, 1);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    bool FromCornerChooseAdjacentCellAsO()
    {
        if (cells[0, 0].current == PlayerOption.O || cells[2, 0].current == PlayerOption.O || cells[0, 2].current == PlayerOption.O || cells[2, 2].current == PlayerOption.O)
        {
            Debug.Log("Adjacent O");
            bool foundEmpty = false;
            while (!foundEmpty)
            {
                int randonNumber = UnityEngine.Random.Range(0, 4);
                if (randonNumber == 0 && cells[0, 0].current == PlayerOption.O && (cells[1, 0].current == PlayerOption.NONE || cells[0, 1].current == PlayerOption.NONE))
                {
                    int randomNumberTwo = UnityEngine.Random.Range(0, 2);
                    if (randomNumberTwo == 0 && cells[1, 0].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(1, 0);
                        return true;
                    }
                    else if (randomNumberTwo == 1 && cells[0, 1].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(0, 1);
                        return true;
                    }
                }
                else if (randonNumber == 1 && cells[2, 0].current == PlayerOption.O && (cells[1, 0].current == PlayerOption.NONE || cells[2, 1].current == PlayerOption.NONE))
                {
                    int randomNumberTwo = UnityEngine.Random.Range(0, 2);
                    if (randomNumberTwo == 0 && cells[1, 0].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(1, 0);
                        return true;
                    }
                    else if (randomNumberTwo == 1 && cells[2, 1].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(2, 1);
                        return true;
                    }
                }
                else if (randonNumber == 2 && cells[2, 2].current == PlayerOption.O && (cells[1, 2].current == PlayerOption.NONE || cells[2, 1].current == PlayerOption.NONE))
                {
                    int randomNumberTwo = UnityEngine.Random.Range(0, 2);
                    if (randomNumberTwo == 0 && cells[1, 2].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(1, 2);
                        return true;
                    }
                    else if (randomNumberTwo == 1 && cells[2, 1].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(2, 1);
                        return true;
                    }
                }
                else if (randonNumber == 3 && cells[0, 2].current == PlayerOption.O && (cells[1, 2].current == PlayerOption.NONE || cells[0, 1].current == PlayerOption.NONE))
                {
                    int randomNumberTwo = UnityEngine.Random.Range(0, 2);
                    if (randomNumberTwo == 0 && cells[1, 2].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(1, 2);
                        return true;
                    }
                    else if (randomNumberTwo == 1 && cells[0, 1].current == PlayerOption.NONE)
                    {
                        foundEmpty = true;
                        ChooseSpace(0, 1);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    PlayerOption GetOtherPlayer()
    {
        switch(currentPlayer)
        {
            case PlayerOption.X:
                return PlayerOption.O;
            case PlayerOption.O:
                return PlayerOption.X;
            default:
                return PlayerOption.NONE;
        }
    }

    public void ChooseSpace(int column, int row)
    {
        // can't choose space if game is over
        if (GetWinner() != PlayerOption.NONE)
            return;

        // can't choose a space that's already taken
        if (cells[column, row].current != PlayerOption.NONE)
            return;

        // set the cell to the player's mark
        cells[column, row].current = currentPlayer;

        // update the visual to display X or O
        board.UpdateCellVisual(column, row, currentPlayer);

        // if there's no winner, keep playing, otherwise end the game
        if(GetWinner() == PlayerOption.NONE)
            EndTurn();
        else
        {
            Debug.Log("GAME OVER!");
        }
    }

    public void EndTurn()
    {
        // increment player, if it goes over player 2, loop back to player 1
        currentPlayer += 1;
        if ((int)currentPlayer > 2)
            currentPlayer = PlayerOption.X;
    }

    public PlayerOption GetWinner()
    {
        // sum each row/column based on what's in each cell X = 1, O = -1, blank = 0
        // we have a winner if the sum = 3 (X) or -3 (O)
        int sum = 0;

        // check rows
        for (int i = 0; i < Rows; i++)
        {
            sum = 0;
            for (int j = 0; j < Columns; j++)
            {
                var value = 0;
                if (cells[j, i].current == PlayerOption.X)
                    value = 1;
                else if (cells[j, i].current == PlayerOption.O)
                    value = -1;

                sum += value;
            }

            if (sum == 3)
                return PlayerOption.X;
            else if (sum == -3)
                return PlayerOption.O;

        }

        // check columns
        for (int j = 0; j < Columns; j++)
        {
            sum = 0;
            for (int i = 0; i < Rows; i++)
            {
                var value = 0;
                if (cells[j, i].current == PlayerOption.X)
                    value = 1;
                else if (cells[j, i].current == PlayerOption.O)
                    value = -1;

                sum += value;
            }

            if (sum == 3)
                return PlayerOption.X;
            else if (sum == -3)
                return PlayerOption.O;

        }

        // check diagonals
        // top left to bottom right
        sum = 0;
        for(int i = 0; i < Rows; i++)
        {
            int value = 0;
            if (cells[i, i].current == PlayerOption.X)
                value = 1;
            else if (cells[i, i].current == PlayerOption.O)
                value = -1;

            sum += value;
        }

        if (sum == 3)
            return PlayerOption.X;
        else if (sum == -3)
            return PlayerOption.O;

        // top right to bottom left
        sum = 0;
        for (int i = 0; i < Rows; i++)
        {
            int value = 0;

            if (cells[Columns - 1 - i, i].current == PlayerOption.X)
                value = 1;
            else if (cells[Columns - 1 - i, i].current == PlayerOption.O)
                value = -1;

            sum += value;
        }

        if (sum == 3)
            return PlayerOption.X;
        else if (sum == -3)
            return PlayerOption.O;

        return PlayerOption.NONE;
    }
}
