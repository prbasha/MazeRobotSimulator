using MazeRobotSimulator.Common;
using System;

namespace MazeRobotSimulator.Model
{
    /// <summary>
    /// The MazeCell class represents a single maze cell.
    /// </summary>
    public class MazeCell : NotificationBase
    {
        #region Fields

        private CellType _cellType = CellType.Wall;
        private CellRole _cellRole = CellRole.None;
        private CellMark _cellMark = CellMark.None;
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
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the cell role.
        /// </summary>
        public CellRole CellRole
        {
            get
            {
                return _cellRole;
            }
            set
            {
                _cellRole = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the cell mark.
        /// </summary>
        public CellMark CellMark
        {
            get
            {
                return _cellMark;
            }
            private set
            {
                _cellMark = value;
                RaisePropertyChanged();
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
                RaisePropertyChanged();
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
            CellMark = CellMark.None;
            CellRole = CellRole.None;
            ContainsRobot = false;
        }

        /// <summary>
        /// The MarkCell method is called to apply a mark to the cell.
        /// </summary>
        public void MarkCell()
        {
            try
            {
                switch (CellMark)
                {
                    case CellMark.None:
                        CellMark = CellMark.Once;
                        break;
                    case CellMark.Once:
                        CellMark = CellMark.Twice;
                        break;
                    case CellMark.Twice:
                        throw new Exception("Call has already been marked twice.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeCell.MarkCell(): " + ex.ToString());
            }
        }

        #endregion
    }
}
