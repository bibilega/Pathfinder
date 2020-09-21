using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinder 
{
    public static List<Cell> FindPath(Cell startCell, Cell finishCell)
    {
        var reachableNodes = new List<PathNode>();
        var exploredNodes = new List<PathNode>();

        var estimateCost = GetEstimateCost(startCell, finishCell);
        PathNode startNode = new PathNode(startCell, null, 0, estimateCost);

        reachableNodes.Add(startNode);

        while(reachableNodes.Count > 0)
        {
            //Выбираем предположительно ближающую точку
            var currentNode = ChooseNode(reachableNodes);

            if (currentNode.Cell.Position == finishCell.Position)
                return GetPath(currentNode);

            reachableNodes.Remove(currentNode);
            exploredNodes.Add(currentNode);

            //Просматриваем соседние узлы
            var NeighbourNodes = GetNeighbourNodes(currentNode, finishCell);
            foreach (var neighbourNode in NeighbourNodes)
            {
                //Если этот узел уже исследован - не смотрим
                var exploredNode = exploredNodes.Find(node => node.Cell.Position == neighbourNode.Cell.Position);
                if (exploredNode != null)
                    continue;

                var reachableNode = reachableNodes.FirstOrDefault(node => node.Cell.Position == neighbourNode.Cell.Position);
                if (reachableNode == null)
                    reachableNodes.Add(neighbourNode);
                else
                {
                    if(reachableNode.NodeCost > neighbourNode.NodeCost)
                    {
                        reachableNode.PreviuosPathNode = currentNode;
                        reachableNode.NodeCost = neighbourNode.NodeCost;
                    }
                }
            }
        }
        return new List<Cell>();
    }

    private static int GetEstimateCost(Cell start, Cell finish)
    {
        var result = Mathf.Abs(start.Position.X - finish.Position.X) + Mathf.Abs(start.Position.Z - finish.Position.Z);
        return result;
    }

    private static PathNode ChooseNode(List<PathNode> reachableNodes)
    {
        int minCost = 100000;
        PathNode bestNode = null;

        foreach (var node in reachableNodes)
        {
            if(node.TotalCost < minCost)
            {
                minCost = node.TotalCost;
                bestNode = node;
            }
        }

        return bestNode;
    }

    private static List<Cell> GetPath(PathNode node)
    {
        var result = new List<Cell>();
        var currentNode = node;

        while(currentNode != null)
        {
            result.Add(currentNode.Cell);
            currentNode = currentNode.PreviuosPathNode;
        }

        return result;
    }

    private static List<PathNode> GetNeighbourNodes(PathNode currentNode, Cell finishCell)
    {
        var result = new List<PathNode>();
        var fieldController = FieldController.Instance;

        var NeighbourCells = fieldController.GetAdjacentCell(currentNode.Cell);

        foreach(var cell in NeighbourCells)
        {
            var nodeCost = currentNode.NodeCost + fieldController.DistanceBetweenCell;
            var estimateCost = GetEstimateCost(cell, finishCell);
            var node = new PathNode(cell, currentNode, nodeCost, estimateCost);
            result.Add(node);
        }

        return result;
    }

    public class PathNode
    {
        public Cell Cell;
        public PathNode PreviuosPathNode;
        public int NodeCost;
        public int EstimateCost;
        public int TotalCost => NodeCost + EstimateCost;

        public PathNode(Cell cell, PathNode previousPathNode, int nodeCost, int estimateCost)
        {
            Cell = cell;
            PreviuosPathNode = previousPathNode;
            NodeCost = nodeCost;
            EstimateCost = estimateCost;
        }
    }


   
}


