using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public interface IScore
    {
        Color PlayerColor();

        int PlayerNumber();

        int Score();
    }

    public class ScoreDescendingComparer<T> : IComparer<T> where T : IScore
    {
        public int Compare(T x, T y)
        {
            return y.Score() - x.Score();
        }
    }
}
