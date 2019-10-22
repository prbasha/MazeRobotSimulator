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

        private ObservableCollection<MazeCell> _mazeCells;              // The maze (a collection of maze cells).
        private SimulationState _simulationState = SimulationState.Default;   // The current state of the simulation.
        private Random _randomNumberGenerator;                          // A random number generator.
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
        /// Gets or sets the simulation state.
        /// </summary>
        public SimulationState SimulationState
        {
            get
            {
                return _simulationState;
            }
            private set
            {
                _simulationState = value;
                RaisePropertyChanged("SimulationState");
                RaisePropertyChanged("CanGenerateMaze");
                RaisePropertyChanged("CanResetMaze");
                RaisePropertyChanged("CanStartSimulation");
                RaisePropertyChanged("CanStopSimulation");
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
        /// Gets a boolean flag indicating if the maze can be reset.
        /// </summary>
        public bool CanResetMaze
        {
            get
            {
                return SimulationState == SimulationState.MazeGenerated || SimulationState == SimulationState.Stopped;
            }
        }

        /// <summary>
        /// Gets a boolean flag indicating if the simulation can start.
        /// </summary>
        public bool CanStartSimulation
        {
            get
            {
                return SimulationState == SimulationState.MazeGenerated;
            }
        }

        /// <summary>
        /// Gets a boolean flag indicating if the simulation can stop.
        /// </summary>
        public bool CanStopSimulation
        {
            get
            {
                return SimulationState == SimulationState.Running;
            }
        }

        #endregion

        #region Methods
        
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
                    
                    ResetMaze();    // Clear the maze.
                    List<MazeCell> frontierCells = new List<MazeCell>();   // Create a collection of maze walls.
                    
                    // Select a random cell to start.
                    MazeCell initialCell = ChooseRandomCell();
                    initialCell.CellType = CellType.Passage;
                    frontierCells.AddRange(RetrieveFrontierNeighbours(initialCell));  // Add the neighbouring cells to the frontier.

                    while (frontierCells.Count > 0)
                    {
                        // Choose a random cell from the frontier.
                        MazeCell currentCell = frontierCells.ElementAt(_randomNumberGenerator.Next(frontierCells.Count));
                        frontierCells.Remove(currentCell);  // Remove this cell from the frontier.
                        
                        if (currentCell.CellType == CellType.Passage)
                        {
                            // If the cell is already a passage, move on to the next frontier cell.
                            continue;
                        }
                        
                        // Retrieve the "passage" neighbours for the current cell (these neighbours are already part of the maze).
                        List<MazeCell> passageNeighbours = RetrievePassageNeighbours(currentCell);
                        if (passageNeighbours.Count > 0)
                        {
                            // Select a random "passage" neighbour.
                            MazeCell selectedNeighbour = passageNeighbours.ElementAt(_randomNumberGenerator.Next(passageNeighbours.Count));

                            // Connect the current cell to the selected neighbour.
                            ConnectCells(currentCell, selectedNeighbour);
                            currentCell.CellType = CellType.Passage;
                            
                            // Retrieve the frontier neighbours for the current cell, and add them to the frontier.
                            frontierCells.AddRange(RetrieveFrontierNeighbours(currentCell));
                        }
                        
                        await Task.Delay(Constants.MazeGenerationDelayMilliSeconds);
                    }

                    // Set the start/end cells.
                    MazeCell startCell = MazeCells.First(x => x.CellType == CellType.Passage);
                    startCell.CellType = CellType.Start;
                    MazeCell endCell = MazeCells.Last(x => x.CellType == CellType.Passage);
                    endCell.CellType = CellType.End;

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

                    SimulationState = SimulationState.Default;  // Reset the simulation state.
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.ResetMaze(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The StartSimulation method is called to start the simulation.
        /// </summary>
        public void StartSimulation()
        {
            try
            {
                // TBD.
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.StartSimulation(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The StartSimulation method is called to stop the simulation.
        /// </summary>
        public void StopSimulation()
        {
            try
            {
                // TBD.
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.StopSimulation(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The ChooseRandomCell method is called to choose a random cell in the maze.
        /// This random cell can not be on the edge of the maze, as the maze edges are all walls.
        /// </summary>
        /// <returns></returns>
        private MazeCell ChooseRandomCell()
        {
            try
            {
                // X and Y indexes must not be on the edges.
                int cellIndexX = _randomNumberGenerator.Next(1, Constants.MazeWidth-1);
                int cellIndexY = _randomNumberGenerator.Next(1, Constants.MazeHeight-1);

                if (IsCellIndexValid(cellIndexX * cellIndexY))
                {
                    return MazeCells.ElementAt(cellIndexX * cellIndexY);
                }
                else
                {
                    throw new Exception("Unable to choose a random cell.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.ChooseRandomCell(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The IsCellIndexValid method is called to determine if a provided cell index is within range of the maze cell collection.
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        private bool IsCellIndexValid(int cellIndex)
        {
            return cellIndex >= 0 && cellIndex < MazeCells.Count;
        }

        /// <summary>
        /// The IsCellOnTheEdge method is called to determine if the provided cell is on the edge of the maze grid.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private bool IsCellOnTheEdge(MazeCell cell)
        {
            try
            {
                if (cell == null)
                {
                    throw new Exception("cell can not be null.");
                }

                int cellIndex = MazeCells.IndexOf(cell);   // Retrieve the index of the cell.
                
                // Determine if the current cell is on the north/east/south/west edge of the maze.
                bool northEdge = cellIndex < Constants.MazeWidth ? true : false;
                bool eastEdge = ((cellIndex + 1) % Constants.MazeWidth) == 0 ? true : false;
                bool westEdge = (cellIndex % Constants.MazeWidth) == 0 ? true : false;
                bool southEdge = (cellIndex + Constants.MazeWidth) >= (Constants.MazeWidth * Constants.MazeHeight) ? true : false;

                return northEdge || eastEdge || westEdge || southEdge;
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.IsCellOnTheEdge(MazeCell cell): " + ex.ToString());
            }
        }
        
        /// <summary>
        /// The RetrieveFrontierNeighbours method is called to retrieve the frontier neighbour cells for the provided cell.
        /// A "frontier" cell is a wall cell that has not yet been visited.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private List<MazeCell> RetrieveFrontierNeighbours(MazeCell cell)
        {
            try
            {
                if (cell == null)
                {
                    throw new Exception("cell can not be null.");
                }

                List<MazeCell> cellNeighbours = RetrieveNeighbours(cell);
                return cellNeighbours.FindAll(x => (x.CellType == CellType.Wall && !IsCellOnTheEdge(x)));
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.RetrieveFrontierNeighbours(MazeCell cell): " + ex.ToString());
            }
        }

        /// <summary>
        /// The RetrievePassageNeighbours method is called to retrieve the passage neighbour cells for the provided cell.
        /// A "passge" cell is a cell that is already part of the maze.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private List<MazeCell> RetrievePassageNeighbours(MazeCell cell)
        {
            try
            {
                if (cell == null)
                {
                    throw new Exception("cell can not be null.");
                }

                List<MazeCell> cellNeighbours = RetrieveNeighbours(cell);
                return cellNeighbours.FindAll(x => x.CellType == CellType.Passage);
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.RetrievePassageNeighbours(MazeCell cell): " + ex.ToString());
            }
        }

        /// <summary>
        /// The RetrieveNeighbours method is called to retrieve the neighbour cells for the provided cell.
        /// The neighbour cells are 2 cells from the provided cell, in all 4 directions. This is because a "wall" cell exists between each cell and its neighbour cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private List<MazeCell> RetrieveNeighbours(MazeCell cell)
        {
            try
            {
                if (cell == null)
                {
                    throw new Exception("cell can not be null.");
                }
                
                int cellIndex = MazeCells.IndexOf(cell);   // Retrieve the index of the cell.

                // Determine the indexes for the current cell's neighbours.
                int northNeighbourIndex = cellIndex - 2*Constants.MazeWidth;
                int eastNeighbourIndex = cellIndex + 2;
                int southNeighbourIndex = cellIndex + 2*Constants.MazeHeight;
                int westNeighbourIndex = cellIndex - 2;

                // Determine if the current cell is on the north/east/south/west edge of the maze - certain neighbours must be ignored if the current cell is on an edge.
                bool northEdge = cellIndex < Constants.MazeWidth ? true : false;
                bool eastEdge = ((cellIndex + 1) % Constants.MazeWidth) == 0 ? true : false;
                bool westEdge = (cellIndex % Constants.MazeWidth) == 0 ? true : false;
                bool southEdge = (cellIndex + Constants.MazeWidth) >= (Constants.MazeWidth * Constants.MazeHeight) ? true : false;

                // Retrieve the current cell's neighbours.
                List<MazeCell> cellNeightbours = new List<MazeCell>();
                // North cell.
                if (!northEdge && IsCellIndexValid(northNeighbourIndex))
                {
                    cellNeightbours.Add(MazeCells[northNeighbourIndex]);
                }
                // East cell.
                if (!eastEdge && IsCellIndexValid(eastNeighbourIndex))
                {
                    cellNeightbours.Add(MazeCells[eastNeighbourIndex]);
                }
                // South cell.
                if (!southEdge && IsCellIndexValid(southNeighbourIndex))
                {
                    cellNeightbours.Add(MazeCells[southNeighbourIndex]);
                }
                // West cell.
                if (!westEdge && IsCellIndexValid(westNeighbourIndex))
                {
                    cellNeightbours.Add(MazeCells[westNeighbourIndex]);
                }

                return cellNeightbours;
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.RetrieveNeighbours(MazeCell cell): " + ex.ToString());
            }
        }

        /// <summary>
        /// The ConnectCells method is called to connect two cells.
        /// The wall cell that exists between the two cells becomes a passage.
        /// </summary>
        /// <param name="cellOne"></param>
        /// <param name="cellTwo"></param>
        private void ConnectCells(MazeCell cellOne, MazeCell cellTwo)
        {
            try
            {
                if (cellOne == null)
                {
                    throw new Exception("cellOne can not be null.");
                }
                if (cellTwo == null)
                {
                    throw new Exception("cellTwo can not be null.");
                }

                int indexOne = MazeCells.IndexOf(cellOne);
                int indexTwo = MazeCells.IndexOf(cellTwo);

                if (!IsCellIndexValid(indexOne))
                {
                    throw new Exception("can not determine index for cellOne.");
                }
                if (!IsCellIndexValid(indexTwo))
                {
                    throw new Exception("can not determine index for cellTwo.");
                }

                int offset = indexOne - indexTwo;
                int wallIndex = indexOne - (offset / 2);

                if (IsCellIndexValid(wallIndex))
                {
                    MazeCell wallCell = MazeCells[wallIndex];
                    wallCell.CellType = CellType.Passage;   
                }
                else
                {
                    throw new Exception("unable to retireve cell between cellOne and cellTwo.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.ConnectCells(MazeCell cellOne, MazeCell cellTwo): " + ex.ToString());
            }
        }

        #endregion
    }
}
