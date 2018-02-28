using System;

namespace Liga
{
    class Teilnehmer
    {
        public string Teamname { get; }
        public int Staerke { get; }
        public int Siege { get; set; } = 0;
        public int Unentschieden { get; set; } = 0;
        public int Niederlagen { get; set; } = 0;
        public int Tore { get; set; } = 0;
        public int Gegentore { get; set; } = 0;
        public int Tordifferenz { get; set; } = 0;
        public int Punkte { get; set; } = 0;
        public int ID { get; }

        public Teilnehmer(string bTeamname, int bTeamstaerke, int bID)
        {
            Teamname = bTeamname;
            Staerke = bTeamstaerke;
            ID = bID;
            Console.WriteLine(bTeamname + " (Staerke: " + bTeamstaerke + ") hinzugefuegt.");
        }

        /// <summary>
        /// Sucht die höchste Niederlage aus den Spielpaarungen raus.
        /// </summary>
        /// <param name="bSpielpaarungen"></param>
        /// <param name="bTeilnehmer"></param>
        /// <returns></returns>
        /// <remarks>
        /// Diese Funktion wurde nur implementiert, um eine Methode eines Objektes zu besitzen, die nicht static ist.
        /// Achtung: Diese Ausgabe funktioniert nur vor der Sortierung der Tabelle, da sich
        /// die Reihenfolge im Array ändert und somit die ID nicht mehr gleich die Position im Array darstellt.
        /// Ein vorheriges Raussuchen des neuen Index korrigiert das Problem, was man noch hinzufügen könnte.
        /// </remarks>
        public string HoechsteNiederlage(Spielpaarungen[] bSpielpaarungen, Teilnehmer[] bTeilnehmer)
        {           
            string Rueckgabe = "";  // Rückgabestring
            int MaxTordifferenz = 0; // Höchste gefundene Tordifferenz (Betrag)

            for (int i = 0; i < bSpielpaarungen.Length; i++)
            {
                // Wenn das Team in der Spielpaarungen drin vor kommt, eine Niederlage erlitten hat und der Betrag der Tordifferenz größer als MaxTordifferenz ist...
                if (((bSpielpaarungen[i].Index1 == ID & bSpielpaarungen[i].Tore1 < bSpielpaarungen[i].Tore2) || (bSpielpaarungen[i].Index2 == ID & bSpielpaarungen[i].Tore1 > bSpielpaarungen[i].Tore2))
                    && Math.Abs(bSpielpaarungen[i].Tore1 - bSpielpaarungen[i].Tore2) >= MaxTordifferenz)
                {
                    int GegnerID = -1; // Arrayindex des Gegners
                    // Setzen der GegnerID
                    if (bSpielpaarungen[i].Index1 == ID) { GegnerID = bSpielpaarungen[i].Index2; } else { GegnerID = bSpielpaarungen[i].Index1; }
                    // Zusammensetzen des Rückgabestrings
                    Rueckgabe = Teamname + ": Hoechste Niederlage war gegen " + bTeilnehmer[GegnerID].Teamname + " mit einem ";
                    if (bSpielpaarungen[i].Tore1 >= bSpielpaarungen[i].Tore2) { Rueckgabe += bSpielpaarungen[i].Tore2 + ":" + bSpielpaarungen[i].Tore1; }
                        else { Rueckgabe += bSpielpaarungen[i].Tore1 + ":" + bSpielpaarungen[i].Tore2; }
                    // Setzen der neuen MaxTordifferenz
                    MaxTordifferenz = Math.Abs(bSpielpaarungen[i].Tore1 - bSpielpaarungen[i].Tore2);
                }
            }
            // Sollte keine Niederlage gefunden worden sein
            if (Rueckgabe == "") { Rueckgabe = Teamname + ": Keine Niederlage verzeichnet."; }
            return Rueckgabe;
        }

        /// <summary>
        /// Gibt die sortierte Tabelle in der Konsole aus.
        /// </summary>
        /// <param name="bTeilnehmer"></param>
        public static void TabelleAusgeben(Teilnehmer[] bTeilnehmer)
        {
            // Sortieren der Teilnehmer nach Punkten (und Tordifferenz)
            bTeilnehmer =  TabelleSortieren(bTeilnehmer);

            Console.WriteLine("");
            const string format = "{0, -10} {1, -3} {2, -3} {3, -3} {4, -3} {5, -3} {6, -3} {7, -3} {8, -5} {9, -3}";

            Console.WriteLine(string.Format(format, "Teamname", "St", "SP", "S", "U", "N", "TO", "GT", "TD", "P"));
            Console.WriteLine("-----------------------------------------------");

            for (int i = 0; i < bTeilnehmer.Length; i++)
            {
                Console.WriteLine(string.Format(format, bTeilnehmer[i].Teamname, bTeilnehmer[i].Staerke.ToString("00"), 
                                                        (bTeilnehmer[i].Siege + bTeilnehmer[i].Unentschieden + bTeilnehmer[i].Niederlagen),  
                                                        bTeilnehmer[i].Siege, bTeilnehmer[i].Unentschieden, bTeilnehmer[i].Niederlagen,
                                                        bTeilnehmer[i].Tore, bTeilnehmer[i].Gegentore, bTeilnehmer[i].Tordifferenz, 
                                                        bTeilnehmer[i].Punkte));
            }
        }

        /// <summary>
        /// Sortieren der Teilnehmer nach Punkte und Tordifferenz.
        /// </summary>
        /// <param name="bTeilnehmer"></param>
        /// <returns></returns>
        /// <remarks>
        /// Implementierung eines InsertSort-Algorithmus. Sollte die Punktezahl gleich, aber die Tordifferenz geringer sind, wird die Position getauscht.
        /// </remarks>
        private static Teilnehmer[] TabelleSortieren(Teilnehmer[] bTeilnehmer)
        {
            Console.Write("Tabelle wird sortiert...");
            for (int i = 0; i < bTeilnehmer.Length - 1; i++)
            {
                for (int j = i + 1; j > 0; j --)
                {
                    if (bTeilnehmer[j - 1].Punkte < bTeilnehmer[j].Punkte || (bTeilnehmer[j - 1].Punkte ==  bTeilnehmer[j].Punkte && bTeilnehmer[j - 1].Tordifferenz < bTeilnehmer[j].Tordifferenz))
                    {
                        Teilnehmer tmpTeilnehmer = bTeilnehmer[j - 1];
                        bTeilnehmer[j - 1] = bTeilnehmer[j];
                        bTeilnehmer[j] = tmpTeilnehmer;
                    }
                }
            }
            Console.WriteLine("fertig!");
            return bTeilnehmer;
        }
    }
}
