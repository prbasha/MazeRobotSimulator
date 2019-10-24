using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeRobotSimulator.Model
{
    /// <summary>
    /// The MazeSegment class represents a single segment of the maze.
    /// A maze segment is made up of a center cell, and the four cells surrounding the center cell.
    /// </summary>
    public class MazeSegment
    {
        #region Fields

        private List<Direction> _cellsNotVisited;
        private List<Direction> _cellsVisitedOnce;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// Creates a new MazeSegment from a center cell and four neighbouring cells.
        /// </summary>
        public MazeSegment(MazeCell centerCell, MazeCell northCell, MazeCell eastCell, MazeCell southCell, MazeCell westCell)
        {
            try
            {
                CenterCell = centerCell ?? throw new Exception("Center cell can not be null."); // Assign the center cell.

                // Assign the neighbour cells.
                if (northCell == null && eastCell == null && southCell == null && westCell == null)
                {
                    throw new Exception("All four neighbour cells can not be null.");
                }
                NorthCell = northCell == null ? new MazeCell() { CellType = CellType.Wall } : northCell;
                EastCell = eastCell == null ? new MazeCell() { CellType = CellType.Wall } : eastCell;
                SouthCell = southCell == null ? new MazeCell() { CellType = CellType.Wall } : southCell;
                WestCell = westCell == null ? new MazeCell() { CellType = CellType.Wall } : westCell;

                // Build the list of cells not visited.
                _cellsNotVisited = new List<Direction>();
                if (NorthCell.CellType == CellType.Passage && NorthCell.CellMark == CellMark.None)
                {
                    _cellsNotVisited.Add(Direction.North);
                }
                if (EastCell.CellType == CellType.Passage && EastCell.CellMark == CellMark.None)
                {
                    _cellsNotVisited.Add(Direction.East);
                }
                if (SouthCell.CellType == CellType.Passage && SouthCell.CellMark == CellMark.None)
                {
                    _cellsNotVisited.Add(Direction.South);
                }
                if (WestCell.CellType == CellType.Passage && WestCell.CellMark == CellMark.None)
                {
                    _cellsNotVisited.Add(Direction.West);
                }

                // Build the list of cells visited once.
                _cellsVisitedOnce = new List<Direction>();
                if (NorthCell.CellType == CellType.Passage && NorthCell.CellMark == CellMark.Once)
                {
                    _cellsVisitedOnce.Add(Direction.North);
                }
                if (EastCell.CellType == CellType.Passage && EastCell.CellMark == CellMark.Once)
                {
                    _cellsVisitedOnce.Add(Direction.East);
                }
                if (SouthCell.CellType == CellType.Passage && SouthCell.CellMark == CellMark.Once)
                {
                    _cellsVisitedOnce.Add(Direction.South);
                }
                if (WestCell.CellType == CellType.Passage && WestCell.CellMark == CellMark.Once)
                {
                    _cellsVisitedOnce.Add(Direction.West);
                }

                SegmentType = DetermineSegmentType();   // Determine the segment type.
            }
            catch (Exception ex)
            {
                throw new Exception("MazeSegment(MazeCell centerCell, MazeCell northCell, MazeCell eastCell, MazeCell southCell, MazeCell westCell): " + ex.ToString());
            }
        }

        #endregion

        #region Events
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the center cell.
        /// </summary>
        public MazeCell CenterCell { get; private set; }

        /// <summary>
        /// Gets or sets the north cell.
        /// </summary>
        public MazeCell NorthCell { get; private set; }

        /// <summary>
        /// Gets or sets the east cell.
        /// </summary>
        public MazeCell EastCell { get; private set; }

        /// <summary>
        /// Gets or sets the south cell.
        /// </summary>
        public MazeCell SouthCell { get; private set; }

        /// <summary>
        /// Gets or sets the west cell.
        /// </summary>
        public MazeCell WestCell { get; private set; }

        /// <summary>
        /// Gets or sets the segment type.
        /// </summary>
        public SegmentType SegmentType { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// The DetermineSegmentType is called to determine the type iof this segment.
        /// </summary>
        /// <returns></returns>
        private SegmentType DetermineSegmentType()
        {
            try
            {
                // Dead-ends.
                if (NorthCell.CellType == CellType.Passage && EastCell.CellType == CellType.Wall && SouthCell.CellType == CellType.Wall && WestCell.CellType == CellType.Wall)
                {
                    return SegmentType.DeadEnd;
                }
                else if (NorthCell.CellType == CellType.Wall && EastCell.CellType == CellType.Passage && SouthCell.CellType == CellType.Wall && WestCell.CellType == CellType.Wall)
                {
                    return SegmentType.DeadEnd;
                }
                else if (NorthCell.CellType == CellType.Wall && EastCell.CellType == CellType.Wall && SouthCell.CellType == CellType.Passage && WestCell.CellType == CellType.Wall)
                {
                    return SegmentType.DeadEnd;
                }
                else if (NorthCell.CellType == CellType.Wall && EastCell.CellType == CellType.Wall && SouthCell.CellType == CellType.Wall && WestCell.CellType == CellType.Passage)
                {
                    return SegmentType.DeadEnd;
                }
                // Corridors.
                else if (NorthCell.CellType == CellType.Passage && EastCell.CellType == CellType.Wall && SouthCell.CellType == CellType.Passage && WestCell.CellType == CellType.Wall)
                {
                    return SegmentType.Corridor;
                }
                else if (NorthCell.CellType == CellType.Wall && EastCell.CellType == CellType.Passage && SouthCell.CellType == CellType.Wall && WestCell.CellType == CellType.Passage)
                {
                    return SegmentType.Corridor;
                }
                // Junctions.
                else
                {
                    return SegmentType.Junction;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeSegment.DetermineSegmentType(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The ChooseDirection method is called to choose a direction, based on the type of segment, and the surrounding cell markings.
        /// </summary>
        /// <returns></returns>
        public Direction ChooseDirection(Direction currentDirection)
        {
            try
            {
                // Corridor - No change to direction.
                if (SegmentType == SegmentType.Corridor)
                {
                    return currentDirection;
                }

                // Deadend - Choose the only available passage.
                if (SegmentType == SegmentType.DeadEnd)
                {
                    if (NorthCell.CellType == CellType.Passage)
                    {
                        return Direction.North;
                    }
                    else if (EastCell.CellType == CellType.Passage)
                    {
                        return Direction.East;
                    }
                    else if (SouthCell.CellType == CellType.Passage)
                    {
                        return Direction.South;
                    }
                    else if (WestCell.CellType == CellType.Passage)
                    {
                        return Direction.West;
                    }
                }

                // Junction - choose a new direction.
                Random randomNumberGenerator = new Random();
                Direction newDirection = Direction.North;
                if (_cellsNotVisited.Count > 0)
                {
                    // If any cells are marked none, choose one at random.
                    newDirection = _cellsNotVisited.ElementAt(randomNumberGenerator.Next(_cellsNotVisited.Count));
                }
                else if (_cellsVisitedOnce.Count  > 0 && _cellsVisitedOnce.Count != 4)
                {
                    // If some (not all) cells are marked once, and others cells are marked twice, choose a "marked once" cell at random.
                    newDirection = _cellsVisitedOnce.ElementAt(randomNumberGenerator.Next(_cellsVisitedOnce.Count));
                }
                else if (_cellsVisitedOnce.Count == 4)
                {
                    // If all cells are marked once, turn around.
                    newDirection = SwitchDirection(currentDirection);
                }
                
                return newDirection;
            }
            catch (Exception ex)
            {
                throw new Exception("MazeSegment.ChooseDirection(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The SwitchDirection method is called to switch the provided direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private Direction SwitchDirection(Direction direction)
        {
            try
            {
                switch (direction)
                {
                    case Direction.North:
                        return Direction.South;
                    case Direction.East:
                        return Direction.West;
                    case Direction.South:
                        return Direction.North;
                    case Direction.West:
                        return Direction.East;
                    default:
                        throw new Exception("Unable to switch the provided direction.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("TurnAround(Direction direction): " + ex.ToString());
            }
        }

        #endregion
    }
}
