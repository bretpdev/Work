using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace EmployeeHistory
{
    class Program
    {
        const string getUserIDXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetUserIDListInput><SynchronizationToken /></GetUserIDListInput>";
        const string xmlSyncToken = "";
        const int Username_Key = 112;
        const int Password_Key = 212;
        const string lastRecordOnlyToken = "[lastrecordonly]";
        static void Main(string[] args)
        {
            if (DataAccessHelper.StandardArgsCheck(args, "EmployeeHistory"))
            {
                string filterLoginId = args.Skip(1).SingleOrDefault();
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                DataAccessHelper.UseDefaultConsoleWriter();
                var pld = ProcessLogger.RegisterApplication("HREMPHIST", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());
                var client = new SilkRoadService.PSWebServiceSoapClient("PSWebServiceSoap" + (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live ? "LIVE" : ""));
                var parser = new XmlParser();
                var data = new DataAccess(new LogDataAccess(DataAccessHelper.CurrentMode, pld.ProcessLogId, false, false));

                var username = data.GetEncryptedCredential(Username_Key);
                var password = data.GetEncryptedCredential(Password_Key);

                string sessionnum = client.LogIn(username, password, "");
                var userList = new List<string>();
                var tempUserList = new List<string>();
                do
                {
                    string xml = getUserIDXml;
                    if (!string.IsNullOrEmpty(parser.LastSynchronizationToken))
                        xml = xml.Replace("<SynchronizationToken />", "<SynchronizationToken>" + parser.LastSynchronizationToken + "</SynchronizationToken>");
                    var userListXml = client.GetUserIDList(sessionnum, xml);
                    tempUserList = parser.ParseUserIds(userListXml.Data).ToList();
                    userList.AddRange(tempUserList);
                } while (tempUserList.Any());

                if (filterLoginId == lastRecordOnlyToken)
                    userList = userList.Skip(userList.Count - 1).ToList();
                else if (filterLoginId != null)
                    userList = userList.Where(o => filterLoginId == null || filterLoginId.ToLower() == o.ToLower()).ToList();

                Console.WriteLine("Found {0} live employee records.", userList.Count);
                //userList = new string[] { "John.Tester" }.ToList();
                foreach (var user in userList)
                {
                    Console.Write("Processing employee {0}: ", user);
                    var userData = client.GetUserProfileEx(sessionnum, user, "", "", "");
                    var serializer = new XmlSerializer(typeof(users));
                    var deserializedUserBlock = (users)serializer.Deserialize(new System.IO.StringReader(userData.Data));
                    //File.WriteAllText(user + ".txt", new System.IO.StringReader(userData.Data).ReadToEnd());
                    //continue;
                    var deserializedUser = deserializedUserBlock.Items[0];
                    if (deserializedUser.KeyProperties != null)
                        foreach (var keyProperty in deserializedUser.KeyProperties)
                        {
                            if (keyProperty.Person != null)
                            {
                                var manager = keyProperty.Person.SingleOrDefault(o => o.name == "Current Manager");
                                if (manager != null)
                                {
                                    deserializedUser.ManagerEmail = manager.Email;
                                    deserializedUser.ManagerAuthParam = manager.ManagerID;
                                    deserializedUser.ManagerLoginID = manager.LoginID;
                                    string name = manager.value;
                                    if (!name.Contains(' '))
                                        name += " ";
                                    deserializedUser.ManagerFirstName = name.Split(' ').First();
                                    deserializedUser.ManagerLastName = name.Split(' ').Last();
                                    break;
                                }
                            }
                        }

                    var pendingHistory = AvatierHistory.Parse(deserializedUser);
                    var existingHistory = data.AvatierHistoryGetMostRecent(pendingHistory.UserGuid);
                    if (existingHistory == null)
                    {
                        pendingHistory.UpdateTypeId = UpdateType.PROVISION;
                        data.AvatierHistoryInsert(pendingHistory);
                        Console.WriteLine("Recorded new history information.");
                    }
                    else if (AvatierHistory.SameHistory(pendingHistory, existingHistory))
                    {
                        Console.WriteLine("No changes found.");
                    }
                    else
                    {
                        if (pendingHistory.TerminationDate.HasValue && !existingHistory.TerminationDate.HasValue)
                        {
                            pendingHistory.UpdateTypeId = UpdateType.TERMINATION;
                            data.AvatierHistoryInsert(pendingHistory);
                            Console.WriteLine("Added termination record.");
                        }
                        if (pendingHistory.Role != existingHistory.Role)
                        {
                            pendingHistory.UpdateTypeId = UpdateType.TRANSFER;
                            data.AvatierHistoryInsert(pendingHistory);
                            Console.WriteLine("Added transfer record.");
                        }
                        if (pendingHistory.FirstName != existingHistory.FirstName || pendingHistory.MiddleName != existingHistory.MiddleName || pendingHistory.LastName != existingHistory.LastName)
                        {
                            pendingHistory.UpdateTypeId = UpdateType.RENAME;
                            data.AvatierHistoryInsert(pendingHistory);
                            Console.WriteLine("Added rename record.");
                        }
                        if (!AvatierHistory.SamePropertyPushFields(pendingHistory, existingHistory))
                        {
                            pendingHistory.UpdateTypeId = UpdateType.PROPERTYPUSH;
                            data.AvatierHistoryInsert(pendingHistory);
                            Console.WriteLine("Added property push record.");
                        }

                    }
                }
                ProcessLogger.LogEnd(pld.ProcessLogId);
            }
        }
    }
}
