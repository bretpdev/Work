using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    class CloseDUDEFlow : BaseUserSelectedFlow
    {
        public override void Process()
        {
            //ReflectionInterfaceForDUDE closer = new ReflectionInterfaceForDUDE();
            //closer.ExitReflectionSession();
            SessionInteractionComponents.KillReflection();
        }
    }
}
