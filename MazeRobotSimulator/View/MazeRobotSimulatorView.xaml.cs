using MazeRobotSimulator.ViewModel;
using System.Windows;

namespace MazeRobotSimulator.View
{
    /// <summary>
    /// The MazeRobotSimulatorView class represents the View for the Maze Robot Simulator.
    /// </summary>
    public partial class MazeRobotSimulatorView : Window
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MazeRobotSimulatorView()
        {
            InitializeComponent();

            // Create the View Model.
            MazeRobotSimulatorViewModel viewModel = new MazeRobotSimulatorViewModel();
            DataContext = viewModel;    // Set the data context for all data binding operations.
        }

        #endregion

        #region Events
        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion
    }
}
