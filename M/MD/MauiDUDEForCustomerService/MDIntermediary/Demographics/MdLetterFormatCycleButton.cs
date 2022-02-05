using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.WinForms;

namespace MDIntermediary
{
    class MdLetterFormatCycleButton : CycleButton<MDLetters.Formats>
    {
        public MdLetterFormatCycleButton()
            : base()
        {
        }

        public void Populate(List<MDLetters.Formats> list)
        {
            this.Options.AddRange(
                list.Select(o =>
                    new CycleOption<MDLetters.Formats>()
                    {
                        Label = o.CorrespondenceFormat,
                        Value = o
                    }
                )
            );
        }
    }
}
