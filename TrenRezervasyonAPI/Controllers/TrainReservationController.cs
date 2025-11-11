using Microsoft.AspNetCore.Mvc;
using TrainReservationAPI.Models;

namespace TrainReservationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainReservationController : ControllerBase
    {
        [HttpPost("rezervasyon-yap")]
        public IActionResult RezervasyonYap([FromBody] RezervasyonTalebi talep)
        {
            var sonuc = new RezervasyonSonucu();

            // Boş veya hatalı istek kontrolü
            if (talep == null || talep.Tren == null || talep.Tren.Vagonlar.Count == 0)
                return BadRequest("Geçersiz veri gönderildi.");

            int kalanKisi = talep.RezervasyonYapilacakKisiSayisi;

            // Eğer herkes aynı vagonda oturmak zorundaysa:
            if (!talep.KisilerFarkliVagonlaraYerlestirilebilir)
            {
                foreach (var vagon in talep.Tren.Vagonlar)
                {
                    int maxDolu = (int)(vagon.Kapasite * 0.7);
                    int bosKoltuk = maxDolu - vagon.DoluKoltukAdet;

                    if (bosKoltuk >= kalanKisi)
                    {
                        sonuc.RezervasyonYapilabilir = true;
                        sonuc.YerlesimAyrinti.Add(new Yerlesim
                        {
                            VagonAdi = vagon.Ad,
                            KisiSayisi = kalanKisi
                        });
                        return Ok(sonuc);
                    }
                }

                // Uygun vagon bulunamadıysa
                sonuc.RezervasyonYapilabilir = false;
                return Ok(sonuc);
            }

            // Eğer farklı vagonlara da dağıtılabiliyorsa:
            foreach (var vagon in talep.Tren.Vagonlar)
            {
                int maxDolu = (int)(vagon.Kapasite * 0.7);
                int bosKoltuk = maxDolu - vagon.DoluKoltukAdet;

                if (bosKoltuk <= 0) continue;

                int yerlestirilecek = Math.Min(bosKoltuk, kalanKisi);
                kalanKisi -= yerlestirilecek;

                sonuc.YerlesimAyrinti.Add(new Yerlesim
                {
                    VagonAdi = vagon.Ad,
                    KisiSayisi = yerlestirilecek
                });

                if (kalanKisi == 0)
                {
                    sonuc.RezervasyonYapilabilir = true;
                    break;
                }
            }

            // Hâlâ kişi kaldıysa rezervasyon yapılamaz
            if (kalanKisi > 0)
            {
                sonuc.RezervasyonYapilabilir = false;
                sonuc.YerlesimAyrinti.Clear();
            }

            return Ok(sonuc);
        }
    }
}
