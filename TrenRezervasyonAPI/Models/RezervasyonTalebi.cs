namespace TrainReservationAPI.Models
{
    public class RezervasyonTalebi
    {
        public Tren Tren { get; set; } = new();
        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
    }
}
