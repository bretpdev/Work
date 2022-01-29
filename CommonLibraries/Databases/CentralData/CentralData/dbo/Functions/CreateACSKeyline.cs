using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString CreateACSKeyline(string ssn, string person, string address)
    {
        person = person.ToUpper();
        address = address.ToUpper();

        if (person != "B" && person != "R")
            throw new Exception("Person parameter must be B or R");

        if (address != "L" && address != "T" && address != "A" && address != "B")
            throw new Exception("Address parameter must be L,T,A, or B");

        string encodedSsn = string.Empty;

        if (person.ToUpper() == "B")
        {
            string nextLetter = string.Empty;
            person = "P";
            for (int i = 0; i <= 8; i++)
            {
                switch (int.Parse(ssn.Substring(i, 1)))
                {
                    case 1:
                        nextLetter = "R";
                        break;
                    case 2:
                        nextLetter = "E";
                        break;
                    case 3:
                        nextLetter = "T";
                        break;
                    case 4:
                        nextLetter = "H";
                        break;
                    case 5:
                        nextLetter = "G";
                        break;
                    case 6:
                        nextLetter = "U";
                        break;
                    case 7:
                        nextLetter = "A";
                        break;
                    case 8:
                        nextLetter = "L";
                        break;
                    case 9:
                        nextLetter = "Y";
                        break;
                    case 0:
                        nextLetter = "M";
                        break;
                }
                encodedSsn += nextLetter;
            }
        }
        else
        {
            encodedSsn = ssn.Substring(0, 2) + "/" + ssn.Substring(3, 6);
        }

        string workingKeyline = person + encodedSsn + DateTime.Now.ToString("MMdd") + address;

        int checkDigit = 0;
        for (int i = 0; i <= workingKeyline.Length - 1; i++)
        {
            int keylineBitValue = 0;
            switch (workingKeyline.Substring(i, 1))
            {
                case "A":
                    keylineBitValue = 1;
                    break;
                case "B":
                    keylineBitValue = 2;
                    break;
                case "C":
                    keylineBitValue = 3;
                    break;
                case "D":
                    keylineBitValue = 4;
                    break;
                case "E":
                    keylineBitValue = 5;
                    break;
                case "F":
                    keylineBitValue = 6;
                    break;
                case "G":
                    keylineBitValue = 7;
                    break;
                case "H":
                    keylineBitValue = 8;
                    break;
                case "I":
                    keylineBitValue = 9;
                    break;
                case "J":
                    keylineBitValue = 10;
                    break;
                case "K":
                    keylineBitValue = 11;
                    break;
                case "L":
                    keylineBitValue = 12;
                    break;
                case "M":
                    keylineBitValue = 13;
                    break;
                case "N":
                    keylineBitValue = 14;
                    break;
                case "O":
                    keylineBitValue = 15;
                    break;
                case "P":
                    keylineBitValue = 0;
                    break;
                case "Q":
                    keylineBitValue = 1;
                    break;
                case "R":
                    keylineBitValue = 2;
                    break;
                case "S":
                    keylineBitValue = 3;
                    break;
                case "T":
                    keylineBitValue = 4;
                    break;
                case "U":
                    keylineBitValue = 5;
                    break;
                case "V":
                    keylineBitValue = 6;
                    break;
                case "W":
                    keylineBitValue = 7;
                    break;
                case "X":
                    keylineBitValue = 8;
                    break;
                case "Y":
                    keylineBitValue = 9;
                    break;
                case "Z":
                    keylineBitValue = 10;
                    break;
                case "/":
                    keylineBitValue = 15;
                    break;
                default:
                    keylineBitValue = int.Parse(workingKeyline.Substring(i, 1));
                    break;
            }

            if ((i % 2) == 0) { keylineBitValue *= 2; }

            while (keylineBitValue.ToString().Length > 1)
            {
                string keylineBit = keylineBitValue.ToString();
                keylineBitValue = 0;
                for (int x = 0; x <= keylineBit.Length - 1; x++)
                {
                    keylineBitValue += int.Parse(keylineBit.Substring(x, 1));
                }
            }

            checkDigit += keylineBitValue;

            checkDigit = checkDigit == 10 ? 0 : checkDigit;

        }//end for

        checkDigit = 10 - (checkDigit % 10);

        return new SqlString (string.Format("#{0}{1}#", workingKeyline, checkDigit.ToString()));
    }
}
