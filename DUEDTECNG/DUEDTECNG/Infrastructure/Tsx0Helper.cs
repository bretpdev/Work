using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace DUEDTECNG
{
    class Tsx0Helper
    {
        ReflectionInterface RI;
        Func<string, bool> Tsx0tProcessing;
        Action OnReflectionError;
        public Tsx0Helper(ReflectionInterface ri, Func<string, bool> tsx0tProcessing, Action onReflectionError)
        {
            RI = ri;
            Tsx0tProcessing = tsx0tProcessing;
            OnReflectionError = onReflectionError;
        }
        public bool Iterate(string ssn)
        {
            RI.FastPath("TX3Z/ATS0N" + ssn);
            if (RI.ScreenCode == "TSX0P")
                return Tsx0pProcessing();
            else if (RI.ScreenCode == "TSX0S")
                return Tsx0sProcessing();
            else if (RI.ScreenCode == "TSX0R")
                return Tsx0rProcessing();
            return false;
        }

        private bool Tsx0pProcessing()
        {
            bool processedSuccessfully = true;
            PageHelper.Iterate(RI, (row, s) =>
            {
                RI.PutText(21, 12, RI.GetText(row, 4, 3), ReflectionInterface.Key.Enter, true);
                bool processed = false;
                if (RI.MessageCode == "03459")
                    OnReflectionError();
                else if (RI.ScreenCode == "TSX0S")
                    processed = Tsx0sProcessing();
                else if (RI.ScreenCode == "TSX0R")
                    processed = Tsx0rProcessing();
                if (!processed)
                {
                    processedSuccessfully = false;
                    s.ContinueIterating = false;
                }
                else
                    RI.Hit(ReflectionInterface.Key.F12);
            });
            return processedSuccessfully;
        }

        private bool Tsx0sProcessing()
        {
            var settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 11;
            settings.MaxRow = 22;
            bool processedSuccessfully = true;
            PageHelper.Iterate(RI, row =>
            {
                if (RI.CheckForText(row, 3, "_"))
                {
                    RI.PutText(row, 3, "X", ReflectionInterface.Key.Enter);
                    if (RI.MessageCode == "03459")
                        OnReflectionError();
                    else if (Tsx0rProcessing())
                    {
                        RI.Hit(ReflectionInterface.Key.F12);
                        if (RI.CheckForText(row, 3, "X"))
                            RI.PutText(row, 3, " ");
                        return;
                    }
                }
                processedSuccessfully = false;
                settings.ContinueIterating = false;

            }, settings);

            return processedSuccessfully;
        }

        private bool Tsx0rProcessing()
        {
            string scheduleType = RI.GetText(10, 48, 3);
            if (string.IsNullOrWhiteSpace(scheduleType))
                scheduleType = "L";
            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.MessageCode.IsIn("03459", "02877"))
            {
                OnReflectionError();
                return false;
            }
            if (RI.MessageCode == "01027")
                RI.PutText(10, 3, "X", ReflectionInterface.Key.Enter);
            if (RI.MessageCode == "02875" || !Tsx0tProcessing(scheduleType))
                return false;

            RI.Hit(ReflectionInterface.Key.F12);
            return true;
        }
    }
}
