using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocad_Get_layer_18_04_2024
{
    public class GetLine
    {
        [CommandMethod("U83_Get_lenght")] 
        public void GetLines()
        {
            List<string> poliThreeLenght = new List<string>();
            List<string> poliLenght = new List<string>();
            List<string> linesLenght = new List<string>();
            List<string> stringsAll = new List<string>();
            int countStr = 0;
            
            Document acadDocument = Application.DocumentManager.MdiActiveDocument;
            Database acadCurDb = acadDocument.Database;
            using (Transaction tr = acadCurDb.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = tr.GetObject(acadCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord blockTableRecord = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                try
                {  
                var linesTreeD = from ObjectId polyline in blockTableRecord where tr.GetObject(polyline, OpenMode.ForRead) is Polyline3d select (Polyline3d)tr.GetObject(polyline, OpenMode.ForRead);
                var plines = from ObjectId polyline in blockTableRecord where tr.GetObject(polyline, OpenMode.ForRead) is Polyline select (Polyline)tr.GetObject(polyline, OpenMode.ForRead);
                var linesD = from ObjectId polyline in blockTableRecord where tr.GetObject(polyline, OpenMode.ForRead) is Line select (Line)tr.GetObject(polyline, OpenMode.ForRead);
                if (linesTreeD != null)
                {
                    foreach (Polyline3d line in linesTreeD)
                    {
                        poliThreeLenght.Add(line.Layer.ToString() + "\t" + line.Length.ToString());
                        stringsAll.Add(line.Layer.ToString() + "\t" + line.Length.ToString());
                        acadDocument.Editor.WriteMessage($"\nСлой 3d pol: {line.Layer}");
                        acadDocument.Editor.WriteMessage($"\nДлина 3d pol: {line.Length}");
                            countStr++;
                    }
                }
                if (linesD != null)
                {
                    foreach (Polyline line in plines)
                    {       
                        poliLenght.Add(line.Layer.ToString() + "\t" + line.Length.ToString());
                        stringsAll.Add(line.Layer.ToString() + "\t" + line.Length.ToString());
                        acadDocument.Editor.WriteMessage($"\nСлой  pol: {line.Layer}");
                        acadDocument.Editor.WriteMessage($"\nДлина  pol: {line.Length}");
                            countStr++;
                    }
                }
                if (linesD != null)
                {
                    foreach (Line line in linesD)
                    {
                        linesLenght.Add(line.Layer.ToString() + "\t" + line.Length.ToString());
                        stringsAll.Add(line.Layer.ToString() + "\t" + line.Length.ToString());
                        acadDocument.Editor.WriteMessage($"\nСлой line: {line.Layer}");
                        acadDocument.Editor.WriteMessage($"\nДлина line: {line.Length}");
                            countStr++;
                    }
                }
                }
                catch (System.Exception ex)
                {
                    acadDocument.Editor.WriteMessage($"\n {ex.Message}");
                    tr.Abort();
                }
                acadDocument.Editor.WriteMessage($"\n кол-во обьектов: {countStr}");
                SaveCsv saveCsv = new SaveCsv();
                saveCsv.saveCsv(stringsAll);
            }
        }
    }
}
