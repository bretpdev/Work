using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            ReflectionInterface ri = new ReflectionInterface();
            ri.Login("", "", DataAccessHelper.Region.CornerStone);
            new TCPAFEDPS.TcpaPhoneScrubFed(ri).Main();

            ri.CloseSession();
        }
    }
}