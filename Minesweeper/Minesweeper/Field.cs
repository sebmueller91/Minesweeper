using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minesweeper
{
  class Field
  {
    private Cell[,] field;
    private int nRows, nCols, nMines;
    private bool initialized = false;

    public int GetRows()
    {
      return nRows;
    }

    public int GetCols()
    {
      return nCols;
    }

    public Cell[,] GetField()
    {
      return field;
    }

    public int GetNumberMines()
    {
      return nMines;
    }

    /// <summary>
    /// returns true if all non-mine-fields have been opened, otherwise false
    /// </summary>
    /// <returns></returns>
    public bool Finished()
    {
      for (int i = 0; i < nRows; i++)
      {
        for (int j = 0; j < nCols; j++)
        {
          if (field[i, j].IsMine() == false && field[i, j].IsOpen() == false)
          {
            return false;
          }
        }
      }

      return true;
    }

    public bool OpenCell(int pos)
    {
      return OpenCell(GetRowFromPos(pos), GetColFromPos(pos));
    }

    /// <summary>
    /// Opens the cell (row, col) and if no mines are in the neighbourhood, the neighbouring fields are opened 
    /// by recursively calling this method
    /// </summary>
    /// <param name="row"></param> Row index of the chosen cell
    /// <param name="col"></param> Column index of the chosen cell
    /// <returns></returns> true if the chosen field is not a mine, false otherwise
    public bool OpenCell(int row, int col)
    {
      if (row < 0 || row >= nRows || col < 0 || col >= nCols || field[row, col].IsOpen() == true)
      {
        return true;
      }

      if (initialized == false)
      {
        InitMines(row, col);
        initialized = true;
      }

      if (field[row, col].IsMine() == true)
      {
        field[row, col].Open();
        return false;
      }
      else
      {
        field[row, col].Open();
      }

      // if no mines in neighbourhood, open neighbouring fields that exist
      if (field[row, col].GetNNeighbouringMines() == 0)
      {
        for (int i = row - 1; i <= row + 1; i++)
        {
          for (int j = col - 1; j <= col + 1; j++)
          {
            if (i >= 0 && i < nRows && j >= 0 && j < nCols && !(i == col && j == row) && field[i, j].IsOpen() == false)
            {
              OpenCell(i, j);
            }
          }
        }
      }

      return true;
    }

    /// <summary>
    /// Initializes the number of rows, columns and mines according to the difficulty
    /// and allocates the memory
    /// </summary>
    /// <param name="difficulty"></param>
    public void StartSession(string difficulty)
    {
      if (difficulty.ToUpper() == "EASY")
      {
        nRows = nCols = 8;
        nMines = 10;
      }
      else if (difficulty.ToUpper() == "NORMAL")
      {
        nRows = nCols = 16;
        nMines = 40;
      }
      else if (difficulty.ToUpper() == "HARD")
      {
        nRows = 24;
        nCols = 30;
        nMines = 99;
      }

      // init cells
      field = new Cell[nRows, nCols];

      for (int i = 0; i < nRows; i++)
      {
        for (int j = 0; j < nCols; j++)
        {
          field[i, j] = new Cell();
        }
      }
    }

    /// <summary>
    /// Initializes the mines in the field at random positions.
    /// StartSession() must have been called before using this method!
    /// </summary>
    /// <param name="fC"></param> denotes the column of the first opened cell
    /// <param name="fR"></param> denotes the row of the first opened cell
    private void InitMines(int fR, int fC)
    {
      List<int> indicesList = new List<int>();
      for (int i = 0; i < nRows; i++)
      {
        for (int j = 0; j < nCols; j++)
        {
          int pos = GetPos(i, j);
          if (((i >= fR - 1) && (i <= fR + 1) && (j >= fC - 1) && (j <= fC + 1)) == false) // TODO
          {
            indicesList.Add(pos);
          }
        }
      }

      for (int i = fR-1; i <= fR+1; i++)
      {
        for (int j = fC-1; j <= fC+1; j++)
        {
          indicesList.Remove(GetPos(i, j));
        }
      }

      int[] indices = indicesList.ToArray();

      Random rand = new Random();
      for (int i = 0; i < indices.Length; i++)
      {
        int j = rand.Next(i, indices.Length);
        int tmp = indices[i];
        indices[i] = indices[j];
        indices[j] = tmp;
      }

      for (int i = 0; i < nMines; i++)
      {
        field[GetRowFromPos(indices[i]), GetColFromPos(indices[i])].SetMine();
      }

      // set mines in neighbourhood
      for (int i = 0; i < nRows; i++)
      {
        for (int j = 0; j < nCols; j++)
        {
          int nMinesAround = 0;
          for (int ii = i - 1; ii <= i + 1; ii++)
          {
            for (int jj = j - 1; jj <= j + 1; jj++)
            {
              if (ii >= 0 && ii < nRows && jj >= 0 && jj < nCols && !(i == ii && j == jj))
              {
                if (field[ii, jj].IsMine())
                {
                  nMinesAround++;
                }
              }
            }
          }
          field[i, j].SetNNeighbouringMines(nMinesAround);
        }
      }
    }

    /// <summary>
    /// Returns the row for the given position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetRowFromPos(int pos)
    {
      return pos / nCols;
    }

    /// <summary>
    /// Returns the column for the given position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetColFromPos(int pos)
    {
      return pos % nCols;
    }

    public int GetPos(int row, int col)
    {
      return row * nCols + col;
    }
  }
}
