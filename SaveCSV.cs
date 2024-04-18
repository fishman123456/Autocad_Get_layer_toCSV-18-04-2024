using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Autocad_Get_layer_18_04_2024
{
    public class SaveCsv
    {
        public async void saveCsv(List<string> text)
        {
            // для русских букв в екселе подключить NUGET
            // NuGet\Install-Package
            // Вот нужный пакет 

            // System.Text.Encoding.CodePages

            // Вот нужный пакет 
            // -Version 8.0.0
            // Register the CodePages encoding provider at application startup to enable using single and double byte encodings.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // проверка по текущей дате
            CheckDateWork.CheckDate();
            // открываем диалог для сохранения файла в поток
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.Filter = "Text files(*.csv)|*.csv|All files(*.*)|*.*";
            // если не сохраняем то выходим
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string path = saveFileDialog1.FileName;
            // сохраняем текст в файл
            try
            {
                // полная перезапись файла  new FileStream(path, FileMode.OpenOrCreate), Encoding.GetEncoding(1251))
                using (StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding(1251)))
                {
                    // перебираем список в стрингбилдер
                    StringBuilder sb = new StringBuilder();
                    foreach (string line in text)
                    {
                        sb.Append(line);
                        sb.Append("\n");
                    }
                    // асинхронная перезапись  файла
                    await writer.WriteAsync(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

