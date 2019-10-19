using MazeRobotSimulator.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MazeRobotSimulator.Model
{
    /// <summary>
    /// The Maze class represents a computer-generated maze with a robot.
    /// The maze is generated using the Randomized Prim's algorithm.
    /// A robot navigates the maze using Trémaux's algorithm. 
    /// </summary>
    public class Maze : NotificationBase
    {
        #region Fields

        private ObservableCollection<MazeCell> _mazeCells;
        private List<MazeCell> _mazeWalls;
        private SimulationState _mazeState = SimulationState.Default;
        private Random _randomNumberGenerator;
        // TBD: add robot.

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Maze()
        {
            try
            {
                _randomNumberGenerator = new Random();  // Initialise random number generator.
                ResetMaze();                            // Reset the maze.
            }
            catch (Exception ex)
            {
                throw new Exception("Maze(): " + ex.ToString());
            }
        }

        #endregion

        #region Events
        #endregion

        #region Properties

        /// <summary>
        /// Gets the width of the maze (in cells).
        /// </summary>
        public int MazeWidthCells
        {
            get
            {
                return Constants.MazeWidth;
            }
        }

        /// <summary>
        /// Gets the height of the maze (in cells).
        /// </summary>
        public int MazeHeightCells
        {
            get
            {
                return Constants.MazeHeight;
            }
        }

        /// <summary>
        /// Gets the collection of maze cells.
        /// </summary>
        public ObservableCollection<MazeCell> MazeCells
        {
            get
            {
                if (_mazeCells == null)
                {
                    _mazeCells = new ObservableCollection<MazeCell>();
                    while (_mazeCells.Count != Constants.MazeWidth * Constants.MazeHeight)
                    {
                        _mazeCells.Add(new MazeCell());
                    }
                }

                return _mazeCells;
            }
            private set
            {
                _mazeCells = value;
                RaisePropertyChanged("MazeCells");
            }
        }

        /// <summary>
        /// Gets or sets the maze state.
        /// </summary>
        public SimulationState SimulationState
        {
            get
            {
                return _mazeState;
            }
            private set
            {
                _mazeState = value;
                RaisePropertyChanged("SimulationState");
                RaisePropertyChanged("CanGenerateMaze");
                RaisePropertyChanged("CanResetMaze");
            }
        }

        /// <summary>
        /// Gets a boolean flag indicating if a new maze can be generated.
        /// </summary>
        public bool CanGenerateMaze
        {
            get
            {
                return SimulationState == SimulationState.Default;
            }
        }

        /// <summary>
        /// Gets a boolean flag indicating if the simulation can be reset.
        /// </summary>
        public bool CanResetMaze
        {
            get
            {
                return SimulationState == SimulationState.MazeGenerated;
            }
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// The ResetMaze method is called to reset the maze.
        /// </summary>
        public void ResetMaze()
        {
            try
            {
                if (CanResetMaze)
                {
                    // Create and populate the maze.
                    MazeCells = new ObservableCollection<MazeCell>();
                    while (MazeCells.Count != Constants.MazeWidth * Constants.MazeHeight)
                    {
                        MazeCells.Add(new MazeCell());
                    }

                    SimulationState = SimulationState.Default;  // Reset the maze state.
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.ResetMaze(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The GenerateNewMaze method is called to generate a new maze.
        /// The Randomized Prim's algorithm. is used to generate the maze.
        /// https://en.wikipedia.org/wiki/Maze_generation_algorithm#Randomized_Prim's_algorithm
        /// </summary>
        /// <returns></returns>
        public async Task GenerateNewMaze()
        {
            try
            {
                if (CanGenerateMaze)
                {
                    SimulationState = SimulationState.MazeGenerating;

                    // Clear the maze and stack.
                    ResetMaze();
                    _mazeGeneratorStack = new Stack<MazeCell>();

                    // Select a random cell to start.
                    MazeCell startCell = ChooseRandomCell();
                    startCell.CellType = CellType.Passage;

                    // TBD: Implement Randomized Prim's Algorithm.
                    await Task.Delay(Constants.MazeGenerationDelayMilliSeconds);
                    
                    // TBD: Set the start/end cells.
                    // TBD: Place the robot at the start cell.

                    SimulationState = SimulationState.MazeGenerated;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.GenerateNewMaze(): " + ex.ToString());
            }
        }
        
        /// <summary>
        /// The ChooseRandomCell method is called to choose a random cell in the maze.
        /// </summary>
        /// <returns></returns>
        private MazeCell ChooseRandomCell()
        {
            try
            {
                // Select a random cell to start.
                int cellIndex = _randomNumberGenerator.Next(Constants.MazeHeight * Constants.MazeWidth);
                return MazeCells.ElementAt(cellIndex);
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.ChooseRandomCell(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The IsCellIndexValid method is called to determine if a provided cell index is within range of the maze robot cell collection.
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        private bool IsCellIndexValid(int cellIndex)
        {
            return cellIndex >= 0 && cellIndex < MazeCells.Count;
        }

        #endregion
    }
}
