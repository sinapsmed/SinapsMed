using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Forms.Enum;
using Entities.DTOs.FormDtos;
using Entities.Enums.Appointment;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DataAccess.Concrete.SQLServer.EFDALs.Appointments
{
    public class AppointmentService
    {
        private static readonly Random _random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string GenerateUniqueUnicalKey(AppDbContext dbContext)
        {
            string newId;
            do
            {
                newId = "SMR-" + new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            }
            while (dbContext.Appointments.Any(u => u.UnicalKey == newId));

            return newId;
        }
        public static string GeneratePdf(AnamnezFormDetailed form)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var pathDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "AnamnezForms");
            if (!Directory.Exists(pathDirectory))
                Directory.CreateDirectory(pathDirectory);

            var path = Path.Combine(pathDirectory, $"{form.Id}.pdf");
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header()
                        .ShowOnce()
                        .Column(col =>
                        {
                            col.Item().AlignRight().Row(row =>
                            {
                                row.ConstantItem(100).Image(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "Mail", "Logo.png"));
                            });

                            col.Item().AlignCenter().Text("ANAMNEZ FORMU - " + form.Service.Title)
                                .SemiBold().FontSize(20).FontColor(Colors.Black);

                            col.Item().AlignCenter().Text("Xəstəlik tarixi " + form.Date.ToString("dd/MM/yyyy"))
                                .FontSize(11).FontColor(Colors.Grey.Darken1);
                        });

                    page.Content()
                        .PaddingHorizontal(20)
                        .PaddingVertical(10)
                        .Column(col =>
                        {
                            #region RowSpans
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10).Text("1. Bölüm").SemiBold().Italic().FontSize(16);
                                 col.Item().PaddingLeft(11).Text($"{form.Service.Title}").FontSize(13);
                             });

                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10).Text("2. Mütəxəssisin adı, soyadı").SemiBold().Italic().FontSize(16);
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.Expert.FullName}").FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10).Text("3. Pasiyentin adı, soyadı").SemiBold().Italic().FontSize(16);
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.User.FullName}").FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10).Text("4. Randevu sifariş nömrəsi (ID)").SemiBold().Italic().FontSize(16);
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.Number}").FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10).Text("5. Istifadəçi Kodu").SemiBold().Italic().FontSize(16);
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.User.UnikalId}").FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10).Text("6. Pasiyentin doğum tarixi (yaşı)").SemiBold().Italic().FontSize(16);
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.User.DateOfBrith?.ToString("dd MMMM yyyy")}").FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10).Text("7. Pasiyentin yaş aralığı").SemiBold().Italic().FontSize(16);
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text(AgeStr(form.AgeRange)).FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10).Text("8. Cinsiyyəti").SemiBold().Italic().FontSize(16);
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text(Gender(form.User.Gender)).FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10).Text("9. SİNAPSMED randevu/müayinə tarixi").SemiBold().Italic().FontSize(16);
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.Date.ToString("dd MMMM yyyy HH:mm")}").FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10)
                                        .Text(txt =>
                                            {
                                                txt.Span("*").FontSize(20).FontColor(Colors.Red.Accent3);
                                                txt.Span("10. Şikayət").SemiBold().Italic().FontSize(16);
                                            });
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text(form.Complaints).FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                            {
                                row.Spacing(15);
                                row.AutoItem().PaddingVertical(10)
                                    .Text(txt =>
                                            {
                                                txt.Span("*").FontSize(20).FontColor(Colors.Red.Accent3);
                                                txt.Span("11. Siqaret").SemiBold().Italic().FontSize(16);
                                            });
                                col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text(CigaretteStr(form.Cigarette)).FontSize(13);
                            });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10)
                                    .Text(txt =>
                                        {
                                            txt.Span("*").FontSize(20).FontColor(Colors.Red.Accent3);
                                            txt.Span("12. Alkoqol").SemiBold().Italic().FontSize(16);
                                        });
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text(AlcoholStr(form.Alcohol)).FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10)
                                    .Text(txt =>
                                        {
                                            txt.Span("*").FontSize(20).FontColor(Colors.Red.Accent3);
                                            txt.Span("13. Yanaşı xəstəliklər").SemiBold().Italic().FontSize(16);
                                        });
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.OtherDiseases}").FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10).Text("14. Allergiya (xüsusi maddəyə)").SemiBold().Italic().FontSize(16);
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.Allergy}").FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                             {
                                 row.Spacing(15);
                                 row.AutoItem().PaddingVertical(10)
                                    .Text(txt =>
                                        {
                                            txt.Span("*").FontSize(20).FontColor(Colors.Red.Accent3);
                                            txt.Span("16. Şikayət tarixçəsi").SemiBold().Italic().FontSize(16);
                                        });
                                 col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.CheckUpNotes}").FontSize(13);
                             });
                            col.Item().PaddingTop(20).Row(row =>
                            {
                                row.Spacing(15);
                                row.AutoItem().PaddingVertical(10).Text("17. Onlayn müayinə göstəriciləri").SemiBold().Italic().FontSize(16);
                                col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.OnlineIndicators}").FontSize(13);
                            });
                            col.Item().PaddingTop(20).Row(row =>
                            {
                                row.Spacing(15);
                                row.AutoItem().PaddingVertical(10)
                                    .Text(txt =>
                                        {
                                            txt.Span("*").FontSize(20).FontColor(Colors.Red.Accent3);
                                            txt.Span("18. VİTAL GÖSTƏRİCİLƏR").SemiBold().Italic().FontSize(16);
                                        });
                                col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.VitalIndicators}").FontSize(13);
                            });
                            col.Item().PaddingTop(20).Row(row =>
                            {
                                row.Spacing(15);
                                row.AutoItem().PaddingVertical(10).Text("19. Boy (sm)").SemiBold().Italic().FontSize(16);
                                col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.Height}").FontSize(13);
                            });
                            col.Item().PaddingTop(20).Row(row =>
                            {
                                row.Spacing(15);
                                row.AutoItem().PaddingVertical(10).Text("20. Çəki (kq)").SemiBold().Italic().FontSize(16);
                                col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.Kq}").FontSize(13);
                            });
                            col.Item().PaddingTop(20).Column(row =>
                            {
                                row.Spacing(15);
                                row.Item().Column(c =>
                                {
                                    c.Item().PaddingVertical(10).Text("21. Laborator müayinə nəticələri").SemiBold().Italic().FontSize(16);
                                    c.Item().Text("(Qeyri radioloji nəticələr - qan, sidik, yaxma, biopsiya və s.)")
                                        .FontSize(11).FontColor(Colors.Grey.Darken1);
                                });
                                row.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text(form.LaboratoryResult).FontSize(13);
                            });

                            col.Item().PaddingTop(20).Column(row =>
                            {
                                row.Spacing(15);
                                row.Item().Column(c =>
                                {
                                    c.Item().PaddingBottom(11).Text("22. Radioloji müayinə nəticələri").SemiBold().Italic().FontSize(16);
                                    c.Item().Text("(EKQ, EXO, EEQ, EMQ, Audiometriya, endoskopiya kimi nəticələr də daxil olmaqla)")
                                        .FontSize(11).FontColor(Colors.Grey.Darken1);
                                });
                                row.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text(form.Allergy).FontSize(13);
                            });

                            col.Item().PaddingTop(20).Column(column =>
                            {
                                column.Spacing(10);

                                // Başlık
                                column.Item().PaddingBottom(5).Text(txt =>
                                {
                                    txt.Span("*").FontSize(20).FontColor(Colors.Red.Accent3);
                                    txt.Span("23. Diaqnoz").SemiBold().Italic().FontSize(16);
                                });

                                // Tablo
                                column.Item().Table(table =>
                                {
                                    var allTypes = Enum.GetValues(typeof(DiagnosisType)).Cast<DiagnosisType>().ToList();
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3);
                                        foreach (var type in allTypes)
                                        {
                                            columns.RelativeColumn(0.5f);
                                        }
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Border(1).PaddingVertical(5).Text("Diagnosis (ICD10 Code)").SemiBold();
                                        foreach (var type in allTypes)
                                        {
                                            header.Cell().Border(1).PaddingVertical(5).AlignCenter().Text(type.ToString()).SemiBold();
                                        }
                                    });

                                    foreach (var diagnosis in form.Diagnoses)
                                    {
                                        table.Cell().Border(1).PaddingVertical(5).Text($"{diagnosis.WHO_Full_Desc} ({diagnosis.ICD10_Code})");

                                        foreach (var type in allTypes)
                                        {
                                            table.Cell().Border(1).AlignCenter().Text(diagnosis.Type == type ? Diagnosis(diagnosis.Type) : "");
                                        }
                                    }
                                });
                            });


                            col.Item().PaddingTop(20).Row(row =>
                            {
                                row.Spacing(15);
                                row.AutoItem().PaddingVertical(10).Text("24. YEKUN QƏRAR").SemiBold().Italic().FontSize(16);
                                col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.CheckUpVisitRecords}").FontSize(13);
                            });

                            col.Item().PaddingTop(20).Row(row =>
                                {
                                    row.Spacing(15);
                                    row.AutoItem().PaddingVertical(10).Text("25. KONTROL VİZİT TARIXI VƏ QEYDLƏRİ").SemiBold().Italic().FontSize(16);
                                    col.Item().BorderBottom(1).BorderColor(Colors.Teal.Lighten1).PaddingLeft(11).Text($"{form.NextCheckUpTime.ToString("G")} ,{form.NextCheckUpNotes}").FontSize(13);
                                });

                            #endregion
                        });

                    page.Footer()
                        .Padding(1)
                        .AlignRight()
                        .Text(x =>
                        {
                            x.CurrentPageNumber();
                        });
                });
            })
            .GeneratePdf(path);

            return path;
        }
        public static string Gender(bool gen)
        {
            return gen ? "Kişi" : "Qadın";
        }
        public static string AlcoholStr(Alcohol alcohol)
        {
            return alcohol switch
            {
                Alcohol.No => "Yox",
                Alcohol.YesRegularLessThan100MlInADay => "Var (müntəzəm araliqlarla və gündə 100 ml-dən az)",
                Alcohol.YesRegularMoreThan100MlInADay => "Var (müntəzəm aralıqlarla və gündə ən az 100 ml olmaqla)",
                Alcohol.YesIrRegularOnlySpecialDays => "Var (qeyri-müntəzəm, məclisdən məclisə)",
                Alcohol.Alcoholic => "Alkoqolik",
                _ => throw new NotImplementedException(),
            };
        }

        public static string AppointmentTypeSwithc(AppointmentType type)
        {
            return type switch
            {
                AppointmentType.MySelf => "Özüm",
                AppointmentType.Other => "Başqa istifadəçi",
                AppointmentType.Children => "Uşaq",
                AppointmentType.Parent => "Valideyn",
                _ => throw new NotImplementedException(),
            };
        }
        public static string CigaretteStr(Cigarette cigarette)
        {
            return cigarette switch
            {
                Cigarette.No => "Yox",
                Cigarette.MoreOnePackInADay => "> 1 p/gün",
                Cigarette.LessOnePackInADay => "< 1 p/gün",
                _ => throw new NotImplementedException(),
            };
        }
        public static string AgeStr(AgeRange range)
        {
            return range switch
            {
                AgeRange.Over84 => "84>",
                AgeRange.Age65_84 => "65-84",
                AgeRange.Age30_64 => "30-64",
                AgeRange.Age18_29 => "18-29",
                AgeRange.Age13_17 => "13-17",
                AgeRange.Age8_12 => "8-12",
                AgeRange.Age4_7 => "4-7",
                AgeRange.Age1_3 => "1-3",
                AgeRange.Under1 => "<1",
                _ => throw new NotImplementedException()
            };
        }
        public static AgeRange GetAgeRange(DateTime birth)
        {
            DateTime now = DateTime.UtcNow;
            int age = now.Year - birth.Year;

            if (birth > now.AddYears(-age)) age--;

            if (age > 84) return AgeRange.Over84;
            if (age >= 65) return AgeRange.Age65_84;
            if (age >= 30) return AgeRange.Age30_64;
            if (age >= 18) return AgeRange.Age18_29;
            if (age >= 13) return AgeRange.Age13_17;
            if (age >= 8) return AgeRange.Age8_12;
            if (age >= 4) return AgeRange.Age4_7;
            if (age >= 1) return AgeRange.Age1_3;
            return AgeRange.Under1;

        }
        public static string Diagnosis(DiagnosisType type)
        {
            return type switch
            {
                DiagnosisType.Pre => "Ön diaqnoz",
                DiagnosisType.Exact => "Dəqiq Diaqnoz",
                DiagnosisType.Differential => "Differensiyal Diaqnoz",
                _ => throw new NotImplementedException(),
            };
        }
    }
}