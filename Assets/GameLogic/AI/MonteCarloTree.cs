using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MCTree
{
    public MCNode Root;
    public MCTree(BoardState board)
    {
        MCState startingState = new(board, board.CurrentTeam);
        Root = new(startingState, null);
    }

    public int GetBestMove()
    {
        int index = 0;
        float bestWinratio = 0;
        List<MCNode> children = Root.Children;
        foreach (MCNode child in children)
        {
            if(child.State.WinRatio > bestWinratio)
            {
                bestWinratio = child.State.WinRatio;
                index = children.IndexOf(child);
            }
        }
        return index;
    }
}

[Serializable]
public class MCNode
{
    public MCNode(MCState state, MCNode parent)
    {
        State = state;
        Parent = parent;
    }

    public MCState State { get; private set; }
    public MCNode Parent { get; private set; }

    [field: NonSerialized]
    public List<MCNode> Children { get; private set; }
    public bool IsUnknown { get; private set; } = true;
    public bool IsFullyExplored { get; private set; } = false;
    public void SelectNode(float selectionConstant)
    {
        if (Children == null)
        {
            ExpandNode();
        }
        if (IsUnknown)
        {
            foreach (MCNode node in Children)
            {
                if (node.State.Visits == 0)
                {
                    node.Simulate();
                    return;
                }
            }
            IsUnknown = false;
        }
        int totalVisits = 0;
        foreach (MCNode child in Children)
        {
            if (!child.IsFullyExplored)
            {
                totalVisits += child.State.Visits;
            }
        }
        if (totalVisits == 0)
        {
            IsFullyExplored = true;
            return;
        }
        double currentValue = 0f;
        MCNode currentChild = Children[0];
        foreach (MCNode child in Children)
        {
            if (child.IsFullyExplored)
            {
                continue;
            }
            int wins = child.State.Wins;
            float visits = child.State.Visits;
            double newValue = (wins / visits) + (selectionConstant * Math.Sqrt(Math.Log(totalVisits) / visits));
            if (newValue > currentValue)
            {
                currentValue = newValue;
                currentChild = child;
            }
        }
        currentChild.SelectNode(selectionConstant);
    }
    public void ExpandNode()
    {
        Children = new List<MCNode>();
        List<BoardState> boards = State.Board.GetPossibleStates();
        foreach (BoardState board in boards)
        {
            MCState state = new(board, State.Board.CurrentTeam);
            MCNode child = new(state, this);
            Children.Add(child);
        }
    }
    public void Simulate()
    {
        BoardState board = State.Board;
        BoardState previousBoard = null;
        while (board != null)
        {
            previousBoard = board;
            board = board.GetRandomState();
        }
        int winningPlayer = previousBoard.CurrentTeam;
        Backpropagate(winningPlayer);
    }

    public void Backpropagate(int winningPlayer)
    {
        State.UpdateTotals(winningPlayer);
        Parent?.Backpropagate(winningPlayer);
    }
}

[Serializable]
public class MCState
{
    public MCState(BoardState board, int player)
    {
        Board = board;
        Player = player;
        Visits = 0;
        Wins = 0;
    }

    public BoardState Board { get; private set; }
    public int Player { get; private set; }
    public int Visits { get; private set; }
    public int Wins { get; private set; }
    public float WinRatio { get { return Wins / (float)Visits; } }

    public void UpdateTotals(int winningPlayer)
    {
        Visits++;
        if (winningPlayer == Player)
        {
            Wins++;
        }
    }
}