using Microsoft.EntityFrameworkCore;
using Nursan.Business.Manager;
using Nursan.Domain.Entity;
using Nursan.Domain.ModelsDisposible;
using Nursan.Logging.Messages;
using Nursan.Persistanse.Result;
using Nursan.Validations.ValidationCode;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace Nursan.Core.Printing
{
    public class DirectPrinting
    {
        SerialPort serialPort;
        UretimOtomasyonContext rm = new UretimOtomasyonContext();
        SyBarcodeOut yaziciDegisken;
        SyPrinter yazici; List<OrAlert> alertList;
        IzDonanimCount _izDonanimCount;
        int sayma = 0;
        string str1 = null;
        string str2 = null;
        string str3 = null;
        //string str4 = "Sistemsel onay icin kalite teknisyenine";
        //string str5 = "Haber Veriniz";
        //string str6 = "HATA OLUSTU";
        SuccessDataResults<IzDonanimCount> izdonanim;
        SuccessDataResults<OrHarnessModel> harnessModel;
        SuccessDataResults<List<OrAlert>> alert;
        //List<OrAlertBaglanti> alertbaglanti;
        HarnesDonanimCoax _coax;
        string[] fixum;
        private string idDonanimDolu;
        //Tetikle tetikle = new Tetikle();
        //OzelCameraIdler ozelCameraIdler = new OzelCameraIdler();
        public DirectPrinting(SyBarcodeOut barcode, IzDonanimCount donanimCount, SyPrinter print, int veriler)
        {
            Messaglama.MessagYaz(barcode.BarcodeIcerik + "Ctor Final Dushtu");
            yaziciDegisken = barcode;
            _izDonanimCount = donanimCount;
            yazici = print;
            alertList = new();
            idDonanimDolu = _izDonanimCount.IdDonanim.ToString();
            idDonanimDolu = idDonanimDolu.PadLeft((int)yaziciDegisken.PadLeft, '0');
            fixum = _izDonanimCount.DonanimReferans.Split('-');
            //serialPort = new SerialPort();
            // ozelCameraIdler = tetikle.TetikleIstasyon(id_donanim);
            // if (ozelCameraIdler == null)
            //  {}
            //bursda bir kontrol yapmak lqzim!
            //izdonanim = GitIzDonanimCountAraGetir(idDonanimDolu, $"{barcode.prefix}-{barcode.family}-{barcode.suffix}");
            //harnessModel = GitReferasnAraGetir(izdonanim.Data);
            //alert = GitReferasnALertAraGetir(izdonanim.Data);
        }
        public DirectPrinting(SyBarcodeOut barcode, IzDonanimCount donanimCount, SyPrinter print)
        {
            Messaglama.MessagYaz(barcode.BarcodeIcerik + "Ctor Final Dushtu");
            yaziciDegisken = barcode;
            _izDonanimCount = donanimCount;
            yazici = print;
            alertList = new();
            idDonanimDolu = _izDonanimCount.IdDonanim.ToString();
            idDonanimDolu = idDonanimDolu.PadLeft((int)yaziciDegisken.PadLeft, '0');
            fixum = _izDonanimCount.DonanimReferans.Split('-');
            izdonanim = GitIzDonanimCountAraGetir(idDonanimDolu, $"{barcode.prefix}-{barcode.family}-{barcode.suffix}");
            harnessModel = GitReferasnAraGetir(_izDonanimCount);
            alert = GitReferasnALertAraGetir(_izDonanimCount);
            // serialPort = new SerialPort();
            // ozelCameraIdler = tetikle.TetikleIstasyon(id_donanim);
            // if (ozelCameraIdler == null)
            //  {}
            //burda bir kontrol yapmak lqzim!
        }
        public DirectPrinting(SyBarcodeOut barcode, IzGenerateId donanimCount, SyPrinter print)
        {
            Messaglama.MessagYaz(barcode.BarcodeIcerik + "Ctor Final Dushtu");
            yaziciDegisken = barcode;
            yaziciDegisken.CreateDate = TarihHesapla.GetSystemDate();
            yazici = print;
            alertList = new();
            //_izDonanimCount = donanimCount;
            //idDonanimDolu = donanimCount.Id.ToString().PadLeft((int)yaziciDegisken.PadLeft,'0');
            //fixum = barcode.BarcodeIcerik.Split(' ');

            // ozelCameraIdler = tetikle.TetikleIstasyon(id_donanim);
            // if (ozelCameraIdler == null)
            //  {}
        }
        public DirectPrinting(SyBarcodeOut barcode, SyPrinter print, HarnesDonanimCoax coax)
        {
            Messaglama.MessagYaz(barcode.BarcodeIcerik + "Ctor Final Dushtu");
            yaziciDegisken = barcode;
            yazici = print;
            _coax = coax;
        }
        public SyBarcodeOut FinalEtiket(int sayac, string vardiya, string density, int Yazici)
        {
            if (izdonanim.Data != null)
            {
                if (alert.Data != null)
                {
                    if (harnessModel != null)
                    {
                        for (int i = 0; i < alert.Data.Count; i++)
                        {
                            if (this.sayma <= 0 || this.sayma <= 2)
                            {
                                str1 = string.Concat(str1, ",", alert.Data[i].Name);
                            }
                            else if (this.sayma <= 3 || this.sayma <= 5)
                            {
                                str2 = string.Concat(str2, ",", alert.Data[i].Name);
                            }
                            else if (this.sayma <= 6 || this.sayma <= 9)
                            {
                                str3 = string.Concat(str3, ",", alert.Data[i].Name);
                            }
                            this.sayma++;
                        }

                        if (str1 == null || str2 == null ? false : str3 != null)
                        {
                            str1 = str1.Substring(1);
                            str2 = str2.Substring(1);
                            str3 = str3.Substring(1);
                        }
                        else if (str1 == null || str2 == null ? false : str3 == null)
                        {
                            str1 = str1.Substring(1);
                            str2 = str2.Substring(1);
                        }
                        else if (str1 == null || str2 != null ? false : str3 == null)
                        {
                            str1 = str1.Substring(1);
                        }
                        yaziciDegisken.PsName = vardiya;
                        yaziciDegisken.eclntcode = harnessModel.Data.SideCode;
                        yaziciDegisken.releace = harnessModel.Data.Release;
                        yaziciDegisken.suffix = Regex.Replace(fixum[2], "[^a-z,A-Z,@,^,/,]", "");
                        yaziciDegisken.family = fixum[1];
                        yaziciDegisken.prefix = fixum[0];
                        yaziciDegisken.Sira1 = str1;
                        yaziciDegisken.Sira2 = str2;
                        yaziciDegisken.Sira3 = str3;
                        yaziciDegisken.IdDonanim = idDonanimDolu;
                    }
                    else
                    {
                        idDonanimDolu = harnessModel.Message == null ? "" : this.harnessModel.Message;
                        yaziciDegisken.PsName = harnessModel.Message == null ? "" : this.harnessModel.Message;
                        yaziciDegisken.eclntcode = harnessModel.Message == null ? "" : this.harnessModel.Message;
                        yaziciDegisken.releace = harnessModel.Message == null ? "" : this.harnessModel.Message;
                    }
                }
                else
                {
                    idDonanimDolu = alert.Message == null ? "" : this.alert.Message; ;
                    yaziciDegisken.PsName = alert.Message == null ? "" : this.alert.Message;
                    yaziciDegisken.eclntcode = alert.Message == null ? "" : this.alert.Message; ;
                    yaziciDegisken.releace = alert.Message == null ? "" : this.alert.Message; ;
                }
            }
            else
            {
                idDonanimDolu = izdonanim.Message == null ? "" : this.izdonanim.Message;
                yaziciDegisken.PsName = izdonanim.Message == null ? "" : this.izdonanim.Message;
                yaziciDegisken.eclntcode = izdonanim.Message == null ? "" : this.izdonanim.Message;
                yaziciDegisken.releace = izdonanim.Message == null ? "" : this.izdonanim.Message;
            }
            Messaglama.MessagYaz(yaziciDegisken.BarcodeIcerik + "Alert Degerlendi");
            return PrintFile(yaziciDegisken);
        }
        // Bunu Heryerde Kullaninabilirsin
        public SyBarcodeOut FinalHataEtiketBas(SyBarcodeOut syBarcodeOut)
        {
            syBarcodeOut.prefix = yaziciDegisken.prefix;
            syBarcodeOut.family = yaziciDegisken.family;
            syBarcodeOut.suffix = yaziciDegisken.suffix;
            //yaziciDegisken=new SyBarcodeOut();
            //yaziciDegisken.PsName = Environment.MachineName + vardiya;
            //yaziciDegisken.eclntcode = yaziciDegisken.eclntcode;
            ////yaziciDegisken.releace = harnessModel.Data.Release;
            //yaziciDegisken.CreateDate = yaziciDegisken.CreateDate; 
            ////yaziciDegisken.Sira1 = str1;
            ////yaziciDegisken.Sira2 = str2;
            ////yaziciDegisken.Sira3 = str3;
            //yaziciDegisken.IdDonanim = idDonanimDolu;

            return PrintFile(syBarcodeOut);
        }
        public SyBarcodeOut KucukEtiketBas(string vardiya)
        {
            return PrintFile(yaziciDegisken);
        }
        public SyBarcodeOut AntenKabloBas(string iddonanim)
        {
            return PrintFileForech(yaziciDegisken, _coax);
        }
        /// <summary>
        /// PrintFile from Final Barcode
        /// </summary>
        /// <param name = "_yaziciDegisken"> </param>
        /// <param name = "_coax"> </param>
        /// <param name = "harnessModel.Data.CustomerID"> </param>
        /// <returns> </returns>
        private SyBarcodeOut PrintFile(SyBarcodeOut _yaziciDegisken)
        {
            StreamReader reader = new StreamReader(Environment.CurrentDirectory + yazici.PrintngFile);
            string content = reader.ReadToEnd();
            reader.Close();

            var replacements = new Dictionary<string, string>
            {
                { "@barcodeicerik@",yaziciDegisken.BarcodeIcerik },
                { "@referans@", $"{_yaziciDegisken.prefix}-{_yaziciDegisken.family}-{_yaziciDegisken.suffix}"},
                { "@eclntcode@", _yaziciDegisken.eclntcode},
                { "@releace@", _yaziciDegisken.releace},
                { "@pcname@",_yaziciDegisken.PsName},
                { "@datetime@", _yaziciDegisken.CreateDate.ToString()},
                { "@id@",_yaziciDegisken.IdDonanim},
                { "@prefix@", _yaziciDegisken.prefix},
                { "@family@", _yaziciDegisken.family},
                { "@suffix@", _yaziciDegisken.suffix},
                { "@sira1@", _yaziciDegisken.Sira1},
                { "@sira2@", _yaziciDegisken.Sira2},
                { "@sira3@", _yaziciDegisken.Sira3},
                { "@OzelChar@", _yaziciDegisken.OzelChar == null?"":_yaziciDegisken.OzelChar.TrimEnd().ToUpper()},
                { "@density@", XMLIslemi.XmlDensity()},
                { "@nameetiket@",yaziciDegisken.Name },
                { "@customerId@", harnessModel == null?"":$"{harnessModel.Data.CustomerID.TrimEnd().ToUpper()}" }
             };
            var output = replacements.Aggregate(content, (current, replacement) => current.Replace(replacement.Key, replacement.Value));

            sayma = 0;
            if (yazici.Interface == "USB")
            {
                RawPrinterHelper.SendStringToPrinter(yazici.Name.ToString(), output);
                Messaglama.MessagYaz($"{yazici.Name.ToString()} {yazici.Interface} Basti");
                yaziciDegisken = null;
                return _yaziciDegisken;
            }
            else if (yazici.Interface == "COM")
            {
                SeriPortBaglan(XMLIslemi.XmlPrintOku());
                this.serialPort.WriteLine(output);
                this.serialPort.Close();
                this.serialPort.Dispose();
                Messaglama.MessagYaz($"{yazici.Name.ToString()} {yazici.Interface} Basti");
                yaziciDegisken = null;
                return _yaziciDegisken;
            }
            else if (yazici.Interface == "NET")
            {
                byte[] fileBytes = System.Text.UTF8Encoding.UTF8.GetBytes(output);
                PrintHelper printHelper = new PrintHelper(fileBytes, yazici.Ip, 9100);
                printHelper.PrintData();
                RawPrinterHelper.SendStringToPrinter(yazici.Name.ToString(), output);
                Messaglama.MessagYaz($"{yazici.Name.ToString()} {yazici.Interface} Basti");
                // _yaziciDegisken = null;
                return _yaziciDegisken;
            }
            else
            {
                Messaglama.MessagYaz($"{yazici.Name.ToString()} {yazici.Interface} HATA");
                yaziciDegisken = null;
                return _yaziciDegisken;
            }
        }
        /// <summary>
        /// PrintFile from Final Barcode
        /// </summary>
        /// <param name = "_yaziciDegisken"> </param>
        /// <param name = "_coax"> </param>
        /// <param name = "harnessModel.Data.CustomerID"> </param>
        /// <returns> </returns>
        /// 
        public SyBarcodeOut PrintFileNew(SyBarcodeOut _yaziciDegisken)
        {
            StreamReader reader = new StreamReader(Environment.CurrentDirectory + yazici.PrintngFile);
            string content = reader.ReadToEnd();
            reader.Close();

            var replacements = new Dictionary<string, string>
            {
                {"@barcodeicerik@",yaziciDegisken.BarcodeIcerik },
                { "@referans@", $"{_yaziciDegisken.prefix}-{_yaziciDegisken.family}-{_yaziciDegisken.suffix}"},
                { "@eclntcode@", _yaziciDegisken.eclntcode},
                { "@releace@", _yaziciDegisken.releace},
                { "@pcname@",_yaziciDegisken.PsName},
                { "@datetime@", _yaziciDegisken.CreateDate.ToString()},
                { "@id@",_yaziciDegisken.IdDonanim},
                { "@prefix@", _yaziciDegisken.prefix},
                { "@family@", _yaziciDegisken.family},
                { "@suffix@", _yaziciDegisken.suffix},
                { "@sira1@", _yaziciDegisken.Sira1},
                { "@sira2@", _yaziciDegisken.Sira2},
                { "@sira3@", _yaziciDegisken.Sira3},
                { "@OzelChar@", _yaziciDegisken.OzelChar == null?"":_yaziciDegisken.OzelChar.TrimEnd().ToUpper()},
                { "@density@", XMLIslemi.XmlDensity()},
                { "@nameetiket@",yaziciDegisken.Name },
                {"@customerId@", harnessModel == null?"":harnessModel.Data.CustomerID.TrimEnd().ToUpper() }
             };
            var output = replacements.Aggregate(content, (current, replacement) => current.Replace(replacement.Key, replacement.Value));

            sayma = 0;
            if (yazici.Interface == "USB")
            {
                RawPrinterHelper.SendStringToPrinter(yazici.Name.ToString(), output);
                Messaglama.MessagYaz($"{yazici.Name.ToString()} {yazici.Interface} Basti");
                yaziciDegisken = null;
                return _yaziciDegisken;
            }
            else if (yazici.Interface == "COM")
            {
                SeriPortBaglan(XMLIslemi.XmlPrintOku());
                this.serialPort.WriteLine(output);
                this.serialPort.Close();
                Messaglama.MessagYaz($"{yazici.Name.ToString()} {yazici.Interface} Basti");
                yaziciDegisken = null;
                return _yaziciDegisken;
            }
            else
            {
                Messaglama.MessagYaz($"{yazici.Name.ToString()} {yazici.Interface} HATA");
                yaziciDegisken = null;
                return _yaziciDegisken;
            }

        }
        /// <summary>
        /// PrintFile from Coax
        /// </summary>
        /// <param name = "_yaziciDegisken"> </param>
        /// <param name = "_coax"> </param>
        /// <param name = "harnessModel.Data.CustomerID"> </param>
        /// <returns> </returns>
        private SyBarcodeOut PrintFileForech(SyBarcodeOut _yaziciDegisken, HarnesDonanimCoax _coax)
        {
            StreamReader reader = new StreamReader(Environment.CurrentDirectory + yazici.PrintngFile);
            string content = reader.ReadToEnd();
            reader.Close();

            var replacements = new Dictionary<string, string>
            {
                { "@barcodeicerik@", _yaziciDegisken.BarcodeIcerik},
                { "@referans@", $"{ _coax.harnessModel}"},
                { "@eclntcode@", _yaziciDegisken.eclntcode},
                { "@releace@", _yaziciDegisken.releace},
                { "@pcname@", _yaziciDegisken.PsName},
                { "@datetime@", _yaziciDegisken.CreateDate.ToString()},
                { "@id@", _yaziciDegisken.IdDonanim},
                { "@prefix@", _yaziciDegisken.prefix},
                { "@family@", _yaziciDegisken.family},
                { "@suffix@", _yaziciDegisken.suffix},
                { "@sira1@", _coax.Sira1 },
                { "@sira2@", _coax.Sira2 },
                { "@sira3@", _coax.Sira3 },
                { "@sira4@", _coax.Sira4 },
                { "@sira5@", _coax.Sira5 },
                { "@sira6@", _coax.Sira6 },
                { "@sira7@", _coax.Sira7 },
                { "@sira8@", _coax.Sira8 },
                { "@sira9@", _coax.Sira9 },
                { "@sira10@", _coax.Sira10 },
                { "@sira11@", _coax.Sira11 },
                { "@sira12@", _coax.Sira12 },
                { "@sira13@", _coax.Sira13 },
                { "@sira14@", _coax.Sira14 },
                { "@sira15@", _coax.Sira15 },
                { "@sira16@", _coax.Sira16 },
                { "@sira17@", _coax.Sira17 },
                { "@sira18@", _coax.Sira18 },
                { "@density@", XMLIslemi.XmlDensity()},
                { "@nameetiket@",yaziciDegisken.Name },
                { "@sayi@",yaziciDegisken.Leght.ToString() },
                { "@OzelChar@", _yaziciDegisken.OzelChar == null? "" : _yaziciDegisken.OzelChar.TrimEnd().ToUpper()},
                { "@customerId@", harnessModel == null?"":harnessModel.Data.CustomerID.TrimEnd().ToUpper() }

             };
            var output = replacements.Aggregate(content, (current, replacement) => current.Replace(replacement.Key, replacement.Value));

            sayma = 0;
            if (yazici.Interface == "USB")
            {
                RawPrinterHelper.SendStringToPrinter(yazici.Name.ToString(), output);
                Messaglama.MessagYaz($"{yazici.Name.ToString()} {yazici.Interface} Basti");
                yaziciDegisken = null;
                return _yaziciDegisken;
            }
            else if (yazici.Interface == "COM")
            {
                SeriPortBaglan(XMLIslemi.XmlPrintOku());
                this.serialPort.WriteLine(output);
                this.serialPort.Close();
                Messaglama.MessagYaz($"{yazici.Name.ToString()} {yazici.Interface} Basti");
                yaziciDegisken = null;
                return _yaziciDegisken;
            }
            else
            {
                Messaglama.MessagYaz($"{yazici.Name.ToString()} {yazici.Interface} HATA");
                yaziciDegisken = null;
                return _yaziciDegisken;
            }

        }
        public void sytems()
        {
            int video = 0;
            if (video == 0)
            {
            }
            else
            {
            }
        }
        private void SeriPortBaglan(string com)
        {
            serialPort = new SerialPort();
            if (!this.serialPort.IsOpen)
            {
                try
                {
                    this.serialPort.BaudRate = int.Parse("9600");
                    this.serialPort.DataBits = int.Parse("8");
                    this.serialPort.StopBits = StopBits.One;
                    this.serialPort.Parity = Parity.None;
                    this.serialPort.PortName = com;
                    this.serialPort.Open();
                }
                catch (Exception exception)
                {
                    // this.serialPort.Close();
                }
            }
        }
        /// <summary>
        /// Burda Referans Activ Denetlenmesi Yapilabilir
        /// </summary>
        /// <param name="id_donanim"></param>
        /// <param name="referansim"></param>
        /// <returns></returns>
        private SuccessDataResults<IzDonanimCount> GitIzDonanimCountAraGetir(string id_donanim, string referansim)
        {
            try
            {
                //Buna dikkat et hata veriyor!

                var result = rm.IzGenerateIds.Include(x => x.HarnesModel).SingleOrDefault(x => x.Id == int.Parse(id_donanim));

                if (result != null)
                {

                    if (_izDonanimCount.OrHarnessModel == result.HarnesModel.HarnessModelName && result.HarnesModel.Access == false)
                    {
                        return new SuccessDataResults<IzDonanimCount>(_izDonanimCount, "Systemde OK");
                    }
                    else if (_izDonanimCount.OrHarnessModel == result.HarnesModel.HarnessModelName && result.HarnesModel.Access == true)
                    {
                        FinalHataEtiketBas(new SyBarcodeOut { BarcodeIcerik = "Donanim Kilitli!", Sira1 = "Donanim Kilitli!", Sira2 = "Donanim Kilitli!", Sira3 = "Donanim Kilitli!" });
                        return new SuccessDataResults<IzDonanimCount>(_izDonanimCount, "Donanim Kilitli!");
                    }
                    else if (_izDonanimCount.OrHarnessModel != result.HarnesModel.HarnessModelName)
                    {
                        return new SuccessDataResults<IzDonanimCount>(_izDonanimCount, "Donanimlar Uyusmuyor!");
                    }
                    else
                    {
                        return new SuccessDataResults<IzDonanimCount>(_izDonanimCount, "Systemde Bir hata Olustu!");
                    }
                }
                else
                {
                    FinalHataEtiketBas(new SyBarcodeOut { BarcodeIcerik = "Donanim Kilitli!", Sira1 = "Donanim Kilitli!", Sira2 = "Donanim Kilitli!", Sira3 = "Donanim Kilitli!" });
                    return new SuccessDataResults<IzDonanimCount>(_izDonanimCount, "Donanim Referans Kilitli");
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                return new SuccessDataResults<IzDonanimCount>("Systemde Bir Hata Olustu " + ex.Message);
            }
            // return new SuccessDataResults<IzDonanimCount>();
        }
        /// <summary>
        /// Burda Alert Activmi? Denetleme Yapilabilir
        /// </summary>
        /// <param name="alertHarness"></param>
        /// <returns></returns>
        private SuccessDataResults<List<OrAlert>> GitReferasnALertAraGetir(IzDonanimCount izDonanim)
        {
            try
            {
                //Alert Activ mi? denetlenmesi
                var sysIzGeneraciq = rm.IzGenerateIds.FirstOrDefault(x => x.Id == izDonanim.IdDonanim);
                var alert = rm.OrAlerts.Where(x => x.AlertNumber == sysIzGeneraciq.AlertNumber).ToList();
                alertList.AddRange(alert);
                if (alertList.Any(x => x.AlertAccess == true))
                {
                    FinalHataEtiketBas(new SyBarcodeOut { BarcodeIcerik = "Alert Kilitli!", Sira1 = "Alert Kilitli!", Sira2 = "Alert Kilitli!", Sira3 = "Alert Kilitli!" });
                    return new SuccessDataResults<List<OrAlert>>(null, "Alert Kilitli");

                }
                else
                {
                    return new SuccessDataResults<List<OrAlert>>(alertList, "Gelen ALert");
                }

            }
            catch (ErrorExceptionHandller ex)
            {

                return new SuccessDataResults<List<OrAlert>>("Systemde Bir Hata Olustu ");
            }

        }
        private SuccessDataResults<OrHarnessModel> GitReferasnAraGetir(IzDonanimCount IzDonanimCount)
        {
            try
            {
                var eclnt = rm.OrHarnessModels.FirstOrDefault(x => x.HarnessModelName == IzDonanimCount.OrHarnessModel && x.Access != true);

                if (eclnt != null)
                {
                    return new SuccessDataResults<OrHarnessModel>(eclnt, "Gelen Referans");
                }
                else
                {
                    yaziciDegisken = new();
                    yaziciDegisken.BarcodeIcerik = "Systemde bu Referans Kilitli!"; yaziciDegisken.Sira1 = "Systemde bu Referans Kilitli!"; yaziciDegisken.Sira2 = "Systemde bu Referans Kilitli!"; yaziciDegisken.Sira3 = "Systemde bu Referans Kilitli!";
                    PrintFile(yaziciDegisken);
                    return new SuccessDataResults<OrHarnessModel>(eclnt, "Donaim Kilitli");
                }
            }
            catch (ErrorExceptionHandller ex)
            {

                return new SuccessDataResults<OrHarnessModel>("Systemde Bir Hata Olustu " + ex.Message);
            }

        }
        //private SuccessDataResults<List<OrAlertBaglanti>> GetAlertbaglanti(OrHarnessModel harnes)
        //{
        //    var result = rm.OrAlertBaglantis.Include(x => x.Harness).Where(x => x.HarnessId == harnes.Id).ToList();
        //    return new SuccessDataResults<List<OrAlertBaglanti>>(result, "AlertBaglanti Geldi!");
        //}

    }
}
