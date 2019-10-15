using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    public static class MyClass
    {
        static Random rand = new Random();
        public static Random random{ get { return rand; } }
    }

    public delegate void DelegateSquare(Square square);
    public delegate void DelegateVoid();
    public delegate bool DelegateBool();
    public delegate void DelegateMove(Square square, Position position);

    interface IMove
    {
        bool Move(Square[,] massSquare, DelegateMove SetNewPos);
    }
}
