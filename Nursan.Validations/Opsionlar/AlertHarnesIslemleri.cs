using Microsoft.EntityFrameworkCore;
using Nursan.Domain.Entity;

namespace Nursan.Validations.Opsionlar
{
    public class AlertHarnesIslemleri
    {
        private readonly UretimOtomasyonContext _context;
        //private readonly List<AlertHarnesIslemleri> alertHarness;
        public AlertHarnesIslemleri(UretimOtomasyonContext context)
        {
            _context = context;
            //alertHarness = new List<AlertHarnesIslemleri>();
        }
        public AlertHarnesIslemleri()
        {

        }
        public int baglantiId { get; set; }
        public int HarnessId { get; set; }
        public int AlertId { get; set; }
        public string HarnesModel { get; set; }
        public string AlertName { get; set; }
        public bool? HarnesAccess { get; set; }
        public bool? AlertAccess { get; set; }
        public string AlertNumber { get; set; }
        /// <summary>
        /// Chat GPT
        /// </summary>
        /// <returns></returns>
        public List<AlertHarnesIslemleri> GetharnesAndAlert()
        {
            var veri = _context.OrAlertBaglantis
                .Include(x => x.Harness)
                .Include(x => x.Alert)
                .Select(x => new
                {
                    x.Id,
                    x.HarnessId,
                    x.Harness.HarnessModelName,
                    x.AlertId,
                    x.Alert.Name,
                    x.Harness.Access,
                    x.Alert.AlertAccess,
                    x.AlertNumber
                }) 
                ;

            var alertHarness = veri.Select(i => new AlertHarnesIslemleri
            {
                baglantiId = i.Id,
                HarnessId = i.HarnessId,
                HarnesModel = i.HarnessModelName,
                AlertName = i.Name,
                HarnesAccess = i.Access,
                AlertId = i.AlertId,
                AlertAccess = i.AlertAccess,
                AlertNumber = i.AlertNumber.ToString()
            });

            return alertHarness.ToList();
        }
        //public List<AlertHarnesIslemleri> GetharnesAndAlert()
        //{
        //    var veri = _context.OrAlertBaglantis.Include(x => x.Harness).Include(x => x.Alert).Select(x => new { x.Id, x.HarnessId, x.Harness.HarnessModelName, x.AlertId, x.Alert.Name, x.Harness.Access, x.Alert.AlertAccess, x.AlertNumber }).ToList();
        //    foreach (var i in veri)
        //    {
        //        alertHarness.Add(new AlertHarnesIslemleri { baglantiId = i.Id, HarnessId = i.HarnessId, HarnesModel = i.HarnessModelName, AlertName = i.Name, HarnesAccess = i.Access, AlertId = i.AlertId, AlertAccess = i.AlertAccess, AlertNumber =  i.AlertNumber.ToString()});
        //    }
        //    return alertHarness;
        //}
        //public List<AlertHarnesIslemleri> GetharnesAndAlert(int number)
        //{
        //    var veri = _context.OrAlertBaglantis.Include(x => x.Harness).Include(x => x.Alert).Select(x => new { x.Id, x.HarnessId, x.Harness.HarnessModelName, x.AlertId, x.Alert.Name, x.Harness.Access, x.Alert.AlertAccess, x.AlertNumber }).Where(x=>x.AlertNumber==number).ToList();
        //    foreach (var i in veri)
        //    {
        //        alertHarness.Add(new AlertHarnesIslemleri { baglantiId = i.Id, HarnessId = i.HarnessId, HarnesModel = i.HarnessModelName, AlertName = i.Name, HarnesAccess = i.Access, AlertId = i.AlertId, AlertAccess = i.AlertAccess, AlertNumber = i.AlertNumber.ToString() });
        //    }
        //    return alertHarness;
        //}
    }
}
