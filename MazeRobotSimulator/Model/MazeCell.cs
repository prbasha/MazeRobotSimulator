using MazeRobotSimulator.Common;

namespace MazeRobotSimulator.Model
{
    /// <summary>
    /// The MazeCell class represents a single maze cell.
    /// </summary>
    public class MazeCell : NotificationBase
    {
        #region Fields

        private CellType _cellType = CellType.Wall;
        private bool _containsRobot = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MazeCell()
        {
        }

        #endregion

        #region Events
        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets the cell type.
        /// </summary>
        public CellType CellType
        {
            get
            {
                return _cellType;
            }
            set
            {
                _cellType = value;
                RaisePropertyChanged("CellType");
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if the robot is inside this cell.
        /// </summary>
        public bool ContainsRobot
        {
            get
            {
                return _containsRobot;
            }
            set
            {
                _containsRobot = value;
                RaisePropertyChanged("ContainsRobot");
            }
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// The ResetCell method is called to reset the cell.
        /// </summary>
        public void ResetCell()
        {
            CellType = CellType.Wall;
            ContainsRobot = false;
        }

        #endregion
    }
}
