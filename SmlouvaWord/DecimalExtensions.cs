using System;

namespace SmlouvaWord
{
    public static class DecimalExtensions
    {
        public static string ToWords(this decimal amount)
        {
            string amountByWords = "";
            string StrCis, pom2 = "", mena = " korun českých";
            int Rad, Ofs = 1, pom, pom1, Poloha;
            string[] Jedn = new string[10] { "", "jedna", "dvě", "tři", "čtyři", "pět", "šest", "sedm", "osm", "devět" };
            string[] Des1 = new string[10] { "deset", "jedenáct", "dvanáct", "třináct", "čtrnáct", "patnáct", "šestnáct", "sedmnáct", "osmnáct", "devatenáct" };
            string[] Des = new string[10] { "", "", "dvacet", "třicet", "čtyřicet", "padesát", "šedesát", "sedmdesát", "osmdesát", "devadesát" };
            string[] Sta = new string[10] { "", "jednosto", "dvěsta", "třista", "čtyřista", "pětset", "šestset", "sedmset", "osmset", "devětset" };
            string[] Tis = new string[10] { "tisíc", "tisíc", "tisíce", "tisíce", "tisíce", "tisíc", "tisíc", "tisíc", "tisíc", "tisíc" };
            string[] JednTM = new string[10] { "", "jeden", "dva", "tři", "čtyři", "pět", "šest", "sedm", "osm", "devět" };
            string[] Mil = new string[10] { "milionů", "milion", "miliony", "miliony", "miliony", "milionů", "milionů", "milionů", "milionů", "milionů" };

            StrCis = String.Format("{0:0.00}", amount);
            Poloha = StrCis.IndexOf(",") - 1; //poloha řádu jednotek v čísle

            if (Poloha < 9)
            {
                Rad = 0;     //řád čislice v čísle
                do
                {
                    pom = System.Int32.Parse(StrCis.Substring(Poloha, 1));
                    if (Poloha > 0)
                    {
                        pom1 = System.Int32.Parse(StrCis.Substring(Poloha - 1, 1));
                    }
                    else
                    {
                        pom1 = 0;
                    }

                    switch (Rad)
                    {
                        case 0:
                            if (pom1 == 1)
                            {
                                pom2 = Des1[pom];
                                Ofs = 2;
                            }
                            else
                            {
                                if (amount > 21)
                                {
                                    pom2 = JednTM[pom];
                                }
                                else
                                {
                                    pom2 = Jedn[pom];
                                }

                                Ofs = 1;
                            }
                            break;
                        case 1:
                            pom2 = Des[pom];
                            Ofs = 1;
                            break;
                        case 2:
                            pom2 = Sta[pom];
                            Ofs = 1;
                            break;
                        case 3:
                            if (amount < 22000)
                            {
                                pom2 = (pom1 != 1) ? Jedn[pom] : Des1[pom];
                            }
                            else
                            {
                                pom2 = (pom1 != 1) ? JednTM[pom] : Des1[pom];
                            }
                            Ofs = (pom1 != 1) ? 1 : 2;
                            if (Poloha > 2) //když zůstávají ještě >3 číslice
                            {
                                if (StrCis.Substring(Poloha - 2, 3) != "000")
                                {
                                    pom2 = pom2 + ((pom1 != 1) ? Tis[pom] : "tisíc");
                                }
                                else
                                {
                                    Ofs = 3; //přeskočí na řád 6 - miliony                                
                                }
                            }
                            else
                            {
                                if (amount < 22000)
                                {
                                    pom2 = pom2 + ((pom1 != 1) ? Tis[pom] : "tisíc");
                                }
                                else
                                {
                                    pom2 = pom2 + "tisíc";
                                }
                            }
                            break;
                        case 4:
                            pom2 = Des[pom];
                            Ofs = 1;
                            break;
                        case 5:
                            pom2 = Sta[pom];
                            Ofs = 1;
                            break;
                        case 6:
                            pom2 = (pom1 != 1) ? JednTM[pom] + Mil[pom] : Des1[pom] + "milionů";
                            Ofs = (pom1 != 1) ? 1 : 2;
                            break;
                        case 7:
                            pom2 = Des[pom];
                            Ofs = 1;
                            break;
                        case 8:
                            pom2 = Sta[pom];
                            Ofs = 1;
                            break;
                        default:
                            pom2 = "Nekorektní číslo!";
                            break;
                    }
                    amountByWords = pom2 + amountByWords;
                    Poloha = Poloha - Ofs;
                    Rad = Rad + Ofs;
                }
                while (Poloha > -1);

                if (amount < 5)
                {
                    mena = " koruny české";
                }
                if (amount == 1)
                {
                    mena = " koruna česká";
                }

                amountByWords = amountByWords + mena;
            }
            else
            {
                amountByWords = "Číslo je větší než 999 999 999 !";
            }

            return amountByWords;
        }
    }
}
