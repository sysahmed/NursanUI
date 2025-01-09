namespace Nursan.Domain.ModelsDisposible
{
    public class Gelen
    {
        public string referanslar { get; set; }

        public int id { get; set; }

        public DateTime tarih { get; set; }

        public string vardiya { get; set; }

        public string bolge { get; set; }

        public int goz { get; set; }

        public int hata { get; set; }

        public string onarma { get; set; }

        public string konveyor { get; set; }

        public DateTime kontarih { get; set; }

        public int gelenhatakod { get; set; }

        public string gelenhata { get; set; }

        public bool revorkta { get; set; }
    }
}
