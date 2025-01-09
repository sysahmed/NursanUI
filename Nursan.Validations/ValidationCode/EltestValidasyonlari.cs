using Microsoft.EntityFrameworkCore;
using Nursan.Business.Manager;
using Nursan.Domain.AmbarModels;
using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.SortedList;

namespace Nursan.Validations.ValidationCode
{
    public class EltestValidasyonlari : ValidationCode
    {
        // private List<string> listGelenSira;
        private readonly UnitOfWork _repo;
        private readonly UnitOfWorkAmbar _repoIslemler;
        TorkService torkServices;
        StreamReader file;
        // HataErrorServices hataErrorServices;
        IzDonanimCount donanimReferans;
        ErTestHatalari erTestHatalari1;
        ErTestHatalari erTestHatalari2;

        public EltestValidasyonlari(UnitOfWork repo) : base(repo)
        {
            _repo = repo;
            //listGelenSira = new List<string>();
            //hataErrorServices = new HataErrorServices(_repo);
            erTestHatalari1 = new ErTestHatalari();
            erTestHatalari2 = new ErTestHatalari();
            donanimReferans = new IzDonanimCount();
            _repoIslemler = new UnitOfWorkAmbar(new AmbarContext());

        }
        public Result GitSystemeYukle(ReadOnlySpan<char> name)
        {
            string[] getParca = StringSpanConverter.SplitWithoutAllocationReturnArray(name, '_');
            torkServices = new TorkService(_repo, new UrVardiya { Name = getParca[2] });
            return torkServices.GetElTestDonanimBarcode(getParca);
        }
        public bool GithataYukle(string name)
        {
            string str = null;
            bool flag;
            int num = 0;
            this.file = new StreamReader($"{name}");
            try
            {
                try
                {
                    List<string> strs = new List<string>();
                    while (true)
                    {
                        string str1 = this.file.ReadLine();
                        str = str1;
                        if (str1 == null)
                        {
                            break;
                        }
                        if (num > 0)
                        {
                            strs.Add(str);
                        }
                        num++;
                    }
                    DateTime now = DateTime.Now;
                    string str2 = now.ToString().Replace("/", "").Replace(":", "");
                    this.file.Close();
                    this.GelenVeri(strs);
                    if (this.file.ToString().Length == 0)
                    {
                        return true;
                    }
                    flag = true;
                }
                catch (ErrorExceptionHandller ex)
                {
                    if (str != null)
                    {
                        this.file.Close();
                    }
                    flag = false;
                }
            }
            finally
            {
            }
            return flag;
        }
        //DateTime time; int? idDonanim; int idi = 0; string[] values = null;
        public void GelenVeri(List<string> lines)
        {
            Messaglama.MessagYaz($"{lines[0]}");
            int sayi = 0;
            int veri1 = 0; //string format = "dd:MM:yyyy HH:mm:ss";
            foreach (var values in lines)
            {
                try
                {
                    erTestHatalari1 = erTestHatalari1 ?? new ErTestHatalari();
                    erTestHatalari2 = erTestHatalari2 ?? new ErTestHatalari();
                    sayi++;
                    string[] itro = values.Split(';');
                    if (itro[8].Length > 0)
                    {
                        if (sayi > 1) { erTestHatalari2.IdDonanim = int.Parse(itro[1].ToString()); };
                        donanimReferans = _repo.GetRepository<IzDonanimCount>().Get(x => x.IdDonanim == int.Parse(itro[1]) && x.UrIstasyon.ModulerYapi.Etap == "Konveyor").Data;

                        erTestHatalari1.IdDonanim = int.Parse(itro[1].ToString());
                        erTestHatalari1.Referans = itro[0].ToString();
                        erTestHatalari1.Vardiya = Environment.MachineName + itro[4];
                        erTestHatalari1.CreateDate = DateTime.Now;
                        if (donanimReferans != null)
                        {
                            erTestHatalari1.Konveyor = _repo.GetRepository<OpMashin>().Get(x => x.Id == donanimReferans.MashinId).Data.MasineName;
                            erTestHatalari1.KonVeyorTarih = donanimReferans.CreateDate;
                        }
                        erTestHatalari1.Onarma = itro[7].ToString();
                        erTestHatalari1.HataCodu = int.Parse(itro[8].ToString());
                        if (erTestHatalari1.IdDonanim == erTestHatalari2.IdDonanim && erTestHatalari1.HataCodu == 116)
                        {
                            erTestHatalari2.Bolge2 = itro[5].ToString();
                            erTestHatalari2.Goz2 = itro[6].ToString();
                            _repo.GetRepository<ErTestHatalari>().Update(erTestHatalari2);
                            erTestHatalari1 = null;
                            sayi = 0;
                        }
                        else if (Convert.ToInt32(itro[8]) > 1000)
                        {
                            try
                            {
                                Messaglama.MessagYaz("Ariza M Girildi!!!");
                                var rem = _repo.GetRepository<ErErrorCode>().Get(x => x.ErrorCode == itro[8]).Data;

                                var pcName = _repoIslemler.GetRepositoryAmbar<PcName>().Get(x => x.Pcname1 == Environment.MachineName).Data;
                                //Messaglama.MessagYaz(rem.ErrorName);
                                //Messaglama.MessagYaz(itro[5]);
                                //Messaglama.MessagYaz(itro[6]);
                                //Messaglama.MessagYaz(pcName.Pcid.ToString());
                                //Messaglama.MessagYaz(DateTime.Now.ToString());
                                Islemler veriler = new Islemler { Ariza = rem.ErrorName, Bolge = $"{itro[5]} {itro[6]}", PcId = pcName.Pcid, Tarih = erTestHatalari1.CreateDate, Active = true, Role = 5 };
                                _repoIslemler.GetRepositoryAmbar<Islemler>().Add(veriler);
                            }
                            catch (ErrorExceptionHandller ex)
                            {
                                Messaglama.MessagYaz("Hata:" + ex.Message);
                            }
                        }
                        else if (erTestHatalari1.IdDonanim != veri1 && erTestHatalari1.HataCodu != 116)
                        {
                            erTestHatalari1.Bolge1 = itro[5].ToString();
                            erTestHatalari1.Goz1 = itro[6].ToString();
                            _repo.GetRepository<ErTestHatalari>().Add(erTestHatalari1);
                            erTestHatalari1 = null;
                        }
                        else
                        {
                            erTestHatalari1.Bolge1 = itro[5].ToString();
                            erTestHatalari1.Goz1 = itro[6].ToString();
                            erTestHatalari1.Bolge2 = null;
                            erTestHatalari1.Goz2 = null;
                            _repo.GetRepository<ErTestHatalari>().Add(erTestHatalari1);
                            erTestHatalari2 = erTestHatalari1;
                            erTestHatalari1 = null;
                        }
                    }
                    else
                    {
                        Messaglama.MessagYaz($"Else Dustu!!!");
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Messaglama.MessagYaz($"{ex.Message}");
                }
            }
            erTestHatalari2 = null;
            erTestHatalari1 = null;
        }

    }
}
