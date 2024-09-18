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
                if(IsItEmptyBoard())            // Perfect 1st Turn
                {
                    ChooseRandomCorner();
                    break;
                }
                if (AttemptToBlockO())
                    break;
                randomPossibleCell();
                break;
            case PlayerOption.O:
                if(CanOGoOnMiddle())            // Perfect 1st Turn
                {
                    ChooseSpace(1, 1);
                }
                else if(CheckIfAllCornersAreEmpty()) // 2nd turn if X took the middle
                {
                    ChooseRandomCorner();
                }
                break;
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

    void ChooseRandomCorner()
    {
        int randonNumber = UnityEngine.Random.Range(0, 4);
        if (randonNumber == 0)
            ChooseSpace(0, 0);
        else if (randonNumber == 1)
            ChooseSpace(2, 0);
        else if (randonNumber == 2)
            ChooseSpace(0, 2);
        else if (randonNumber == 3)
            ChooseSpace(2, 2);
    }

    bool CanOGoOnMiddle()
    {
        if (cells[1, 1].current == PlayerOption.NONE)
        {
            if (cells[0, 0].current == PlayerOption.X || cells[2, 0].current == PlayerOption.X || cells[0, 2].current == PlayerOption.X || cells[2, 2].current == PlayerOption.X)
                return true;
        }
        return false;

    }

    void randomPossibleCell()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (cells[j, i].current != PlayerOption.NONE)
                    ChooseSpace(j, i);
            }
        }
    }

    bool AttemptToBlockO()
    {
        //if      (cells[0, 0].current == PlayerOption.O && cells[1, 0].current == PlayerOption.O)
        //{
        //ChooseSpace(2, 0); // O O ! > ? ? ? > ? ? ?
        //}
        //else if (cells[0, 1].current == PlayerOption.O && cells[1, 1].current == PlayerOption.O)
        //{
        //ChooseSpace(2, 1); // ? ? ? > O O ! > ? ? ?
        //}
        //else if (cells[0, 2].current == PlayerOption.O && cells[1, 2].current == PlayerOption.O)
        //{
        //ChooseSpace(2, 2); // ? ? ? > ? ? ? > O O !
        //}
        //else if (cells[1, 0].current == PlayerOption.O && cells[2, 0].current == PlayerOption.O)
        //{
        //ChooseSpace(0, 0); // ! O O > ? ? ? > ? ? ?
        //}
        //else if (cells[1, 1].current == PlayerOption.O && cells[2, 1].current == PlayerOption.O)
        //{
        //ChooseSpace(0, 1); // ? ? ? > ! O O > ? ? ?
        //}
        //else if (cells[1, 2].current == PlayerOption.O && cells[2, 2].current == PlayerOption.O)
        //{
        //ChooseSpace(0, 2); // ? ? ? > ? ? ? > ! O O
        //}
        Debug.Log("hi");
        for(int i = 0; i < 2; i++) // O O ! All across the board (horizontal, left-right read)
        {
            if(cells[0, i].current == PlayerOption.O && cells[1, i].current == PlayerOption.O)
            {
                if(cells[2, i].current != PlayerOption.X)
                {
                    ChooseSpace(2, i);
                    return true;
                }
            }
        }
        for(int i = 0; i < 2; i++) // ! O O All across the board (horizontal, left-right read)
        {
            if (cells[1,i].current == PlayerOption.O && cells[2, i].current == PlayerOption.O)
            {
                if (cells[0, i].current != PlayerOption.X)
                {
                    ChooseSpace(0, i);
                    return true;
                }
            }
        }
        for(int i = 0; i < 2; i++) // O O ! All across the board (vertical, top-bottom read)
        {
            if (cells[i,0].current == PlayerOption.O && cells[i,1].current == PlayerOption.O)
            {
                if (cells[i, 2].current != PlayerOption.X)
                {
                    ChooseSpace(i, 2);
                    return true;
                }
            }
        }
        for (int i = 1; i < 3; i++) // ! O O All across the board (vertical, top-bottom read)
        {
            if (cells[i, 1].current == PlayerOption.O && cells[i, 2].current == PlayerOption.O)
            {
                if (cells[i, 0].current != PlayerOption.X)
                {
                    ChooseSpace(i, 0);
                    return true;
                }
            }
        }
        for(int i = 0; i < 3; i += 2) // All diagonal cases
        {
            for (int x = 0; x < 3; x += 2)
            {
                if (cells[i, x].current == PlayerOption.O && cells[1, 1].current == PlayerOption.O)
                {
                    if(i == 2 && x == 2)
                    {
                        if (cells[i, x].current != PlayerOption.X)
                        {
                            ChooseSpace(i, x);
                            return true;
                        }
                    }
                    else if(i == 2 && x == 0)
                    {
                        if (cells[i, x].current != PlayerOption.X)
                        {
                            ChooseSpace(i, x);
                            return true;
                        }
                    }
                    else if(i == 0 && x == 2)
                    {
                        if (cells[i, x].current != PlayerOption.X)
                        {
                            ChooseSpace(i, x);
                            return true;
                        }
                    }
                    else if(i == 2 && x == 0)
                    {
                        if (cells[i, x].current != PlayerOption.X)
                        {
                            ChooseSpace(i, x);
                            return true;
                        }
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
