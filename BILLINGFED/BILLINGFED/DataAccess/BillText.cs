using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Uheaa.Common;

namespace BILLINGFED
{
    public class BillText
    {
        public string FirstSpecialMessageTitle { get; set; }
        public string FirstSpecialMessage { get; set; }
        public string SecondSpecialMessageTitle { get; set; }
        public string SecondSpecialMessage { get; set; }
        public float Message1XCoord { get; set; }
        public float Message1YCoord { get; set; }
        public int Message1FontTypeId { get; set; }
        public float Message2XCoord { get; set; }
        public float Message2YCoord { get; set; }

        public static string GetSecondMessageSpecialText(List<string> lineData, BillText bill, int reportNumber)
        {
            return reportNumber.IsIn(22, 23) ? GetMergedMessage(lineData, bill) : GetSecondMessageText(bill.SecondSpecialMessage.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList());
        }

        private static string GetMergedMessage(List<string> lineData, BillText bill)
        {
            DateTime earliestDate = lineData[0].SplitAndRemoveQuotes(",")[47].ToDate();

            double sumAmount = lineData.Where(p => p.SplitAndRemoveQuotes(",")[47].ToDateNullable() == earliestDate).First().SplitAndRemoveQuotes(",")[46].Replace("$", "").ToDouble();
            //Create a new string of the message so it doesn't overwrite the default message merge fields
            string message = bill.SecondSpecialMessage.Replace("[PaymentAmount]", "$" + sumAmount).Replace("[PaymentDate]", earliestDate.ToShortDateString());
            return GetSecondMessageText(message.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList());
        }

        public static string GetSecondMessageText(List<string> secondMessage)
        {
            string second = "";
            string third = "";
            string fourth = "";
            if (secondMessage[1].Contains("¬")) // ¬ symbol created with Alt + 170
            {
                second = secondMessage[1].Split('¬')[0];
                third = secondMessage[1].Split('¬')[1];
                fourth = secondMessage.Count > 2 ? secondMessage[2] : "";
            }
            if (third.IsPopulated())
            {
                secondMessage[1] = second;
                if (secondMessage.Count > 2)
                    secondMessage[2] = " "; //Add a blank space to move the text down a line.
                else
                    secondMessage.Add(" "); //Add a blank space to move the text down a line.
                secondMessage.Add(third);
                if (fourth.IsPopulated())
                    secondMessage.Add(fourth);
            }
            return string.Join("/r/n", secondMessage).Replace("/r/n", "\r\n");
        }
    }
}