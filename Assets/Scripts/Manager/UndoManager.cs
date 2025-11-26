using System.Collections.Generic;
using UnityEngine;

public class UndoManager : MonoBehaviour
{
    private Stack<int[,]> gridHistory = new Stack<int[,]>();
    private Stack<int> scoreHistory = new Stack<int>();
    private bool lastUndoWasUsed = false;
    public bool LastUndoUsed => lastUndoWasUsed;


    public void SaveState(int[,] currentGrid, int currentScore)
    {
        int size = currentGrid.GetLength(0);
        int[,] clone = new int[size, size];
        System.Array.Copy(currentGrid, clone, currentGrid.Length);
        gridHistory.Push(clone);
        scoreHistory.Push(currentScore);
    }

    public bool CanUndo() => gridHistory.Count > 0;

    public (int[,], int) Undo()
    {
        if (!CanUndo())
            return (null, 0);

        lastUndoWasUsed = true;

        int[,] lastGrid = gridHistory.Pop();
        int lastScore = scoreHistory.Pop();
        return (lastGrid, lastScore);
    }
    
    public void SaveAfterPenaltyOnly(int newScore)
    {
        // update only score history top WITHOUT affecting grid layout
        if (scoreHistory.Count > 0)
        {
            scoreHistory.Pop();
            scoreHistory.Push(newScore);
        }
    }


    public void PopLastState()
    {
        if (gridHistory.Count > 0) gridHistory.Pop();
        if (scoreHistory.Count > 0) scoreHistory.Pop();
    }

    public void Clear()
    {
        gridHistory.Clear();
        scoreHistory.Clear();
    }
}