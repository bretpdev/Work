
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBM.Data.DB2;

namespace ConnectToAES
{
    class Program
    {
        static void Main(string[] args)
        {


            //ExecuteComQuery();
            //ExecuteFedQuery();
            //ExecuteComQueryTest();
            //ExecuteFedQueryVUK3();
            ExecuteFedQueryVUk1();
         
        }

        private static void ExecuteFedQuery()
        {
            Console.WriteLine("Fed Query");
            DB2Connection conn1 = new DB2Connection();

            conn1.ConnectionString = "Database=DNFPUTDL;UserID=rmbatch1;Password=A1apples;Server=host203.aessuccess.org:41520";
            conn1.Open();
            Console.WriteLine("Connected");
            string query = "select distinct bf_ssn, ln_seq from PKUB.ln10_lon where bf_ssn = '041823505'";
            DB2Command cmd = conn1.CreateCommand();
            cmd.CommandText = query;
            DB2DataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine(string.Format("{0},{1}", r.GetString(0), r.GetInt16(1)));
            }
            Console.ReadKey();
            Console.WriteLine("Complete");

            conn1.Close();
        }

        private static void ExecuteFedQueryVUk1()
        {
            Console.WriteLine("Fed Query");
            DB2Connection conn1 = new DB2Connection("Database=DNFPRQUT;UserID=rmbatch1;Password=A1apples;Server=host203.aessuccess.org:42520");

            //conn1.ConnectionString = "Database=DNFPRQUT;UserID=rmbatch1;Password=A1apples;Server=host203.aessuccess.org:42520";
            conn1.Open();
            Console.WriteLine("Connected");
            string query = "DECLARE GLOBAL TEMPORARY TABLE SESSION.NOPD30_LOANS (ACC_ID  CHAR(10)) ON COMMIT PRESERVE ROWS; ";
            DB2Command cmd = conn1.CreateCommand();
            //cmd.CommandText = query;
            //cmd.ExecuteNonQuery();
            //query = "INSERT INTO SESSION.NOPD30_LOANS select distinct bf_ssn from PKUB.ln10_lon where bf_ssn = '001780577'";
            //cmd.CommandText = query;
            //cmd.ExecuteNonQuery();
            query = "select * from PKUB.ln10_lon";
            cmd.CommandText = query;
            
            DB2DataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine(string.Format("{0}", r.GetString(0)));
            }
            Console.ReadKey();
            Console.WriteLine("Complete");

            conn1.Close();
        }

        private static void ExecuteFedQueryVUK3()
        {
            Console.WriteLine("Fed Query");
            DB2Connection conn1 = new DB2Connection();

            conn1.ConnectionString = "Database=DNFPRUUT;UserID=rmbatch1;Password=A1apples;Server=12.177.52.203:42520";
            conn1.Open();
            Console.WriteLine("Connected");
            string query = "select distinct bf_ssn, ln_seq from PKUB.ln10_lon where bf_ssn = '063646001'";
            DB2Command cmd = conn1.CreateCommand();
            cmd.CommandText = query;
            DB2DataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine(string.Format("{0},{1}", r.GetString(0), r.GetInt16(1)));
            }
            Console.ReadKey();
            Console.WriteLine("Complete");

            conn1.Close();
        }

        private static void ExecuteComQuery()
        {
            Console.WriteLine("Uheaa Query");
            DB2Connection conn1 = new DB2Connection();

            conn1.ConnectionString = "Database=DLGSUTWH;UserID=rmbatch1;Password=A1apples;Server=host143.aessuccess.org:40070";
            conn1.Open();
            Console.WriteLine("Connected");
            string query = "select distinct bf_ssn, ln_seq from OLWHRM1.ln10_lon WHERE BF_SSN = '042645496'";
            DB2Command cmd = conn1.CreateCommand();
            cmd.CommandText = query;
            DB2DataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine(string.Format("{0},{1}", r.GetString(0), r.GetInt16(1)));
            }
            Console.WriteLine("Complete");
            Console.ReadKey();



            conn1.Close();
        }

        private static void ExecuteComQueryTest()
        {
            Console.WriteLine("Uheaa Query");
            DB2Connection conn1 = new DB2Connection();

            conn1.ConnectionString = "Database=DLGSWQUT;UserID=rmjryan;Password=A1almond;Server=12.177.52.34:42130";
            conn1.Open();
            Console.WriteLine("Connected");
            string query = "select distinct bf_ssn, ln_seq from OLWHRM1.ln10_lon WHERE BF_SSN = '046740495'";
            DB2Command cmd = conn1.CreateCommand();
            cmd.CommandText = query;
            DB2DataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                Console.WriteLine(string.Format("{0},{1}", r.GetString(0), r.GetInt16(1)));
            }
            Console.WriteLine("Complete");
            Console.ReadKey();



            conn1.Close();
        }
    }
}
