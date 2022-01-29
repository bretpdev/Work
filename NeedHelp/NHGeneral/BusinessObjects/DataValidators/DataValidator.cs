using System;
using SubSystemShared;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Policy;

namespace NHGeneral
{
    class DataValidator
    {
        /// <summary>
        /// Checks if data is valid based off table data noted validator.  Throws InvalidUserInputException exception if data is not valid.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="step"></param>
        public static void IsValidData(TicketData data, FlowStep step)
        {
            if (!string.IsNullOrEmpty(step.DataValidationID))
            {
                //create correct validator and try and validate the data.
                //BaseDataValidator validationProcessor = Activator.CreateInstance(Assembly.GetExecutingAssembly().GetName().Name, string.Format("NeedHelp.{0}", step.DataValidationID), false, BindingFlags.Default, null, (new Object[] { }), null, (new Object[] { }), new Evidence()).Unwrap() as BaseDataValidator;
                BaseDataValidator validationProcessor;
                List<string> errors = new List<string>();
                switch (step.DataValidationID.Trim())
                {
                    case "DV1":
                        validationProcessor = new DV1();
                        errors = validationProcessor.ValidateData(data);
                        break;
                    case "DV2":
                        validationProcessor = new DV2();
                        errors = validationProcessor.ValidateData(data);
                        break;
                    case "DV3":
                        validationProcessor = new DV3();
                        errors = validationProcessor.ValidateData(data);
                        break;
                    case "DV4":
                        validationProcessor = new DV4();
                        errors = validationProcessor.ValidateData(data);
                        break;
                    case "DV5":
                        validationProcessor = new DV5();
                        errors = validationProcessor.ValidateData(data);
                        break;
                    case "DV6":
                        validationProcessor = new DV6();
                        errors = validationProcessor.ValidateData(data);
                        break;
                    case "DV7":
                        validationProcessor = new DV7();
                        errors = validationProcessor.ValidateData(data);
                        break;
                    case "DV8":
                        validationProcessor = new DV8();
                        errors = validationProcessor.ValidateData(data);
                        break;
                }

                if (errors.Count > 0)
                {
                    //if errors were found during validation then throw an exception for the UI to catch
                    throw new InvalidUserInputException(errors);
                }
            }
        }
    }
}