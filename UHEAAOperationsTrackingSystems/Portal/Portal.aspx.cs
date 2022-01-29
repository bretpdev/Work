using System;

namespace UHEAAOperationsTrackingSystems
{
    public partial class Portal : BaseContentPage
    {

        protected override void OnInit(EventArgs e)
        {
            _systemLook = LookCoordinator.SystemLook.DefaultAndPortal;
            base.OnInit(e);
        }


    }
}
