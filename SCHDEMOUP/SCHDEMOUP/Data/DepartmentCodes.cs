using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCHDEMOUP
{
    class DepartmentCodes
    {
        public enum UpdateType
        {
            General,
            Financial,
            Registrar,
            Bursar,
            All
        }

        private string _onlinkDepartment;
        public string OnelinkDepartment
        {
            get
            {
                return _onlinkDepartment;
            }
        }

        private string _compassDepartment;
        public string CompassDepartment 
        {
            get
            {
                return _compassDepartment;
            }
        }

        public string SchoolCode { get; set; }
        public string SubmitDate { get; set; }

        private UpdateType _updateType;
        public UpdateType Type
        {
            get
            {
                return _updateType;
            }
            set
            {
                switch (value)
                {
                    case UpdateType.General:
                        _onlinkDepartment = "GEN";
                        _compassDepartment = "000";
                        _updateType = value;
                        break;
                    case UpdateType.Financial:
                        _onlinkDepartment = "110";
                        _compassDepartment = "004";
                        _updateType = value;
                        break;
                    case UpdateType.Registrar:
                        _onlinkDepartment = "112";
                        _compassDepartment = "001";
                        _updateType = value;
                        break;
                    case UpdateType.Bursar:
                        _onlinkDepartment = "111";
                        _compassDepartment = "003";
                        _updateType = value;
                        break;
                    case UpdateType.All:
                        _onlinkDepartment = "ALL";
                        _compassDepartment = "ALL";
                        _updateType = value;
                        break;
                }
            }
        }
    }
}
