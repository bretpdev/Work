using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace INCIDENTRP
{
	partial class DataAccess
	{
		[UsesSproc(IncidentReportingUheaa, "spDeleteThreatBackgroundNoise")]
		public void DeleteBackgroundNoise(long ticketNumber)
		{
			LDA.Execute("spDeleteThreatBackgroundNoise", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteThreatBombInfo")]
		public void DeleteBombInfo(long ticketNumber)
		{
			LDA.Execute("spDeleteThreatBombInfo", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteThreatCallerInfo")]
		public void DeleteCaller(long ticketNumber)
		{
			LDA.Execute("spDeleteThreatCallerInfo", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteThreatDialect")]
		public void DeleteDialect(long ticketNumber)
		{
			LDA.Execute("spDeleteThreatDialect", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}
		[UsesSproc(IncidentReportingUheaa, "spDeleteThreatLanguage")]

		public void DeleteLanguage(long ticketNumber)
		{
			LDA.Execute("spDeleteThreatLanguage", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteThreatManner")]
		public void DeleteManner(long ticketNumber)
		{
			LDA.Execute("spDeleteThreatManner", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteThreatInfo")]
		public void DeleteThreatInfo(long ticketNumber)
		{
			LDA.Execute("spDeleteThreatInfo", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spDeleteThreatVoice")]
		public void DeleteVoice(long ticketNumber)
		{
			LDA.Execute("spDeleteThreatVoice", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber));
		}

		[UsesSproc(IncidentReportingUheaa, "spGetThreatBackgroundNoise")]
		public BackgroundNoise LoadBackgroundNoise(long ticketNumber)
		{
			return LDA.ExecuteList<BackgroundNoise>("spGetThreatBackgroundNoise", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber),
				SqlParams.Single("TicketType", Ticket.THREAT)).Result.SingleOrDefault() ?? new BackgroundNoise();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetThreatBombInfo")]
		public BombInfo LoadBombInfo(long ticketNumber)
		{
			return LDA.ExecuteList<BombInfo>("spGetThreatBombInfo", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber),
				SqlParams.Single("TicketType", Ticket.THREAT)).Result.SingleOrDefault() ?? new BombInfo();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetThreatCallerInfo")]
		public Caller LoadCaller(long ticketNumber)
		{
			FlattenedCaller flat = LDA.ExecuteList<FlattenedCaller>("spGetThreatCallerInfo", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber),
				SqlParams.Single("TicketType", Ticket.THREAT)).Result.SingleOrDefault();
			Caller caller = new Caller();
			if (flat != null)
			{
				caller.AccountNumber = flat.AccountNumber;
				caller.Address = flat.Address;
				caller.Age = flat.Age;
				caller.CallDuration = flat.CallDuration;
				caller.CallerIsFamiliarWithUheaa = flat.CallerIsFamiliarWithUheaa;
				caller.FamiliaritySpecifics = flat.FamiliaritySpecifics;
				caller.Name = flat.Name;
				caller.PhoneNumber = flat.PhoneNumber;
				caller.RecognizedTheCallersVoice = flat.RecognizedTheCallersVoice;
				caller.Sex = flat.Sex;
			}
			return caller;
		}

		[UsesSproc(IncidentReportingUheaa, "spGetThreatDialect")]
		public Dialect LoadDialect(long ticketNumber)
		{
			return LDA.ExecuteList<Dialect>("spGetThreatDialect", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber),
				SqlParams.Single("TicketType", Ticket.THREAT)).Result.SingleOrDefault() ?? new Dialect();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetThreatLanguage")]
		public Language LoadLanguage(long ticketNumber)
		{
			return LDA.ExecuteList<Language>("spGetThreatLanguage", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber),
				SqlParams.Single("TicketType", Ticket.THREAT)).Result.SingleOrDefault() ?? new Language();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetThreatManner")]
		public Manner LoadManner(long ticketNumber)
		{
			return LDA.ExecuteList<Manner>("spGetThreatManner", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber),
				SqlParams.Single("TicketType", Ticket.THREAT)).Result.SingleOrDefault() ?? new Manner();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetThreatInfo")]
		public ThreatInfo LoadThreatInfo(long ticketNumber)
		{
			return LDA.ExecuteList<ThreatInfo>("spGetThreatInfo", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber),
				SqlParams.Single("TicketType", Ticket.THREAT)).Result.SingleOrDefault() ?? new ThreatInfo();
		}

		[UsesSproc(IncidentReportingUheaa, "spGetThreatVoice")]
		public Voice LoadVoice(long ticketNumber)
		{
			return LDA.ExecuteList<Voice>("spGetThreatVoice", IncidentReportingUheaa,
				SqlParams.Single("TicketNumber", ticketNumber),
				SqlParams.Single("TicketType", Ticket.THREAT)).Result.SingleOrDefault() ?? new Voice();
		}

		[UsesSproc(IncidentReportingUheaa, "spSetThreatBackgroundNoise")]
		public void SaveBackgroundNoise(BackgroundNoise noise, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Airplanes", noise.Airplanes));
			parms.Add(new SqlParameter("Animals", noise.Animals));
			parms.Add(new SqlParameter("Conversation", noise.Conversation));
			parms.Add(new SqlParameter("Crowd", noise.Crowd));
			parms.Add(new SqlParameter("FactoryMachines", noise.FactoryMachines));
			parms.Add(new SqlParameter("Music", noise.Music));
			parms.Add(new SqlParameter("OfficeMachines", noise.OfficeMachines));
			parms.Add(new SqlParameter("Party", noise.Party));
			parms.Add(new SqlParameter("PublicAddressSystem", noise.PublicAddressSystem));
			parms.Add(new SqlParameter("SchoolBell", noise.SchoolBell));
			parms.Add(new SqlParameter("StreetTraffic", noise.StreetTraffic));
			parms.Add(new SqlParameter("Trains", noise.Trains));
			parms.Add(new SqlParameter("Other", noise.Other));
			parms.Add(new SqlParameter("OtherDescription", noise.OtherDescription));
			LDA.Execute("spSetThreatBackgroundNoise", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetThreatBombInfo")]
		public void SaveBombInfo(BombInfo info, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Location", info.Location));
			parms.Add(new SqlParameter("DetonationTime", info.DetonationTime));
			parms.Add(new SqlParameter("Appearance", info.Appearance));
			parms.Add(new SqlParameter("WhoPlacedAndWhy", info.WhoPlacedAndWhy));
			parms.Add(new SqlParameter("CallerName", info.CallerName));
			LDA.Execute("spSetThreatBombInfo", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetThreatCallerInfo")]
		public void SaveCaller(Caller caller, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("CallDuration", caller.CallDuration));
			parms.Add(new SqlParameter("RecognizedTheCallersVoice", caller.RecognizedTheCallersVoice));
			parms.Add(new SqlParameter("CallerIsFamiliarWithUheaa", caller.CallerIsFamiliarWithUheaa));
			parms.Add(new SqlParameter("FamiliaritySpecifics", caller.FamiliaritySpecifics));
			parms.Add(new SqlParameter("Sex", caller.Sex));
			parms.Add(new SqlParameter("Age", caller.Age));
			parms.Add(new SqlParameter("Name", caller.Name));
			parms.Add(new SqlParameter("PhoneNumber", caller.PhoneNumber));
			parms.Add(new SqlParameter("Address", caller.Address));
			parms.Add(new SqlParameter("AccountNumber", caller.AccountNumber));
			LDA.Execute("spSetThreatCallerInfo", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetThreatDialect")]
		public void SaveDialect(Dialect dialect, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("English", dialect.English));
			parms.Add(new SqlParameter("RegionalAmerican", dialect.RegionalAmerican));
			parms.Add(new SqlParameter("RegionalAmericanDescription", dialect.RegionalAmericanDescription));
			parms.Add(new SqlParameter("ForeignAccent", dialect.ForeignAccent));
			parms.Add(new SqlParameter("ForeignAccentDescription", dialect.ForeignAccentDescription));
			LDA.Execute("spSetThreatDialect", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetThreatLanguage")]
		public void SaveLanguage(Language language, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Educated", language.Educated));
			parms.Add(new SqlParameter("Uneducated", language.Uneducated));
			parms.Add(new SqlParameter("FoulOrProfane", language.FoulOrProfane));
			parms.Add(new SqlParameter("Other", language.Other));
			parms.Add(new SqlParameter("OtherDescription", language.OtherDescription));
			LDA.Execute("spSetThreatLanguage", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetThreatManner")]
		public void SaveManner(Manner manner, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Angry", manner.Angry));
			parms.Add(new SqlParameter("BusinessLike", manner.BusinessLike));
			parms.Add(new SqlParameter("Calm", manner.Calm));
			parms.Add(new SqlParameter("Coherent", manner.Coherent));
			parms.Add(new SqlParameter("Deliberate", manner.Deliberate));
			parms.Add(new SqlParameter("Emotional", manner.Emotional));
			parms.Add(new SqlParameter("IllAtEase", manner.IllAtEase));
			parms.Add(new SqlParameter("Incoherent", manner.Incoherent));
			parms.Add(new SqlParameter("Irrational", manner.Irrational));
			parms.Add(new SqlParameter("Laughing", manner.Laughing));
			parms.Add(new SqlParameter("Rational", manner.Rational));
			parms.Add(new SqlParameter("Righteous", manner.Righteous));
			parms.Add(new SqlParameter("Shouting", manner.Shouting));
			parms.Add(new SqlParameter("Slow", manner.Slow));
			parms.Add(new SqlParameter("Other", manner.Other));
			parms.Add(new SqlParameter("OtherDescription", manner.OtherDescription));
			LDA.Execute("spSetThreatManner", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetThreatInfo")]
		public void SaveThreatInfo(ThreatInfo info, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Wording", info.WordingOfThreat));
			parms.Add(new SqlParameter("Nature", info.NatureOfCall));
			parms.Add(new SqlParameter("Remarks", info.AdditionalRemarks));
			LDA.Execute("spSetThreatInfo", IncidentReportingUheaa, parms.ToArray());
		}

		[UsesSproc(IncidentReportingUheaa, "spSetThreatVoice")]
		public void SaveVoice(Voice voice, long ticketNumber)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("TicketNumber", ticketNumber));
			parms.Add(new SqlParameter("Distinct", voice.Distinct));
			parms.Add(new SqlParameter("Distorted", voice.Distorted));
			parms.Add(new SqlParameter("Fast", voice.Fast));
			parms.Add(new SqlParameter("High", voice.High));
			parms.Add(new SqlParameter("Hoarse", voice.Hoarse));
			parms.Add(new SqlParameter("Lisp", voice.Lisp));
			parms.Add(new SqlParameter("Nasal", voice.Nasal));
			parms.Add(new SqlParameter("Slow", voice.Slow));
			parms.Add(new SqlParameter("Slurred", voice.Slurred));
			parms.Add(new SqlParameter("Stuttering", voice.Stuttering));
			parms.Add(new SqlParameter("Other", voice.Other));
			parms.Add(new SqlParameter("OtherDescription", voice.OtherDescription));
			LDA.Execute("spSetThreatVoice", IncidentReportingUheaa, parms.ToArray());
		}

		#region Projection Classes

		private class FlattenedCaller
		{
			public string CallDuration { get; set; }
			public bool RecognizedTheCallersVoice { get; set; }
			public bool CallerIsFamiliarWithUheaa { get; set; }
			public string FamiliaritySpecifics { get; set; }
			public string Sex { get; set; }
			public string Age { get; set; }
			public string Name { get; set; }
			public string PhoneNumber { get; set; }
			public string Address { get; set; }
			public string AccountNumber { get; set; }
		}

		#endregion Projection Classes
	}
}