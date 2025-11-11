namespace TrainReservationAPI.Models
{
    public class Yerlesim
    {
        public string VagonAdi { get; set; } = string.Empty;
        public int KisiSayisi { get; set; }
    }

    public class RezervasyonSonucu
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<Yerlesim> YerlesimAyrinti { get; set; } = new();
    }
}
