using MazeRobotSimulator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRobotSimulator.Model
{
    /// <summary>
    /// The Robot class represents a robot that can navigate a maze.
    /// </summary>
    public class Robot : NotificationBase
    {

        #region Fields

        private MazeCell _currentLocation;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Robot()
        {
        }

        #endregion

        #region Events
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the robot's current location.
        /// </summary>
        public MazeCell CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            private set
            {
                _currentLocation = value;
                RaisePropertyChanged("CurrentLocation");
            }
        }

        /// <summary>
        /// Gets or sets the robot's current location.
        /// </summary>
        public Direction CurrentDirection { get; private set; }

        /// <summary>
        /// Gets a boolean flag indicating if the robot has reached the end of the maze.
        /// </summary>
        public bool IsAtEnd
        {
            get
            {
                return CurrentLocation != null && CurrentLocation.ContainsRobot && CurrentLocation.CellRole == CellRole.End;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The SetLocation method is called to set the location of the robot.
        /// </summary>
        /// <param name="location"></param>
        public void SetLocation(MazeCell location)
        {
            try
            {
                if (location == null)
                {
                    return;
                }

                if (CurrentLocation != null)
                {
                    // Remove the robot from its existing location.
                    CurrentLocation.ContainsRobot = false;
                }

                // Move the robot to its new location.
                CurrentLocation = location;
                CurrentLocation.ContainsRobot = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Robot.SetLocation(MazeCell location): " + ex.ToString());
            }
        }
        
        /// <summary>
        /// The Move method is called to move the robot.
        /// The Trémaux's algorithm is used to control the robot.
        /// https://en.wikipedia.org/wiki/Maze_solving_algorithm#Tr%C3%A9maux's_algorithm
        /// </summary>
        /// <param name="mazeSegment"></param>
        public void Move(MazeSegment mazeSegment)
        {
            try
            {
                if (mazeSegment == null)
                {
                    throw new Exception("maze segment can not be null.");
                }

                if (CurrentLocation == null)
                {
                    // The robot is not in the maze - do nothing.
                    return;
                }
                
                if (CurrentLocation.CellRole == CellRole.End)
                {
                    // The robot has reached the end of the maze - do nothing.
                    // TBD: Raise an event at the bottom of this method.
                    return;
                }
                
                // If the robot is NOT at a junction, mark the location.
                if (mazeSegment.SegmentType != SegmentType.Junction)
                {
                    CurrentLocation.MarkCell();
                }

                // Turn the robot, based on the type of maze segment.
                CurrentDirection = mazeSegment.ChooseDirection(CurrentDirection);

                // Attempt to move the robot forwards.
                MazeCell newCurrentLocation = null;
                if (CurrentDirection == Direction.North && mazeSegment.NorthCell.CellMark != CellMark.Twice)
                {
                    newCurrentLocation = mazeSegment.NorthCell;
                }
                else if (CurrentDirection == Direction.East && mazeSegment.EastCell.CellMark != CellMark.Twice)
                {
                    newCurrentLocation = mazeSegment.EastCell;
                }
                else if (CurrentDirection == Direction.South && mazeSegment.SouthCell.CellMark != CellMark.Twice)
                {
                    newCurrentLocation = mazeSegment.SouthCell;
                }
                else if (CurrentDirection == Direction.West && mazeSegment.WestCell.CellMark != CellMark.Twice)
                {
                    newCurrentLocation = mazeSegment.WestCell;
                }

                // Update the current location.
                CurrentLocation.ContainsRobot = false;
                CurrentLocation = newCurrentLocation != null ? newCurrentLocation : CurrentLocation;
                CurrentLocation.ContainsRobot = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Robot.Move(MazeSegment mazeSegment): " + ex.ToString());
            }
        }
        
        /// <summary>
        /// The Remove method is called to remove the robot from the maze.
        /// </summary>
        public void Remove()
        {
            try
            {
                if (CurrentLocation != null)
                {
                    CurrentLocation.ContainsRobot = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Robot.Remove: " + ex.ToString());
            }
        }

        #endregion
    }
}
