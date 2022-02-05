using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.CommonScreens
{
    public class CTX6J : Screen
    {
        public CTX6J(IReflectionSession session)
            : base(session)
        {
            Queue = new Field(session, 7, 42, 10);
            SubQueue = new Field(session, 8, 42, 10);
            UserId = new Field(session, 13, 42, 10);
            AssignedUserId = new Field(session, 8, 15, 10);
        }


        public override void Navigate()
        {
            session.FastPath("CTX6J");
        }

        public Field Queue { get; private set; }
        public Field SubQueue { get; private set; }
        public Field UserId { get; private set; }
        public Field AssignedUserId { get; private set; }
    }
}
