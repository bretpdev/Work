using System;
using System.Collections.Generic;
using System.Threading;
using Q;

namespace OLCOSKPREQ
{
	class ErrorReport : ScriptSessionBase
	{
		private const string FILE_NAME = @"T:\OneLINKskipNotProcessed.doc";
		private List<string> _exclusions;
		private List<string> _ssns;

		public ErrorReport(ReflectionInterface ri)
			: base(ri)
		{
			_exclusions = new List<string>();
			_ssns = new List<string>();
		}

		public void Add(string ssn)
		{
			if (!_exclusions.Contains(ssn)) { _ssns.Add(ssn); }
		}//Add()

		public void Exclude(string ssn)
		{
			_exclusions.Add(ssn);
			if (_ssns.Contains(ssn)) { _ssns.Remove(ssn); }
		}//Exclude()

		public void Print()
		{
			if (_ssns.Count == 0) { return; }

			using (WordFacade turd = WordFacade.CreateDocument())
			{
				turd.Visible = false;
				//set font and insert text
				turd.FontSize = 20;
				turd.WriteLine("OneLINK Skip Requests Not Processed");
				turd.FontSize = 12;
				foreach (string ssn in _ssns)
				{
					string accountNumber = "";
					try
					{
						accountNumber = GetDemographicsFromLP22(ssn).AccountNumber;
					}
					catch (Exception)
					{
						//If the SSN is null, empty, or not on LP22, leave the account number blank.
					}
					turd.WriteLine(accountNumber);
				}//foreach

				turd.SaveAs(FILE_NAME);
				Thread.Sleep(2000);

				turd.Print();
			}//using
		}//Print()
	}//class
}//namespace
