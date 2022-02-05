using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.CommonScreens
{
    public class Field
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Length { get; private set; }
        private IReflectionSession session;
        public Field(IReflectionSession session, int x, int y, int length)
        {
            X = x; Y = y; Length = length;
            this.session = session;
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", "Field length must be greater than 0.");
        }
        public string Value
        {
            get
            {
                return session.GetText(X, Y, Length);
            }
            set
            {
                session.PutText(X, Y, value);
            }
        }
        public void Clear()
        {
            session.BlankField(X, Y);
        }
    }
}
