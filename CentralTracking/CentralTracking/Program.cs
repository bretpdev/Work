using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace CentralTracking
{
    static class Program
    {
        private static List<EntityType> entityTypeList;
        private static List<Entity> entityList;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            //ProcessLogger.RegisterApplication(Assembly.GetExecutingAssembly().GetName().Name, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly());


            //TODO: pull the current user info from database according to environment.username
            //Users currentUser = new Users();
            LoginHelper.Login();

            //SaveEntityType("DCR");
            //SaveAttributeDataType("Double");

            EntitiesDisplay entitiesDisplay = new EntitiesDisplay();
            entitiesDisplay.ShowDialog();
        }

        public static void SaveEntityType(string entityTypeDescription)
        {
            using (CentralTrackingEntities ct = new CentralTrackingEntities())
            {
                try
                {
                    EntityType et = ct.EntityTypes.AddOrFind(entityTypeDescription);
                    ct.SaveChanges();

                    et.Inactivate();
                    ct.SaveChanges();

                    string message = "";
                    foreach (EntityType item in ct.EntityTypes)
                    {
                        message += item.EntityTypeDescription + " " + item.EntityTypeId + "  " + item.Active + "\r\n";
                    }
                    MessageBox.Show(message);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n\r\n" + ex.InnerException);
                }


            }
        }

        public static void SaveAttributeDataType(string description)
        {
            using (CentralTrackingEntities ct = new CentralTrackingEntities())
            {
                try
                {
                    AttributeDataType dataType = ct.AttributeDataTypes.AddByDescription(description);
                    ct.SaveChanges();

                    dataType.Inactivate();
                    ct.SaveChanges();

                    string message = "";
                    foreach (AttributeDataType item in ct.AttributeDataTypes)
                    {
                        message += item.AttributeDataTypeDescription + " " + item.AttributeDataTypeId + "  " + item.Active + "\r\n";
                    }
                    MessageBox.Show(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n\r\n" + ex.InnerException);
                }
            }
        }

        public static void Entities()
        {
            using (CentralTrackingEntities ct = new CentralTrackingEntities())
            {
                try
                {
                    Entity entity = new Entity();
                    entity.EntityTypeId = (entityTypeList.Where(e => e.EntityTypeDescription == "Employee").Single()).EntityTypeId;
                    entity.EntityName = "System User";
                    entity.CreatedBy = 0;

                    ct.Entities.Add(entity);
                    ct.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n\r\n" + ex.InnerException);
                }

                entityList = (from e in ct.Entities
                              select e).ToList();

                string message = "";
                foreach (Entity item in entityList)
                {
                    message += item.EntityName + "\r\n";
                }
                MessageBox.Show(message);
            }
        }
    }
}
