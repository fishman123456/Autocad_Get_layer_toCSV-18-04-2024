using Autodesk.AutoCAD.Runtime;
using System.Windows.Forms;

namespace Autocad_Get_layer_18_04_2024

{
    public class Cmds

    {
        [CommandMethod("PB")]

        public void ProgressBarManaged(int countst)

        {

            ProgressMeter pm = new ProgressMeter();

            pm.Start("Testing Progress Bar");

            pm.SetLimit(100);

            // Now our lengthy operation

            for (int i = 0; i <= countst; i++)

            {

                System.Threading.Thread.Sleep(50);

                // Increment Progress Meter...

                pm.MeterProgress();

                // This allows AutoCAD to repaint

                Application.DoEvents();

            }

            pm.Stop();

        }

    }

}