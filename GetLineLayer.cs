using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocad_Get_layer_18_04_2024
{
    public class GetLineLayer
    {
        // This method can have any name
        [CommandMethod("U83_Get_lay")]
        public void TestDisplayLayers()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            List<string> info = LayersToList(db);
            ed.WriteMessage(DateTime.Now.ToString()+"\n" + "кол-во кабеля" + "\n");
            foreach (string lname in info)
                ed.WriteMessage(lname+"\n");
        }

        public List<string> LayersToList(Database db)
        {
            // Открываем для редактирования документ
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            List<string> lstlay = new List<string>();

            //LayerTableRecord layer;
            using (Transaction tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                try
                {


                    LayerTable lt = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                    // Создайте список фильтров таким образом, чтобы только
                    // выбираются ссылки на блоки

                    TypedValue[] filList = new TypedValue[] {
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "<and"),
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "<or"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "POLYLINE"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "LWPOLYLINE"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "POLYLINE2D"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "POLYLINE3d"),
            new TypedValue(Convert.ToInt32(DxfCode.Start), "CIRCLE"),
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "or>"),
            new TypedValue(Convert.ToInt32(DxfCode.Operator), "and>")
        };
                    SelectionFilter filter = new SelectionFilter(filList);
                    PromptSelectionOptions opts = new PromptSelectionOptions();
                    opts.MessageForAdding = "Выберите обьекты: ";
                    PromptSelectionResult res = ed.GetSelection(opts, filter);
                    // Do nothing if selection is unsuccessful

                    if (res.Status != PromptStatus.OK)
                        return lstlay;
                    SelectionSet selSet = res.Value;
                    // добавляем в массив выбранные обьекты
                    ObjectId[] idArray = selSet.GetObjectIds();

                    int countK = 0;
                    // код для создания  слоёв выбранных полилиний
                    foreach (ObjectId id in idArray)
                    {
                        Entity ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;
                        countK++;
                        var layerW = ent.Layer.ToString();
                        lstlay.Add(layerW);
                    }
                    lstlay.Insert(0, countK.ToString());
                    Cmds cmds = new Cmds();
                    cmds.ProgressBarManaged(countK);
                    // код для создания существующих не замороженных слоёв
                    //foreach (ObjectId layerId in lt)
                    //{
                    //    layer = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                    //    if (!layer.IsFrozen)
                    //    {
                    //        lstlay.Add(layer.Name);
                    //    }

                    //}
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage($"\n {ex.Message}");
                    tr.Abort();
                }
            }

            // сохраняем в файл
            SaveCsv saveCsv = new SaveCsv();
            saveCsv.saveCsv(lstlay);
            return lstlay;
        }
    }
}
