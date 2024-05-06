using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices.Filters;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

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
            Editor ed = acadDocument.Editor;
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
                        //    stringsAll.Add("'" + line.Handle + ";" + line.GetType() + ";" + line.Layer.ToString() + ";" + line.Length.ToString());
                            acadDocument.Editor.WriteMessage($"\nСлой 3d pol: {line.Layer} номер {line.Handle}");
                            acadDocument.Editor.WriteMessage($"\nДлина 3d pol: {line.Length}");
                            countStr++;
                        }
                        Cmds cmds = new Cmds();
                        cmds.ProgressBarManaged(linesTreeD.Count());
                    }
                    if (linesD != null)
                    {
                        foreach (Polyline line in plines)
                        {
                            poliLenght.Add(line.Layer.ToString() + "\t" + line.Length.ToString());
                          //  stringsAll.Add("'" + line.Handle + ";" + line.GetType() + ";" + line.Layer.ToString() + ";" + line.Length.ToString());
                            acadDocument.Editor.WriteMessage($"\nСлой  pol: {line.Layer} номер {line.Handle}");
                            acadDocument.Editor.WriteMessage($"\nДлина  pol: {line.Length}");
                            countStr++;
                        }
                        Cmds cmds = new Cmds();
                        cmds.ProgressBarManaged(plines.Count());
                    }
                    #region
                    // пробуем с выбором обьектов 2 д полилинии
                    PromptSelectionOptions opts = new PromptSelectionOptions();
                    opts.MessageForAdding = "Выберите обьекты: ";
                    PromptSelectionResult res = ed.GetSelection(opts);
                    if (res.Status != PromptStatus.OK)
                    { return; }
                    SelectionSet selSet = res.Value;
                    // добавляем в массив выбранные обьекты
                    ObjectId[] idArray = selSet.GetObjectIds();

                    foreach (ObjectId id in idArray)
                    {
                            Polyline ent = tr.GetObject(id, OpenMode.ForWrite) as Polyline;
                            acadDocument.Editor.WriteMessage($"\nСлой pol: {ent.Layer} номер {ent.Handle} длина {ent.Length}");
                            stringsAll.Add("'" + ent.Handle + ";" + ent.GetType() + ";" + ent.Layer.ToString() + ";" + ent.Length.ToString());   
                    }
                    #endregion
                    //if (linesD != null)
                    //{
                    //    foreach (Line line in linesD)
                    //    {
                    //        linesLenght.Add(line.Layer.ToString() + "\t" + line.Length.ToString());
                    //        stringsAll.Add("'" + line.Handle + ";" + line.GetType() + ";" + line.Layer.ToString() + ";" + line.Length.ToString());
                    //        acadDocument.Editor.WriteMessage($"\nСлой line: {line.Layer} номер {line.Handle}");
                    //        acadDocument.Editor.WriteMessage($"\nДлина line: {line.Length}");
                    //        countStr++;
                    //    }
                    //    Cmds cmds = new Cmds();
                    //    cmds.ProgressBarManaged(linesD.Count());
                    //}
                }
                catch (System.Exception ex)
                {
                    acadDocument.Editor.WriteMessage($"\n {ex.Message}");
                    tr.Abort();
                }
                acadDocument.Editor.WriteMessage($"\nКол-во обьектов: {countStr}");

                SaveCsv saveCsv = new SaveCsv();
                saveCsv.saveCsv(stringsAll);
            }
        }
    }
}
