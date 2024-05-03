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

                var lines = from ObjectId polyline in blockTableRecord where acadTrans.GetObject(polyline, OpenMode.ForRead) is Polyline2d select (Polyline2d)acadTrans.GetObject(polyline, OpenMode.ForRead);

                foreach (Polyline2d line in lines)
                {
                    acadDocument.Editor.WriteMessage("\nДлина: {0}", line.Length);
                }
            }
        }
    }
}
