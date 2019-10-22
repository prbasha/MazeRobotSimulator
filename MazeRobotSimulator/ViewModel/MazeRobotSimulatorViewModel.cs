using MazeRobotSimulator.Common;
using MazeRobotSimulator.Model;
using System;
using System.ComponentModel;

namespace MazeRobotSimulator.ViewModel
{
    /// <summary>
    /// The MazeRobotSimulatorViewModel class represents the View Model for the Maze Robot Simulator.
    /// </summary>
    public class MazeRobotSimulatorViewModel : NotificationBase
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MazeRobotSimulatorViewModel()
        {
            try
            {
                // Initialise the model class.
                Maze = new Maze();
                Maze.PropertyChanged += OnMazePropertyChanged;

                // Initialise the commands.
                GenerateMazeCommand = new DelegateCommand(OnGenerateMaze, CanGenerateMaze);
                ResetMazeCommand = new DelegateCommand(OnResetMaze, CanResetMaze);
                StartSimulationCommand = new DelegateCommand(OnStartSimulation, CanStartSimulation);
                StopSimulationCommand = new DelegateCommand(OnStopSimulation, CanStopSimulation);
            }
            catch (Exception ex)
            {
                throw new Exception("MazeRobotSimulatorViewModel(): " + ex.ToString());
            }
        }

        #endregion

        #region Events
        #endregion

        #region Properties

        /// <summary>
        /// Gets the maze model class.
        /// </summary>
        public Maze Maze { get; }

        /// <summary>
        /// Gets or sets the generate maze command.
        /// </summary>
        public DelegateCommand GenerateMazeCommand { get; private set; }

        /// <summary>
        /// Gets or sets the reset maze command.
        /// </summary>
        public DelegateCommand ResetMazeCommand { get; private set; }

        /// <summary>
        /// Gets or sets the start simulation command.
        /// </summary>
        public DelegateCommand StartSimulationCommand { get; private set; }

        /// <summary>
        /// Gets or sets the stop simulation command.
        /// </summary>
        public DelegateCommand StopSimulationCommand { get; private set; }

        #endregion

        #region Methods
        
        /// <summary>
        /// The OnGenerateMaze method is called to generate a new maze.
        /// </summary>
        /// <param name="arg"></param>
        public async void OnGenerateMaze(object arg)
        {
            try
            {
                if (Maze != null)
                {
                    await Maze.GenerateNewMaze();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel.OnGenerateMaze(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// The CanGenerateMaze method is callled to determine if a new maze can be generated.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool CanGenerateMaze(object arg)
        {
            return Maze != null && Maze.CanGenerateMaze;
        }

        /// <summary>
        /// The OnResetMaze method is called to reset the simulation.
        /// </summary>
        /// <param name="arg"></param>
        public void OnResetMaze(object arg)
        {
            try
            {
                if (Maze != null)
                {
                    Maze.ResetMaze();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel.OnResetMaze(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// The CanResetMaze method is callled to determine if the simulation can be reset.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool CanResetMaze(object arg)
        {
            return Maze != null && Maze.CanResetMaze;
        }

        /// <summary>
        /// The OnStartSimulation method is called to start the simulation.
        /// </summary>
        /// <param name="arg"></param>
        public void OnStartSimulation(object arg)
        {
            try
            {
                if (Maze != null)
                {
                    Maze.StartSimulation();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel.OnStartSimulation(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// The CanStartSimulation method is callled to determine if the simulation can be started.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool CanStartSimulation(object arg)
        {
            return Maze != null && Maze.CanStartSimulation;
        }

        /// <summary>
        /// The OnStopSimulation method is called to stop the simulation.
        /// </summary>
        /// <param name="arg"></param>
        public void OnStopSimulation(object arg)
        {
            try
            {
                if (Maze != null)
                {
                    Maze.StopSimulation();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel.OnStopSimulation(object arg): " + ex.ToString());
            }
        }

        /// <summary>
        /// The CanStopSimulation method is callled to determine if the simulation can be stoped.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool CanStopSimulation(object arg)
        {
            return Maze != null && Maze.CanStopSimulation;
        }

        /// <summary>
        /// The OnMazePropertyChanged method is called when a property in the Maze model class changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMazePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                GenerateMazeCommand.RaiseCanExecuteChanged();
                ResetMazeCommand.RaiseCanExecuteChanged();
                StartSimulationCommand.RaiseCanExecuteChanged();
                StopSimulationCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                throw new Exception("MazeGeneratorViewModel.OnMazePropertyChanged(object sender, PropertyChangedEventArgs e): " + ex.ToString());
            }
        }

        #endregion
    }
}
