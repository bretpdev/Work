using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddDateForFSAReports
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DateToRun d = new DateToRun();
            d.ShowDialog();
            List<FileData> data = new List<FileData>();
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = @"T:\";
            file.ShowDialog();


            using (StreamReader sr = new StreamReader(file.FileName))
            {
                while (!sr.EndOfStream)
                {
                    data.Add(FileData.Parse(sr.ReadLine()));
                }
            }
            string fileName = Path.GetExtension(file.FileName) == ".txt" ?
                (Path.GetFileNameWithoutExtension(file.FileName) + "Corrected.txt") : (Path.GetFileName(file.FileName) + "Corrected.txt");

            string newFileName = Path.Combine(@"T:\", fileName);
            using (StreamWriter sw = new StreamWriter(newFileName))
            {
                foreach (FileData line in data)
                {
                    sw.WriteLine(line.ToString(d.SelectedDate));
                }
            }

            MessageBox.Show("Complete");

        }
    }
}
