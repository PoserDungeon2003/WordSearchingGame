using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class BoardData : ScriptableObject
{
    [Serializable]
    public class SearchingWord
    {
        public string word;
    }

    [Serializable]
    public class BoardRow
    {
        public int Size;
        public string[] Row;

        public BoardRow() { }
        public BoardRow(int size)
        {
            
        }
        public void CreateRow(int size)
        {
            Size = size;
            Row = new string[Size];
            ClearRow();
        }
        public void ClearRow()
        {
            for (int i = 0; i < Size; i++)
            {
                Row[i] = " ";
            }
        }
    }

    public float timeInSeconds;
    public int Columns = 0;
    public int Rows = 0;

    public BoardRow[] Board;

    public void ClearWithEmptyString()
    {
        for (int i = 0; i < Columns; i++)
        {
            Board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        Board = new BoardRow[Columns];
        for (int i = 0; i < Columns; i++)
        {
            Board[i] = new BoardRow(Rows);
        }
    }
}