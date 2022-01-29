using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uheaa.Common.WebApi;

namespace RestDatabaseApi
{
    public class CdwController : DbControllerBase
    {
        public override WebApiControllers ControllerId => WebApiControllers.CdwDb;
        public override string ControllerFriendlyName => "CDW DB";
    }
    public class UdwController : DbControllerBase
    {
        public override WebApiControllers ControllerId => WebApiControllers.UdwDb;
        public override string ControllerFriendlyName => "UDW DB";

    }
    public class UlsController : DbControllerBase
    {
        public override WebApiControllers ControllerId => WebApiControllers.UlsDb;
        public override string ControllerFriendlyName => "ULS DB";

    }
    public class ClsController : DbControllerBase
    {
        public override WebApiControllers ControllerId => WebApiControllers.ClsDb;
        public override string ControllerFriendlyName => "CLS DB";

    }
    public class CsysController : DbControllerBase
    {
        public override WebApiControllers ControllerId => WebApiControllers.CsysDb;
        public override string ControllerFriendlyName => "CSYS DB";

    }
    public class NoradController : DbControllerBase
    {
        public override WebApiControllers ControllerId => WebApiControllers.NoradDb;
        public override string ControllerFriendlyName => "NORAD DB";

    }
}