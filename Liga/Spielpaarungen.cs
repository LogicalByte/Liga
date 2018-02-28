using System;

namespace Liga
{
    class Spielpaarungen
    {
        public int Index1 { get; }
        public int Index2 { get; }
        public int Tore1 { get; set; }
        public int Tore2 { get; set; }

        public Spielpaarungen(int bIndex1, int bIndex2)
        {
            Index1 = bIndex1;
            Index2 = bIndex2;
        }

        /// <summary>
        /// Generiert anhand der übergebenen Teilnehmer die Spielpaarungen.
        /// </summary>
        /// <param name="bTeilnehmer">Aktuelle Teilnehmer</param>
        /// <returns>Generierte Spielpaarungen</returns>
        /// <remarks>
        /// Vorgehensweise: 
        /// Man nehme zum Beispiel 5 Teams und stelle diese in eine Zeile: 1234567
        /// Runde 1 besitzt den Abstand 1. Das heißt, der Algorithmus geht von links nach rechts jeden einzelnen Eintrag durch
        /// und bildet mit seinem rechten Nachbarn eine Spielpaarung: 1-2, 2-3, 3-4, 4-5, 5-6, 6-7, 7-1
        /// Runde 2 besitzt den Abstand 2. Auch hier geht der Algorithmus von links nach rechts jeden einzelnen Eintrag durch,
        /// bildet aber mit jedem zweiten Nachbarn eine Spielpaarung: 1-3, 2-4, 3-5, 4-6, 5-7, 6-1, 7-2
        /// Und schließlich in Runde 3 (der letzten Runde) besitzt der Abstand den Wert 3. Gleiche Vorgehensweise, allerdings unterschiedliche
        /// Schrittanzahl, sofern die Teilnehmerzahl 6 betragen würde. Bei gerader Teilnehmerzahl ist die Schrittzahl die halbe Teilnehmerzahl, 
        /// bei ungerader Teilnehmerzahl ändert sich nichts. Es werden also in diesem Beispiel die Spielpaarungen 1-4, 2-5, 3-6, 4-7, 5-1
        /// und 6-2 gebildet.
        /// Die Gesamtanzahl der Runden beträgt immer die Hälfte der Teilnehmerzahl (abgerundet), wobei bei der letzten Runde immer die in 
        /// Runde 3 beschriebene Sonderregel beachtet werden muss.
        /// Ergbnis:
        /// Man erhält eine Jeder-gegen-Jeden-Generierung mit ausbalanciertem Heim- und Auswärtsspielpaarungen. Einziges Manko dieses Algorithmus
        /// ist die erste Runde, in der die Reihenfolge 1-2, 3-4, 5-6, 7-1, 2-3, 4-5, 6-7 sinnvoller gewesen wäre, da die Durchführung nicht
        /// von einem bereits spielenden Team blockiert wird. Dies lässt sich allerdings im Anschluss sehr leicht korrigieren, worauf in
        /// diesem Beispiel erstmal verzichtet worden ist. Auch könnte die Streuung der Partien, damit manche Spielpausen nicht zu lang sind,
        /// anspassen.
        /// Wichtig: Es findet keine Neuinitialisierung statt. Würde diese Methode ein weiteres Mal aufgerufen werden, werden die Punkte, Tore und
        /// alles andere draufaddiert.
        /// </remarks>
        static public Spielpaarungen[] SpielpaarungenGenerieren(Teilnehmer[] bTeilnehmer)
        {
            Console.Write("Spielpaarungen werden generiert...");
            Spielpaarungen[] spielpaarungen = new Spielpaarungen[bTeilnehmer.Length * (bTeilnehmer.Length - 1) / 2]; // Beinhaltet alle Spielpaarungen
            bool IstGerade = false; // Ob die Teilnehmerzahl gerade ist
            int MaxRunden = bTeilnehmer.Length / 2; // Maximale Rundenanzahl
            int AktRunde = 0; // Derzeitige Runde
            int Index = 0; // Position im Array von spielpaarungen

            // Setzt IstGerade auf true, wenn die Teilnehmerzahl gerade ist
            if (bTeilnehmer.Length % 2 == 0) { IstGerade = true; }

            // Schleife über die gesamten Runden
            for (int i = 1; i <= MaxRunden; i++)
            {
                AktRunde++;
                int MaxZaehler = 0;  // Maximale Paarungsanzahl dieser Runde

                // Je nachdem in welcher Runde wir uns befinden und ob die Teilnehmerzahl (un-)gerade ist, wird der MaxZaehler entsprechend gesetzt.
                if ((IstGerade == true && AktRunde < MaxRunden) || IstGerade == false) { MaxZaehler = bTeilnehmer.Length; }
                if (IstGerade == true && AktRunde == MaxRunden) { MaxZaehler = bTeilnehmer.Length / 2; }

                for (int j = 0; j <= MaxZaehler - 1; j++)
                {
                    // Prüft, ob der Index der Gastmannschaft den des Teilnehmerarrays überschreiten würde und korrigiert dies entsprechend
                    int Team2 = CheckTeamRange(j + AktRunde, bTeilnehmer.Length);

                    // j ist der Index des Heimteams und Team2 der Index des Auswärtsteams im Teilnehmerarray
                    spielpaarungen[Index] = new Spielpaarungen(j, Team2);
                    Index++;
                }
            }
            Console.WriteLine("fertig!");
            return spielpaarungen;
        }

        /// <summary>
        /// Überprüft, ob der Index die Größe des Teilnehmerarrays überschreiten würde und korrifiert dies ggf.
        /// </summary>
        /// <param name="Index">Index laut Schleife</param>
        /// <param name="Teilnehmerzahl">Die Teilnehmerzahl</param>
        /// <returns>Korrigierte Index</returns>
        static private int CheckTeamRange(int Index, int Teilnehmerzahl)
        {
            if (Index >= Teilnehmerzahl)
            {
                Index = Index - Teilnehmerzahl;
            }

            return Index;
        }

        /// <summary>
        /// Austragen der Spielbegegenungen.
        /// </summary>
        /// <param name="bTeilnehmer">Alle Teilnehmer</param>
        /// <param name="bSpielpaarungen">Alle Spielpaarungen</param>
        /// <remarks>
        /// Es wird nach und nach durch die Spielpaarungen durchgegangen, die jeweilige Teamstärke entnommen und daraus die Spielstärke
        /// ermittelt.
        /// Dazu wird die Stärke mit einem zufälligen Wert zwischen 1 und 100 multipliziert und anschließend gegeneinander verglichen.
        /// Die Tore werden mit einem Zufallsgenerator ermittelt. Die Spanne beträgt zwischen 1 und 4 für den Sieger, der Verlierer
        /// erhält 0 bis (Tore des Siegers - 1) Tore.
        /// Hier kommt der Vorteil zugute, dass der Index der Teams aus dem Teilnehmerarray gespeichert wurde. Anhand diesem lässt
        /// sich an die entsprechende Stelle springen. Die Zeitkomplexität beträgt somit O(n).
        /// </remarks>
        static public void SpieleAustragen(Teilnehmer[] bTeilnehmer, Spielpaarungen[] bSpielpaarungen, bool SpielpaarungenAnzeigen)
        {
            Console.WriteLine("Spiele werden ausgetragen...");
            Random rnd = new Random();

            for (int i = 0; i < bSpielpaarungen.Length; i++)
            {
                double Team1 = 0.0; // (Spiel-) Stärke vom Heimteam
                double Team2 = 0.0; // (Spiel-) Strärke vom Auswärtsteam

                Team1 = bTeilnehmer[bSpielpaarungen[i].Index1].Staerke * rnd.Next(1, 100);
                Team2 = bTeilnehmer[bSpielpaarungen[i].Index2].Staerke * rnd.Next(1, 100);
                double Unentschieden = Team1 * 10 / 100 + Team2 * 10 / 100;

                if (Math.Abs(Team1 - Team2) > Unentschieden && Team1 > Team2)
                {
                    // Bestimmung der Tore des Siegerteams
                    int Tore1 = rnd.Next(1, 5);
                    // Bestimmung der Tore des Verliererteams
                    int Tore2 = rnd.Next(0, Tore1);
                    ErgebnisEintragen(bSpielpaarungen[i], bTeilnehmer[bSpielpaarungen[i].Index1], bTeilnehmer[bSpielpaarungen[i].Index2], Tore1, Tore2, 1, 0, 0, 0, 0, 1, Tore1 - Tore2, Tore2 - Tore1, 3, 0);
                }
                else if (Math.Abs(Team1 - Team2) > Unentschieden && Team1 < Team2)
                {
                    // Bestimmung der Tore des Siegerteams
                    int Tore2 = rnd.Next(1, 5);
                    // Bestimmung der Tore des Verliererteams
                    int Tore1 = rnd.Next(0, Tore2);
                    ErgebnisEintragen(bSpielpaarungen[i], bTeilnehmer[bSpielpaarungen[i].Index1], bTeilnehmer[bSpielpaarungen[i].Index2], Tore1, Tore2, 0, 1, 0, 0, 1, 0, Tore1 - Tore2, Tore2 - Tore1, 0, 3);
                }
                else
                {
                    //...ansonsten ein Unentschieden
                    int Tore = rnd.Next(0, 5);
                    ErgebnisEintragen(bSpielpaarungen[i], bTeilnehmer[bSpielpaarungen[i].Index1], bTeilnehmer[bSpielpaarungen[i].Index2], Tore, Tore, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1);
                }

                // Wenn die Konstante gesetzt ist, werden alle Spielpaarungen angezeigt (hier: jede frisch ausgetragene)
                if (SpielpaarungenAnzeigen == true)
                {
                    const string format = "{0, -10} {1, -3}  {2, -10} {3, -1} {4, -1} {5, -1}";
                    Console.WriteLine(string.Format(format, bTeilnehmer[bSpielpaarungen[i].Index1].Teamname, " - ", bTeilnehmer[bSpielpaarungen[i].Index2].Teamname, bSpielpaarungen[i].Tore1, ":", bSpielpaarungen[i].Tore2));
                }
            }
            Console.WriteLine("fertig!");
        }

        /// <summary>
        /// Trägt das übergebene Ergebnis in die Spielpaarungen ein
        /// </summary>
        /// <param name="bSpielpaarung">Objekt der Spielpaarungen</param>
        /// <param name="bTeilnehmer1">Objekt des Heimteams</param>
        /// <param name="bTeilnehmer2">Objekt des Auswärtssteams</param>
        /// <param name="Tore1">Tore des Heimteams</param>
        /// <param name="Tore2">Tore des Auswärtsteams</param>
        /// <param name="Siege1">Summand für das Heimteam</param>
        /// <param name="Siege2">Summand für das Auswärtsteam</param>
        /// <param name="Unentschieden1">Summand für das Heimteam</param>
        /// <param name="Unentschieden2">Summand für das Auswärtsteam</param>
        /// <param name="Niederlage1">Summand für das Heimteam</param>
        /// <param name="Niederlage2">Summand für das Auswärtsteam</param>
        /// <param name="Tordifferenz1">Summand für das Heimteam</param>
        /// <param name="Tordifferenz2">Summand für das Auswärtsteam</param>
        /// <param name="Punkte1">Summand für das Heimteam</param>
        /// <param name="Punkte2">Summand für das Auswärtsteam</param>
        private static void ErgebnisEintragen(Spielpaarungen bSpielpaarung, Teilnehmer bTeilnehmer1, Teilnehmer bTeilnehmer2, int Tore1, int Tore2, 
                                              int Siege1, int Siege2, int Unentschieden1, int Unentschieden2, int Niederlage1, int Niederlage2, 
                                              int Tordifferenz1, int Tordifferenz2, int Punkte1, int Punkte2)
        {
            bSpielpaarung.Tore1 = Tore1;
            bSpielpaarung.Tore2 = Tore2;
            bTeilnehmer1.Siege += Siege1;
            bTeilnehmer2.Siege += Siege2;
            bTeilnehmer1.Unentschieden += Unentschieden1;
            bTeilnehmer2.Unentschieden += Unentschieden2;
            bTeilnehmer1.Niederlagen += Niederlage1;
            bTeilnehmer2.Niederlagen += Niederlage2;
            bTeilnehmer1.Tore += Tore1;
            bTeilnehmer2.Tore += Tore2;
            bTeilnehmer1.Gegentore += Tore2;
            bTeilnehmer2.Gegentore += Tore1;
            bTeilnehmer1.Tordifferenz += Tordifferenz1;
            bTeilnehmer2.Tordifferenz += Tordifferenz2;
            bTeilnehmer1.Punkte += Punkte1;
            bTeilnehmer2.Punkte += Punkte2;
        }
    }
}
