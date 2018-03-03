using System;

namespace Liga
{
    class Program
    {
        // Definieren der Konstanten
        private const int Teilnehmeranzahl = 5;
        private const bool SpielpaarungenAnzeigen = true;
        private const bool TabelleSortieren = true;
        private const bool TabelleAnzeigen = true;

        static void Main(string[] args)  
        {
            // Initialisieren der Teilnehmer gemäß der Teilnehmeranzahl
            Teilnehmer[] teilnehmer = new Teilnehmer[Teilnehmeranzahl];
            // Initialisieren der Spielpaarungen gemäß der Formel n/2*(n-1), wobei n=Teilnehmeranzahl
            Spielpaarungen[] spielpaarungen = new Spielpaarungen[Teilnehmeranzahl * (Teilnehmeranzahl - 1) / 2];

            Random rnd = new Random();
            // Erstellen der Teilnehmer
            for (int i = 0; i < Teilnehmeranzahl; i++)
            {
                teilnehmer[i] = new Teilnehmer("Team " + (i + 1), rnd.Next(1, 100), i);
            }
            // Erstellen der Spielpaarungen
            spielpaarungen = Spielpaarungen.SpielpaarungenGenerieren(teilnehmer);
            // Austragen der Spielpaarungen
            Spielpaarungen.SpieleAustragen(teilnehmer, spielpaarungen, SpielpaarungenAnzeigen);

            // Sortierung und Ausgabe der Endtabelle
            if (TabelleAnzeigen == true) { Teilnehmer.TabelleAusgeben(teilnehmer, TabelleSortieren); }
            teilnehmer[0].Teilnehmerinfo(spielpaarungen, teilnehmer);
        }
    }
}
