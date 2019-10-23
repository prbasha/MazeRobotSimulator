using System;

namespace MazeRobotSimulator.Model
{
    /// <summary>
    /// The MazeSegment class represents a single segment of the maze.
    /// A maze segment is made up of a center cell, and the four cells surrounding the center cell.
    /// </summary>
    public class MazeSegment
    {
        #region Fields
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
                // Assign the cells.
                CenterCell = centerCell ?? throw new Exception("Center cell can not be null.");
                NorthCell = northCell ?? throw new Exception("North cell can not be null.");
                EastCell = eastCell ?? throw new Exception("East cell can not be null.");
                SouthCell = southCell ?? throw new Exception("South cell can not be null.");
                WestCell = westCell ?? throw new Exception("West cell can not be null.");

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

                // Deadend - turn around.
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

                // TBD: Junction - choose a new direction.
                Random randomNumberGenerator = new Random();
                Direction newDirection = Direction.North;
                // If any cells are marked none, choose one at random.
                // If some cells are marked once, and others are marked twice, choose a "marked once" cell at random.
                // If all cells are marked once, turn around.


                return newDirection;
            }
            catch (Exception ex)
            {
                throw new Exception("MazeSegment.ChooseDirection(): " + ex.ToString());
            }
        }

        #endregion
    }
}
