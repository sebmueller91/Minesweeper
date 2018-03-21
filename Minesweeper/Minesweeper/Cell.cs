using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
  public class Cell
  {
    private bool mine;
    private bool opened;
    private int nNeighbouringMines;

    public Cell()
    {
      this.opened = false;
      this.mine = false;
      this.nNeighbouringMines = 0;
    }

    public bool IsMine()
    {
      return this.mine;
    }

    public void SetMine()
    {
      this.mine = true;
    }

    public bool IsOpen()
    {
      return this.opened;
    }

    public void Open()
    {
      this.opened = true;
    }

    public void SetNNeighbouringMines(int n)
    {
      this.nNeighbouringMines = n;
    }

    public int GetNNeighbouringMines()
    {
      return this.nNeighbouringMines;
    }
  }
}
