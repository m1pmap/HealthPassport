using DocumentFormat.OpenXml.Packaging;
using HealthPassport.DAL.Interfaces;
using HealthPassport.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Packaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Windows;
using System.Collections.Generic;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using HealthPassport.DAL.Models;
using System.Drawing;
using System.Globalization;
using System.Windows.Media.Imaging;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using DocumentFormat.OpenXml.Drawing.Charts;
using HealthPassport.Interfaces;
using System.Collections.ObjectModel;

namespace HealthPassport.Services
{ 
    public class WordWorker_Service : IExportWorker
    {
        private readonly ByteArrayToImageSourceConverter_Service _imageSourceConverter;
        public WordWorker_Service(ByteArrayToImageSourceConverter_Service imageSourceConverter)
        {
            imageSourceConverter = _imageSourceConverter;
        }
        public void exportEmployeeAntropologicalResearches(Employee_ViewModel employee)
        {
            throw new NotImplementedException();
        }

        public void exportEmployeeDiseases(Employee_ViewModel employee)
        {
            throw new NotImplementedException();
        }

        public void exportEmployeeEducations(Employee_ViewModel employee)
        {
            throw new NotImplementedException();
        }

        public void exportEmployeeFamilyStatuses(Employee_ViewModel employee)
        {
            throw new NotImplementedException();
        }

        public void exportEmployeeInfo(Employee_ViewModel employee)
        {
            //try
            //{
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Word Files (*.doc)|*.doc",
                    Title = "Сохранить Excel документ"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
                    {
                        // Добавление основного элемента (main part)
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = new Body();
                        mainPart.Document.Append(body);

                        // Добавление заголовка
                        ParagraphProperties paragraphProperties = new ParagraphProperties
                        {
                            Justification = new Justification() { Val = JustificationValues.Center },
                            SpacingBetweenLines = new SpacingBetweenLines { After = "0" }

                        };


                        Run fioHeaderRun = new Run();
                        SetRunStyles(fioHeaderRun, employee.FIO ,"Arial Black", 17, "Gray");


                        Paragraph paragraph = new Paragraph();
                        paragraph.Append(paragraphProperties);
                        paragraph.Append(fioHeaderRun);
                        body.Append(paragraph);

                        // Добавление таблицы
                        Table table = new Table();

                        // Создание свойств таблицы
                        TableProperties tableProperties = new TableProperties(
                            new TableBorders(
                                new TopBorder { Val = BorderValues.None },
                                new BottomBorder { Val = BorderValues.None },
                                new LeftBorder { Val = BorderValues.None },
                                new RightBorder { Val = BorderValues.None },
                                new InsideHorizontalBorder { Val = BorderValues.None },
                                new InsideVerticalBorder { Val = BorderValues.None }
                            )
                        );
                        table.Append(tableProperties);

                        // Добавление строки таблицы
                        TableRow tableRow = new TableRow();

                        // Добавление ячейки с изображением
                        TableCell imageCell = new TableCell();
                        Drawing element = AddImageToWord(mainPart, employee.Photo);
                        var imageParagraph = new Paragraph(new Run(element));
                        imageCell.Append(imageParagraph);
                        tableRow.Append(imageCell);

                        // Добавление ячейки с текстом
                        TableCell textCell = new TableCell();
                        textCell.Append(SetSimpleParagraphStyles(new Paragraph(new Run(new Text($"Дата рождения: {employee.Birthday}"))), JustificationValues.Left, false));
                        textCell.Append(SetSimpleParagraphStyles(new Paragraph(new Run(new Text($"Должность: {employee.Job}"))), JustificationValues.Left, false));
                        textCell.Append(SetSimpleParagraphStyles(new Paragraph(new Run(new Text($"Подразделение: {employee.Jobs[0].Subunit}"))), JustificationValues.Left, false));
                        textCell.Append(SetSimpleParagraphStyles(new Paragraph(new Run(new Text($"Последняя болезнь: {employee.Diseases[0].Name}"))), JustificationValues.Left, false));
                        textCell.Append(SetSimpleParagraphStyles(new Paragraph(new Run(new Text($"Семейное положение: {employee.FamilyStatus}"))), JustificationValues.Left, false));
                        textCell.Append(SetSimpleParagraphStyles(new Paragraph(new Run(new Text($"Образование: {employee.Education}"))), JustificationValues.Left, false));
                        textCell.Append(SetSimpleParagraphStyles(new Paragraph(new Run(new Text($"Почта: {employee.MailAdress}"))), JustificationValues.Left, false));
                        //textCell.Append(paragraphProperties);
                        tableRow.Append(textCell);

                        table.Append(tableRow);
                        body.Append(table);

                        mainPart.Document.Save();
                    }
                }
            //}
            //catch (InvalidOperationException)
            //{
            //    MessageBox.Show("Файл открыт в другой программе.", "Открытие невозможно", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        public void exportEmployeeJobs(Employee_ViewModel employee)
        {
            throw new NotImplementedException();
        }

        public void exportEmployeeVaccinations(Employee_ViewModel employee)
        {
            throw new NotImplementedException();
        }










        private void SetRunStyles(Run run, string text, string fontFamily = null, double fontSize = 14, string forground = null)
        {
            RunProperties runProperties = new RunProperties();
            if(fontFamily != null)
            {
                runProperties.Append(new RunFonts() { HighAnsi = fontFamily });
            }
            runProperties.Append(new FontSize() { Val = (fontSize * 2).ToString() });

            if (forground != null)
            {
                runProperties.Append(new Color() { Val = forground });
            }

            run.Append(runProperties);
            run.Append(new Text(text));
        }

        private Paragraph SetSimpleParagraphStyles(Paragraph paragraph, JustificationValues justification, bool isSpacingBetweenLines = false)
        {
            ParagraphProperties paragraphProperties = new ParagraphProperties
            {
                Justification = new Justification() { Val = justification },
                SpacingBetweenLines = new SpacingBetweenLines { After = "0"}
            };

            if (!isSpacingBetweenLines) // Если НЕ нужно межстрочное расстояние
            {
                paragraphProperties.Append(new SpacingBetweenLines() { After = "-5" }); // Убираем интервал после абзаца
            }

            paragraph.Append(paragraphProperties);

            return paragraph;
        }


        private Drawing AddImageToWord(MainDocumentPart mainPart, byte[] imageData)
        {
            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
            using (var stream = new MemoryStream(imageData))
            {
                imagePart.FeedData(stream);
            }

            var element = new Drawing(
                new DW.Inline(
                    new DW.Extent { Cx = 1806500L, Cy = 1752500L }, //в EMUs
                    new DW.EffectExtent { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
                    new DW.DocProperties { Id = (UInt32Value)1U, Name = "Picture 1" },
                    new DW.NonVisualGraphicFrameDrawingProperties(new DocumentFormat.OpenXml.Drawing.GraphicFrameLocks { NoChangeAspect = true }),
                    new DocumentFormat.OpenXml.Drawing.Graphic(
                        new DocumentFormat.OpenXml.Drawing.GraphicData(
                            new DocumentFormat.OpenXml.Drawing.Pictures.Picture(
                                new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureProperties(
                                    new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualDrawingProperties { Id = (UInt32Value)0U, Name = "New Image" },
                                    new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureDrawingProperties()),
                                new DocumentFormat.OpenXml.Drawing.Pictures.BlipFill(
                                    new DocumentFormat.OpenXml.Drawing.Blip(
                                        new DocumentFormat.OpenXml.Drawing.BlipExtensionList(
                                            new DocumentFormat.OpenXml.Drawing.BlipExtension { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" })
                                        )
                                    {
                                        Embed = mainPart.GetIdOfPart(imagePart),
                                        CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print
                                    },
                                    new DocumentFormat.OpenXml.Drawing.Stretch(new DocumentFormat.OpenXml.Drawing.FillRectangle())),
                                new DocumentFormat.OpenXml.Drawing.Pictures.ShapeProperties(
                                    new DocumentFormat.OpenXml.Drawing.Transform2D(
                                        new DocumentFormat.OpenXml.Drawing.Offset { X = 0L, Y = 0L },
                                        new DocumentFormat.OpenXml.Drawing.Extents { Cx = 1218000L, Cy = 1256300L }
                                    ),
                                    new DocumentFormat.OpenXml.Drawing.PresetGeometry(new DocumentFormat.OpenXml.Drawing.AdjustValueList()) { Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle })
                            )
                        )
                        {
                            Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture"
                        }
                    )
                )
            );
            return element;
        }

        public void exportEmployeesListInfo(ObservableCollection<Employee_ViewModel> employees, string label)
        {
            throw new NotImplementedException();
        }
    }
}
