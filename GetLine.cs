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
        [CommandMethod("U_83_Get_lenght")] 
        public void GetLines()
        {
            Document acadDocument = Application.DocumentManager.MdiActiveDocument;
            Database acadCurDb = acadDocument.Database;
            using (Transaction acadTrans = acadCurDb.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = acadTrans.GetObject(acadCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord blockTableRecord = acadTrans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;

                var lines = from ObjectId polyline in blockTableRecord where acadTrans.GetObject(polyline, OpenMode.ForRead) is Polyline3d select (Polyline3d)acadTrans.GetObject(polyline, OpenMode.ForRead);
                var plines = from ObjectId polyline in blockTableRecord where acadTrans.GetObject(polyline, OpenMode.ForRead) is Polyline select (Polyline)acadTrans.GetObject(polyline, OpenMode.ForRead);
                foreach (Polyline3d line in lines)
                {
                    acadDocument.Editor.WriteMessage($"\nСлой 3d pol: {line.Layer}");
                    acadDocument.Editor.WriteMessage($"\nДлина 3d pol: {line.Length}");
                }
                foreach (Polyline line in plines)
                {
                    acadDocument.Editor.WriteMessage($"\nСлой pol: {line.Layer}");
                    acadDocument.Editor.WriteMessage($"\nДлина pol: {line.Length}");
                }
            }
        }
    }
}
