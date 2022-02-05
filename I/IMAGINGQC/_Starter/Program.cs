using Q;

namespace _Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            ReflectionInterface ri = new ReflectionInterface(true);
            ri.PauseForInsert();

            new IMAGINGQC.ImagingQC(ri).Main();
            ri.CloseSession();
        }
    }
}
