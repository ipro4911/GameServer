// Decompiled with JetBrains decompiler
// Type: RegionName
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections;

public static class RegionName
{
  private static Hashtable GEOIP_REGION_NAME;

  public static string getRegionName(string ccode, string region)
  {
    if (RegionName.GEOIP_REGION_NAME == null)
      RegionName.init_region_names();
    if (region == null || region == "00")
      return (string) null;
    if (!RegionName.GEOIP_REGION_NAME.ContainsKey((object) ccode))
      return (string) null;
    return (string) ((Hashtable) RegionName.GEOIP_REGION_NAME[(object) ccode])[(object) region];
  }

  private static void init_region_names()
  {
    RegionName.GEOIP_REGION_NAME = new Hashtable();
    RegionName.GEOIP_REGION_NAME.Add((object) "AD", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Canillo"
      },
      {
        (object) "03",
        (object) "Encamp"
      },
      {
        (object) "04",
        (object) "La Massana"
      },
      {
        (object) "05",
        (object) "Ordino"
      },
      {
        (object) "06",
        (object) "Sant Julia de Loria"
      },
      {
        (object) "07",
        (object) "Andorra la Vella"
      },
      {
        (object) "08",
        (object) "Escaldes-Engordany"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "AE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Abu Dhabi"
      },
      {
        (object) "02",
        (object) "Ajman"
      },
      {
        (object) "03",
        (object) "Dubai"
      },
      {
        (object) "04",
        (object) "Fujairah"
      },
      {
        (object) "05",
        (object) "Ras Al Khaimah"
      },
      {
        (object) "06",
        (object) "Sharjah"
      },
      {
        (object) "07",
        (object) "Umm Al Quwain"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "AF", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Badakhshan"
      },
      {
        (object) "02",
        (object) "Badghis"
      },
      {
        (object) "03",
        (object) "Baghlan"
      },
      {
        (object) "05",
        (object) "Bamian"
      },
      {
        (object) "06",
        (object) "Farah"
      },
      {
        (object) "07",
        (object) "Faryab"
      },
      {
        (object) "08",
        (object) "Ghazni"
      },
      {
        (object) "09",
        (object) "Ghowr"
      },
      {
        (object) "10",
        (object) "Helmand"
      },
      {
        (object) "11",
        (object) "Herat"
      },
      {
        (object) "13",
        (object) "Kabol"
      },
      {
        (object) "14",
        (object) "Kapisa"
      },
      {
        (object) "17",
        (object) "Lowgar"
      },
      {
        (object) "18",
        (object) "Nangarhar"
      },
      {
        (object) "19",
        (object) "Nimruz"
      },
      {
        (object) "23",
        (object) "Kandahar"
      },
      {
        (object) "24",
        (object) "Kondoz"
      },
      {
        (object) "26",
        (object) "Takhar"
      },
      {
        (object) "27",
        (object) "Vardak"
      },
      {
        (object) "28",
        (object) "Zabol"
      },
      {
        (object) "29",
        (object) "Paktika"
      },
      {
        (object) "30",
        (object) "Balkh"
      },
      {
        (object) "31",
        (object) "Jowzjan"
      },
      {
        (object) "32",
        (object) "Samangan"
      },
      {
        (object) "33",
        (object) "Sar-e Pol"
      },
      {
        (object) "34",
        (object) "Konar"
      },
      {
        (object) "35",
        (object) "Laghman"
      },
      {
        (object) "36",
        (object) "Paktia"
      },
      {
        (object) "37",
        (object) "Khowst"
      },
      {
        (object) "38",
        (object) "Nurestan"
      },
      {
        (object) "39",
        (object) "Oruzgan"
      },
      {
        (object) "40",
        (object) "Parvan"
      },
      {
        (object) "41",
        (object) "Daykondi"
      },
      {
        (object) "42",
        (object) "Panjshir"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "AG", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Barbuda"
      },
      {
        (object) "03",
        (object) "Saint George"
      },
      {
        (object) "04",
        (object) "Saint John"
      },
      {
        (object) "05",
        (object) "Saint Mary"
      },
      {
        (object) "06",
        (object) "Saint Paul"
      },
      {
        (object) "07",
        (object) "Saint Peter"
      },
      {
        (object) "08",
        (object) "Saint Philip"
      },
      {
        (object) "09",
        (object) "Redonda"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "AL", (object) new Hashtable()
    {
      {
        (object) "40",
        (object) "Berat"
      },
      {
        (object) "41",
        (object) "Diber"
      },
      {
        (object) "42",
        (object) "Durres"
      },
      {
        (object) "43",
        (object) "Elbasan"
      },
      {
        (object) "44",
        (object) "Fier"
      },
      {
        (object) "45",
        (object) "Gjirokaster"
      },
      {
        (object) "46",
        (object) "Korce"
      },
      {
        (object) "47",
        (object) "Kukes"
      },
      {
        (object) "48",
        (object) "Lezhe"
      },
      {
        (object) "49",
        (object) "Shkoder"
      },
      {
        (object) "50",
        (object) "Tirane"
      },
      {
        (object) "51",
        (object) "Vlore"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "AM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Aragatsotn"
      },
      {
        (object) "02",
        (object) "Ararat"
      },
      {
        (object) "03",
        (object) "Armavir"
      },
      {
        (object) "04",
        (object) "Geghark'unik'"
      },
      {
        (object) "05",
        (object) "Kotayk'"
      },
      {
        (object) "06",
        (object) "Lorri"
      },
      {
        (object) "07",
        (object) "Shirak"
      },
      {
        (object) "08",
        (object) "Syunik'"
      },
      {
        (object) "09",
        (object) "Tavush"
      },
      {
        (object) "10",
        (object) "Vayots' Dzor"
      },
      {
        (object) "11",
        (object) "Yerevan"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "AO", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Benguela"
      },
      {
        (object) "02",
        (object) "Bie"
      },
      {
        (object) "03",
        (object) "Cabinda"
      },
      {
        (object) "04",
        (object) "Cuando Cubango"
      },
      {
        (object) "05",
        (object) "Cuanza Norte"
      },
      {
        (object) "06",
        (object) "Cuanza Sul"
      },
      {
        (object) "07",
        (object) "Cunene"
      },
      {
        (object) "08",
        (object) "Huambo"
      },
      {
        (object) "09",
        (object) "Huila"
      },
      {
        (object) "12",
        (object) "Malanje"
      },
      {
        (object) "13",
        (object) "Namibe"
      },
      {
        (object) "14",
        (object) "Moxico"
      },
      {
        (object) "15",
        (object) "Uige"
      },
      {
        (object) "16",
        (object) "Zaire"
      },
      {
        (object) "17",
        (object) "Lunda Norte"
      },
      {
        (object) "18",
        (object) "Lunda Sul"
      },
      {
        (object) "19",
        (object) "Bengo"
      },
      {
        (object) "20",
        (object) "Luanda"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "AR", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Buenos Aires"
      },
      {
        (object) "02",
        (object) "Catamarca"
      },
      {
        (object) "03",
        (object) "Chaco"
      },
      {
        (object) "04",
        (object) "Chubut"
      },
      {
        (object) "05",
        (object) "Cordoba"
      },
      {
        (object) "06",
        (object) "Corrientes"
      },
      {
        (object) "07",
        (object) "Distrito Federal"
      },
      {
        (object) "08",
        (object) "Entre Rios"
      },
      {
        (object) "09",
        (object) "Formosa"
      },
      {
        (object) "10",
        (object) "Jujuy"
      },
      {
        (object) "11",
        (object) "La Pampa"
      },
      {
        (object) "12",
        (object) "La Rioja"
      },
      {
        (object) "13",
        (object) "Mendoza"
      },
      {
        (object) "14",
        (object) "Misiones"
      },
      {
        (object) "15",
        (object) "Neuquen"
      },
      {
        (object) "16",
        (object) "Rio Negro"
      },
      {
        (object) "17",
        (object) "Salta"
      },
      {
        (object) "18",
        (object) "San Juan"
      },
      {
        (object) "19",
        (object) "San Luis"
      },
      {
        (object) "20",
        (object) "Santa Cruz"
      },
      {
        (object) "21",
        (object) "Santa Fe"
      },
      {
        (object) "22",
        (object) "Santiago del Estero"
      },
      {
        (object) "23",
        (object) "Tierra del Fuego"
      },
      {
        (object) "24",
        (object) "Tucuman"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "AT", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Burgenland"
      },
      {
        (object) "02",
        (object) "Karnten"
      },
      {
        (object) "03",
        (object) "Niederosterreich"
      },
      {
        (object) "04",
        (object) "Oberosterreich"
      },
      {
        (object) "05",
        (object) "Salzburg"
      },
      {
        (object) "06",
        (object) "Steiermark"
      },
      {
        (object) "07",
        (object) "Tirol"
      },
      {
        (object) "08",
        (object) "Vorarlberg"
      },
      {
        (object) "09",
        (object) "Wien"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "AU", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Australian Capital Territory"
      },
      {
        (object) "02",
        (object) "New South Wales"
      },
      {
        (object) "03",
        (object) "Northern Territory"
      },
      {
        (object) "04",
        (object) "Queensland"
      },
      {
        (object) "05",
        (object) "South Australia"
      },
      {
        (object) "06",
        (object) "Tasmania"
      },
      {
        (object) "07",
        (object) "Victoria"
      },
      {
        (object) "08",
        (object) "Western Australia"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "AZ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Abseron"
      },
      {
        (object) "02",
        (object) "Agcabadi"
      },
      {
        (object) "03",
        (object) "Agdam"
      },
      {
        (object) "04",
        (object) "Agdas"
      },
      {
        (object) "05",
        (object) "Agstafa"
      },
      {
        (object) "06",
        (object) "Agsu"
      },
      {
        (object) "07",
        (object) "Ali Bayramli"
      },
      {
        (object) "08",
        (object) "Astara"
      },
      {
        (object) "09",
        (object) "Baki"
      },
      {
        (object) "10",
        (object) "Balakan"
      },
      {
        (object) "11",
        (object) "Barda"
      },
      {
        (object) "12",
        (object) "Beylaqan"
      },
      {
        (object) "13",
        (object) "Bilasuvar"
      },
      {
        (object) "14",
        (object) "Cabrayil"
      },
      {
        (object) "15",
        (object) "Calilabad"
      },
      {
        (object) "16",
        (object) "Daskasan"
      },
      {
        (object) "17",
        (object) "Davaci"
      },
      {
        (object) "18",
        (object) "Fuzuli"
      },
      {
        (object) "19",
        (object) "Gadabay"
      },
      {
        (object) "20",
        (object) "Ganca"
      },
      {
        (object) "21",
        (object) "Goranboy"
      },
      {
        (object) "22",
        (object) "Goycay"
      },
      {
        (object) "23",
        (object) "Haciqabul"
      },
      {
        (object) "24",
        (object) "Imisli"
      },
      {
        (object) "25",
        (object) "Ismayilli"
      },
      {
        (object) "26",
        (object) "Kalbacar"
      },
      {
        (object) "27",
        (object) "Kurdamir"
      },
      {
        (object) "28",
        (object) "Lacin"
      },
      {
        (object) "29",
        (object) "Lankaran"
      },
      {
        (object) "30",
        (object) "Lankaran"
      },
      {
        (object) "31",
        (object) "Lerik"
      },
      {
        (object) "32",
        (object) "Masalli"
      },
      {
        (object) "33",
        (object) "Mingacevir"
      },
      {
        (object) "34",
        (object) "Naftalan"
      },
      {
        (object) "35",
        (object) "Naxcivan"
      },
      {
        (object) "36",
        (object) "Neftcala"
      },
      {
        (object) "37",
        (object) "Oguz"
      },
      {
        (object) "38",
        (object) "Qabala"
      },
      {
        (object) "39",
        (object) "Qax"
      },
      {
        (object) "40",
        (object) "Qazax"
      },
      {
        (object) "41",
        (object) "Qobustan"
      },
      {
        (object) "42",
        (object) "Quba"
      },
      {
        (object) "43",
        (object) "Qubadli"
      },
      {
        (object) "44",
        (object) "Qusar"
      },
      {
        (object) "45",
        (object) "Saatli"
      },
      {
        (object) "46",
        (object) "Sabirabad"
      },
      {
        (object) "47",
        (object) "Saki"
      },
      {
        (object) "48",
        (object) "Saki"
      },
      {
        (object) "49",
        (object) "Salyan"
      },
      {
        (object) "50",
        (object) "Samaxi"
      },
      {
        (object) "51",
        (object) "Samkir"
      },
      {
        (object) "52",
        (object) "Samux"
      },
      {
        (object) "53",
        (object) "Siyazan"
      },
      {
        (object) "54",
        (object) "Sumqayit"
      },
      {
        (object) "55",
        (object) "Susa"
      },
      {
        (object) "56",
        (object) "Susa"
      },
      {
        (object) "57",
        (object) "Tartar"
      },
      {
        (object) "58",
        (object) "Tovuz"
      },
      {
        (object) "59",
        (object) "Ucar"
      },
      {
        (object) "60",
        (object) "Xacmaz"
      },
      {
        (object) "61",
        (object) "Xankandi"
      },
      {
        (object) "62",
        (object) "Xanlar"
      },
      {
        (object) "63",
        (object) "Xizi"
      },
      {
        (object) "64",
        (object) "Xocali"
      },
      {
        (object) "65",
        (object) "Xocavand"
      },
      {
        (object) "66",
        (object) "Yardimli"
      },
      {
        (object) "67",
        (object) "Yevlax"
      },
      {
        (object) "68",
        (object) "Yevlax"
      },
      {
        (object) "69",
        (object) "Zangilan"
      },
      {
        (object) "70",
        (object) "Zaqatala"
      },
      {
        (object) "71",
        (object) "Zardab"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BA", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Federation of Bosnia and Herzegovina"
      },
      {
        (object) "02",
        (object) "Republika Srpska"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BB", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Christ Church"
      },
      {
        (object) "02",
        (object) "Saint Andrew"
      },
      {
        (object) "03",
        (object) "Saint George"
      },
      {
        (object) "04",
        (object) "Saint James"
      },
      {
        (object) "05",
        (object) "Saint John"
      },
      {
        (object) "06",
        (object) "Saint Joseph"
      },
      {
        (object) "07",
        (object) "Saint Lucy"
      },
      {
        (object) "08",
        (object) "Saint Michael"
      },
      {
        (object) "09",
        (object) "Saint Peter"
      },
      {
        (object) "10",
        (object) "Saint Philip"
      },
      {
        (object) "11",
        (object) "Saint Thomas"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BD", (object) new Hashtable()
    {
      {
        (object) "81",
        (object) "Dhaka"
      },
      {
        (object) "82",
        (object) "Khulna"
      },
      {
        (object) "83",
        (object) "Rajshahi"
      },
      {
        (object) "84",
        (object) "Chittagong"
      },
      {
        (object) "85",
        (object) "Barisal"
      },
      {
        (object) "86",
        (object) "Sylhet"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Antwerpen"
      },
      {
        (object) "03",
        (object) "Hainaut"
      },
      {
        (object) "04",
        (object) "Liege"
      },
      {
        (object) "05",
        (object) "Limburg"
      },
      {
        (object) "06",
        (object) "Luxembourg"
      },
      {
        (object) "07",
        (object) "Namur"
      },
      {
        (object) "08",
        (object) "Oost-Vlaanderen"
      },
      {
        (object) "09",
        (object) "West-Vlaanderen"
      },
      {
        (object) "10",
        (object) "Brabant Wallon"
      },
      {
        (object) "11",
        (object) "Brussels Hoofdstedelijk Gewest"
      },
      {
        (object) "12",
        (object) "Vlaams-Brabant"
      },
      {
        (object) "13",
        (object) "Flanders"
      },
      {
        (object) "14",
        (object) "Wallonia"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BF", (object) new Hashtable()
    {
      {
        (object) "15",
        (object) "Bam"
      },
      {
        (object) "19",
        (object) "Boulkiemde"
      },
      {
        (object) "20",
        (object) "Ganzourgou"
      },
      {
        (object) "21",
        (object) "Gnagna"
      },
      {
        (object) "28",
        (object) "Kouritenga"
      },
      {
        (object) "33",
        (object) "Oudalan"
      },
      {
        (object) "34",
        (object) "Passore"
      },
      {
        (object) "36",
        (object) "Sanguie"
      },
      {
        (object) "40",
        (object) "Soum"
      },
      {
        (object) "42",
        (object) "Tapoa"
      },
      {
        (object) "44",
        (object) "Zoundweogo"
      },
      {
        (object) "45",
        (object) "Bale"
      },
      {
        (object) "46",
        (object) "Banwa"
      },
      {
        (object) "47",
        (object) "Bazega"
      },
      {
        (object) "48",
        (object) "Bougouriba"
      },
      {
        (object) "49",
        (object) "Boulgou"
      },
      {
        (object) "50",
        (object) "Gourma"
      },
      {
        (object) "51",
        (object) "Houet"
      },
      {
        (object) "52",
        (object) "Ioba"
      },
      {
        (object) "53",
        (object) "Kadiogo"
      },
      {
        (object) "54",
        (object) "Kenedougou"
      },
      {
        (object) "55",
        (object) "Komoe"
      },
      {
        (object) "56",
        (object) "Komondjari"
      },
      {
        (object) "57",
        (object) "Kompienga"
      },
      {
        (object) "58",
        (object) "Kossi"
      },
      {
        (object) "59",
        (object) "Koulpelogo"
      },
      {
        (object) "60",
        (object) "Kourweogo"
      },
      {
        (object) "61",
        (object) "Leraba"
      },
      {
        (object) "62",
        (object) "Loroum"
      },
      {
        (object) "63",
        (object) "Mouhoun"
      },
      {
        (object) "64",
        (object) "Namentenga"
      },
      {
        (object) "65",
        (object) "Naouri"
      },
      {
        (object) "66",
        (object) "Nayala"
      },
      {
        (object) "67",
        (object) "Noumbiel"
      },
      {
        (object) "68",
        (object) "Oubritenga"
      },
      {
        (object) "69",
        (object) "Poni"
      },
      {
        (object) "70",
        (object) "Sanmatenga"
      },
      {
        (object) "71",
        (object) "Seno"
      },
      {
        (object) "72",
        (object) "Sissili"
      },
      {
        (object) "73",
        (object) "Sourou"
      },
      {
        (object) "74",
        (object) "Tuy"
      },
      {
        (object) "75",
        (object) "Yagha"
      },
      {
        (object) "76",
        (object) "Yatenga"
      },
      {
        (object) "77",
        (object) "Ziro"
      },
      {
        (object) "78",
        (object) "Zondoma"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BG", (object) new Hashtable()
    {
      {
        (object) "33",
        (object) "Mikhaylovgrad"
      },
      {
        (object) "38",
        (object) "Blagoevgrad"
      },
      {
        (object) "39",
        (object) "Burgas"
      },
      {
        (object) "40",
        (object) "Dobrich"
      },
      {
        (object) "41",
        (object) "Gabrovo"
      },
      {
        (object) "42",
        (object) "Grad Sofiya"
      },
      {
        (object) "43",
        (object) "Khaskovo"
      },
      {
        (object) "44",
        (object) "Kurdzhali"
      },
      {
        (object) "45",
        (object) "Kyustendil"
      },
      {
        (object) "46",
        (object) "Lovech"
      },
      {
        (object) "47",
        (object) "Montana"
      },
      {
        (object) "48",
        (object) "Pazardzhik"
      },
      {
        (object) "49",
        (object) "Pernik"
      },
      {
        (object) "50",
        (object) "Pleven"
      },
      {
        (object) "51",
        (object) "Plovdiv"
      },
      {
        (object) "52",
        (object) "Razgrad"
      },
      {
        (object) "53",
        (object) "Ruse"
      },
      {
        (object) "54",
        (object) "Shumen"
      },
      {
        (object) "55",
        (object) "Silistra"
      },
      {
        (object) "56",
        (object) "Sliven"
      },
      {
        (object) "57",
        (object) "Smolyan"
      },
      {
        (object) "58",
        (object) "Sofiya"
      },
      {
        (object) "59",
        (object) "Stara Zagora"
      },
      {
        (object) "60",
        (object) "Turgovishte"
      },
      {
        (object) "61",
        (object) "Varna"
      },
      {
        (object) "62",
        (object) "Veliko Turnovo"
      },
      {
        (object) "63",
        (object) "Vidin"
      },
      {
        (object) "64",
        (object) "Vratsa"
      },
      {
        (object) "65",
        (object) "Yambol"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BH", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Al Hadd"
      },
      {
        (object) "02",
        (object) "Al Manamah"
      },
      {
        (object) "05",
        (object) "Jidd Hafs"
      },
      {
        (object) "06",
        (object) "Sitrah"
      },
      {
        (object) "08",
        (object) "Al Mintaqah al Gharbiyah"
      },
      {
        (object) "09",
        (object) "Mintaqat Juzur Hawar"
      },
      {
        (object) "10",
        (object) "Al Mintaqah ash Shamaliyah"
      },
      {
        (object) "11",
        (object) "Al Mintaqah al Wusta"
      },
      {
        (object) "12",
        (object) "Madinat"
      },
      {
        (object) "13",
        (object) "Ar Rifa"
      },
      {
        (object) "14",
        (object) "Madinat Hamad"
      },
      {
        (object) "15",
        (object) "Al Muharraq"
      },
      {
        (object) "16",
        (object) "Al Asimah"
      },
      {
        (object) "17",
        (object) "Al Janubiyah"
      },
      {
        (object) "18",
        (object) "Ash Shamaliyah"
      },
      {
        (object) "19",
        (object) "Al Wusta"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BI", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Bujumbura"
      },
      {
        (object) "09",
        (object) "Bubanza"
      },
      {
        (object) "10",
        (object) "Bururi"
      },
      {
        (object) "11",
        (object) "Cankuzo"
      },
      {
        (object) "12",
        (object) "Cibitoke"
      },
      {
        (object) "13",
        (object) "Gitega"
      },
      {
        (object) "14",
        (object) "Karuzi"
      },
      {
        (object) "15",
        (object) "Kayanza"
      },
      {
        (object) "16",
        (object) "Kirundo"
      },
      {
        (object) "17",
        (object) "Makamba"
      },
      {
        (object) "18",
        (object) "Muyinga"
      },
      {
        (object) "19",
        (object) "Ngozi"
      },
      {
        (object) "20",
        (object) "Rutana"
      },
      {
        (object) "21",
        (object) "Ruyigi"
      },
      {
        (object) "22",
        (object) "Muramvya"
      },
      {
        (object) "23",
        (object) "Mwaro"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BJ", (object) new Hashtable()
    {
      {
        (object) "07",
        (object) "Alibori"
      },
      {
        (object) "08",
        (object) "Atakora"
      },
      {
        (object) "09",
        (object) "Atlanyique"
      },
      {
        (object) "10",
        (object) "Borgou"
      },
      {
        (object) "11",
        (object) "Collines"
      },
      {
        (object) "12",
        (object) "Kouffo"
      },
      {
        (object) "13",
        (object) "Donga"
      },
      {
        (object) "14",
        (object) "Littoral"
      },
      {
        (object) "15",
        (object) "Mono"
      },
      {
        (object) "16",
        (object) "Oueme"
      },
      {
        (object) "17",
        (object) "Plateau"
      },
      {
        (object) "18",
        (object) "Zou"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Devonshire"
      },
      {
        (object) "02",
        (object) "Hamilton"
      },
      {
        (object) "03",
        (object) "Hamilton"
      },
      {
        (object) "04",
        (object) "Paget"
      },
      {
        (object) "05",
        (object) "Pembroke"
      },
      {
        (object) "06",
        (object) "Saint George"
      },
      {
        (object) "07",
        (object) "Saint George's"
      },
      {
        (object) "08",
        (object) "Sandys"
      },
      {
        (object) "09",
        (object) "Smiths"
      },
      {
        (object) "10",
        (object) "Southampton"
      },
      {
        (object) "11",
        (object) "Warwick"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BN", (object) new Hashtable()
    {
      {
        (object) "07",
        (object) "Alibori"
      },
      {
        (object) "08",
        (object) "Belait"
      },
      {
        (object) "09",
        (object) "Brunei and Muara"
      },
      {
        (object) "10",
        (object) "Temburong"
      },
      {
        (object) "11",
        (object) "Collines"
      },
      {
        (object) "12",
        (object) "Kouffo"
      },
      {
        (object) "13",
        (object) "Donga"
      },
      {
        (object) "14",
        (object) "Littoral"
      },
      {
        (object) "15",
        (object) "Tutong"
      },
      {
        (object) "16",
        (object) "Oueme"
      },
      {
        (object) "17",
        (object) "Plateau"
      },
      {
        (object) "18",
        (object) "Zou"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BO", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Chuquisaca"
      },
      {
        (object) "02",
        (object) "Cochabamba"
      },
      {
        (object) "03",
        (object) "El Beni"
      },
      {
        (object) "04",
        (object) "La Paz"
      },
      {
        (object) "05",
        (object) "Oruro"
      },
      {
        (object) "06",
        (object) "Pando"
      },
      {
        (object) "07",
        (object) "Potosi"
      },
      {
        (object) "08",
        (object) "Santa Cruz"
      },
      {
        (object) "09",
        (object) "Tarija"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BR", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Acre"
      },
      {
        (object) "02",
        (object) "Alagoas"
      },
      {
        (object) "03",
        (object) "Amapa"
      },
      {
        (object) "04",
        (object) "Amazonas"
      },
      {
        (object) "05",
        (object) "Bahia"
      },
      {
        (object) "06",
        (object) "Ceara"
      },
      {
        (object) "07",
        (object) "Distrito Federal"
      },
      {
        (object) "08",
        (object) "Espirito Santo"
      },
      {
        (object) "11",
        (object) "Mato Grosso do Sul"
      },
      {
        (object) "13",
        (object) "Maranhao"
      },
      {
        (object) "14",
        (object) "Mato Grosso"
      },
      {
        (object) "15",
        (object) "Minas Gerais"
      },
      {
        (object) "16",
        (object) "Para"
      },
      {
        (object) "17",
        (object) "Paraiba"
      },
      {
        (object) "18",
        (object) "Parana"
      },
      {
        (object) "20",
        (object) "Piaui"
      },
      {
        (object) "21",
        (object) "Rio de Janeiro"
      },
      {
        (object) "22",
        (object) "Rio Grande do Norte"
      },
      {
        (object) "23",
        (object) "Rio Grande do Sul"
      },
      {
        (object) "24",
        (object) "Rondonia"
      },
      {
        (object) "25",
        (object) "Roraima"
      },
      {
        (object) "26",
        (object) "Santa Catarina"
      },
      {
        (object) "27",
        (object) "Sao Paulo"
      },
      {
        (object) "28",
        (object) "Sergipe"
      },
      {
        (object) "29",
        (object) "Goias"
      },
      {
        (object) "30",
        (object) "Pernambuco"
      },
      {
        (object) "31",
        (object) "Tocantins"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BS", (object) new Hashtable()
    {
      {
        (object) "05",
        (object) "Bimini"
      },
      {
        (object) "06",
        (object) "Cat Island"
      },
      {
        (object) "10",
        (object) "Exuma"
      },
      {
        (object) "13",
        (object) "Inagua"
      },
      {
        (object) "15",
        (object) "Long Island"
      },
      {
        (object) "16",
        (object) "Mayaguana"
      },
      {
        (object) "18",
        (object) "Ragged Island"
      },
      {
        (object) "22",
        (object) "Harbour Island"
      },
      {
        (object) "23",
        (object) "New Providence"
      },
      {
        (object) "24",
        (object) "Acklins and Crooked Islands"
      },
      {
        (object) "25",
        (object) "Freeport"
      },
      {
        (object) "26",
        (object) "Fresh Creek"
      },
      {
        (object) "27",
        (object) "Governor's Harbour"
      },
      {
        (object) "28",
        (object) "Green Turtle Cay"
      },
      {
        (object) "29",
        (object) "High Rock"
      },
      {
        (object) "30",
        (object) "Kemps Bay"
      },
      {
        (object) "31",
        (object) "Marsh Harbour"
      },
      {
        (object) "32",
        (object) "Nichollstown and Berry Islands"
      },
      {
        (object) "33",
        (object) "Rock Sound"
      },
      {
        (object) "34",
        (object) "Sandy Point"
      },
      {
        (object) "35",
        (object) "San Salvador and Rum Cay"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BT", (object) new Hashtable()
    {
      {
        (object) "05",
        (object) "Bumthang"
      },
      {
        (object) "06",
        (object) "Chhukha"
      },
      {
        (object) "07",
        (object) "Chirang"
      },
      {
        (object) "08",
        (object) "Daga"
      },
      {
        (object) "09",
        (object) "Geylegphug"
      },
      {
        (object) "10",
        (object) "Ha"
      },
      {
        (object) "11",
        (object) "Lhuntshi"
      },
      {
        (object) "12",
        (object) "Mongar"
      },
      {
        (object) "13",
        (object) "Paro"
      },
      {
        (object) "14",
        (object) "Pemagatsel"
      },
      {
        (object) "15",
        (object) "Punakha"
      },
      {
        (object) "16",
        (object) "Samchi"
      },
      {
        (object) "17",
        (object) "Samdrup"
      },
      {
        (object) "18",
        (object) "Shemgang"
      },
      {
        (object) "19",
        (object) "Tashigang"
      },
      {
        (object) "20",
        (object) "Thimphu"
      },
      {
        (object) "21",
        (object) "Tongsa"
      },
      {
        (object) "22",
        (object) "Wangdi Phodrang"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BW", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Central"
      },
      {
        (object) "03",
        (object) "Ghanzi"
      },
      {
        (object) "04",
        (object) "Kgalagadi"
      },
      {
        (object) "05",
        (object) "Kgatleng"
      },
      {
        (object) "06",
        (object) "Kweneng"
      },
      {
        (object) "08",
        (object) "North-East"
      },
      {
        (object) "09",
        (object) "South-East"
      },
      {
        (object) "10",
        (object) "Southern"
      },
      {
        (object) "11",
        (object) "North-West"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BY", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Brestskaya VobLasts'"
      },
      {
        (object) "02",
        (object) "Homyel'skaya VobLasts'"
      },
      {
        (object) "03",
        (object) "Hrodzyenskaya VobLasts'"
      },
      {
        (object) "04",
        (object) "Minsk"
      },
      {
        (object) "05",
        (object) "Minskaya VobLasts'"
      },
      {
        (object) "06",
        (object) "Mahilyowskaya VobLasts'"
      },
      {
        (object) "07",
        (object) "Vitsyebskaya VobLasts'"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "BZ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Belize"
      },
      {
        (object) "02",
        (object) "Cayo"
      },
      {
        (object) "03",
        (object) "Corozal"
      },
      {
        (object) "04",
        (object) "Orange Walk"
      },
      {
        (object) "05",
        (object) "Stann Creek"
      },
      {
        (object) "06",
        (object) "Toledo"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CA", (object) new Hashtable()
    {
      {
        (object) "AB",
        (object) "Alberta"
      },
      {
        (object) "BC",
        (object) "British Columbia"
      },
      {
        (object) "MB",
        (object) "Manitoba"
      },
      {
        (object) "NB",
        (object) "New Brunswick"
      },
      {
        (object) "NL",
        (object) "Newfoundland"
      },
      {
        (object) "NS",
        (object) "Nova Scotia"
      },
      {
        (object) "NT",
        (object) "Northwest Territories"
      },
      {
        (object) "NU",
        (object) "Nunavut"
      },
      {
        (object) "ON",
        (object) "Ontario"
      },
      {
        (object) "PE",
        (object) "Prince Edward Island"
      },
      {
        (object) "QC",
        (object) "Quebec"
      },
      {
        (object) "SK",
        (object) "Saskatchewan"
      },
      {
        (object) "YT",
        (object) "Yukon Territory"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CD", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bandundu"
      },
      {
        (object) "02",
        (object) "Equateur"
      },
      {
        (object) "04",
        (object) "Kasai-Oriental"
      },
      {
        (object) "05",
        (object) "Katanga"
      },
      {
        (object) "06",
        (object) "Kinshasa"
      },
      {
        (object) "08",
        (object) "Bas-Congo"
      },
      {
        (object) "09",
        (object) "Orientale"
      },
      {
        (object) "10",
        (object) "Maniema"
      },
      {
        (object) "11",
        (object) "Nord-Kivu"
      },
      {
        (object) "12",
        (object) "Sud-Kivu"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CF", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bamingui-Bangoran"
      },
      {
        (object) "02",
        (object) "Basse-Kotto"
      },
      {
        (object) "03",
        (object) "Haute-Kotto"
      },
      {
        (object) "04",
        (object) "Mambere-Kadei"
      },
      {
        (object) "05",
        (object) "Haut-Mbomou"
      },
      {
        (object) "06",
        (object) "Kemo"
      },
      {
        (object) "07",
        (object) "Lobaye"
      },
      {
        (object) "08",
        (object) "Mbomou"
      },
      {
        (object) "09",
        (object) "Nana-Mambere"
      },
      {
        (object) "11",
        (object) "Ouaka"
      },
      {
        (object) "12",
        (object) "Ouham"
      },
      {
        (object) "13",
        (object) "Ouham-Pende"
      },
      {
        (object) "14",
        (object) "Cuvette-Ouest"
      },
      {
        (object) "15",
        (object) "Nana-Grebizi"
      },
      {
        (object) "16",
        (object) "Sangha-Mbaere"
      },
      {
        (object) "17",
        (object) "Ombella-Mpoko"
      },
      {
        (object) "18",
        (object) "Bangui"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CG", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bouenza"
      },
      {
        (object) "04",
        (object) "Kouilou"
      },
      {
        (object) "05",
        (object) "Lekoumou"
      },
      {
        (object) "06",
        (object) "Likouala"
      },
      {
        (object) "07",
        (object) "Niari"
      },
      {
        (object) "08",
        (object) "Plateaux"
      },
      {
        (object) "10",
        (object) "Sangha"
      },
      {
        (object) "11",
        (object) "Pool"
      },
      {
        (object) "12",
        (object) "Brazzaville"
      },
      {
        (object) "13",
        (object) "Cuvette"
      },
      {
        (object) "14",
        (object) "Cuvette-Ouest"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CH", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Aargau"
      },
      {
        (object) "02",
        (object) "Ausser-Rhoden"
      },
      {
        (object) "03",
        (object) "Basel-Landschaft"
      },
      {
        (object) "04",
        (object) "Basel-Stadt"
      },
      {
        (object) "05",
        (object) "Bern"
      },
      {
        (object) "06",
        (object) "Fribourg"
      },
      {
        (object) "07",
        (object) "Geneve"
      },
      {
        (object) "08",
        (object) "Glarus"
      },
      {
        (object) "09",
        (object) "Graubunden"
      },
      {
        (object) "10",
        (object) "Inner-Rhoden"
      },
      {
        (object) "11",
        (object) "Luzern"
      },
      {
        (object) "12",
        (object) "Neuchatel"
      },
      {
        (object) "13",
        (object) "Nidwalden"
      },
      {
        (object) "14",
        (object) "Obwalden"
      },
      {
        (object) "15",
        (object) "Sankt Gallen"
      },
      {
        (object) "16",
        (object) "Schaffhausen"
      },
      {
        (object) "17",
        (object) "Schwyz"
      },
      {
        (object) "18",
        (object) "Solothurn"
      },
      {
        (object) "19",
        (object) "Thurgau"
      },
      {
        (object) "20",
        (object) "Ticino"
      },
      {
        (object) "21",
        (object) "Uri"
      },
      {
        (object) "22",
        (object) "Valais"
      },
      {
        (object) "23",
        (object) "Vaud"
      },
      {
        (object) "24",
        (object) "Zug"
      },
      {
        (object) "25",
        (object) "Zurich"
      },
      {
        (object) "26",
        (object) "Jura"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CI", (object) new Hashtable()
    {
      {
        (object) "74",
        (object) "Agneby"
      },
      {
        (object) "75",
        (object) "Bafing"
      },
      {
        (object) "76",
        (object) "Bas-Sassandra"
      },
      {
        (object) "77",
        (object) "Denguele"
      },
      {
        (object) "78",
        (object) "Dix-Huit Montagnes"
      },
      {
        (object) "79",
        (object) "Fromager"
      },
      {
        (object) "80",
        (object) "Haut-Sassandra"
      },
      {
        (object) "81",
        (object) "Lacs"
      },
      {
        (object) "82",
        (object) "Lagunes"
      },
      {
        (object) "83",
        (object) "Marahoue"
      },
      {
        (object) "84",
        (object) "Moyen-Cavally"
      },
      {
        (object) "85",
        (object) "Moyen-Comoe"
      },
      {
        (object) "86",
        (object) "N'zi-Comoe"
      },
      {
        (object) "87",
        (object) "Savanes"
      },
      {
        (object) "88",
        (object) "Sud-Bandama"
      },
      {
        (object) "89",
        (object) "Sud-Comoe"
      },
      {
        (object) "90",
        (object) "Vallee du Bandama"
      },
      {
        (object) "91",
        (object) "Worodougou"
      },
      {
        (object) "92",
        (object) "Zanzan"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CL", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Valparaiso"
      },
      {
        (object) "02",
        (object) "Aisen del General Carlos Ibanez del Campo"
      },
      {
        (object) "03",
        (object) "Antofagasta"
      },
      {
        (object) "04",
        (object) "Araucania"
      },
      {
        (object) "05",
        (object) "Atacama"
      },
      {
        (object) "06",
        (object) "Bio-Bio"
      },
      {
        (object) "07",
        (object) "Coquimbo"
      },
      {
        (object) "08",
        (object) "Libertador General Bernardo O'Higgins"
      },
      {
        (object) "09",
        (object) "Los Lagos"
      },
      {
        (object) "10",
        (object) "Magallanes y de la Antartica Chilena"
      },
      {
        (object) "11",
        (object) "Maule"
      },
      {
        (object) "12",
        (object) "Region Metropolitana"
      },
      {
        (object) "13",
        (object) "Tarapaca"
      },
      {
        (object) "14",
        (object) "Los Lagos"
      },
      {
        (object) "15",
        (object) "Tarapaca"
      },
      {
        (object) "16",
        (object) "Arica y Parinacota"
      },
      {
        (object) "17",
        (object) "Los Rios"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CM", (object) new Hashtable()
    {
      {
        (object) "04",
        (object) "Est"
      },
      {
        (object) "05",
        (object) "Littoral"
      },
      {
        (object) "07",
        (object) "Nord-Ouest"
      },
      {
        (object) "08",
        (object) "Ouest"
      },
      {
        (object) "09",
        (object) "Sud-Ouest"
      },
      {
        (object) "10",
        (object) "Adamaoua"
      },
      {
        (object) "11",
        (object) "Centre"
      },
      {
        (object) "12",
        (object) "Extreme-Nord"
      },
      {
        (object) "13",
        (object) "Nord"
      },
      {
        (object) "14",
        (object) "Sud"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CN", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Anhui"
      },
      {
        (object) "02",
        (object) "Zhejiang"
      },
      {
        (object) "03",
        (object) "Jiangxi"
      },
      {
        (object) "04",
        (object) "Jiangsu"
      },
      {
        (object) "05",
        (object) "Jilin"
      },
      {
        (object) "06",
        (object) "Qinghai"
      },
      {
        (object) "07",
        (object) "Fujian"
      },
      {
        (object) "08",
        (object) "Heilongjiang"
      },
      {
        (object) "09",
        (object) "Henan"
      },
      {
        (object) "10",
        (object) "Hebei"
      },
      {
        (object) "11",
        (object) "Hunan"
      },
      {
        (object) "12",
        (object) "Hubei"
      },
      {
        (object) "13",
        (object) "Xinjiang"
      },
      {
        (object) "14",
        (object) "Xizang"
      },
      {
        (object) "15",
        (object) "Gansu"
      },
      {
        (object) "16",
        (object) "Guangxi"
      },
      {
        (object) "18",
        (object) "Guizhou"
      },
      {
        (object) "19",
        (object) "Liaoning"
      },
      {
        (object) "20",
        (object) "Nei Mongol"
      },
      {
        (object) "21",
        (object) "Ningxia"
      },
      {
        (object) "22",
        (object) "Beijing"
      },
      {
        (object) "23",
        (object) "Shanghai"
      },
      {
        (object) "24",
        (object) "Shanxi"
      },
      {
        (object) "25",
        (object) "Shandong"
      },
      {
        (object) "26",
        (object) "Shaanxi"
      },
      {
        (object) "28",
        (object) "Tianjin"
      },
      {
        (object) "29",
        (object) "Yunnan"
      },
      {
        (object) "30",
        (object) "Guangdong"
      },
      {
        (object) "31",
        (object) "Hainan"
      },
      {
        (object) "32",
        (object) "Sichuan"
      },
      {
        (object) "33",
        (object) "Chongqing"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CO", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Amazonas"
      },
      {
        (object) "02",
        (object) "Antioquia"
      },
      {
        (object) "03",
        (object) "Arauca"
      },
      {
        (object) "04",
        (object) "Atlantico"
      },
      {
        (object) "08",
        (object) "Caqueta"
      },
      {
        (object) "09",
        (object) "Cauca"
      },
      {
        (object) "10",
        (object) "Cesar"
      },
      {
        (object) "11",
        (object) "Choco"
      },
      {
        (object) "12",
        (object) "Cordoba"
      },
      {
        (object) "14",
        (object) "Guaviare"
      },
      {
        (object) "15",
        (object) "Guainia"
      },
      {
        (object) "16",
        (object) "Huila"
      },
      {
        (object) "17",
        (object) "La Guajira"
      },
      {
        (object) "19",
        (object) "Meta"
      },
      {
        (object) "20",
        (object) "Narino"
      },
      {
        (object) "21",
        (object) "Norte de Santander"
      },
      {
        (object) "22",
        (object) "Putumayo"
      },
      {
        (object) "23",
        (object) "Quindio"
      },
      {
        (object) "24",
        (object) "Risaralda"
      },
      {
        (object) "25",
        (object) "San Andres y Providencia"
      },
      {
        (object) "26",
        (object) "Santander"
      },
      {
        (object) "27",
        (object) "Sucre"
      },
      {
        (object) "28",
        (object) "Tolima"
      },
      {
        (object) "29",
        (object) "Valle del Cauca"
      },
      {
        (object) "30",
        (object) "Vaupes"
      },
      {
        (object) "31",
        (object) "Vichada"
      },
      {
        (object) "32",
        (object) "Casanare"
      },
      {
        (object) "33",
        (object) "Cundinamarca"
      },
      {
        (object) "34",
        (object) "Distrito Especial"
      },
      {
        (object) "35",
        (object) "Bolivar"
      },
      {
        (object) "36",
        (object) "Boyaca"
      },
      {
        (object) "37",
        (object) "Caldas"
      },
      {
        (object) "38",
        (object) "Magdalena"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CR", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Alajuela"
      },
      {
        (object) "02",
        (object) "Cartago"
      },
      {
        (object) "03",
        (object) "Guanacaste"
      },
      {
        (object) "04",
        (object) "Heredia"
      },
      {
        (object) "06",
        (object) "Limon"
      },
      {
        (object) "07",
        (object) "Puntarenas"
      },
      {
        (object) "08",
        (object) "San Jose"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CU", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Pinar del Rio"
      },
      {
        (object) "02",
        (object) "Ciudad de la Habana"
      },
      {
        (object) "03",
        (object) "Matanzas"
      },
      {
        (object) "04",
        (object) "Isla de la Juventud"
      },
      {
        (object) "05",
        (object) "Camaguey"
      },
      {
        (object) "07",
        (object) "Ciego de Avila"
      },
      {
        (object) "08",
        (object) "Cienfuegos"
      },
      {
        (object) "09",
        (object) "Granma"
      },
      {
        (object) "10",
        (object) "Guantanamo"
      },
      {
        (object) "11",
        (object) "La Habana"
      },
      {
        (object) "12",
        (object) "Holguin"
      },
      {
        (object) "13",
        (object) "Las Tunas"
      },
      {
        (object) "14",
        (object) "Sancti Spiritus"
      },
      {
        (object) "15",
        (object) "Santiago de Cuba"
      },
      {
        (object) "16",
        (object) "Villa Clara"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CV", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Boa Vista"
      },
      {
        (object) "02",
        (object) "Brava"
      },
      {
        (object) "04",
        (object) "Maio"
      },
      {
        (object) "05",
        (object) "Paul"
      },
      {
        (object) "07",
        (object) "Ribeira Grande"
      },
      {
        (object) "08",
        (object) "Sal"
      },
      {
        (object) "10",
        (object) "Sao Nicolau"
      },
      {
        (object) "11",
        (object) "Sao Vicente"
      },
      {
        (object) "13",
        (object) "Mosteiros"
      },
      {
        (object) "14",
        (object) "Praia"
      },
      {
        (object) "15",
        (object) "Santa Catarina"
      },
      {
        (object) "16",
        (object) "Santa Cruz"
      },
      {
        (object) "17",
        (object) "Sao Domingos"
      },
      {
        (object) "18",
        (object) "Sao Filipe"
      },
      {
        (object) "19",
        (object) "Sao Miguel"
      },
      {
        (object) "20",
        (object) "Tarrafal"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CY", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Famagusta"
      },
      {
        (object) "02",
        (object) "Kyrenia"
      },
      {
        (object) "03",
        (object) "Larnaca"
      },
      {
        (object) "04",
        (object) "Nicosia"
      },
      {
        (object) "05",
        (object) "Limassol"
      },
      {
        (object) "06",
        (object) "Paphos"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "CZ", (object) new Hashtable()
    {
      {
        (object) "52",
        (object) "Hlavni mesto Praha"
      },
      {
        (object) "78",
        (object) "Jihomoravsky kraj"
      },
      {
        (object) "79",
        (object) "Jihocesky kraj"
      },
      {
        (object) "80",
        (object) "Vysocina"
      },
      {
        (object) "81",
        (object) "Karlovarsky kraj"
      },
      {
        (object) "82",
        (object) "Kralovehradecky kraj"
      },
      {
        (object) "83",
        (object) "Liberecky kraj"
      },
      {
        (object) "84",
        (object) "Olomoucky kraj"
      },
      {
        (object) "85",
        (object) "Moravskoslezsky kraj"
      },
      {
        (object) "86",
        (object) "Pardubicky kraj"
      },
      {
        (object) "87",
        (object) "Plzensky kraj"
      },
      {
        (object) "88",
        (object) "Stredocesky kraj"
      },
      {
        (object) "89",
        (object) "Ustecky kraj"
      },
      {
        (object) "90",
        (object) "Zlinsky kraj"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "DE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Baden-Wurttemberg"
      },
      {
        (object) "02",
        (object) "Bayern"
      },
      {
        (object) "03",
        (object) "Bremen"
      },
      {
        (object) "04",
        (object) "Hamburg"
      },
      {
        (object) "05",
        (object) "Hessen"
      },
      {
        (object) "06",
        (object) "Niedersachsen"
      },
      {
        (object) "07",
        (object) "Nordrhein-Westfalen"
      },
      {
        (object) "08",
        (object) "Rheinland-Pfalz"
      },
      {
        (object) "09",
        (object) "Saarland"
      },
      {
        (object) "10",
        (object) "Schleswig-Holstein"
      },
      {
        (object) "11",
        (object) "Brandenburg"
      },
      {
        (object) "12",
        (object) "Mecklenburg-Vorpommern"
      },
      {
        (object) "13",
        (object) "Sachsen"
      },
      {
        (object) "14",
        (object) "Sachsen-Anhalt"
      },
      {
        (object) "15",
        (object) "Thuringen"
      },
      {
        (object) "16",
        (object) "Berlin"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "DJ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Ali Sabieh"
      },
      {
        (object) "04",
        (object) "Obock"
      },
      {
        (object) "05",
        (object) "Tadjoura"
      },
      {
        (object) "06",
        (object) "Dikhil"
      },
      {
        (object) "07",
        (object) "Djibouti"
      },
      {
        (object) "08",
        (object) "Arta"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "DK", (object) new Hashtable()
    {
      {
        (object) "17",
        (object) "Hovedstaden"
      },
      {
        (object) "18",
        (object) "Midtjylland"
      },
      {
        (object) "19",
        (object) "Nordjylland"
      },
      {
        (object) "20",
        (object) "Sjelland"
      },
      {
        (object) "21",
        (object) "Syddanmark"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "DM", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Saint Andrew"
      },
      {
        (object) "03",
        (object) "Saint David"
      },
      {
        (object) "04",
        (object) "Saint George"
      },
      {
        (object) "05",
        (object) "Saint John"
      },
      {
        (object) "06",
        (object) "Saint Joseph"
      },
      {
        (object) "07",
        (object) "Saint Luke"
      },
      {
        (object) "08",
        (object) "Saint Mark"
      },
      {
        (object) "09",
        (object) "Saint Patrick"
      },
      {
        (object) "10",
        (object) "Saint Paul"
      },
      {
        (object) "11",
        (object) "Saint Peter"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "DO", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Azua"
      },
      {
        (object) "02",
        (object) "Baoruco"
      },
      {
        (object) "03",
        (object) "Barahona"
      },
      {
        (object) "04",
        (object) "Dajabon"
      },
      {
        (object) "05",
        (object) "Distrito Nacional"
      },
      {
        (object) "06",
        (object) "Duarte"
      },
      {
        (object) "08",
        (object) "Espaillat"
      },
      {
        (object) "09",
        (object) "Independencia"
      },
      {
        (object) "10",
        (object) "La Altagracia"
      },
      {
        (object) "11",
        (object) "Elias Pina"
      },
      {
        (object) "12",
        (object) "La Romana"
      },
      {
        (object) "14",
        (object) "Maria Trinidad Sanchez"
      },
      {
        (object) "15",
        (object) "Monte Cristi"
      },
      {
        (object) "16",
        (object) "Pedernales"
      },
      {
        (object) "17",
        (object) "Peravia"
      },
      {
        (object) "18",
        (object) "Puerto Plata"
      },
      {
        (object) "19",
        (object) "Salcedo"
      },
      {
        (object) "20",
        (object) "Samana"
      },
      {
        (object) "21",
        (object) "Sanchez Ramirez"
      },
      {
        (object) "23",
        (object) "San Juan"
      },
      {
        (object) "24",
        (object) "San Pedro De Macoris"
      },
      {
        (object) "25",
        (object) "Santiago"
      },
      {
        (object) "26",
        (object) "Santiago Rodriguez"
      },
      {
        (object) "27",
        (object) "Valverde"
      },
      {
        (object) "28",
        (object) "El Seibo"
      },
      {
        (object) "29",
        (object) "Hato Mayor"
      },
      {
        (object) "30",
        (object) "La Vega"
      },
      {
        (object) "31",
        (object) "Monsenor Nouel"
      },
      {
        (object) "32",
        (object) "Monte Plata"
      },
      {
        (object) "33",
        (object) "San Cristobal"
      },
      {
        (object) "34",
        (object) "Distrito Nacional"
      },
      {
        (object) "35",
        (object) "Peravia"
      },
      {
        (object) "36",
        (object) "San Jose de Ocoa"
      },
      {
        (object) "37",
        (object) "Santo Domingo"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "DZ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Alger"
      },
      {
        (object) "03",
        (object) "Batna"
      },
      {
        (object) "04",
        (object) "Constantine"
      },
      {
        (object) "06",
        (object) "Medea"
      },
      {
        (object) "07",
        (object) "Mostaganem"
      },
      {
        (object) "09",
        (object) "Oran"
      },
      {
        (object) "10",
        (object) "Saida"
      },
      {
        (object) "12",
        (object) "Setif"
      },
      {
        (object) "13",
        (object) "Tiaret"
      },
      {
        (object) "14",
        (object) "Tizi Ouzou"
      },
      {
        (object) "15",
        (object) "Tlemcen"
      },
      {
        (object) "18",
        (object) "Bejaia"
      },
      {
        (object) "19",
        (object) "Biskra"
      },
      {
        (object) "20",
        (object) "Blida"
      },
      {
        (object) "21",
        (object) "Bouira"
      },
      {
        (object) "22",
        (object) "Djelfa"
      },
      {
        (object) "23",
        (object) "Guelma"
      },
      {
        (object) "24",
        (object) "Jijel"
      },
      {
        (object) "25",
        (object) "Laghouat"
      },
      {
        (object) "26",
        (object) "Mascara"
      },
      {
        (object) "27",
        (object) "M'sila"
      },
      {
        (object) "29",
        (object) "Oum el Bouaghi"
      },
      {
        (object) "30",
        (object) "Sidi Bel Abbes"
      },
      {
        (object) "31",
        (object) "Skikda"
      },
      {
        (object) "33",
        (object) "Tebessa"
      },
      {
        (object) "34",
        (object) "Adrar"
      },
      {
        (object) "35",
        (object) "Ain Defla"
      },
      {
        (object) "36",
        (object) "Ain Temouchent"
      },
      {
        (object) "37",
        (object) "Annaba"
      },
      {
        (object) "38",
        (object) "Bechar"
      },
      {
        (object) "39",
        (object) "Bordj Bou Arreridj"
      },
      {
        (object) "40",
        (object) "Boumerdes"
      },
      {
        (object) "41",
        (object) "Chlef"
      },
      {
        (object) "42",
        (object) "El Bayadh"
      },
      {
        (object) "43",
        (object) "El Oued"
      },
      {
        (object) "44",
        (object) "El Tarf"
      },
      {
        (object) "45",
        (object) "Ghardaia"
      },
      {
        (object) "46",
        (object) "Illizi"
      },
      {
        (object) "47",
        (object) "Khenchela"
      },
      {
        (object) "48",
        (object) "Mila"
      },
      {
        (object) "49",
        (object) "Naama"
      },
      {
        (object) "50",
        (object) "Ouargla"
      },
      {
        (object) "51",
        (object) "Relizane"
      },
      {
        (object) "52",
        (object) "Souk Ahras"
      },
      {
        (object) "53",
        (object) "Tamanghasset"
      },
      {
        (object) "54",
        (object) "Tindouf"
      },
      {
        (object) "55",
        (object) "Tipaza"
      },
      {
        (object) "56",
        (object) "Tissemsilt"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "EC", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Galapagos"
      },
      {
        (object) "02",
        (object) "Azuay"
      },
      {
        (object) "03",
        (object) "Bolivar"
      },
      {
        (object) "04",
        (object) "Canar"
      },
      {
        (object) "05",
        (object) "Carchi"
      },
      {
        (object) "06",
        (object) "Chimborazo"
      },
      {
        (object) "07",
        (object) "Cotopaxi"
      },
      {
        (object) "08",
        (object) "El Oro"
      },
      {
        (object) "09",
        (object) "Esmeraldas"
      },
      {
        (object) "10",
        (object) "Guayas"
      },
      {
        (object) "11",
        (object) "Imbabura"
      },
      {
        (object) "12",
        (object) "Loja"
      },
      {
        (object) "13",
        (object) "Los Rios"
      },
      {
        (object) "14",
        (object) "Manabi"
      },
      {
        (object) "15",
        (object) "Morona-Santiago"
      },
      {
        (object) "17",
        (object) "Pastaza"
      },
      {
        (object) "18",
        (object) "Pichincha"
      },
      {
        (object) "19",
        (object) "Tungurahua"
      },
      {
        (object) "20",
        (object) "Zamora-Chinchipe"
      },
      {
        (object) "22",
        (object) "Sucumbios"
      },
      {
        (object) "23",
        (object) "Napo"
      },
      {
        (object) "24",
        (object) "Orellana"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "EE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Harjumaa"
      },
      {
        (object) "02",
        (object) "Hiiumaa"
      },
      {
        (object) "03",
        (object) "Ida-Virumaa"
      },
      {
        (object) "04",
        (object) "Jarvamaa"
      },
      {
        (object) "05",
        (object) "Jogevamaa"
      },
      {
        (object) "06",
        (object) "Kohtla-Jarve"
      },
      {
        (object) "07",
        (object) "Laanemaa"
      },
      {
        (object) "08",
        (object) "Laane-Virumaa"
      },
      {
        (object) "09",
        (object) "Narva"
      },
      {
        (object) "10",
        (object) "Parnu"
      },
      {
        (object) "11",
        (object) "Parnumaa"
      },
      {
        (object) "12",
        (object) "Polvamaa"
      },
      {
        (object) "13",
        (object) "Raplamaa"
      },
      {
        (object) "14",
        (object) "Saaremaa"
      },
      {
        (object) "15",
        (object) "Sillamae"
      },
      {
        (object) "16",
        (object) "Tallinn"
      },
      {
        (object) "17",
        (object) "Tartu"
      },
      {
        (object) "18",
        (object) "Tartumaa"
      },
      {
        (object) "19",
        (object) "Valgamaa"
      },
      {
        (object) "20",
        (object) "Viljandimaa"
      },
      {
        (object) "21",
        (object) "Vorumaa"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "EG", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Ad Daqahliyah"
      },
      {
        (object) "02",
        (object) "Al Bahr al Ahmar"
      },
      {
        (object) "03",
        (object) "Al Buhayrah"
      },
      {
        (object) "04",
        (object) "Al Fayyum"
      },
      {
        (object) "05",
        (object) "Al Gharbiyah"
      },
      {
        (object) "06",
        (object) "Al Iskandariyah"
      },
      {
        (object) "07",
        (object) "Al Isma'iliyah"
      },
      {
        (object) "08",
        (object) "Al Jizah"
      },
      {
        (object) "09",
        (object) "Al Minufiyah"
      },
      {
        (object) "10",
        (object) "Al Minya"
      },
      {
        (object) "11",
        (object) "Al Qahirah"
      },
      {
        (object) "12",
        (object) "Al Qalyubiyah"
      },
      {
        (object) "13",
        (object) "Al Wadi al Jadid"
      },
      {
        (object) "14",
        (object) "Ash Sharqiyah"
      },
      {
        (object) "15",
        (object) "As Suways"
      },
      {
        (object) "16",
        (object) "Aswan"
      },
      {
        (object) "17",
        (object) "Asyut"
      },
      {
        (object) "18",
        (object) "Bani Suwayf"
      },
      {
        (object) "19",
        (object) "Bur Sa'id"
      },
      {
        (object) "20",
        (object) "Dumyat"
      },
      {
        (object) "21",
        (object) "Kafr ash Shaykh"
      },
      {
        (object) "22",
        (object) "Matruh"
      },
      {
        (object) "23",
        (object) "Qina"
      },
      {
        (object) "24",
        (object) "Suhaj"
      },
      {
        (object) "26",
        (object) "Janub Sina'"
      },
      {
        (object) "27",
        (object) "Shamal Sina'"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "ER", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Anseba"
      },
      {
        (object) "02",
        (object) "Debub"
      },
      {
        (object) "03",
        (object) "Debubawi K'eyih Bahri"
      },
      {
        (object) "04",
        (object) "Gash Barka"
      },
      {
        (object) "05",
        (object) "Ma'akel"
      },
      {
        (object) "06",
        (object) "Semenawi K'eyih Bahri"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "ES", (object) new Hashtable()
    {
      {
        (object) "07",
        (object) "Islas Baleares"
      },
      {
        (object) "27",
        (object) "La Rioja"
      },
      {
        (object) "29",
        (object) "Madrid"
      },
      {
        (object) "31",
        (object) "Murcia"
      },
      {
        (object) "32",
        (object) "Navarra"
      },
      {
        (object) "34",
        (object) "Asturias"
      },
      {
        (object) "39",
        (object) "Cantabria"
      },
      {
        (object) "51",
        (object) "Andalucia"
      },
      {
        (object) "52",
        (object) "Aragon"
      },
      {
        (object) "53",
        (object) "Canarias"
      },
      {
        (object) "54",
        (object) "Castilla-La Mancha"
      },
      {
        (object) "55",
        (object) "Castilla y Leon"
      },
      {
        (object) "56",
        (object) "Catalonia"
      },
      {
        (object) "57",
        (object) "Extremadura"
      },
      {
        (object) "58",
        (object) "Galicia"
      },
      {
        (object) "59",
        (object) "Pais Vasco"
      },
      {
        (object) "60",
        (object) "Comunidad Valenciana"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "ET", (object) new Hashtable()
    {
      {
        (object) "44",
        (object) "Adis Abeba"
      },
      {
        (object) "45",
        (object) "Afar"
      },
      {
        (object) "46",
        (object) "Amara"
      },
      {
        (object) "47",
        (object) "Binshangul Gumuz"
      },
      {
        (object) "48",
        (object) "Dire Dawa"
      },
      {
        (object) "49",
        (object) "Gambela Hizboch"
      },
      {
        (object) "50",
        (object) "Hareri Hizb"
      },
      {
        (object) "51",
        (object) "Oromiya"
      },
      {
        (object) "52",
        (object) "Sumale"
      },
      {
        (object) "53",
        (object) "Tigray"
      },
      {
        (object) "54",
        (object) "YeDebub Biheroch Bihereseboch na Hizboch"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "FI", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Aland"
      },
      {
        (object) "06",
        (object) "Lapland"
      },
      {
        (object) "08",
        (object) "Oulu"
      },
      {
        (object) "13",
        (object) "Southern Finland"
      },
      {
        (object) "14",
        (object) "Eastern Finland"
      },
      {
        (object) "15",
        (object) "Western Finland"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "FJ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Central"
      },
      {
        (object) "02",
        (object) "Eastern"
      },
      {
        (object) "03",
        (object) "Northern"
      },
      {
        (object) "04",
        (object) "Rotuma"
      },
      {
        (object) "05",
        (object) "Western"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "FM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Kosrae"
      },
      {
        (object) "02",
        (object) "Pohnpei"
      },
      {
        (object) "03",
        (object) "Chuuk"
      },
      {
        (object) "04",
        (object) "Yap"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "FR", (object) new Hashtable()
    {
      {
        (object) "97",
        (object) "Aquitaine"
      },
      {
        (object) "98",
        (object) "Auvergne"
      },
      {
        (object) "99",
        (object) "Basse-Normandie"
      },
      {
        (object) "A1",
        (object) "Bourgogne"
      },
      {
        (object) "A2",
        (object) "Bretagne"
      },
      {
        (object) "A3",
        (object) "Centre"
      },
      {
        (object) "A4",
        (object) "Champagne-Ardenne"
      },
      {
        (object) "A5",
        (object) "Corse"
      },
      {
        (object) "A6",
        (object) "Franche-Comte"
      },
      {
        (object) "A7",
        (object) "Haute-Normandie"
      },
      {
        (object) "A8",
        (object) "Ile-de-France"
      },
      {
        (object) "A9",
        (object) "Languedoc-Roussillon"
      },
      {
        (object) "B1",
        (object) "Limousin"
      },
      {
        (object) "B2",
        (object) "Lorraine"
      },
      {
        (object) "B3",
        (object) "Midi-Pyrenees"
      },
      {
        (object) "B4",
        (object) "Nord-Pas-de-Calais"
      },
      {
        (object) "B5",
        (object) "Pays de la Loire"
      },
      {
        (object) "B6",
        (object) "Picardie"
      },
      {
        (object) "B7",
        (object) "Poitou-Charentes"
      },
      {
        (object) "B8",
        (object) "Provence-Alpes-Cote d'Azur"
      },
      {
        (object) "B9",
        (object) "Rhone-Alpes"
      },
      {
        (object) "C1",
        (object) "Alsace"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GA", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Estuaire"
      },
      {
        (object) "02",
        (object) "Haut-Ogooue"
      },
      {
        (object) "03",
        (object) "Moyen-Ogooue"
      },
      {
        (object) "04",
        (object) "Ngounie"
      },
      {
        (object) "05",
        (object) "Nyanga"
      },
      {
        (object) "06",
        (object) "Ogooue-Ivindo"
      },
      {
        (object) "07",
        (object) "Ogooue-Lolo"
      },
      {
        (object) "08",
        (object) "Ogooue-Maritime"
      },
      {
        (object) "09",
        (object) "Woleu-Ntem"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GB", (object) new Hashtable()
    {
      {
        (object) "A1",
        (object) "Barking and Dagenham"
      },
      {
        (object) "A2",
        (object) "Barnet"
      },
      {
        (object) "A3",
        (object) "Barnsley"
      },
      {
        (object) "A4",
        (object) "Bath and North East Somerset"
      },
      {
        (object) "A5",
        (object) "Bedfordshire"
      },
      {
        (object) "A6",
        (object) "Bexley"
      },
      {
        (object) "A7",
        (object) "Birmingham"
      },
      {
        (object) "A8",
        (object) "Blackburn with Darwen"
      },
      {
        (object) "A9",
        (object) "Blackpool"
      },
      {
        (object) "B1",
        (object) "Bolton"
      },
      {
        (object) "B2",
        (object) "Bournemouth"
      },
      {
        (object) "B3",
        (object) "Bracknell Forest"
      },
      {
        (object) "B4",
        (object) "Bradford"
      },
      {
        (object) "B5",
        (object) "Brent"
      },
      {
        (object) "B6",
        (object) "Brighton and Hove"
      },
      {
        (object) "B7",
        (object) "Bristol, City of"
      },
      {
        (object) "B8",
        (object) "Bromley"
      },
      {
        (object) "B9",
        (object) "Buckinghamshire"
      },
      {
        (object) "C1",
        (object) "Bury"
      },
      {
        (object) "C2",
        (object) "Calderdale"
      },
      {
        (object) "C3",
        (object) "Cambridgeshire"
      },
      {
        (object) "C4",
        (object) "Camden"
      },
      {
        (object) "C5",
        (object) "Cheshire"
      },
      {
        (object) "C6",
        (object) "Cornwall"
      },
      {
        (object) "C7",
        (object) "Coventry"
      },
      {
        (object) "C8",
        (object) "Croydon"
      },
      {
        (object) "C9",
        (object) "Cumbria"
      },
      {
        (object) "D1",
        (object) "Darlington"
      },
      {
        (object) "D2",
        (object) "Derby"
      },
      {
        (object) "D3",
        (object) "Derbyshire"
      },
      {
        (object) "D4",
        (object) "Devon"
      },
      {
        (object) "D5",
        (object) "Doncaster"
      },
      {
        (object) "D6",
        (object) "Dorset"
      },
      {
        (object) "D7",
        (object) "Dudley"
      },
      {
        (object) "D8",
        (object) "Durham"
      },
      {
        (object) "D9",
        (object) "Ealing"
      },
      {
        (object) "E1",
        (object) "East Riding of Yorkshire"
      },
      {
        (object) "E2",
        (object) "East Sussex"
      },
      {
        (object) "E3",
        (object) "Enfield"
      },
      {
        (object) "E4",
        (object) "Essex"
      },
      {
        (object) "E5",
        (object) "Gateshead"
      },
      {
        (object) "E6",
        (object) "Gloucestershire"
      },
      {
        (object) "E7",
        (object) "Greenwich"
      },
      {
        (object) "E8",
        (object) "Hackney"
      },
      {
        (object) "E9",
        (object) "Halton"
      },
      {
        (object) "F1",
        (object) "Hammersmith and Fulham"
      },
      {
        (object) "F2",
        (object) "Hampshire"
      },
      {
        (object) "F3",
        (object) "Haringey"
      },
      {
        (object) "F4",
        (object) "Harrow"
      },
      {
        (object) "F5",
        (object) "Hartlepool"
      },
      {
        (object) "F6",
        (object) "Havering"
      },
      {
        (object) "F7",
        (object) "Herefordshire"
      },
      {
        (object) "F8",
        (object) "Hertford"
      },
      {
        (object) "F9",
        (object) "Hillingdon"
      },
      {
        (object) "G1",
        (object) "Hounslow"
      },
      {
        (object) "G2",
        (object) "Isle of Wight"
      },
      {
        (object) "G3",
        (object) "Islington"
      },
      {
        (object) "G4",
        (object) "Kensington and Chelsea"
      },
      {
        (object) "G5",
        (object) "Kent"
      },
      {
        (object) "G6",
        (object) "Kingston upon Hull, City of"
      },
      {
        (object) "G7",
        (object) "Kingston upon Thames"
      },
      {
        (object) "G8",
        (object) "Kirklees"
      },
      {
        (object) "G9",
        (object) "Knowsley"
      },
      {
        (object) "H1",
        (object) "Lambeth"
      },
      {
        (object) "H2",
        (object) "Lancashire"
      },
      {
        (object) "H3",
        (object) "Leeds"
      },
      {
        (object) "H4",
        (object) "Leicester"
      },
      {
        (object) "H5",
        (object) "Leicestershire"
      },
      {
        (object) "H6",
        (object) "Lewisham"
      },
      {
        (object) "H7",
        (object) "Lincolnshire"
      },
      {
        (object) "H8",
        (object) "Liverpool"
      },
      {
        (object) "H9",
        (object) "London, City of"
      },
      {
        (object) "I1",
        (object) "Luton"
      },
      {
        (object) "I2",
        (object) "Manchester"
      },
      {
        (object) "I3",
        (object) "Medway"
      },
      {
        (object) "I4",
        (object) "Merton"
      },
      {
        (object) "I5",
        (object) "Middlesbrough"
      },
      {
        (object) "I6",
        (object) "Milton Keynes"
      },
      {
        (object) "I7",
        (object) "Newcastle upon Tyne"
      },
      {
        (object) "I8",
        (object) "Newham"
      },
      {
        (object) "I9",
        (object) "Norfolk"
      },
      {
        (object) "J1",
        (object) "Northamptonshire"
      },
      {
        (object) "J2",
        (object) "North East Lincolnshire"
      },
      {
        (object) "J3",
        (object) "North Lincolnshire"
      },
      {
        (object) "J4",
        (object) "North Somerset"
      },
      {
        (object) "J5",
        (object) "North Tyneside"
      },
      {
        (object) "J6",
        (object) "Northumberland"
      },
      {
        (object) "J7",
        (object) "North Yorkshire"
      },
      {
        (object) "J8",
        (object) "Nottingham"
      },
      {
        (object) "J9",
        (object) "Nottinghamshire"
      },
      {
        (object) "K1",
        (object) "Oldham"
      },
      {
        (object) "K2",
        (object) "Oxfordshire"
      },
      {
        (object) "K3",
        (object) "Peterborough"
      },
      {
        (object) "K4",
        (object) "Plymouth"
      },
      {
        (object) "K5",
        (object) "Poole"
      },
      {
        (object) "K6",
        (object) "Portsmouth"
      },
      {
        (object) "K7",
        (object) "Reading"
      },
      {
        (object) "K8",
        (object) "Redbridge"
      },
      {
        (object) "K9",
        (object) "Redcar and Cleveland"
      },
      {
        (object) "L1",
        (object) "Richmond upon Thames"
      },
      {
        (object) "L2",
        (object) "Rochdale"
      },
      {
        (object) "L3",
        (object) "Rotherham"
      },
      {
        (object) "L4",
        (object) "Rutland"
      },
      {
        (object) "L5",
        (object) "Salford"
      },
      {
        (object) "L6",
        (object) "Shropshire"
      },
      {
        (object) "L7",
        (object) "Sandwell"
      },
      {
        (object) "L8",
        (object) "Sefton"
      },
      {
        (object) "L9",
        (object) "Sheffield"
      },
      {
        (object) "M1",
        (object) "Slough"
      },
      {
        (object) "M2",
        (object) "Solihull"
      },
      {
        (object) "M3",
        (object) "Somerset"
      },
      {
        (object) "M4",
        (object) "Southampton"
      },
      {
        (object) "M5",
        (object) "Southend-on-Sea"
      },
      {
        (object) "M6",
        (object) "South Gloucestershire"
      },
      {
        (object) "M7",
        (object) "South Tyneside"
      },
      {
        (object) "M8",
        (object) "Southwark"
      },
      {
        (object) "M9",
        (object) "Staffordshire"
      },
      {
        (object) "N1",
        (object) "St. Helens"
      },
      {
        (object) "N2",
        (object) "Stockport"
      },
      {
        (object) "N3",
        (object) "Stockton-on-Tees"
      },
      {
        (object) "N4",
        (object) "Stoke-on-Trent"
      },
      {
        (object) "N5",
        (object) "Suffolk"
      },
      {
        (object) "N6",
        (object) "Sunderland"
      },
      {
        (object) "N7",
        (object) "Surrey"
      },
      {
        (object) "N8",
        (object) "Sutton"
      },
      {
        (object) "N9",
        (object) "Swindon"
      },
      {
        (object) "O1",
        (object) "Tameside"
      },
      {
        (object) "O2",
        (object) "Telford and Wrekin"
      },
      {
        (object) "O3",
        (object) "Thurrock"
      },
      {
        (object) "O4",
        (object) "Torbay"
      },
      {
        (object) "O5",
        (object) "Tower Hamlets"
      },
      {
        (object) "O6",
        (object) "Trafford"
      },
      {
        (object) "O7",
        (object) "Wakefield"
      },
      {
        (object) "O8",
        (object) "Walsall"
      },
      {
        (object) "O9",
        (object) "Waltham Forest"
      },
      {
        (object) "P1",
        (object) "Wandsworth"
      },
      {
        (object) "P2",
        (object) "Warrington"
      },
      {
        (object) "P3",
        (object) "Warwickshire"
      },
      {
        (object) "P4",
        (object) "West Berkshire"
      },
      {
        (object) "P5",
        (object) "Westminster"
      },
      {
        (object) "P6",
        (object) "West Sussex"
      },
      {
        (object) "P7",
        (object) "Wigan"
      },
      {
        (object) "P8",
        (object) "Wiltshire"
      },
      {
        (object) "P9",
        (object) "Windsor and Maidenhead"
      },
      {
        (object) "Q1",
        (object) "Wirral"
      },
      {
        (object) "Q2",
        (object) "Wokingham"
      },
      {
        (object) "Q3",
        (object) "Wolverhampton"
      },
      {
        (object) "Q4",
        (object) "Worcestershire"
      },
      {
        (object) "Q5",
        (object) "York"
      },
      {
        (object) "Q6",
        (object) "Antrim"
      },
      {
        (object) "Q7",
        (object) "Ards"
      },
      {
        (object) "Q8",
        (object) "Armagh"
      },
      {
        (object) "Q9",
        (object) "Ballymena"
      },
      {
        (object) "R1",
        (object) "Ballymoney"
      },
      {
        (object) "R2",
        (object) "Banbridge"
      },
      {
        (object) "R3",
        (object) "Belfast"
      },
      {
        (object) "R4",
        (object) "Carrickfergus"
      },
      {
        (object) "R5",
        (object) "Castlereagh"
      },
      {
        (object) "R6",
        (object) "Coleraine"
      },
      {
        (object) "R7",
        (object) "Cookstown"
      },
      {
        (object) "R8",
        (object) "Craigavon"
      },
      {
        (object) "R9",
        (object) "Down"
      },
      {
        (object) "S1",
        (object) "Dungannon"
      },
      {
        (object) "S2",
        (object) "Fermanagh"
      },
      {
        (object) "S3",
        (object) "Larne"
      },
      {
        (object) "S4",
        (object) "Limavady"
      },
      {
        (object) "S5",
        (object) "Lisburn"
      },
      {
        (object) "S6",
        (object) "Derry"
      },
      {
        (object) "S7",
        (object) "Magherafelt"
      },
      {
        (object) "S8",
        (object) "Moyle"
      },
      {
        (object) "S9",
        (object) "Newry and Mourne"
      },
      {
        (object) "T1",
        (object) "Newtownabbey"
      },
      {
        (object) "T2",
        (object) "North Down"
      },
      {
        (object) "T3",
        (object) "Omagh"
      },
      {
        (object) "T4",
        (object) "Strabane"
      },
      {
        (object) "T5",
        (object) "Aberdeen City"
      },
      {
        (object) "T6",
        (object) "Aberdeenshire"
      },
      {
        (object) "T7",
        (object) "Angus"
      },
      {
        (object) "T8",
        (object) "Argyll and Bute"
      },
      {
        (object) "T9",
        (object) "Scottish Borders, The"
      },
      {
        (object) "U1",
        (object) "Clackmannanshire"
      },
      {
        (object) "U2",
        (object) "Dumfries and Galloway"
      },
      {
        (object) "U3",
        (object) "Dundee City"
      },
      {
        (object) "U4",
        (object) "East Ayrshire"
      },
      {
        (object) "U5",
        (object) "East Dunbartonshire"
      },
      {
        (object) "U6",
        (object) "East Lothian"
      },
      {
        (object) "U7",
        (object) "East Renfrewshire"
      },
      {
        (object) "U8",
        (object) "Edinburgh, City of"
      },
      {
        (object) "U9",
        (object) "Falkirk"
      },
      {
        (object) "V1",
        (object) "Fife"
      },
      {
        (object) "V2",
        (object) "Glasgow City"
      },
      {
        (object) "V3",
        (object) "Highland"
      },
      {
        (object) "V4",
        (object) "Inverclyde"
      },
      {
        (object) "V5",
        (object) "Midlothian"
      },
      {
        (object) "V6",
        (object) "Moray"
      },
      {
        (object) "V7",
        (object) "North Ayrshire"
      },
      {
        (object) "V8",
        (object) "North Lanarkshire"
      },
      {
        (object) "V9",
        (object) "Orkney"
      },
      {
        (object) "W1",
        (object) "Perth and Kinross"
      },
      {
        (object) "W2",
        (object) "Renfrewshire"
      },
      {
        (object) "W3",
        (object) "Shetland Islands"
      },
      {
        (object) "W4",
        (object) "South Ayrshire"
      },
      {
        (object) "W5",
        (object) "South Lanarkshire"
      },
      {
        (object) "W6",
        (object) "Stirling"
      },
      {
        (object) "W7",
        (object) "West Dunbartonshire"
      },
      {
        (object) "W8",
        (object) "Eilean Siar"
      },
      {
        (object) "W9",
        (object) "West Lothian"
      },
      {
        (object) "X1",
        (object) "Isle of Anglesey"
      },
      {
        (object) "X2",
        (object) "Blaenau Gwent"
      },
      {
        (object) "X3",
        (object) "Bridgend"
      },
      {
        (object) "X4",
        (object) "Caerphilly"
      },
      {
        (object) "X5",
        (object) "Cardiff"
      },
      {
        (object) "X6",
        (object) "Ceredigion"
      },
      {
        (object) "X7",
        (object) "Carmarthenshire"
      },
      {
        (object) "X8",
        (object) "Conwy"
      },
      {
        (object) "X9",
        (object) "Denbighshire"
      },
      {
        (object) "Y1",
        (object) "Flintshire"
      },
      {
        (object) "Y2",
        (object) "Gwynedd"
      },
      {
        (object) "Y3",
        (object) "Merthyr Tydfil"
      },
      {
        (object) "Y4",
        (object) "Monmouthshire"
      },
      {
        (object) "Y5",
        (object) "Neath Port Talbot"
      },
      {
        (object) "Y6",
        (object) "Newport"
      },
      {
        (object) "Y7",
        (object) "Pembrokeshire"
      },
      {
        (object) "Y8",
        (object) "Powys"
      },
      {
        (object) "Y9",
        (object) "Rhondda Cynon Taff"
      },
      {
        (object) "Z1",
        (object) "Swansea"
      },
      {
        (object) "Z2",
        (object) "Torfaen"
      },
      {
        (object) "Z3",
        (object) "Vale of Glamorgan, The"
      },
      {
        (object) "Z4",
        (object) "Wrexham"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GD", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Saint Andrew"
      },
      {
        (object) "02",
        (object) "Saint David"
      },
      {
        (object) "03",
        (object) "Saint George"
      },
      {
        (object) "04",
        (object) "Saint John"
      },
      {
        (object) "05",
        (object) "Saint Mark"
      },
      {
        (object) "06",
        (object) "Saint Patrick"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Abashis Raioni"
      },
      {
        (object) "02",
        (object) "Abkhazia"
      },
      {
        (object) "03",
        (object) "Adigenis Raioni"
      },
      {
        (object) "04",
        (object) "Ajaria"
      },
      {
        (object) "05",
        (object) "Akhalgoris Raioni"
      },
      {
        (object) "06",
        (object) "Akhalk'alak'is Raioni"
      },
      {
        (object) "07",
        (object) "Akhalts'ikhis Raioni"
      },
      {
        (object) "08",
        (object) "Akhmetis Raioni"
      },
      {
        (object) "09",
        (object) "Ambrolauris Raioni"
      },
      {
        (object) "10",
        (object) "Aspindzis Raioni"
      },
      {
        (object) "11",
        (object) "Baghdat'is Raioni"
      },
      {
        (object) "12",
        (object) "Bolnisis Raioni"
      },
      {
        (object) "13",
        (object) "Borjomis Raioni"
      },
      {
        (object) "14",
        (object) "Chiat'ura"
      },
      {
        (object) "15",
        (object) "Ch'khorotsqus Raioni"
      },
      {
        (object) "16",
        (object) "Ch'okhatauris Raioni"
      },
      {
        (object) "17",
        (object) "Dedop'listsqaros Raioni"
      },
      {
        (object) "18",
        (object) "Dmanisis Raioni"
      },
      {
        (object) "19",
        (object) "Dushet'is Raioni"
      },
      {
        (object) "20",
        (object) "Gardabanis Raioni"
      },
      {
        (object) "21",
        (object) "Gori"
      },
      {
        (object) "22",
        (object) "Goris Raioni"
      },
      {
        (object) "23",
        (object) "Gurjaanis Raioni"
      },
      {
        (object) "24",
        (object) "Javis Raioni"
      },
      {
        (object) "25",
        (object) "K'arelis Raioni"
      },
      {
        (object) "26",
        (object) "Kaspis Raioni"
      },
      {
        (object) "27",
        (object) "Kharagaulis Raioni"
      },
      {
        (object) "28",
        (object) "Khashuris Raioni"
      },
      {
        (object) "29",
        (object) "Khobis Raioni"
      },
      {
        (object) "30",
        (object) "Khonis Raioni"
      },
      {
        (object) "31",
        (object) "K'ut'aisi"
      },
      {
        (object) "32",
        (object) "Lagodekhis Raioni"
      },
      {
        (object) "33",
        (object) "Lanch'khut'is Raioni"
      },
      {
        (object) "34",
        (object) "Lentekhis Raioni"
      },
      {
        (object) "35",
        (object) "Marneulis Raioni"
      },
      {
        (object) "36",
        (object) "Martvilis Raioni"
      },
      {
        (object) "37",
        (object) "Mestiis Raioni"
      },
      {
        (object) "38",
        (object) "Mts'khet'is Raioni"
      },
      {
        (object) "39",
        (object) "Ninotsmindis Raioni"
      },
      {
        (object) "40",
        (object) "Onis Raioni"
      },
      {
        (object) "41",
        (object) "Ozurget'is Raioni"
      },
      {
        (object) "42",
        (object) "P'ot'i"
      },
      {
        (object) "43",
        (object) "Qazbegis Raioni"
      },
      {
        (object) "44",
        (object) "Qvarlis Raioni"
      },
      {
        (object) "45",
        (object) "Rust'avi"
      },
      {
        (object) "46",
        (object) "Sach'kheris Raioni"
      },
      {
        (object) "47",
        (object) "Sagarejos Raioni"
      },
      {
        (object) "48",
        (object) "Samtrediis Raioni"
      },
      {
        (object) "49",
        (object) "Senakis Raioni"
      },
      {
        (object) "50",
        (object) "Sighnaghis Raioni"
      },
      {
        (object) "51",
        (object) "T'bilisi"
      },
      {
        (object) "52",
        (object) "T'elavis Raioni"
      },
      {
        (object) "53",
        (object) "T'erjolis Raioni"
      },
      {
        (object) "54",
        (object) "T'et'ritsqaros Raioni"
      },
      {
        (object) "55",
        (object) "T'ianet'is Raioni"
      },
      {
        (object) "56",
        (object) "Tqibuli"
      },
      {
        (object) "57",
        (object) "Ts'ageris Raioni"
      },
      {
        (object) "58",
        (object) "Tsalenjikhis Raioni"
      },
      {
        (object) "59",
        (object) "Tsalkis Raioni"
      },
      {
        (object) "60",
        (object) "Tsqaltubo"
      },
      {
        (object) "61",
        (object) "Vanis Raioni"
      },
      {
        (object) "62",
        (object) "Zestap'onis Raioni"
      },
      {
        (object) "63",
        (object) "Zugdidi"
      },
      {
        (object) "64",
        (object) "Zugdidis Raioni"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GH", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Greater Accra"
      },
      {
        (object) "02",
        (object) "Ashanti"
      },
      {
        (object) "03",
        (object) "Brong-Ahafo"
      },
      {
        (object) "04",
        (object) "Central"
      },
      {
        (object) "05",
        (object) "Eastern"
      },
      {
        (object) "06",
        (object) "Northern"
      },
      {
        (object) "08",
        (object) "Volta"
      },
      {
        (object) "09",
        (object) "Western"
      },
      {
        (object) "10",
        (object) "Upper East"
      },
      {
        (object) "11",
        (object) "Upper West"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GL", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Nordgronland"
      },
      {
        (object) "02",
        (object) "Ostgronland"
      },
      {
        (object) "03",
        (object) "Vestgronland"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Banjul"
      },
      {
        (object) "02",
        (object) "Lower River"
      },
      {
        (object) "03",
        (object) "Central River"
      },
      {
        (object) "04",
        (object) "Upper River"
      },
      {
        (object) "05",
        (object) "Western"
      },
      {
        (object) "07",
        (object) "North Bank"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GN", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Beyla"
      },
      {
        (object) "02",
        (object) "Boffa"
      },
      {
        (object) "03",
        (object) "Boke"
      },
      {
        (object) "04",
        (object) "Conakry"
      },
      {
        (object) "05",
        (object) "Dabola"
      },
      {
        (object) "06",
        (object) "Dalaba"
      },
      {
        (object) "07",
        (object) "Dinguiraye"
      },
      {
        (object) "09",
        (object) "Faranah"
      },
      {
        (object) "10",
        (object) "Forecariah"
      },
      {
        (object) "11",
        (object) "Fria"
      },
      {
        (object) "12",
        (object) "Gaoual"
      },
      {
        (object) "13",
        (object) "Gueckedou"
      },
      {
        (object) "15",
        (object) "Kerouane"
      },
      {
        (object) "16",
        (object) "Kindia"
      },
      {
        (object) "17",
        (object) "Kissidougou"
      },
      {
        (object) "18",
        (object) "Koundara"
      },
      {
        (object) "19",
        (object) "Kouroussa"
      },
      {
        (object) "21",
        (object) "Macenta"
      },
      {
        (object) "22",
        (object) "Mali"
      },
      {
        (object) "23",
        (object) "Mamou"
      },
      {
        (object) "25",
        (object) "Pita"
      },
      {
        (object) "27",
        (object) "Telimele"
      },
      {
        (object) "28",
        (object) "Tougue"
      },
      {
        (object) "29",
        (object) "Yomou"
      },
      {
        (object) "30",
        (object) "Coyah"
      },
      {
        (object) "31",
        (object) "Dubreka"
      },
      {
        (object) "32",
        (object) "Kankan"
      },
      {
        (object) "33",
        (object) "Koubia"
      },
      {
        (object) "34",
        (object) "Labe"
      },
      {
        (object) "35",
        (object) "Lelouma"
      },
      {
        (object) "36",
        (object) "Lola"
      },
      {
        (object) "37",
        (object) "Mandiana"
      },
      {
        (object) "38",
        (object) "Nzerekore"
      },
      {
        (object) "39",
        (object) "Siguiri"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GQ", (object) new Hashtable()
    {
      {
        (object) "03",
        (object) "Annobon"
      },
      {
        (object) "04",
        (object) "Bioko Norte"
      },
      {
        (object) "05",
        (object) "Bioko Sur"
      },
      {
        (object) "06",
        (object) "Centro Sur"
      },
      {
        (object) "07",
        (object) "Kie-Ntem"
      },
      {
        (object) "08",
        (object) "Litoral"
      },
      {
        (object) "09",
        (object) "Wele-Nzas"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GR", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Evros"
      },
      {
        (object) "02",
        (object) "Rodhopi"
      },
      {
        (object) "03",
        (object) "Xanthi"
      },
      {
        (object) "04",
        (object) "Drama"
      },
      {
        (object) "05",
        (object) "Serrai"
      },
      {
        (object) "06",
        (object) "Kilkis"
      },
      {
        (object) "07",
        (object) "Pella"
      },
      {
        (object) "08",
        (object) "Florina"
      },
      {
        (object) "09",
        (object) "Kastoria"
      },
      {
        (object) "10",
        (object) "Grevena"
      },
      {
        (object) "11",
        (object) "Kozani"
      },
      {
        (object) "12",
        (object) "Imathia"
      },
      {
        (object) "13",
        (object) "Thessaloniki"
      },
      {
        (object) "14",
        (object) "Kavala"
      },
      {
        (object) "15",
        (object) "Khalkidhiki"
      },
      {
        (object) "16",
        (object) "Pieria"
      },
      {
        (object) "17",
        (object) "Ioannina"
      },
      {
        (object) "18",
        (object) "Thesprotia"
      },
      {
        (object) "19",
        (object) "Preveza"
      },
      {
        (object) "20",
        (object) "Arta"
      },
      {
        (object) "21",
        (object) "Larisa"
      },
      {
        (object) "22",
        (object) "Trikala"
      },
      {
        (object) "23",
        (object) "Kardhitsa"
      },
      {
        (object) "24",
        (object) "Magnisia"
      },
      {
        (object) "25",
        (object) "Kerkira"
      },
      {
        (object) "26",
        (object) "Levkas"
      },
      {
        (object) "27",
        (object) "Kefallinia"
      },
      {
        (object) "28",
        (object) "Zakinthos"
      },
      {
        (object) "29",
        (object) "Fthiotis"
      },
      {
        (object) "30",
        (object) "Evritania"
      },
      {
        (object) "31",
        (object) "Aitolia kai Akarnania"
      },
      {
        (object) "32",
        (object) "Fokis"
      },
      {
        (object) "33",
        (object) "Voiotia"
      },
      {
        (object) "34",
        (object) "Evvoia"
      },
      {
        (object) "35",
        (object) "Attiki"
      },
      {
        (object) "36",
        (object) "Argolis"
      },
      {
        (object) "37",
        (object) "Korinthia"
      },
      {
        (object) "38",
        (object) "Akhaia"
      },
      {
        (object) "39",
        (object) "Ilia"
      },
      {
        (object) "40",
        (object) "Messinia"
      },
      {
        (object) "41",
        (object) "Arkadhia"
      },
      {
        (object) "42",
        (object) "Lakonia"
      },
      {
        (object) "43",
        (object) "Khania"
      },
      {
        (object) "44",
        (object) "Rethimni"
      },
      {
        (object) "45",
        (object) "Iraklion"
      },
      {
        (object) "46",
        (object) "Lasithi"
      },
      {
        (object) "47",
        (object) "Dhodhekanisos"
      },
      {
        (object) "48",
        (object) "Samos"
      },
      {
        (object) "49",
        (object) "Kikladhes"
      },
      {
        (object) "50",
        (object) "Khios"
      },
      {
        (object) "51",
        (object) "Lesvos"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GT", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Alta Verapaz"
      },
      {
        (object) "02",
        (object) "Baja Verapaz"
      },
      {
        (object) "03",
        (object) "Chimaltenango"
      },
      {
        (object) "04",
        (object) "Chiquimula"
      },
      {
        (object) "05",
        (object) "El Progreso"
      },
      {
        (object) "06",
        (object) "Escintla"
      },
      {
        (object) "07",
        (object) "Guatemala"
      },
      {
        (object) "08",
        (object) "Huehuetenango"
      },
      {
        (object) "09",
        (object) "Izabal"
      },
      {
        (object) "10",
        (object) "Jalapa"
      },
      {
        (object) "11",
        (object) "Jutiapa"
      },
      {
        (object) "12",
        (object) "Peten"
      },
      {
        (object) "13",
        (object) "Quetzaltenango"
      },
      {
        (object) "14",
        (object) "Quiche"
      },
      {
        (object) "15",
        (object) "Retalhuleu"
      },
      {
        (object) "16",
        (object) "Sacatepequez"
      },
      {
        (object) "17",
        (object) "San Marcos"
      },
      {
        (object) "18",
        (object) "Santa Rosa"
      },
      {
        (object) "19",
        (object) "Solola"
      },
      {
        (object) "20",
        (object) "Suchitepequez"
      },
      {
        (object) "21",
        (object) "Totonicapan"
      },
      {
        (object) "22",
        (object) "Zacapa"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GW", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bafata"
      },
      {
        (object) "02",
        (object) "Quinara"
      },
      {
        (object) "04",
        (object) "Oio"
      },
      {
        (object) "05",
        (object) "Bolama"
      },
      {
        (object) "06",
        (object) "Cacheu"
      },
      {
        (object) "07",
        (object) "Tombali"
      },
      {
        (object) "10",
        (object) "Gabu"
      },
      {
        (object) "11",
        (object) "Bissau"
      },
      {
        (object) "12",
        (object) "Biombo"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "GY", (object) new Hashtable()
    {
      {
        (object) "10",
        (object) "Barima-Waini"
      },
      {
        (object) "11",
        (object) "Cuyuni-Mazaruni"
      },
      {
        (object) "12",
        (object) "Demerara-Mahaica"
      },
      {
        (object) "13",
        (object) "East Berbice-Corentyne"
      },
      {
        (object) "14",
        (object) "Essequibo Islands-West Demerara"
      },
      {
        (object) "15",
        (object) "Mahaica-Berbice"
      },
      {
        (object) "16",
        (object) "Pomeroon-Supenaam"
      },
      {
        (object) "17",
        (object) "Potaro-Siparuni"
      },
      {
        (object) "18",
        (object) "Upper Demerara-Berbice"
      },
      {
        (object) "19",
        (object) "Upper Takutu-Upper Essequibo"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "HN", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Atlantida"
      },
      {
        (object) "02",
        (object) "Choluteca"
      },
      {
        (object) "03",
        (object) "Colon"
      },
      {
        (object) "04",
        (object) "Comayagua"
      },
      {
        (object) "05",
        (object) "Copan"
      },
      {
        (object) "06",
        (object) "Cortes"
      },
      {
        (object) "07",
        (object) "El Paraiso"
      },
      {
        (object) "08",
        (object) "Francisco Morazan"
      },
      {
        (object) "09",
        (object) "Gracias a Dios"
      },
      {
        (object) "10",
        (object) "Intibuca"
      },
      {
        (object) "11",
        (object) "Islas de la Bahia"
      },
      {
        (object) "12",
        (object) "La Paz"
      },
      {
        (object) "13",
        (object) "Lempira"
      },
      {
        (object) "14",
        (object) "Ocotepeque"
      },
      {
        (object) "15",
        (object) "Olancho"
      },
      {
        (object) "16",
        (object) "Santa Barbara"
      },
      {
        (object) "17",
        (object) "Valle"
      },
      {
        (object) "18",
        (object) "Yoro"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "HR", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bjelovarsko-Bilogorska"
      },
      {
        (object) "02",
        (object) "Brodsko-Posavska"
      },
      {
        (object) "03",
        (object) "Dubrovacko-Neretvanska"
      },
      {
        (object) "04",
        (object) "Istarska"
      },
      {
        (object) "05",
        (object) "Karlovacka"
      },
      {
        (object) "06",
        (object) "Koprivnicko-Krizevacka"
      },
      {
        (object) "07",
        (object) "Krapinsko-Zagorska"
      },
      {
        (object) "08",
        (object) "Licko-Senjska"
      },
      {
        (object) "09",
        (object) "Medimurska"
      },
      {
        (object) "10",
        (object) "Osjecko-Baranjska"
      },
      {
        (object) "11",
        (object) "Pozesko-Slavonska"
      },
      {
        (object) "12",
        (object) "Primorsko-Goranska"
      },
      {
        (object) "13",
        (object) "Sibensko-Kninska"
      },
      {
        (object) "14",
        (object) "Sisacko-Moslavacka"
      },
      {
        (object) "15",
        (object) "Splitsko-Dalmatinska"
      },
      {
        (object) "16",
        (object) "Varazdinska"
      },
      {
        (object) "17",
        (object) "Viroviticko-Podravska"
      },
      {
        (object) "18",
        (object) "Vukovarsko-Srijemska"
      },
      {
        (object) "19",
        (object) "Zadarska"
      },
      {
        (object) "20",
        (object) "Zagrebacka"
      },
      {
        (object) "21",
        (object) "Grad Zagreb"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "HT", (object) new Hashtable()
    {
      {
        (object) "03",
        (object) "Nord-Ouest"
      },
      {
        (object) "06",
        (object) "Artibonite"
      },
      {
        (object) "07",
        (object) "Centre"
      },
      {
        (object) "09",
        (object) "Nord"
      },
      {
        (object) "10",
        (object) "Nord-Est"
      },
      {
        (object) "11",
        (object) "Ouest"
      },
      {
        (object) "12",
        (object) "Sud"
      },
      {
        (object) "13",
        (object) "Sud-Est"
      },
      {
        (object) "14",
        (object) "Grand' Anse"
      },
      {
        (object) "15",
        (object) "Nippes"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "HU", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bacs-Kiskun"
      },
      {
        (object) "02",
        (object) "Baranya"
      },
      {
        (object) "03",
        (object) "Bekes"
      },
      {
        (object) "04",
        (object) "Borsod-Abauj-Zemplen"
      },
      {
        (object) "05",
        (object) "Budapest"
      },
      {
        (object) "06",
        (object) "Csongrad"
      },
      {
        (object) "07",
        (object) "Debrecen"
      },
      {
        (object) "08",
        (object) "Fejer"
      },
      {
        (object) "09",
        (object) "Gyor-Moson-Sopron"
      },
      {
        (object) "10",
        (object) "Hajdu-Bihar"
      },
      {
        (object) "11",
        (object) "Heves"
      },
      {
        (object) "12",
        (object) "Komarom-Esztergom"
      },
      {
        (object) "13",
        (object) "Miskolc"
      },
      {
        (object) "14",
        (object) "Nograd"
      },
      {
        (object) "15",
        (object) "Pecs"
      },
      {
        (object) "16",
        (object) "Pest"
      },
      {
        (object) "17",
        (object) "Somogy"
      },
      {
        (object) "18",
        (object) "Szabolcs-Szatmar-Bereg"
      },
      {
        (object) "19",
        (object) "Szeged"
      },
      {
        (object) "20",
        (object) "Jasz-Nagykun-Szolnok"
      },
      {
        (object) "21",
        (object) "Tolna"
      },
      {
        (object) "22",
        (object) "Vas"
      },
      {
        (object) "23",
        (object) "Veszprem"
      },
      {
        (object) "24",
        (object) "Zala"
      },
      {
        (object) "25",
        (object) "Gyor"
      },
      {
        (object) "26",
        (object) "Bekescsaba"
      },
      {
        (object) "27",
        (object) "Dunaujvaros"
      },
      {
        (object) "28",
        (object) "Eger"
      },
      {
        (object) "29",
        (object) "Hodmezovasarhely"
      },
      {
        (object) "30",
        (object) "Kaposvar"
      },
      {
        (object) "31",
        (object) "Kecskemet"
      },
      {
        (object) "32",
        (object) "Nagykanizsa"
      },
      {
        (object) "33",
        (object) "Nyiregyhaza"
      },
      {
        (object) "34",
        (object) "Sopron"
      },
      {
        (object) "35",
        (object) "Szekesfehervar"
      },
      {
        (object) "36",
        (object) "Szolnok"
      },
      {
        (object) "37",
        (object) "Szombathely"
      },
      {
        (object) "38",
        (object) "Tatabanya"
      },
      {
        (object) "39",
        (object) "Veszprem"
      },
      {
        (object) "40",
        (object) "Zalaegerszeg"
      },
      {
        (object) "41",
        (object) "Salgotarjan"
      },
      {
        (object) "42",
        (object) "Szekszard"
      },
      {
        (object) "43",
        (object) "Erd"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "ID", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Aceh"
      },
      {
        (object) "02",
        (object) "Bali"
      },
      {
        (object) "03",
        (object) "Bengkulu"
      },
      {
        (object) "04",
        (object) "Jakarta Raya"
      },
      {
        (object) "05",
        (object) "Jambi"
      },
      {
        (object) "07",
        (object) "Jawa Tengah"
      },
      {
        (object) "08",
        (object) "Jawa Timur"
      },
      {
        (object) "10",
        (object) "Yogyakarta"
      },
      {
        (object) "11",
        (object) "Kalimantan Barat"
      },
      {
        (object) "12",
        (object) "Kalimantan Selatan"
      },
      {
        (object) "13",
        (object) "Kalimantan Tengah"
      },
      {
        (object) "14",
        (object) "Kalimantan Timur"
      },
      {
        (object) "15",
        (object) "Lampung"
      },
      {
        (object) "17",
        (object) "Nusa Tenggara Barat"
      },
      {
        (object) "18",
        (object) "Nusa Tenggara Timur"
      },
      {
        (object) "21",
        (object) "Sulawesi Tengah"
      },
      {
        (object) "22",
        (object) "Sulawesi Tenggara"
      },
      {
        (object) "24",
        (object) "Sumatera Barat"
      },
      {
        (object) "26",
        (object) "Sumatera Utara"
      },
      {
        (object) "28",
        (object) "Maluku"
      },
      {
        (object) "29",
        (object) "Maluku Utara"
      },
      {
        (object) "30",
        (object) "Jawa Barat"
      },
      {
        (object) "31",
        (object) "Sulawesi Utara"
      },
      {
        (object) "32",
        (object) "Sumatera Selatan"
      },
      {
        (object) "33",
        (object) "Banten"
      },
      {
        (object) "34",
        (object) "Gorontalo"
      },
      {
        (object) "35",
        (object) "Kepulauan Bangka Belitung"
      },
      {
        (object) "36",
        (object) "Papua"
      },
      {
        (object) "37",
        (object) "Riau"
      },
      {
        (object) "38",
        (object) "Sulawesi Selatan"
      },
      {
        (object) "39",
        (object) "Irian Jaya Barat"
      },
      {
        (object) "40",
        (object) "Kepulauan Riau"
      },
      {
        (object) "41",
        (object) "Sulawesi Barat"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "IE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Carlow"
      },
      {
        (object) "02",
        (object) "Cavan"
      },
      {
        (object) "03",
        (object) "Clare"
      },
      {
        (object) "04",
        (object) "Cork"
      },
      {
        (object) "06",
        (object) "Donegal"
      },
      {
        (object) "07",
        (object) "Dublin"
      },
      {
        (object) "10",
        (object) "Galway"
      },
      {
        (object) "11",
        (object) "Kerry"
      },
      {
        (object) "12",
        (object) "Kildare"
      },
      {
        (object) "13",
        (object) "Kilkenny"
      },
      {
        (object) "14",
        (object) "Leitrim"
      },
      {
        (object) "15",
        (object) "Laois"
      },
      {
        (object) "16",
        (object) "Limerick"
      },
      {
        (object) "18",
        (object) "Longford"
      },
      {
        (object) "19",
        (object) "Louth"
      },
      {
        (object) "20",
        (object) "Mayo"
      },
      {
        (object) "21",
        (object) "Meath"
      },
      {
        (object) "22",
        (object) "Monaghan"
      },
      {
        (object) "23",
        (object) "Offaly"
      },
      {
        (object) "24",
        (object) "Roscommon"
      },
      {
        (object) "25",
        (object) "Sligo"
      },
      {
        (object) "26",
        (object) "Tipperary"
      },
      {
        (object) "27",
        (object) "Waterford"
      },
      {
        (object) "29",
        (object) "Westmeath"
      },
      {
        (object) "30",
        (object) "Wexford"
      },
      {
        (object) "31",
        (object) "Wicklow"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "IL", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "HaDarom"
      },
      {
        (object) "02",
        (object) "HaMerkaz"
      },
      {
        (object) "03",
        (object) "HaZafon"
      },
      {
        (object) "04",
        (object) "Hefa"
      },
      {
        (object) "05",
        (object) "Tel Aviv"
      },
      {
        (object) "06",
        (object) "Yerushalayim"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "IN", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Andaman and Nicobar Islands"
      },
      {
        (object) "02",
        (object) "Andhra Pradesh"
      },
      {
        (object) "03",
        (object) "Assam"
      },
      {
        (object) "05",
        (object) "Chandigarh"
      },
      {
        (object) "06",
        (object) "Dadra and Nagar Haveli"
      },
      {
        (object) "07",
        (object) "Delhi"
      },
      {
        (object) "09",
        (object) "Gujarat"
      },
      {
        (object) "10",
        (object) "Haryana"
      },
      {
        (object) "11",
        (object) "Himachal Pradesh"
      },
      {
        (object) "12",
        (object) "Jammu and Kashmir"
      },
      {
        (object) "13",
        (object) "Kerala"
      },
      {
        (object) "14",
        (object) "Lakshadweep"
      },
      {
        (object) "16",
        (object) "Maharashtra"
      },
      {
        (object) "17",
        (object) "Manipur"
      },
      {
        (object) "18",
        (object) "Meghalaya"
      },
      {
        (object) "19",
        (object) "Karnataka"
      },
      {
        (object) "20",
        (object) "Nagaland"
      },
      {
        (object) "21",
        (object) "Orissa"
      },
      {
        (object) "22",
        (object) "Puducherry"
      },
      {
        (object) "23",
        (object) "Punjab"
      },
      {
        (object) "24",
        (object) "Rajasthan"
      },
      {
        (object) "25",
        (object) "Tamil Nadu"
      },
      {
        (object) "26",
        (object) "Tripura"
      },
      {
        (object) "28",
        (object) "West Bengal"
      },
      {
        (object) "29",
        (object) "Sikkim"
      },
      {
        (object) "30",
        (object) "Arunachal Pradesh"
      },
      {
        (object) "31",
        (object) "Mizoram"
      },
      {
        (object) "32",
        (object) "Daman and Diu"
      },
      {
        (object) "33",
        (object) "Goa"
      },
      {
        (object) "34",
        (object) "Bihar"
      },
      {
        (object) "35",
        (object) "Madhya Pradesh"
      },
      {
        (object) "36",
        (object) "Uttar Pradesh"
      },
      {
        (object) "37",
        (object) "Chhattisgarh"
      },
      {
        (object) "38",
        (object) "Jharkhand"
      },
      {
        (object) "39",
        (object) "Uttarakhand"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "IQ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Al Anbar"
      },
      {
        (object) "02",
        (object) "Al Basrah"
      },
      {
        (object) "03",
        (object) "Al Muthanna"
      },
      {
        (object) "04",
        (object) "Al Qadisiyah"
      },
      {
        (object) "05",
        (object) "As Sulaymaniyah"
      },
      {
        (object) "06",
        (object) "Babil"
      },
      {
        (object) "07",
        (object) "Baghdad"
      },
      {
        (object) "08",
        (object) "Dahuk"
      },
      {
        (object) "09",
        (object) "Dhi Qar"
      },
      {
        (object) "10",
        (object) "Diyala"
      },
      {
        (object) "11",
        (object) "Arbil"
      },
      {
        (object) "12",
        (object) "Karbala'"
      },
      {
        (object) "13",
        (object) "At Ta'mim"
      },
      {
        (object) "14",
        (object) "Maysan"
      },
      {
        (object) "15",
        (object) "Ninawa"
      },
      {
        (object) "16",
        (object) "Wasit"
      },
      {
        (object) "17",
        (object) "An Najaf"
      },
      {
        (object) "18",
        (object) "Salah ad Din"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "IR", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Azarbayjan-e Bakhtari"
      },
      {
        (object) "03",
        (object) "Chahar Mahall va Bakhtiari"
      },
      {
        (object) "04",
        (object) "Sistan va Baluchestan"
      },
      {
        (object) "05",
        (object) "Kohkiluyeh va Buyer Ahmadi"
      },
      {
        (object) "07",
        (object) "Fars"
      },
      {
        (object) "08",
        (object) "Gilan"
      },
      {
        (object) "09",
        (object) "Hamadan"
      },
      {
        (object) "10",
        (object) "Ilam"
      },
      {
        (object) "11",
        (object) "Hormozgan"
      },
      {
        (object) "12",
        (object) "Kerman"
      },
      {
        (object) "13",
        (object) "Bakhtaran"
      },
      {
        (object) "15",
        (object) "Khuzestan"
      },
      {
        (object) "16",
        (object) "Kordestan"
      },
      {
        (object) "17",
        (object) "Mazandaran"
      },
      {
        (object) "18",
        (object) "Semnan Province"
      },
      {
        (object) "19",
        (object) "Markazi"
      },
      {
        (object) "21",
        (object) "Zanjan"
      },
      {
        (object) "22",
        (object) "Bushehr"
      },
      {
        (object) "23",
        (object) "Lorestan"
      },
      {
        (object) "24",
        (object) "Markazi"
      },
      {
        (object) "25",
        (object) "Semnan"
      },
      {
        (object) "26",
        (object) "Tehran"
      },
      {
        (object) "27",
        (object) "Zanjan"
      },
      {
        (object) "28",
        (object) "Esfahan"
      },
      {
        (object) "29",
        (object) "Kerman"
      },
      {
        (object) "30",
        (object) "Khorasan"
      },
      {
        (object) "31",
        (object) "Yazd"
      },
      {
        (object) "32",
        (object) "Ardabil"
      },
      {
        (object) "33",
        (object) "East Azarbaijan"
      },
      {
        (object) "34",
        (object) "Markazi"
      },
      {
        (object) "35",
        (object) "Mazandaran"
      },
      {
        (object) "36",
        (object) "Zanjan"
      },
      {
        (object) "37",
        (object) "Golestan"
      },
      {
        (object) "38",
        (object) "Qazvin"
      },
      {
        (object) "39",
        (object) "Qom"
      },
      {
        (object) "40",
        (object) "Yazd"
      },
      {
        (object) "41",
        (object) "Khorasan-e Janubi"
      },
      {
        (object) "42",
        (object) "Khorasan-e Razavi"
      },
      {
        (object) "43",
        (object) "Khorasan-e Shemali"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "IS", (object) new Hashtable()
    {
      {
        (object) "03",
        (object) "Arnessysla"
      },
      {
        (object) "05",
        (object) "Austur-Hunavatnssysla"
      },
      {
        (object) "06",
        (object) "Austur-Skaftafellssysla"
      },
      {
        (object) "07",
        (object) "Borgarfjardarsysla"
      },
      {
        (object) "09",
        (object) "Eyjafjardarsysla"
      },
      {
        (object) "10",
        (object) "Gullbringusysla"
      },
      {
        (object) "15",
        (object) "Kjosarsysla"
      },
      {
        (object) "17",
        (object) "Myrasysla"
      },
      {
        (object) "20",
        (object) "Nordur-Mulasysla"
      },
      {
        (object) "21",
        (object) "Nordur-Tingeyjarsysla"
      },
      {
        (object) "23",
        (object) "Rangarvallasysla"
      },
      {
        (object) "28",
        (object) "Skagafjardarsysla"
      },
      {
        (object) "29",
        (object) "Snafellsnes- og Hnappadalssysla"
      },
      {
        (object) "30",
        (object) "Strandasysla"
      },
      {
        (object) "31",
        (object) "Sudur-Mulasysla"
      },
      {
        (object) "32",
        (object) "Sudur-Tingeyjarsysla"
      },
      {
        (object) "34",
        (object) "Vestur-Bardastrandarsysla"
      },
      {
        (object) "35",
        (object) "Vestur-Hunavatnssysla"
      },
      {
        (object) "36",
        (object) "Vestur-Isafjardarsysla"
      },
      {
        (object) "37",
        (object) "Vestur-Skaftafellssysla"
      },
      {
        (object) "40",
        (object) "Norourland Eystra"
      },
      {
        (object) "41",
        (object) "Norourland Vestra"
      },
      {
        (object) "42",
        (object) "Suourland"
      },
      {
        (object) "43",
        (object) "Suournes"
      },
      {
        (object) "44",
        (object) "Vestfiroir"
      },
      {
        (object) "45",
        (object) "Vesturland"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "IT", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Abruzzi"
      },
      {
        (object) "02",
        (object) "Basilicata"
      },
      {
        (object) "03",
        (object) "Calabria"
      },
      {
        (object) "04",
        (object) "Campania"
      },
      {
        (object) "05",
        (object) "Emilia-Romagna"
      },
      {
        (object) "06",
        (object) "Friuli-Venezia Giulia"
      },
      {
        (object) "07",
        (object) "Lazio"
      },
      {
        (object) "08",
        (object) "Liguria"
      },
      {
        (object) "09",
        (object) "Lombardia"
      },
      {
        (object) "10",
        (object) "Marche"
      },
      {
        (object) "11",
        (object) "Molise"
      },
      {
        (object) "12",
        (object) "Piemonte"
      },
      {
        (object) "13",
        (object) "Puglia"
      },
      {
        (object) "14",
        (object) "Sardegna"
      },
      {
        (object) "15",
        (object) "Sicilia"
      },
      {
        (object) "16",
        (object) "Toscana"
      },
      {
        (object) "17",
        (object) "Trentino-Alto Adige"
      },
      {
        (object) "18",
        (object) "Umbria"
      },
      {
        (object) "19",
        (object) "Valle d'Aosta"
      },
      {
        (object) "20",
        (object) "Veneto"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "JM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Clarendon"
      },
      {
        (object) "02",
        (object) "Hanover"
      },
      {
        (object) "04",
        (object) "Manchester"
      },
      {
        (object) "07",
        (object) "Portland"
      },
      {
        (object) "08",
        (object) "Saint Andrew"
      },
      {
        (object) "09",
        (object) "Saint Ann"
      },
      {
        (object) "10",
        (object) "Saint Catherine"
      },
      {
        (object) "11",
        (object) "Saint Elizabeth"
      },
      {
        (object) "12",
        (object) "Saint James"
      },
      {
        (object) "13",
        (object) "Saint Mary"
      },
      {
        (object) "14",
        (object) "Saint Thomas"
      },
      {
        (object) "15",
        (object) "Trelawny"
      },
      {
        (object) "16",
        (object) "Westmoreland"
      },
      {
        (object) "17",
        (object) "Kingston"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "JO", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Al Balqa'"
      },
      {
        (object) "09",
        (object) "Al Karak"
      },
      {
        (object) "12",
        (object) "At Tafilah"
      },
      {
        (object) "15",
        (object) "Al Mafraq"
      },
      {
        (object) "16",
        (object) "Amman"
      },
      {
        (object) "17",
        (object) "Az Zaraqa"
      },
      {
        (object) "18",
        (object) "Irbid"
      },
      {
        (object) "19",
        (object) "Ma'an"
      },
      {
        (object) "20",
        (object) "Ajlun"
      },
      {
        (object) "21",
        (object) "Al Aqabah"
      },
      {
        (object) "22",
        (object) "Jarash"
      },
      {
        (object) "23",
        (object) "Madaba"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "JP", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Aichi"
      },
      {
        (object) "02",
        (object) "Akita"
      },
      {
        (object) "03",
        (object) "Aomori"
      },
      {
        (object) "04",
        (object) "Chiba"
      },
      {
        (object) "05",
        (object) "Ehime"
      },
      {
        (object) "06",
        (object) "Fukui"
      },
      {
        (object) "07",
        (object) "Fukuoka"
      },
      {
        (object) "08",
        (object) "Fukushima"
      },
      {
        (object) "09",
        (object) "Gifu"
      },
      {
        (object) "10",
        (object) "Gumma"
      },
      {
        (object) "11",
        (object) "Hiroshima"
      },
      {
        (object) "12",
        (object) "Hokkaido"
      },
      {
        (object) "13",
        (object) "Hyogo"
      },
      {
        (object) "14",
        (object) "Ibaraki"
      },
      {
        (object) "15",
        (object) "Ishikawa"
      },
      {
        (object) "16",
        (object) "Iwate"
      },
      {
        (object) "17",
        (object) "Kagawa"
      },
      {
        (object) "18",
        (object) "Kagoshima"
      },
      {
        (object) "19",
        (object) "Kanagawa"
      },
      {
        (object) "20",
        (object) "Kochi"
      },
      {
        (object) "21",
        (object) "Kumamoto"
      },
      {
        (object) "22",
        (object) "Kyoto"
      },
      {
        (object) "23",
        (object) "Mie"
      },
      {
        (object) "24",
        (object) "Miyagi"
      },
      {
        (object) "25",
        (object) "Miyazaki"
      },
      {
        (object) "26",
        (object) "Nagano"
      },
      {
        (object) "27",
        (object) "Nagasaki"
      },
      {
        (object) "28",
        (object) "Nara"
      },
      {
        (object) "29",
        (object) "Niigata"
      },
      {
        (object) "30",
        (object) "Oita"
      },
      {
        (object) "31",
        (object) "Okayama"
      },
      {
        (object) "32",
        (object) "Osaka"
      },
      {
        (object) "33",
        (object) "Saga"
      },
      {
        (object) "34",
        (object) "Saitama"
      },
      {
        (object) "35",
        (object) "Shiga"
      },
      {
        (object) "36",
        (object) "Shimane"
      },
      {
        (object) "37",
        (object) "Shizuoka"
      },
      {
        (object) "38",
        (object) "Tochigi"
      },
      {
        (object) "39",
        (object) "Tokushima"
      },
      {
        (object) "40",
        (object) "Tokyo"
      },
      {
        (object) "41",
        (object) "Tottori"
      },
      {
        (object) "42",
        (object) "Toyama"
      },
      {
        (object) "43",
        (object) "Wakayama"
      },
      {
        (object) "44",
        (object) "Yamagata"
      },
      {
        (object) "45",
        (object) "Yamaguchi"
      },
      {
        (object) "46",
        (object) "Yamanashi"
      },
      {
        (object) "47",
        (object) "Okinawa"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Central"
      },
      {
        (object) "02",
        (object) "Coast"
      },
      {
        (object) "03",
        (object) "Eastern"
      },
      {
        (object) "05",
        (object) "Nairobi Area"
      },
      {
        (object) "06",
        (object) "North-Eastern"
      },
      {
        (object) "07",
        (object) "Nyanza"
      },
      {
        (object) "08",
        (object) "Rift Valley"
      },
      {
        (object) "09",
        (object) "Western"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KG", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bishkek"
      },
      {
        (object) "02",
        (object) "Chuy"
      },
      {
        (object) "03",
        (object) "Jalal-Abad"
      },
      {
        (object) "04",
        (object) "Naryn"
      },
      {
        (object) "05",
        (object) "Osh"
      },
      {
        (object) "06",
        (object) "Talas"
      },
      {
        (object) "07",
        (object) "Ysyk-Kol"
      },
      {
        (object) "08",
        (object) "Osh"
      },
      {
        (object) "09",
        (object) "Batken"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KH", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Batdambang"
      },
      {
        (object) "02",
        (object) "Kampong Cham"
      },
      {
        (object) "03",
        (object) "Kampong Chhnang"
      },
      {
        (object) "04",
        (object) "Kampong Speu"
      },
      {
        (object) "05",
        (object) "Kampong Thum"
      },
      {
        (object) "06",
        (object) "Kampot"
      },
      {
        (object) "07",
        (object) "Kandal"
      },
      {
        (object) "08",
        (object) "Koh Kong"
      },
      {
        (object) "09",
        (object) "Kracheh"
      },
      {
        (object) "10",
        (object) "Mondulkiri"
      },
      {
        (object) "11",
        (object) "Phnum Penh"
      },
      {
        (object) "12",
        (object) "Pursat"
      },
      {
        (object) "13",
        (object) "Preah Vihear"
      },
      {
        (object) "14",
        (object) "Prey Veng"
      },
      {
        (object) "15",
        (object) "Ratanakiri Kiri"
      },
      {
        (object) "16",
        (object) "Siem Reap"
      },
      {
        (object) "17",
        (object) "Stung Treng"
      },
      {
        (object) "18",
        (object) "Svay Rieng"
      },
      {
        (object) "19",
        (object) "Takeo"
      },
      {
        (object) "25",
        (object) "Banteay Meanchey"
      },
      {
        (object) "29",
        (object) "Batdambang"
      },
      {
        (object) "30",
        (object) "Pailin"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KI", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Gilbert Islands"
      },
      {
        (object) "02",
        (object) "Line Islands"
      },
      {
        (object) "03",
        (object) "Phoenix Islands"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Anjouan"
      },
      {
        (object) "02",
        (object) "Grande Comore"
      },
      {
        (object) "03",
        (object) "Moheli"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KN", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Christ Church Nichola Town"
      },
      {
        (object) "02",
        (object) "Saint Anne Sandy Point"
      },
      {
        (object) "03",
        (object) "Saint George Basseterre"
      },
      {
        (object) "04",
        (object) "Saint George Gingerland"
      },
      {
        (object) "05",
        (object) "Saint James Windward"
      },
      {
        (object) "06",
        (object) "Saint John Capisterre"
      },
      {
        (object) "07",
        (object) "Saint John Figtree"
      },
      {
        (object) "08",
        (object) "Saint Mary Cayon"
      },
      {
        (object) "09",
        (object) "Saint Paul Capisterre"
      },
      {
        (object) "10",
        (object) "Saint Paul Charlestown"
      },
      {
        (object) "11",
        (object) "Saint Peter Basseterre"
      },
      {
        (object) "12",
        (object) "Saint Thomas Lowland"
      },
      {
        (object) "13",
        (object) "Saint Thomas Middle Island"
      },
      {
        (object) "15",
        (object) "Trinity Palmetto Point"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KP", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Chagang-do"
      },
      {
        (object) "03",
        (object) "Hamgyong-namdo"
      },
      {
        (object) "06",
        (object) "Hwanghae-namdo"
      },
      {
        (object) "07",
        (object) "Hwanghae-bukto"
      },
      {
        (object) "08",
        (object) "Kaesong-si"
      },
      {
        (object) "09",
        (object) "Kangwon-do"
      },
      {
        (object) "11",
        (object) "P'yongan-bukto"
      },
      {
        (object) "12",
        (object) "P'yongyang-si"
      },
      {
        (object) "13",
        (object) "Yanggang-do"
      },
      {
        (object) "14",
        (object) "Namp'o-si"
      },
      {
        (object) "15",
        (object) "P'yongan-namdo"
      },
      {
        (object) "17",
        (object) "Hamgyong-bukto"
      },
      {
        (object) "18",
        (object) "Najin Sonbong-si"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KR", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Cheju-do"
      },
      {
        (object) "03",
        (object) "Cholla-bukto"
      },
      {
        (object) "05",
        (object) "Ch'ungch'ong-bukto"
      },
      {
        (object) "06",
        (object) "Kangwon-do"
      },
      {
        (object) "10",
        (object) "Pusan-jikhalsi"
      },
      {
        (object) "11",
        (object) "Seoul-t'ukpyolsi"
      },
      {
        (object) "12",
        (object) "Inch'on-jikhalsi"
      },
      {
        (object) "13",
        (object) "Kyonggi-do"
      },
      {
        (object) "14",
        (object) "Kyongsang-bukto"
      },
      {
        (object) "15",
        (object) "Taegu-jikhalsi"
      },
      {
        (object) "16",
        (object) "Cholla-namdo"
      },
      {
        (object) "17",
        (object) "Ch'ungch'ong-namdo"
      },
      {
        (object) "18",
        (object) "Kwangju-jikhalsi"
      },
      {
        (object) "19",
        (object) "Taejon-jikhalsi"
      },
      {
        (object) "20",
        (object) "Kyongsang-namdo"
      },
      {
        (object) "21",
        (object) "Ulsan-gwangyoksi"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KW", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Al Ahmadi"
      },
      {
        (object) "02",
        (object) "Al Kuwayt"
      },
      {
        (object) "05",
        (object) "Al Jahra"
      },
      {
        (object) "07",
        (object) "Al Farwaniyah"
      },
      {
        (object) "08",
        (object) "Hawalli"
      },
      {
        (object) "09",
        (object) "Mubarak al Kabir"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KY", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Creek"
      },
      {
        (object) "02",
        (object) "Eastern"
      },
      {
        (object) "03",
        (object) "Midland"
      },
      {
        (object) "04",
        (object) "South Town"
      },
      {
        (object) "05",
        (object) "Spot Bay"
      },
      {
        (object) "06",
        (object) "Stake Bay"
      },
      {
        (object) "07",
        (object) "West End"
      },
      {
        (object) "08",
        (object) "Western"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "KZ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Almaty"
      },
      {
        (object) "02",
        (object) "Almaty City"
      },
      {
        (object) "03",
        (object) "Aqmola"
      },
      {
        (object) "04",
        (object) "Aqtobe"
      },
      {
        (object) "05",
        (object) "Astana"
      },
      {
        (object) "06",
        (object) "Atyrau"
      },
      {
        (object) "07",
        (object) "West Kazakhstan"
      },
      {
        (object) "08",
        (object) "Bayqonyr"
      },
      {
        (object) "09",
        (object) "Mangghystau"
      },
      {
        (object) "10",
        (object) "South Kazakhstan"
      },
      {
        (object) "11",
        (object) "Pavlodar"
      },
      {
        (object) "12",
        (object) "Qaraghandy"
      },
      {
        (object) "13",
        (object) "Qostanay"
      },
      {
        (object) "14",
        (object) "Qyzylorda"
      },
      {
        (object) "15",
        (object) "East Kazakhstan"
      },
      {
        (object) "16",
        (object) "North Kazakhstan"
      },
      {
        (object) "17",
        (object) "Zhambyl"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LA", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Attapu"
      },
      {
        (object) "02",
        (object) "Champasak"
      },
      {
        (object) "03",
        (object) "Houaphan"
      },
      {
        (object) "04",
        (object) "Khammouan"
      },
      {
        (object) "05",
        (object) "Louang Namtha"
      },
      {
        (object) "07",
        (object) "Oudomxai"
      },
      {
        (object) "08",
        (object) "Phongsali"
      },
      {
        (object) "09",
        (object) "Saravan"
      },
      {
        (object) "10",
        (object) "Savannakhet"
      },
      {
        (object) "11",
        (object) "Vientiane"
      },
      {
        (object) "13",
        (object) "Xaignabouri"
      },
      {
        (object) "14",
        (object) "Xiangkhoang"
      },
      {
        (object) "17",
        (object) "Louangphrabang"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LB", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Beqaa"
      },
      {
        (object) "02",
        (object) "Al Janub"
      },
      {
        (object) "03",
        (object) "Liban-Nord"
      },
      {
        (object) "04",
        (object) "Beyrouth"
      },
      {
        (object) "05",
        (object) "Mont-Liban"
      },
      {
        (object) "06",
        (object) "Liban-Sud"
      },
      {
        (object) "07",
        (object) "Nabatiye"
      },
      {
        (object) "08",
        (object) "Beqaa"
      },
      {
        (object) "09",
        (object) "Liban-Nord"
      },
      {
        (object) "10",
        (object) "Aakk,r"
      },
      {
        (object) "11",
        (object) "Baalbek-Hermel"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LC", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Anse-la-Raye"
      },
      {
        (object) "02",
        (object) "Dauphin"
      },
      {
        (object) "03",
        (object) "Castries"
      },
      {
        (object) "04",
        (object) "Choiseul"
      },
      {
        (object) "05",
        (object) "Dennery"
      },
      {
        (object) "06",
        (object) "Gros-Islet"
      },
      {
        (object) "07",
        (object) "Laborie"
      },
      {
        (object) "08",
        (object) "Micoud"
      },
      {
        (object) "09",
        (object) "Soufriere"
      },
      {
        (object) "10",
        (object) "Vieux-Fort"
      },
      {
        (object) "11",
        (object) "Praslin"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LI", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Balzers"
      },
      {
        (object) "02",
        (object) "Eschen"
      },
      {
        (object) "03",
        (object) "Gamprin"
      },
      {
        (object) "04",
        (object) "Mauren"
      },
      {
        (object) "05",
        (object) "Planken"
      },
      {
        (object) "06",
        (object) "Ruggell"
      },
      {
        (object) "07",
        (object) "Schaan"
      },
      {
        (object) "08",
        (object) "Schellenberg"
      },
      {
        (object) "09",
        (object) "Triesen"
      },
      {
        (object) "10",
        (object) "Triesenberg"
      },
      {
        (object) "11",
        (object) "Vaduz"
      },
      {
        (object) "21",
        (object) "Gbarpolu"
      },
      {
        (object) "22",
        (object) "River Gee"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LK", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Amparai"
      },
      {
        (object) "02",
        (object) "Anuradhapura"
      },
      {
        (object) "03",
        (object) "Badulla"
      },
      {
        (object) "04",
        (object) "Batticaloa"
      },
      {
        (object) "06",
        (object) "Galle"
      },
      {
        (object) "07",
        (object) "Hambantota"
      },
      {
        (object) "09",
        (object) "Kalutara"
      },
      {
        (object) "10",
        (object) "Kandy"
      },
      {
        (object) "11",
        (object) "Kegalla"
      },
      {
        (object) "12",
        (object) "Kurunegala"
      },
      {
        (object) "14",
        (object) "Matale"
      },
      {
        (object) "15",
        (object) "Matara"
      },
      {
        (object) "16",
        (object) "Moneragala"
      },
      {
        (object) "17",
        (object) "Nuwara Eliya"
      },
      {
        (object) "18",
        (object) "Polonnaruwa"
      },
      {
        (object) "19",
        (object) "Puttalam"
      },
      {
        (object) "20",
        (object) "Ratnapura"
      },
      {
        (object) "21",
        (object) "Trincomalee"
      },
      {
        (object) "23",
        (object) "Colombo"
      },
      {
        (object) "24",
        (object) "Gampaha"
      },
      {
        (object) "25",
        (object) "Jaffna"
      },
      {
        (object) "26",
        (object) "Mannar"
      },
      {
        (object) "27",
        (object) "Mullaittivu"
      },
      {
        (object) "28",
        (object) "Vavuniya"
      },
      {
        (object) "29",
        (object) "Central"
      },
      {
        (object) "30",
        (object) "North Central"
      },
      {
        (object) "31",
        (object) "Northern"
      },
      {
        (object) "32",
        (object) "North Western"
      },
      {
        (object) "33",
        (object) "Sabaragamuwa"
      },
      {
        (object) "34",
        (object) "Southern"
      },
      {
        (object) "35",
        (object) "Uva"
      },
      {
        (object) "36",
        (object) "Western"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LR", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bong"
      },
      {
        (object) "04",
        (object) "Grand Cape Mount"
      },
      {
        (object) "05",
        (object) "Lofa"
      },
      {
        (object) "06",
        (object) "Maryland"
      },
      {
        (object) "07",
        (object) "Monrovia"
      },
      {
        (object) "09",
        (object) "Nimba"
      },
      {
        (object) "10",
        (object) "Sino"
      },
      {
        (object) "11",
        (object) "Grand Bassa"
      },
      {
        (object) "12",
        (object) "Grand Cape Mount"
      },
      {
        (object) "13",
        (object) "Maryland"
      },
      {
        (object) "14",
        (object) "Montserrado"
      },
      {
        (object) "17",
        (object) "Margibi"
      },
      {
        (object) "18",
        (object) "River Cess"
      },
      {
        (object) "19",
        (object) "Grand Gedeh"
      },
      {
        (object) "20",
        (object) "Lofa"
      },
      {
        (object) "21",
        (object) "Gbarpolu"
      },
      {
        (object) "22",
        (object) "River Gee"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LS", (object) new Hashtable()
    {
      {
        (object) "10",
        (object) "Berea"
      },
      {
        (object) "11",
        (object) "Butha-Buthe"
      },
      {
        (object) "12",
        (object) "Leribe"
      },
      {
        (object) "13",
        (object) "Mafeteng"
      },
      {
        (object) "14",
        (object) "Maseru"
      },
      {
        (object) "15",
        (object) "Mohales Hoek"
      },
      {
        (object) "16",
        (object) "Mokhotlong"
      },
      {
        (object) "17",
        (object) "Qachas Nek"
      },
      {
        (object) "18",
        (object) "Quthing"
      },
      {
        (object) "19",
        (object) "Thaba-Tseka"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LT", (object) new Hashtable()
    {
      {
        (object) "56",
        (object) "Alytaus Apskritis"
      },
      {
        (object) "57",
        (object) "Kauno Apskritis"
      },
      {
        (object) "58",
        (object) "Klaipedos Apskritis"
      },
      {
        (object) "59",
        (object) "Marijampoles Apskritis"
      },
      {
        (object) "60",
        (object) "Panevezio Apskritis"
      },
      {
        (object) "61",
        (object) "Siauliu Apskritis"
      },
      {
        (object) "62",
        (object) "Taurages Apskritis"
      },
      {
        (object) "63",
        (object) "Telsiu Apskritis"
      },
      {
        (object) "64",
        (object) "Utenos Apskritis"
      },
      {
        (object) "65",
        (object) "Vilniaus Apskritis"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LU", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Diekirch"
      },
      {
        (object) "02",
        (object) "Grevenmacher"
      },
      {
        (object) "03",
        (object) "Luxembourg"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LV", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Aizkraukles"
      },
      {
        (object) "02",
        (object) "Aluksnes"
      },
      {
        (object) "03",
        (object) "Balvu"
      },
      {
        (object) "04",
        (object) "Bauskas"
      },
      {
        (object) "05",
        (object) "Cesu"
      },
      {
        (object) "06",
        (object) "Daugavpils"
      },
      {
        (object) "07",
        (object) "Daugavpils"
      },
      {
        (object) "08",
        (object) "Dobeles"
      },
      {
        (object) "09",
        (object) "Gulbenes"
      },
      {
        (object) "10",
        (object) "Jekabpils"
      },
      {
        (object) "11",
        (object) "Jelgava"
      },
      {
        (object) "12",
        (object) "Jelgavas"
      },
      {
        (object) "13",
        (object) "Jurmala"
      },
      {
        (object) "14",
        (object) "Kraslavas"
      },
      {
        (object) "15",
        (object) "Kuldigas"
      },
      {
        (object) "16",
        (object) "Liepaja"
      },
      {
        (object) "17",
        (object) "Liepajas"
      },
      {
        (object) "18",
        (object) "Limbazu"
      },
      {
        (object) "19",
        (object) "Ludzas"
      },
      {
        (object) "20",
        (object) "Madonas"
      },
      {
        (object) "21",
        (object) "Ogres"
      },
      {
        (object) "22",
        (object) "Preilu"
      },
      {
        (object) "23",
        (object) "Rezekne"
      },
      {
        (object) "24",
        (object) "Rezeknes"
      },
      {
        (object) "25",
        (object) "Riga"
      },
      {
        (object) "26",
        (object) "Rigas"
      },
      {
        (object) "27",
        (object) "Saldus"
      },
      {
        (object) "28",
        (object) "Talsu"
      },
      {
        (object) "29",
        (object) "Tukuma"
      },
      {
        (object) "30",
        (object) "Valkas"
      },
      {
        (object) "31",
        (object) "Valmieras"
      },
      {
        (object) "32",
        (object) "Ventspils"
      },
      {
        (object) "33",
        (object) "Ventspils"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "LY", (object) new Hashtable()
    {
      {
        (object) "03",
        (object) "Al Aziziyah"
      },
      {
        (object) "05",
        (object) "Al Jufrah"
      },
      {
        (object) "08",
        (object) "Al Kufrah"
      },
      {
        (object) "13",
        (object) "Ash Shati'"
      },
      {
        (object) "30",
        (object) "Murzuq"
      },
      {
        (object) "34",
        (object) "Sabha"
      },
      {
        (object) "41",
        (object) "Tarhunah"
      },
      {
        (object) "42",
        (object) "Tubruq"
      },
      {
        (object) "45",
        (object) "Zlitan"
      },
      {
        (object) "47",
        (object) "Ajdabiya"
      },
      {
        (object) "48",
        (object) "Al Fatih"
      },
      {
        (object) "49",
        (object) "Al Jabal al Akhdar"
      },
      {
        (object) "50",
        (object) "Al Khums"
      },
      {
        (object) "51",
        (object) "An Nuqat al Khams"
      },
      {
        (object) "52",
        (object) "Awbari"
      },
      {
        (object) "53",
        (object) "Az Zawiyah"
      },
      {
        (object) "54",
        (object) "Banghazi"
      },
      {
        (object) "55",
        (object) "Darnah"
      },
      {
        (object) "56",
        (object) "Ghadamis"
      },
      {
        (object) "57",
        (object) "Gharyan"
      },
      {
        (object) "58",
        (object) "Misratah"
      },
      {
        (object) "59",
        (object) "Sawfajjin"
      },
      {
        (object) "60",
        (object) "Surt"
      },
      {
        (object) "61",
        (object) "Tarabulus"
      },
      {
        (object) "62",
        (object) "Yafran"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MA", (object) new Hashtable()
    {
      {
        (object) "45",
        (object) "Grand Casablanca"
      },
      {
        (object) "46",
        (object) "Fes-Boulemane"
      },
      {
        (object) "47",
        (object) "Marrakech-Tensift-Al Haouz"
      },
      {
        (object) "48",
        (object) "Meknes-Tafilalet"
      },
      {
        (object) "49",
        (object) "Rabat-Sale-Zemmour-Zaer"
      },
      {
        (object) "50",
        (object) "Chaouia-Ouardigha"
      },
      {
        (object) "51",
        (object) "Doukkala-Abda"
      },
      {
        (object) "52",
        (object) "Gharb-Chrarda-Beni Hssen"
      },
      {
        (object) "53",
        (object) "Guelmim-Es Smara"
      },
      {
        (object) "54",
        (object) "Oriental"
      },
      {
        (object) "55",
        (object) "Souss-Massa-Dr,a"
      },
      {
        (object) "56",
        (object) "Tadla-Azilal"
      },
      {
        (object) "57",
        (object) "Tanger-Tetouan"
      },
      {
        (object) "58",
        (object) "Taza-Al Hoceima-Taounate"
      },
      {
        (object) "59",
        (object) "La,youne-Boujdour-Sakia El Hamra"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MC", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "La Condamine"
      },
      {
        (object) "02",
        (object) "Monaco"
      },
      {
        (object) "03",
        (object) "Monte-Carlo"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MD", (object) new Hashtable()
    {
      {
        (object) "51",
        (object) "Gagauzia"
      },
      {
        (object) "57",
        (object) "Chisinau"
      },
      {
        (object) "58",
        (object) "Stinga Nistrului"
      },
      {
        (object) "59",
        (object) "Anenii Noi"
      },
      {
        (object) "60",
        (object) "Balti"
      },
      {
        (object) "61",
        (object) "Basarabeasca"
      },
      {
        (object) "62",
        (object) "Bender"
      },
      {
        (object) "63",
        (object) "Briceni"
      },
      {
        (object) "64",
        (object) "Cahul"
      },
      {
        (object) "65",
        (object) "Cantemir"
      },
      {
        (object) "66",
        (object) "Calarasi"
      },
      {
        (object) "67",
        (object) "Causeni"
      },
      {
        (object) "68",
        (object) "Cimislia"
      },
      {
        (object) "69",
        (object) "Criuleni"
      },
      {
        (object) "70",
        (object) "Donduseni"
      },
      {
        (object) "71",
        (object) "Drochia"
      },
      {
        (object) "72",
        (object) "Dubasari"
      },
      {
        (object) "73",
        (object) "Edinet"
      },
      {
        (object) "74",
        (object) "Falesti"
      },
      {
        (object) "75",
        (object) "Floresti"
      },
      {
        (object) "76",
        (object) "Glodeni"
      },
      {
        (object) "77",
        (object) "Hincesti"
      },
      {
        (object) "78",
        (object) "Ialoveni"
      },
      {
        (object) "79",
        (object) "Leova"
      },
      {
        (object) "80",
        (object) "Nisporeni"
      },
      {
        (object) "81",
        (object) "Ocnita"
      },
      {
        (object) "82",
        (object) "Orhei"
      },
      {
        (object) "83",
        (object) "Rezina"
      },
      {
        (object) "84",
        (object) "Riscani"
      },
      {
        (object) "85",
        (object) "Singerei"
      },
      {
        (object) "86",
        (object) "Soldanesti"
      },
      {
        (object) "87",
        (object) "Soroca"
      },
      {
        (object) "88",
        (object) "Stefan-Voda"
      },
      {
        (object) "89",
        (object) "Straseni"
      },
      {
        (object) "90",
        (object) "Taraclia"
      },
      {
        (object) "91",
        (object) "Telenesti"
      },
      {
        (object) "92",
        (object) "Ungheni"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MG", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Antsiranana"
      },
      {
        (object) "02",
        (object) "Fianarantsoa"
      },
      {
        (object) "03",
        (object) "Mahajanga"
      },
      {
        (object) "04",
        (object) "Toamasina"
      },
      {
        (object) "05",
        (object) "Antananarivo"
      },
      {
        (object) "06",
        (object) "Toliara"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MK", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Aracinovo"
      },
      {
        (object) "02",
        (object) "Bac"
      },
      {
        (object) "03",
        (object) "Belcista"
      },
      {
        (object) "04",
        (object) "Berovo"
      },
      {
        (object) "05",
        (object) "Bistrica"
      },
      {
        (object) "06",
        (object) "Bitola"
      },
      {
        (object) "07",
        (object) "Blatec"
      },
      {
        (object) "08",
        (object) "Bogdanci"
      },
      {
        (object) "09",
        (object) "Bogomila"
      },
      {
        (object) "10",
        (object) "Bogovinje"
      },
      {
        (object) "11",
        (object) "Bosilovo"
      },
      {
        (object) "12",
        (object) "Brvenica"
      },
      {
        (object) "13",
        (object) "Cair"
      },
      {
        (object) "14",
        (object) "Capari"
      },
      {
        (object) "15",
        (object) "Caska"
      },
      {
        (object) "16",
        (object) "Cegrane"
      },
      {
        (object) "17",
        (object) "Centar"
      },
      {
        (object) "18",
        (object) "Centar Zupa"
      },
      {
        (object) "19",
        (object) "Cesinovo"
      },
      {
        (object) "20",
        (object) "Cucer-Sandevo"
      },
      {
        (object) "21",
        (object) "Debar"
      },
      {
        (object) "22",
        (object) "Delcevo"
      },
      {
        (object) "23",
        (object) "Delogozdi"
      },
      {
        (object) "24",
        (object) "Demir Hisar"
      },
      {
        (object) "25",
        (object) "Demir Kapija"
      },
      {
        (object) "26",
        (object) "Dobrusevo"
      },
      {
        (object) "27",
        (object) "Dolna Banjica"
      },
      {
        (object) "28",
        (object) "Dolneni"
      },
      {
        (object) "29",
        (object) "Dorce Petrov"
      },
      {
        (object) "30",
        (object) "Drugovo"
      },
      {
        (object) "31",
        (object) "Dzepciste"
      },
      {
        (object) "32",
        (object) "Gazi Baba"
      },
      {
        (object) "33",
        (object) "Gevgelija"
      },
      {
        (object) "34",
        (object) "Gostivar"
      },
      {
        (object) "35",
        (object) "Gradsko"
      },
      {
        (object) "36",
        (object) "Ilinden"
      },
      {
        (object) "37",
        (object) "Izvor"
      },
      {
        (object) "38",
        (object) "Jegunovce"
      },
      {
        (object) "39",
        (object) "Kamenjane"
      },
      {
        (object) "40",
        (object) "Karbinci"
      },
      {
        (object) "41",
        (object) "Karpos"
      },
      {
        (object) "42",
        (object) "Kavadarci"
      },
      {
        (object) "43",
        (object) "Kicevo"
      },
      {
        (object) "44",
        (object) "Kisela Voda"
      },
      {
        (object) "45",
        (object) "Klecevce"
      },
      {
        (object) "46",
        (object) "Kocani"
      },
      {
        (object) "47",
        (object) "Konce"
      },
      {
        (object) "48",
        (object) "Kondovo"
      },
      {
        (object) "49",
        (object) "Konopiste"
      },
      {
        (object) "50",
        (object) "Kosel"
      },
      {
        (object) "51",
        (object) "Kratovo"
      },
      {
        (object) "52",
        (object) "Kriva Palanka"
      },
      {
        (object) "53",
        (object) "Krivogastani"
      },
      {
        (object) "54",
        (object) "Krusevo"
      },
      {
        (object) "55",
        (object) "Kuklis"
      },
      {
        (object) "56",
        (object) "Kukurecani"
      },
      {
        (object) "57",
        (object) "Kumanovo"
      },
      {
        (object) "58",
        (object) "Labunista"
      },
      {
        (object) "59",
        (object) "Lipkovo"
      },
      {
        (object) "60",
        (object) "Lozovo"
      },
      {
        (object) "61",
        (object) "Lukovo"
      },
      {
        (object) "62",
        (object) "Makedonska Kamenica"
      },
      {
        (object) "63",
        (object) "Makedonski Brod"
      },
      {
        (object) "64",
        (object) "Mavrovi Anovi"
      },
      {
        (object) "65",
        (object) "Meseista"
      },
      {
        (object) "66",
        (object) "Miravci"
      },
      {
        (object) "67",
        (object) "Mogila"
      },
      {
        (object) "68",
        (object) "Murtino"
      },
      {
        (object) "69",
        (object) "Negotino"
      },
      {
        (object) "70",
        (object) "Negotino-Polosko"
      },
      {
        (object) "71",
        (object) "Novaci"
      },
      {
        (object) "72",
        (object) "Novo Selo"
      },
      {
        (object) "73",
        (object) "Oblesevo"
      },
      {
        (object) "74",
        (object) "Ohrid"
      },
      {
        (object) "75",
        (object) "Orasac"
      },
      {
        (object) "76",
        (object) "Orizari"
      },
      {
        (object) "77",
        (object) "Oslomej"
      },
      {
        (object) "78",
        (object) "Pehcevo"
      },
      {
        (object) "79",
        (object) "Petrovec"
      },
      {
        (object) "80",
        (object) "Plasnica"
      },
      {
        (object) "81",
        (object) "Podares"
      },
      {
        (object) "82",
        (object) "Prilep"
      },
      {
        (object) "83",
        (object) "Probistip"
      },
      {
        (object) "84",
        (object) "Radovis"
      },
      {
        (object) "85",
        (object) "Rankovce"
      },
      {
        (object) "86",
        (object) "Resen"
      },
      {
        (object) "87",
        (object) "Rosoman"
      },
      {
        (object) "88",
        (object) "Rostusa"
      },
      {
        (object) "89",
        (object) "Samokov"
      },
      {
        (object) "90",
        (object) "Saraj"
      },
      {
        (object) "91",
        (object) "Sipkovica"
      },
      {
        (object) "92",
        (object) "Sopiste"
      },
      {
        (object) "93",
        (object) "Sopotnica"
      },
      {
        (object) "94",
        (object) "Srbinovo"
      },
      {
        (object) "95",
        (object) "Staravina"
      },
      {
        (object) "96",
        (object) "Star Dojran"
      },
      {
        (object) "97",
        (object) "Staro Nagoricane"
      },
      {
        (object) "98",
        (object) "Stip"
      },
      {
        (object) "99",
        (object) "Struga"
      },
      {
        (object) "A1",
        (object) "Strumica"
      },
      {
        (object) "A2",
        (object) "Studenicani"
      },
      {
        (object) "A3",
        (object) "Suto Orizari"
      },
      {
        (object) "A4",
        (object) "Sveti Nikole"
      },
      {
        (object) "A5",
        (object) "Tearce"
      },
      {
        (object) "A6",
        (object) "Tetovo"
      },
      {
        (object) "A7",
        (object) "Topolcani"
      },
      {
        (object) "A8",
        (object) "Valandovo"
      },
      {
        (object) "A9",
        (object) "Vasilevo"
      },
      {
        (object) "B1",
        (object) "Veles"
      },
      {
        (object) "B2",
        (object) "Velesta"
      },
      {
        (object) "B3",
        (object) "Vevcani"
      },
      {
        (object) "B4",
        (object) "Vinica"
      },
      {
        (object) "B5",
        (object) "Vitoliste"
      },
      {
        (object) "B6",
        (object) "Vranestica"
      },
      {
        (object) "B7",
        (object) "Vrapciste"
      },
      {
        (object) "B8",
        (object) "Vratnica"
      },
      {
        (object) "B9",
        (object) "Vrutok"
      },
      {
        (object) "C1",
        (object) "Zajas"
      },
      {
        (object) "C2",
        (object) "Zelenikovo"
      },
      {
        (object) "C3",
        (object) "Zelino"
      },
      {
        (object) "C4",
        (object) "Zitose"
      },
      {
        (object) "C5",
        (object) "Zletovo"
      },
      {
        (object) "C6",
        (object) "Zrnovci"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "ML", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bamako"
      },
      {
        (object) "03",
        (object) "Kayes"
      },
      {
        (object) "04",
        (object) "Mopti"
      },
      {
        (object) "05",
        (object) "Segou"
      },
      {
        (object) "06",
        (object) "Sikasso"
      },
      {
        (object) "07",
        (object) "Koulikoro"
      },
      {
        (object) "08",
        (object) "Tombouctou"
      },
      {
        (object) "09",
        (object) "Gao"
      },
      {
        (object) "10",
        (object) "Kidal"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Rakhine State"
      },
      {
        (object) "02",
        (object) "Chin State"
      },
      {
        (object) "03",
        (object) "Irrawaddy"
      },
      {
        (object) "04",
        (object) "Kachin State"
      },
      {
        (object) "05",
        (object) "Karan State"
      },
      {
        (object) "06",
        (object) "Kayah State"
      },
      {
        (object) "07",
        (object) "Magwe"
      },
      {
        (object) "08",
        (object) "Mandalay"
      },
      {
        (object) "09",
        (object) "Pegu"
      },
      {
        (object) "10",
        (object) "Sagaing"
      },
      {
        (object) "11",
        (object) "Shan State"
      },
      {
        (object) "12",
        (object) "Tenasserim"
      },
      {
        (object) "13",
        (object) "Mon State"
      },
      {
        (object) "14",
        (object) "Rangoon"
      },
      {
        (object) "17",
        (object) "Yangon"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MN", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Arhangay"
      },
      {
        (object) "02",
        (object) "Bayanhongor"
      },
      {
        (object) "03",
        (object) "Bayan-Olgiy"
      },
      {
        (object) "05",
        (object) "Darhan"
      },
      {
        (object) "06",
        (object) "Dornod"
      },
      {
        (object) "07",
        (object) "Dornogovi"
      },
      {
        (object) "08",
        (object) "Dundgovi"
      },
      {
        (object) "09",
        (object) "Dzavhan"
      },
      {
        (object) "10",
        (object) "Govi-Altay"
      },
      {
        (object) "11",
        (object) "Hentiy"
      },
      {
        (object) "12",
        (object) "Hovd"
      },
      {
        (object) "13",
        (object) "Hovsgol"
      },
      {
        (object) "14",
        (object) "Omnogovi"
      },
      {
        (object) "15",
        (object) "Ovorhangay"
      },
      {
        (object) "16",
        (object) "Selenge"
      },
      {
        (object) "17",
        (object) "Suhbaatar"
      },
      {
        (object) "18",
        (object) "Tov"
      },
      {
        (object) "19",
        (object) "Uvs"
      },
      {
        (object) "20",
        (object) "Ulaanbaatar"
      },
      {
        (object) "21",
        (object) "Bulgan"
      },
      {
        (object) "22",
        (object) "Erdenet"
      },
      {
        (object) "23",
        (object) "Darhan-Uul"
      },
      {
        (object) "24",
        (object) "Govisumber"
      },
      {
        (object) "25",
        (object) "Orhon"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MO", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Ilhas"
      },
      {
        (object) "02",
        (object) "Macau"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MR", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Hodh Ech Chargui"
      },
      {
        (object) "02",
        (object) "Hodh El Gharbi"
      },
      {
        (object) "03",
        (object) "Assaba"
      },
      {
        (object) "04",
        (object) "Gorgol"
      },
      {
        (object) "05",
        (object) "Brakna"
      },
      {
        (object) "06",
        (object) "Trarza"
      },
      {
        (object) "07",
        (object) "Adrar"
      },
      {
        (object) "08",
        (object) "Dakhlet Nouadhibou"
      },
      {
        (object) "09",
        (object) "Tagant"
      },
      {
        (object) "10",
        (object) "Guidimaka"
      },
      {
        (object) "11",
        (object) "Tiris Zemmour"
      },
      {
        (object) "12",
        (object) "Inchiri"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MS", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Saint Anthony"
      },
      {
        (object) "02",
        (object) "Saint Georges"
      },
      {
        (object) "03",
        (object) "Saint Peter"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MU", (object) new Hashtable()
    {
      {
        (object) "12",
        (object) "Black River"
      },
      {
        (object) "13",
        (object) "Flacq"
      },
      {
        (object) "14",
        (object) "Grand Port"
      },
      {
        (object) "15",
        (object) "Moka"
      },
      {
        (object) "16",
        (object) "Pamplemousses"
      },
      {
        (object) "17",
        (object) "Plaines Wilhems"
      },
      {
        (object) "18",
        (object) "Port Louis"
      },
      {
        (object) "19",
        (object) "Riviere du Rempart"
      },
      {
        (object) "20",
        (object) "Savanne"
      },
      {
        (object) "21",
        (object) "Agalega Islands"
      },
      {
        (object) "22",
        (object) "Cargados Carajos"
      },
      {
        (object) "23",
        (object) "Rodrigues"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MV", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Seenu"
      },
      {
        (object) "05",
        (object) "Laamu"
      },
      {
        (object) "30",
        (object) "Alifu"
      },
      {
        (object) "31",
        (object) "Baa"
      },
      {
        (object) "32",
        (object) "Dhaalu"
      },
      {
        (object) "33",
        (object) "Faafu "
      },
      {
        (object) "34",
        (object) "Gaafu Alifu"
      },
      {
        (object) "35",
        (object) "Gaafu Dhaalu"
      },
      {
        (object) "36",
        (object) "Haa Alifu"
      },
      {
        (object) "37",
        (object) "Haa Dhaalu"
      },
      {
        (object) "38",
        (object) "Kaafu"
      },
      {
        (object) "39",
        (object) "Lhaviyani"
      },
      {
        (object) "40",
        (object) "Maale"
      },
      {
        (object) "41",
        (object) "Meemu"
      },
      {
        (object) "42",
        (object) "Gnaviyani"
      },
      {
        (object) "43",
        (object) "Noonu"
      },
      {
        (object) "44",
        (object) "Raa"
      },
      {
        (object) "45",
        (object) "Shaviyani"
      },
      {
        (object) "46",
        (object) "Thaa"
      },
      {
        (object) "47",
        (object) "Vaavu"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MW", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Chikwawa"
      },
      {
        (object) "03",
        (object) "Chiradzulu"
      },
      {
        (object) "04",
        (object) "Chitipa"
      },
      {
        (object) "05",
        (object) "Thyolo"
      },
      {
        (object) "06",
        (object) "Dedza"
      },
      {
        (object) "07",
        (object) "Dowa"
      },
      {
        (object) "08",
        (object) "Karonga"
      },
      {
        (object) "09",
        (object) "Kasungu"
      },
      {
        (object) "11",
        (object) "Lilongwe"
      },
      {
        (object) "12",
        (object) "Mangochi"
      },
      {
        (object) "13",
        (object) "Mchinji"
      },
      {
        (object) "15",
        (object) "Mzimba"
      },
      {
        (object) "16",
        (object) "Ntcheu"
      },
      {
        (object) "17",
        (object) "Nkhata Bay"
      },
      {
        (object) "18",
        (object) "Nkhotakota"
      },
      {
        (object) "19",
        (object) "Nsanje"
      },
      {
        (object) "20",
        (object) "Ntchisi"
      },
      {
        (object) "21",
        (object) "Rumphi"
      },
      {
        (object) "22",
        (object) "Salima"
      },
      {
        (object) "23",
        (object) "Zomba"
      },
      {
        (object) "24",
        (object) "Blantyre"
      },
      {
        (object) "25",
        (object) "Mwanza"
      },
      {
        (object) "26",
        (object) "Balaka"
      },
      {
        (object) "27",
        (object) "Likoma"
      },
      {
        (object) "28",
        (object) "Machinga"
      },
      {
        (object) "29",
        (object) "Mulanje"
      },
      {
        (object) "30",
        (object) "Phalombe"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MX", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Aguascalientes"
      },
      {
        (object) "02",
        (object) "Baja California"
      },
      {
        (object) "03",
        (object) "Baja California Sur"
      },
      {
        (object) "04",
        (object) "Campeche"
      },
      {
        (object) "05",
        (object) "Chiapas"
      },
      {
        (object) "06",
        (object) "Chihuahua"
      },
      {
        (object) "07",
        (object) "Coahuila de Zaragoza"
      },
      {
        (object) "08",
        (object) "Colima"
      },
      {
        (object) "09",
        (object) "Distrito Federal"
      },
      {
        (object) "10",
        (object) "Durango"
      },
      {
        (object) "11",
        (object) "Guanajuato"
      },
      {
        (object) "12",
        (object) "Guerrero"
      },
      {
        (object) "13",
        (object) "Hidalgo"
      },
      {
        (object) "14",
        (object) "Jalisco"
      },
      {
        (object) "15",
        (object) "Mexico"
      },
      {
        (object) "16",
        (object) "Michoacan de Ocampo"
      },
      {
        (object) "17",
        (object) "Morelos"
      },
      {
        (object) "18",
        (object) "Nayarit"
      },
      {
        (object) "19",
        (object) "Nuevo Leon"
      },
      {
        (object) "20",
        (object) "Oaxaca"
      },
      {
        (object) "21",
        (object) "Puebla"
      },
      {
        (object) "22",
        (object) "Queretaro de Arteaga"
      },
      {
        (object) "23",
        (object) "Qintana Roo"
      },
      {
        (object) "24",
        (object) "San Luis Potosi"
      },
      {
        (object) "25",
        (object) "Sinaloa"
      },
      {
        (object) "26",
        (object) "Sonora"
      },
      {
        (object) "27",
        (object) "Tabasco"
      },
      {
        (object) "28",
        (object) "Tamaulipas"
      },
      {
        (object) "29",
        (object) "Tlaxcala"
      },
      {
        (object) "30",
        (object) "Veracruz-Llave"
      },
      {
        (object) "31",
        (object) "Yucatan"
      },
      {
        (object) "32",
        (object) "Zacatecas"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MY", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Johor"
      },
      {
        (object) "02",
        (object) "Kedah"
      },
      {
        (object) "03",
        (object) "Kelantan"
      },
      {
        (object) "04",
        (object) "Melaka"
      },
      {
        (object) "05",
        (object) "Negeri Sembilan"
      },
      {
        (object) "06",
        (object) "Pahang"
      },
      {
        (object) "07",
        (object) "Perak"
      },
      {
        (object) "08",
        (object) "Perlis"
      },
      {
        (object) "09",
        (object) "Pulau Pinang"
      },
      {
        (object) "11",
        (object) "Sarawak"
      },
      {
        (object) "12",
        (object) "Selangor"
      },
      {
        (object) "13",
        (object) "Terengganu"
      },
      {
        (object) "14",
        (object) "Kuala Lumpur"
      },
      {
        (object) "15",
        (object) "Labuan"
      },
      {
        (object) "16",
        (object) "Sabah"
      },
      {
        (object) "17",
        (object) "Putrajaya"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "MZ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Cabo Delgado"
      },
      {
        (object) "02",
        (object) "Gaza"
      },
      {
        (object) "03",
        (object) "Inhambane"
      },
      {
        (object) "04",
        (object) "Maputo"
      },
      {
        (object) "05",
        (object) "Sofala"
      },
      {
        (object) "06",
        (object) "Nampula"
      },
      {
        (object) "07",
        (object) "Niassa"
      },
      {
        (object) "08",
        (object) "Tete"
      },
      {
        (object) "09",
        (object) "Zambezia"
      },
      {
        (object) "10",
        (object) "Manica"
      },
      {
        (object) "11",
        (object) "Maputo"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "NA", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bethanien"
      },
      {
        (object) "02",
        (object) "Caprivi Oos"
      },
      {
        (object) "03",
        (object) "Boesmanland"
      },
      {
        (object) "04",
        (object) "Gobabis"
      },
      {
        (object) "05",
        (object) "Grootfontein"
      },
      {
        (object) "06",
        (object) "Kaokoland"
      },
      {
        (object) "07",
        (object) "Karibib"
      },
      {
        (object) "08",
        (object) "Keetmanshoop"
      },
      {
        (object) "09",
        (object) "Luderitz"
      },
      {
        (object) "10",
        (object) "Maltahohe"
      },
      {
        (object) "11",
        (object) "Okahandja"
      },
      {
        (object) "12",
        (object) "Omaruru"
      },
      {
        (object) "13",
        (object) "Otjiwarongo"
      },
      {
        (object) "14",
        (object) "Outjo"
      },
      {
        (object) "15",
        (object) "Owambo"
      },
      {
        (object) "16",
        (object) "Rehoboth"
      },
      {
        (object) "17",
        (object) "Swakopmund"
      },
      {
        (object) "18",
        (object) "Tsumeb"
      },
      {
        (object) "20",
        (object) "Karasburg"
      },
      {
        (object) "21",
        (object) "Windhoek"
      },
      {
        (object) "22",
        (object) "Damaraland"
      },
      {
        (object) "23",
        (object) "Hereroland Oos"
      },
      {
        (object) "24",
        (object) "Hereroland Wes"
      },
      {
        (object) "25",
        (object) "Kavango"
      },
      {
        (object) "26",
        (object) "Mariental"
      },
      {
        (object) "27",
        (object) "Namaland"
      },
      {
        (object) "28",
        (object) "Caprivi"
      },
      {
        (object) "29",
        (object) "Erongo"
      },
      {
        (object) "30",
        (object) "Hardap"
      },
      {
        (object) "31",
        (object) "Karas"
      },
      {
        (object) "32",
        (object) "Kunene"
      },
      {
        (object) "33",
        (object) "Ohangwena"
      },
      {
        (object) "34",
        (object) "Okavango"
      },
      {
        (object) "35",
        (object) "Omaheke"
      },
      {
        (object) "36",
        (object) "Omusati"
      },
      {
        (object) "37",
        (object) "Oshana"
      },
      {
        (object) "38",
        (object) "Oshikoto"
      },
      {
        (object) "39",
        (object) "Otjozondjupa"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "NE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Agadez"
      },
      {
        (object) "02",
        (object) "Diffa"
      },
      {
        (object) "03",
        (object) "Dosso"
      },
      {
        (object) "04",
        (object) "Maradi"
      },
      {
        (object) "05",
        (object) "Niamey"
      },
      {
        (object) "06",
        (object) "Tahoua"
      },
      {
        (object) "07",
        (object) "Zinder"
      },
      {
        (object) "08",
        (object) "Niamey"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "NG", (object) new Hashtable()
    {
      {
        (object) "05",
        (object) "Lagos"
      },
      {
        (object) "11",
        (object) "Federal Capital Territory"
      },
      {
        (object) "16",
        (object) "Ogun"
      },
      {
        (object) "21",
        (object) "Akwa Ibom"
      },
      {
        (object) "22",
        (object) "Cross River"
      },
      {
        (object) "23",
        (object) "Kaduna"
      },
      {
        (object) "24",
        (object) "Katsina"
      },
      {
        (object) "25",
        (object) "Anambra"
      },
      {
        (object) "26",
        (object) "Benue"
      },
      {
        (object) "27",
        (object) "Borno"
      },
      {
        (object) "28",
        (object) "Imo"
      },
      {
        (object) "29",
        (object) "Kano"
      },
      {
        (object) "30",
        (object) "Kwara"
      },
      {
        (object) "31",
        (object) "Niger"
      },
      {
        (object) "32",
        (object) "Oyo"
      },
      {
        (object) "35",
        (object) "Adamawa"
      },
      {
        (object) "36",
        (object) "Delta"
      },
      {
        (object) "37",
        (object) "Edo"
      },
      {
        (object) "39",
        (object) "Jigawa"
      },
      {
        (object) "40",
        (object) "Kebbi"
      },
      {
        (object) "41",
        (object) "Kogi"
      },
      {
        (object) "42",
        (object) "Osun"
      },
      {
        (object) "43",
        (object) "Taraba"
      },
      {
        (object) "44",
        (object) "Yobe"
      },
      {
        (object) "45",
        (object) "Abia"
      },
      {
        (object) "46",
        (object) "Bauchi"
      },
      {
        (object) "47",
        (object) "Enugu"
      },
      {
        (object) "48",
        (object) "Ondo"
      },
      {
        (object) "49",
        (object) "Plateau"
      },
      {
        (object) "50",
        (object) "Rivers"
      },
      {
        (object) "51",
        (object) "Sokoto"
      },
      {
        (object) "52",
        (object) "Bayelsa"
      },
      {
        (object) "53",
        (object) "Ebonyi"
      },
      {
        (object) "54",
        (object) "Ekiti"
      },
      {
        (object) "55",
        (object) "Gombe"
      },
      {
        (object) "56",
        (object) "Nassarawa"
      },
      {
        (object) "57",
        (object) "Zamfara"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "NI", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Boaco"
      },
      {
        (object) "02",
        (object) "Carazo"
      },
      {
        (object) "03",
        (object) "Chinandega"
      },
      {
        (object) "04",
        (object) "Chontales"
      },
      {
        (object) "05",
        (object) "Esteli"
      },
      {
        (object) "06",
        (object) "Granada"
      },
      {
        (object) "07",
        (object) "Jinotega"
      },
      {
        (object) "08",
        (object) "Leon"
      },
      {
        (object) "09",
        (object) "Madriz"
      },
      {
        (object) "10",
        (object) "Managua"
      },
      {
        (object) "11",
        (object) "Masaya"
      },
      {
        (object) "12",
        (object) "Matagalpa"
      },
      {
        (object) "13",
        (object) "Nueva Segovia"
      },
      {
        (object) "14",
        (object) "Rio San Juan"
      },
      {
        (object) "15",
        (object) "Rivas"
      },
      {
        (object) "16",
        (object) "Zelaya"
      },
      {
        (object) "17",
        (object) "Autonoma Atlantico Norte"
      },
      {
        (object) "18",
        (object) "Region Autonoma Atlantico Sur"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "NL", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Drenthe"
      },
      {
        (object) "02",
        (object) "Friesland"
      },
      {
        (object) "03",
        (object) "Gelderland"
      },
      {
        (object) "04",
        (object) "Groningen"
      },
      {
        (object) "05",
        (object) "Limburg"
      },
      {
        (object) "06",
        (object) "Noord-Brabant"
      },
      {
        (object) "07",
        (object) "Noord-Holland"
      },
      {
        (object) "09",
        (object) "Utrecht"
      },
      {
        (object) "10",
        (object) "Zeeland"
      },
      {
        (object) "11",
        (object) "Zuid-Holland"
      },
      {
        (object) "15",
        (object) "Overijssel"
      },
      {
        (object) "16",
        (object) "Flevoland"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "NO", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Akershus"
      },
      {
        (object) "02",
        (object) "Aust-Agder"
      },
      {
        (object) "04",
        (object) "Buskerud"
      },
      {
        (object) "05",
        (object) "Finnmark"
      },
      {
        (object) "06",
        (object) "Hedmark"
      },
      {
        (object) "07",
        (object) "Hordaland"
      },
      {
        (object) "08",
        (object) "More og Romsdal"
      },
      {
        (object) "09",
        (object) "Nordland"
      },
      {
        (object) "10",
        (object) "Nord-Trondelag"
      },
      {
        (object) "11",
        (object) "Oppland"
      },
      {
        (object) "12",
        (object) "Oslo"
      },
      {
        (object) "13",
        (object) "Ostfold"
      },
      {
        (object) "14",
        (object) "Rogaland"
      },
      {
        (object) "15",
        (object) "Sogn og Fjordane"
      },
      {
        (object) "16",
        (object) "Sor-Trondelag"
      },
      {
        (object) "17",
        (object) "Telemark"
      },
      {
        (object) "18",
        (object) "Troms"
      },
      {
        (object) "19",
        (object) "Vest-Agder"
      },
      {
        (object) "20",
        (object) "Vestfold"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "NP", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bagmati"
      },
      {
        (object) "02",
        (object) "Bheri"
      },
      {
        (object) "03",
        (object) "Dhawalagiri"
      },
      {
        (object) "04",
        (object) "Gandaki"
      },
      {
        (object) "05",
        (object) "Janakpur"
      },
      {
        (object) "06",
        (object) "Karnali"
      },
      {
        (object) "07",
        (object) "Kosi"
      },
      {
        (object) "08",
        (object) "Lumbini"
      },
      {
        (object) "09",
        (object) "Mahakali"
      },
      {
        (object) "10",
        (object) "Mechi"
      },
      {
        (object) "11",
        (object) "Narayani"
      },
      {
        (object) "12",
        (object) "Rapti"
      },
      {
        (object) "13",
        (object) "Sagarmatha"
      },
      {
        (object) "14",
        (object) "Seti"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "NR", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Aiwo"
      },
      {
        (object) "02",
        (object) "Anabar"
      },
      {
        (object) "03",
        (object) "Anetan"
      },
      {
        (object) "04",
        (object) "Anibare"
      },
      {
        (object) "05",
        (object) "Baiti"
      },
      {
        (object) "06",
        (object) "Boe"
      },
      {
        (object) "07",
        (object) "Buada"
      },
      {
        (object) "08",
        (object) "Denigomodu"
      },
      {
        (object) "09",
        (object) "Ewa"
      },
      {
        (object) "10",
        (object) "Ijuw"
      },
      {
        (object) "11",
        (object) "Meneng"
      },
      {
        (object) "12",
        (object) "Nibok"
      },
      {
        (object) "13",
        (object) "Uaboe"
      },
      {
        (object) "14",
        (object) "Yaren"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "NZ", (object) new Hashtable()
    {
      {
        (object) "10",
        (object) "Chatham Islands"
      },
      {
        (object) "E7",
        (object) "Auckland"
      },
      {
        (object) "E8",
        (object) "Bay of Plenty"
      },
      {
        (object) "E9",
        (object) "Canterbury"
      },
      {
        (object) "F1",
        (object) "Gisborne"
      },
      {
        (object) "F2",
        (object) "Hawke's Bay"
      },
      {
        (object) "F3",
        (object) "Manawatu-Wanganui"
      },
      {
        (object) "F4",
        (object) "Marlborough"
      },
      {
        (object) "F5",
        (object) "Nelson"
      },
      {
        (object) "F6",
        (object) "Northland"
      },
      {
        (object) "F7",
        (object) "Otago"
      },
      {
        (object) "F8",
        (object) "Southland"
      },
      {
        (object) "F9",
        (object) "Taranaki"
      },
      {
        (object) "G1",
        (object) "Waikato"
      },
      {
        (object) "G2",
        (object) "Wellington"
      },
      {
        (object) "G3",
        (object) "West Coast"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "OM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Ad Dakhiliyah"
      },
      {
        (object) "02",
        (object) "Al Batinah"
      },
      {
        (object) "03",
        (object) "Al Wusta"
      },
      {
        (object) "04",
        (object) "Ash Sharqiyah"
      },
      {
        (object) "05",
        (object) "Az Zahirah"
      },
      {
        (object) "06",
        (object) "Masqat"
      },
      {
        (object) "07",
        (object) "Musandam"
      },
      {
        (object) "08",
        (object) "Zufar"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "PA", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bocas del Toro"
      },
      {
        (object) "02",
        (object) "Chiriqui"
      },
      {
        (object) "03",
        (object) "Cocle"
      },
      {
        (object) "04",
        (object) "Colon"
      },
      {
        (object) "05",
        (object) "Darien"
      },
      {
        (object) "06",
        (object) "Herrera"
      },
      {
        (object) "07",
        (object) "Los Santos"
      },
      {
        (object) "08",
        (object) "Panama"
      },
      {
        (object) "09",
        (object) "San Blas"
      },
      {
        (object) "10",
        (object) "Veraguas"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "PE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Amazonas"
      },
      {
        (object) "02",
        (object) "Ancash"
      },
      {
        (object) "03",
        (object) "Apurimac"
      },
      {
        (object) "04",
        (object) "Arequipa"
      },
      {
        (object) "05",
        (object) "Ayacucho"
      },
      {
        (object) "06",
        (object) "Cajamarca"
      },
      {
        (object) "07",
        (object) "Callao"
      },
      {
        (object) "08",
        (object) "Cusco"
      },
      {
        (object) "09",
        (object) "Huancavelica"
      },
      {
        (object) "10",
        (object) "Huanuco"
      },
      {
        (object) "11",
        (object) "Ica"
      },
      {
        (object) "12",
        (object) "Junin"
      },
      {
        (object) "13",
        (object) "La Libertad"
      },
      {
        (object) "14",
        (object) "Lambayeque"
      },
      {
        (object) "15",
        (object) "Lima"
      },
      {
        (object) "16",
        (object) "Loreto"
      },
      {
        (object) "17",
        (object) "Madre de Dios"
      },
      {
        (object) "18",
        (object) "Moquegua"
      },
      {
        (object) "19",
        (object) "Pasco"
      },
      {
        (object) "20",
        (object) "Piura"
      },
      {
        (object) "21",
        (object) "Puno"
      },
      {
        (object) "22",
        (object) "San Martin"
      },
      {
        (object) "23",
        (object) "Tacna"
      },
      {
        (object) "24",
        (object) "Tumbes"
      },
      {
        (object) "25",
        (object) "Ucayali"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "PG", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Central"
      },
      {
        (object) "02",
        (object) "Gulf"
      },
      {
        (object) "03",
        (object) "Milne Bay"
      },
      {
        (object) "04",
        (object) "Northern"
      },
      {
        (object) "05",
        (object) "Southern Highlands"
      },
      {
        (object) "06",
        (object) "Western"
      },
      {
        (object) "07",
        (object) "North Solomons"
      },
      {
        (object) "08",
        (object) "Chimbu"
      },
      {
        (object) "09",
        (object) "Eastern Highlands"
      },
      {
        (object) "10",
        (object) "East New Britain"
      },
      {
        (object) "11",
        (object) "East Sepik"
      },
      {
        (object) "12",
        (object) "Madang"
      },
      {
        (object) "13",
        (object) "Manus"
      },
      {
        (object) "14",
        (object) "Morobe"
      },
      {
        (object) "15",
        (object) "New Ireland"
      },
      {
        (object) "16",
        (object) "Western Highlands"
      },
      {
        (object) "17",
        (object) "West New Britain"
      },
      {
        (object) "18",
        (object) "Sandaun"
      },
      {
        (object) "19",
        (object) "Enga"
      },
      {
        (object) "20",
        (object) "National Capital"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "PH", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Abra"
      },
      {
        (object) "02",
        (object) "Agusan del Norte"
      },
      {
        (object) "03",
        (object) "Agusan del Sur"
      },
      {
        (object) "04",
        (object) "Aklan"
      },
      {
        (object) "05",
        (object) "Albay"
      },
      {
        (object) "06",
        (object) "Antique"
      },
      {
        (object) "07",
        (object) "Bataan"
      },
      {
        (object) "08",
        (object) "Batanes"
      },
      {
        (object) "09",
        (object) "Batangas"
      },
      {
        (object) "10",
        (object) "Benguet"
      },
      {
        (object) "11",
        (object) "Bohol"
      },
      {
        (object) "12",
        (object) "Bukidnon"
      },
      {
        (object) "13",
        (object) "Bulacan"
      },
      {
        (object) "14",
        (object) "Cagayan"
      },
      {
        (object) "15",
        (object) "Camarines Norte"
      },
      {
        (object) "16",
        (object) "Camarines Sur"
      },
      {
        (object) "17",
        (object) "Camiguin"
      },
      {
        (object) "18",
        (object) "Capiz"
      },
      {
        (object) "19",
        (object) "Catanduanes"
      },
      {
        (object) "20",
        (object) "Cavite"
      },
      {
        (object) "21",
        (object) "Cebu"
      },
      {
        (object) "22",
        (object) "Basilan"
      },
      {
        (object) "23",
        (object) "Eastern Samar"
      },
      {
        (object) "24",
        (object) "Davao"
      },
      {
        (object) "25",
        (object) "Davao del Sur"
      },
      {
        (object) "26",
        (object) "Davao Oriental"
      },
      {
        (object) "27",
        (object) "Ifugao"
      },
      {
        (object) "28",
        (object) "Ilocos Norte"
      },
      {
        (object) "29",
        (object) "Ilocos Sur"
      },
      {
        (object) "30",
        (object) "Iloilo"
      },
      {
        (object) "31",
        (object) "Isabela"
      },
      {
        (object) "32",
        (object) "Kalinga-Apayao"
      },
      {
        (object) "33",
        (object) "Laguna"
      },
      {
        (object) "34",
        (object) "Lanao del Norte"
      },
      {
        (object) "35",
        (object) "Lanao del Sur"
      },
      {
        (object) "36",
        (object) "La Union"
      },
      {
        (object) "37",
        (object) "Leyte"
      },
      {
        (object) "38",
        (object) "Marinduque"
      },
      {
        (object) "39",
        (object) "Masbate"
      },
      {
        (object) "40",
        (object) "Mindoro Occidental"
      },
      {
        (object) "41",
        (object) "Mindoro Oriental"
      },
      {
        (object) "42",
        (object) "Misamis Occidental"
      },
      {
        (object) "43",
        (object) "Misamis Oriental"
      },
      {
        (object) "44",
        (object) "Mountain"
      },
      {
        (object) "45",
        (object) "Negros Occidental"
      },
      {
        (object) "46",
        (object) "Negros Oriental"
      },
      {
        (object) "47",
        (object) "Nueva Ecija"
      },
      {
        (object) "48",
        (object) "Nueva Vizcaya"
      },
      {
        (object) "49",
        (object) "Palawan"
      },
      {
        (object) "50",
        (object) "Pampanga"
      },
      {
        (object) "51",
        (object) "Pangasinan"
      },
      {
        (object) "53",
        (object) "Rizal"
      },
      {
        (object) "54",
        (object) "Romblon"
      },
      {
        (object) "55",
        (object) "Samar"
      },
      {
        (object) "56",
        (object) "Maguindanao"
      },
      {
        (object) "57",
        (object) "North Cotabato"
      },
      {
        (object) "58",
        (object) "Sorsogon"
      },
      {
        (object) "59",
        (object) "Southern Leyte"
      },
      {
        (object) "60",
        (object) "Sulu"
      },
      {
        (object) "61",
        (object) "Surigao del Norte"
      },
      {
        (object) "62",
        (object) "Surigao del Sur"
      },
      {
        (object) "63",
        (object) "Tarlac"
      },
      {
        (object) "64",
        (object) "Zambales"
      },
      {
        (object) "65",
        (object) "Zamboanga del Norte"
      },
      {
        (object) "66",
        (object) "Zamboanga del Sur"
      },
      {
        (object) "67",
        (object) "Northern Samar"
      },
      {
        (object) "68",
        (object) "Quirino"
      },
      {
        (object) "69",
        (object) "Siquijor"
      },
      {
        (object) "70",
        (object) "South Cotabato"
      },
      {
        (object) "71",
        (object) "Sultan Kudarat"
      },
      {
        (object) "72",
        (object) "Tawitawi"
      },
      {
        (object) "A1",
        (object) "Angeles"
      },
      {
        (object) "A2",
        (object) "Bacolod"
      },
      {
        (object) "A3",
        (object) "Bago"
      },
      {
        (object) "A4",
        (object) "Baguio"
      },
      {
        (object) "A5",
        (object) "Bais"
      },
      {
        (object) "A6",
        (object) "Basilan City"
      },
      {
        (object) "A7",
        (object) "Batangas City"
      },
      {
        (object) "A8",
        (object) "Butuan"
      },
      {
        (object) "A9",
        (object) "Cabanatuan"
      },
      {
        (object) "B1",
        (object) "Cadiz"
      },
      {
        (object) "B2",
        (object) "Cagayan de Oro"
      },
      {
        (object) "B3",
        (object) "Calbayog"
      },
      {
        (object) "B4",
        (object) "Caloocan"
      },
      {
        (object) "B5",
        (object) "Canlaon"
      },
      {
        (object) "B6",
        (object) "Cavite City"
      },
      {
        (object) "B7",
        (object) "Cebu City"
      },
      {
        (object) "B8",
        (object) "Cotabato"
      },
      {
        (object) "B9",
        (object) "Dagupan"
      },
      {
        (object) "C1",
        (object) "Danao"
      },
      {
        (object) "C2",
        (object) "Dapitan"
      },
      {
        (object) "C3",
        (object) "Davao City"
      },
      {
        (object) "C4",
        (object) "Dipolog"
      },
      {
        (object) "C5",
        (object) "Dumaguete"
      },
      {
        (object) "C6",
        (object) "General Santos"
      },
      {
        (object) "C7",
        (object) "Gingoog"
      },
      {
        (object) "C8",
        (object) "Iligan"
      },
      {
        (object) "C9",
        (object) "Iloilo City"
      },
      {
        (object) "D1",
        (object) "Iriga"
      },
      {
        (object) "D2",
        (object) "La Carlota"
      },
      {
        (object) "D3",
        (object) "Laoag"
      },
      {
        (object) "D4",
        (object) "Lapu-Lapu"
      },
      {
        (object) "D5",
        (object) "Legaspi"
      },
      {
        (object) "D6",
        (object) "Lipa"
      },
      {
        (object) "D7",
        (object) "Lucena"
      },
      {
        (object) "D8",
        (object) "Mandaue"
      },
      {
        (object) "D9",
        (object) "Manila"
      },
      {
        (object) "E1",
        (object) "Marawi"
      },
      {
        (object) "E2",
        (object) "Naga"
      },
      {
        (object) "E3",
        (object) "Olongapo"
      },
      {
        (object) "E4",
        (object) "Ormoc"
      },
      {
        (object) "E5",
        (object) "Oroquieta"
      },
      {
        (object) "E6",
        (object) "Ozamis"
      },
      {
        (object) "E7",
        (object) "Pagadian"
      },
      {
        (object) "E8",
        (object) "Palayan"
      },
      {
        (object) "E9",
        (object) "Pasay"
      },
      {
        (object) "F1",
        (object) "Puerto Princesa"
      },
      {
        (object) "F2",
        (object) "Quezon City"
      },
      {
        (object) "F3",
        (object) "Roxas"
      },
      {
        (object) "F4",
        (object) "San Carlos"
      },
      {
        (object) "F5",
        (object) "San Carlos"
      },
      {
        (object) "F6",
        (object) "San Jose"
      },
      {
        (object) "F7",
        (object) "San Pablo"
      },
      {
        (object) "F8",
        (object) "Silay"
      },
      {
        (object) "F9",
        (object) "Surigao"
      },
      {
        (object) "G1",
        (object) "Tacloban"
      },
      {
        (object) "G2",
        (object) "Tagaytay"
      },
      {
        (object) "G3",
        (object) "Tagbilaran"
      },
      {
        (object) "G4",
        (object) "Tangub"
      },
      {
        (object) "G5",
        (object) "Toledo"
      },
      {
        (object) "G6",
        (object) "Trece Martires"
      },
      {
        (object) "G7",
        (object) "Zamboanga"
      },
      {
        (object) "G8",
        (object) "Aurora"
      },
      {
        (object) "H2",
        (object) "Quezon"
      },
      {
        (object) "H3",
        (object) "Negros Occidental"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "PK", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Federally Administered Tribal Areas"
      },
      {
        (object) "02",
        (object) "Balochistan"
      },
      {
        (object) "03",
        (object) "North-West Frontier"
      },
      {
        (object) "04",
        (object) "Punjab"
      },
      {
        (object) "05",
        (object) "Sindh"
      },
      {
        (object) "06",
        (object) "Azad Kashmir"
      },
      {
        (object) "07",
        (object) "Northern Areas"
      },
      {
        (object) "08",
        (object) "Islamabad"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "PL", (object) new Hashtable()
    {
      {
        (object) "72",
        (object) "Dolnoslaskie"
      },
      {
        (object) "73",
        (object) "Kujawsko-Pomorskie"
      },
      {
        (object) "74",
        (object) "Lodzkie"
      },
      {
        (object) "75",
        (object) "Lubelskie"
      },
      {
        (object) "76",
        (object) "Lubuskie"
      },
      {
        (object) "77",
        (object) "Malopolskie"
      },
      {
        (object) "78",
        (object) "Mazowieckie"
      },
      {
        (object) "79",
        (object) "Opolskie"
      },
      {
        (object) "80",
        (object) "Podkarpackie"
      },
      {
        (object) "81",
        (object) "Podlaskie"
      },
      {
        (object) "82",
        (object) "Pomorskie"
      },
      {
        (object) "83",
        (object) "Slaskie"
      },
      {
        (object) "84",
        (object) "Swietokrzyskie"
      },
      {
        (object) "85",
        (object) "Warminsko-Mazurskie"
      },
      {
        (object) "86",
        (object) "Wielkopolskie"
      },
      {
        (object) "87",
        (object) "Zachodniopomorskie"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "PS", (object) new Hashtable()
    {
      {
        (object) "GZ",
        (object) "Gaza"
      },
      {
        (object) "WE",
        (object) "West Bank"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "PT", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Aveiro"
      },
      {
        (object) "03",
        (object) "Beja"
      },
      {
        (object) "04",
        (object) "Braga"
      },
      {
        (object) "05",
        (object) "Braganca"
      },
      {
        (object) "06",
        (object) "Castelo Branco"
      },
      {
        (object) "07",
        (object) "Coimbra"
      },
      {
        (object) "08",
        (object) "Evora"
      },
      {
        (object) "09",
        (object) "Faro"
      },
      {
        (object) "10",
        (object) "Madeira"
      },
      {
        (object) "11",
        (object) "Guarda"
      },
      {
        (object) "13",
        (object) "Leiria"
      },
      {
        (object) "14",
        (object) "Lisboa"
      },
      {
        (object) "16",
        (object) "Portalegre"
      },
      {
        (object) "17",
        (object) "Porto"
      },
      {
        (object) "18",
        (object) "Santarem"
      },
      {
        (object) "19",
        (object) "Setubal"
      },
      {
        (object) "20",
        (object) "Viana do Castelo"
      },
      {
        (object) "21",
        (object) "Vila Real"
      },
      {
        (object) "22",
        (object) "Viseu"
      },
      {
        (object) "23",
        (object) "Azores"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "PY", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Alto Parana"
      },
      {
        (object) "02",
        (object) "Amambay"
      },
      {
        (object) "03",
        (object) "Boqueron"
      },
      {
        (object) "04",
        (object) "Caaguazu"
      },
      {
        (object) "05",
        (object) "Caazapa"
      },
      {
        (object) "06",
        (object) "Central"
      },
      {
        (object) "07",
        (object) "Concepcion"
      },
      {
        (object) "08",
        (object) "Cordillera"
      },
      {
        (object) "10",
        (object) "Guaira"
      },
      {
        (object) "11",
        (object) "Itapua"
      },
      {
        (object) "12",
        (object) "Misiones"
      },
      {
        (object) "13",
        (object) "Neembucu"
      },
      {
        (object) "15",
        (object) "Paraguari"
      },
      {
        (object) "16",
        (object) "Presidente Hayes"
      },
      {
        (object) "17",
        (object) "San Pedro"
      },
      {
        (object) "19",
        (object) "Canindeyu"
      },
      {
        (object) "20",
        (object) "Chaco"
      },
      {
        (object) "21",
        (object) "Nueva Asuncion"
      },
      {
        (object) "23",
        (object) "Alto Paraguay"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "QA", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Ad Dawhah"
      },
      {
        (object) "02",
        (object) "Al Ghuwariyah"
      },
      {
        (object) "03",
        (object) "Al Jumaliyah"
      },
      {
        (object) "04",
        (object) "Al Khawr"
      },
      {
        (object) "05",
        (object) "Al Wakrah Municipality"
      },
      {
        (object) "06",
        (object) "Ar Rayyan"
      },
      {
        (object) "08",
        (object) "Madinat ach Shamal"
      },
      {
        (object) "09",
        (object) "Umm Salal"
      },
      {
        (object) "10",
        (object) "Al Wakrah"
      },
      {
        (object) "11",
        (object) "Jariyan al Batnah"
      },
      {
        (object) "12",
        (object) "Umm Sa'id"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "RO", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Alba"
      },
      {
        (object) "02",
        (object) "Arad"
      },
      {
        (object) "03",
        (object) "Arges"
      },
      {
        (object) "04",
        (object) "Bacau"
      },
      {
        (object) "05",
        (object) "Bihor"
      },
      {
        (object) "06",
        (object) "Bistrita-Nasaud"
      },
      {
        (object) "07",
        (object) "Botosani"
      },
      {
        (object) "08",
        (object) "Braila"
      },
      {
        (object) "09",
        (object) "Brasov"
      },
      {
        (object) "10",
        (object) "Bucuresti"
      },
      {
        (object) "11",
        (object) "Buzau"
      },
      {
        (object) "12",
        (object) "Caras-Severin"
      },
      {
        (object) "13",
        (object) "Cluj"
      },
      {
        (object) "14",
        (object) "Constanta"
      },
      {
        (object) "15",
        (object) "Covasna"
      },
      {
        (object) "16",
        (object) "Dambovita"
      },
      {
        (object) "17",
        (object) "Dolj"
      },
      {
        (object) "18",
        (object) "Galati"
      },
      {
        (object) "19",
        (object) "Gorj"
      },
      {
        (object) "20",
        (object) "Harghita"
      },
      {
        (object) "21",
        (object) "Hunedoara"
      },
      {
        (object) "22",
        (object) "Ialomita"
      },
      {
        (object) "23",
        (object) "Iasi"
      },
      {
        (object) "25",
        (object) "Maramures"
      },
      {
        (object) "26",
        (object) "Mehedinti"
      },
      {
        (object) "27",
        (object) "Mures"
      },
      {
        (object) "28",
        (object) "Neamt"
      },
      {
        (object) "29",
        (object) "Olt"
      },
      {
        (object) "30",
        (object) "Prahova"
      },
      {
        (object) "31",
        (object) "Salaj"
      },
      {
        (object) "32",
        (object) "Satu Mare"
      },
      {
        (object) "33",
        (object) "Sibiu"
      },
      {
        (object) "34",
        (object) "Suceava"
      },
      {
        (object) "35",
        (object) "Teleorman"
      },
      {
        (object) "36",
        (object) "Timis"
      },
      {
        (object) "37",
        (object) "Tulcea"
      },
      {
        (object) "38",
        (object) "Vaslui"
      },
      {
        (object) "39",
        (object) "Valcea"
      },
      {
        (object) "40",
        (object) "Vrancea"
      },
      {
        (object) "41",
        (object) "Calarasi"
      },
      {
        (object) "42",
        (object) "Giurgiu"
      },
      {
        (object) "43",
        (object) "Ilfov"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "RS", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Kosovo"
      },
      {
        (object) "02",
        (object) "Vojvodina"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "RU", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Adygeya, Republic of"
      },
      {
        (object) "02",
        (object) "Aginsky Buryatsky AO"
      },
      {
        (object) "03",
        (object) "Gorno-Altay"
      },
      {
        (object) "04",
        (object) "Altaisky krai"
      },
      {
        (object) "05",
        (object) "Amur"
      },
      {
        (object) "06",
        (object) "Arkhangel'sk"
      },
      {
        (object) "07",
        (object) "Astrakhan'"
      },
      {
        (object) "08",
        (object) "Bashkortostan"
      },
      {
        (object) "09",
        (object) "Belgorod"
      },
      {
        (object) "10",
        (object) "Bryansk"
      },
      {
        (object) "11",
        (object) "Buryat"
      },
      {
        (object) "12",
        (object) "Chechnya"
      },
      {
        (object) "13",
        (object) "Chelyabinsk"
      },
      {
        (object) "14",
        (object) "Chita"
      },
      {
        (object) "15",
        (object) "Chukot"
      },
      {
        (object) "16",
        (object) "Chuvashia"
      },
      {
        (object) "17",
        (object) "Dagestan"
      },
      {
        (object) "18",
        (object) "Evenk"
      },
      {
        (object) "19",
        (object) "Ingush"
      },
      {
        (object) "20",
        (object) "Irkutsk"
      },
      {
        (object) "21",
        (object) "Ivanovo"
      },
      {
        (object) "22",
        (object) "Kabardin-Balkar"
      },
      {
        (object) "23",
        (object) "Kaliningrad"
      },
      {
        (object) "24",
        (object) "Kalmyk"
      },
      {
        (object) "25",
        (object) "Kaluga"
      },
      {
        (object) "26",
        (object) "Kamchatka"
      },
      {
        (object) "27",
        (object) "Karachay-Cherkess"
      },
      {
        (object) "28",
        (object) "Karelia"
      },
      {
        (object) "29",
        (object) "Kemerovo"
      },
      {
        (object) "30",
        (object) "Khabarovsk"
      },
      {
        (object) "31",
        (object) "Khakass"
      },
      {
        (object) "32",
        (object) "Khanty-Mansiy"
      },
      {
        (object) "33",
        (object) "Kirov"
      },
      {
        (object) "34",
        (object) "Komi"
      },
      {
        (object) "35",
        (object) "Komi-Permyak"
      },
      {
        (object) "36",
        (object) "Koryak"
      },
      {
        (object) "37",
        (object) "Kostroma"
      },
      {
        (object) "38",
        (object) "Krasnodar"
      },
      {
        (object) "39",
        (object) "Krasnoyarsk"
      },
      {
        (object) "40",
        (object) "Kurgan"
      },
      {
        (object) "41",
        (object) "Kursk"
      },
      {
        (object) "42",
        (object) "Leningrad"
      },
      {
        (object) "43",
        (object) "Lipetsk"
      },
      {
        (object) "44",
        (object) "Magadan"
      },
      {
        (object) "45",
        (object) "Mariy-El"
      },
      {
        (object) "46",
        (object) "Mordovia"
      },
      {
        (object) "47",
        (object) "Moskva"
      },
      {
        (object) "48",
        (object) "Moscow City"
      },
      {
        (object) "49",
        (object) "Murmansk"
      },
      {
        (object) "50",
        (object) "Nenets"
      },
      {
        (object) "51",
        (object) "Nizhegorod"
      },
      {
        (object) "52",
        (object) "Novgorod"
      },
      {
        (object) "53",
        (object) "Novosibirsk"
      },
      {
        (object) "54",
        (object) "Omsk"
      },
      {
        (object) "55",
        (object) "Orenburg"
      },
      {
        (object) "56",
        (object) "Orel"
      },
      {
        (object) "57",
        (object) "Penza"
      },
      {
        (object) "58",
        (object) "Perm'"
      },
      {
        (object) "59",
        (object) "Primor'ye"
      },
      {
        (object) "60",
        (object) "Pskov"
      },
      {
        (object) "61",
        (object) "Rostov"
      },
      {
        (object) "62",
        (object) "Ryazan'"
      },
      {
        (object) "63",
        (object) "Sakha"
      },
      {
        (object) "64",
        (object) "Sakhalin"
      },
      {
        (object) "65",
        (object) "Samara"
      },
      {
        (object) "66",
        (object) "Saint Petersburg City"
      },
      {
        (object) "67",
        (object) "Saratov"
      },
      {
        (object) "68",
        (object) "North Ossetia"
      },
      {
        (object) "69",
        (object) "Smolensk"
      },
      {
        (object) "70",
        (object) "Stavropol'"
      },
      {
        (object) "71",
        (object) "Sverdlovsk"
      },
      {
        (object) "72",
        (object) "Tambovskaya obLast"
      },
      {
        (object) "73",
        (object) "Tatarstan"
      },
      {
        (object) "74",
        (object) "Taymyr"
      },
      {
        (object) "75",
        (object) "Tomsk"
      },
      {
        (object) "76",
        (object) "Tula"
      },
      {
        (object) "77",
        (object) "Tver'"
      },
      {
        (object) "78",
        (object) "Tyumen'"
      },
      {
        (object) "79",
        (object) "Tuva"
      },
      {
        (object) "80",
        (object) "Udmurt"
      },
      {
        (object) "81",
        (object) "Ul'yanovsk"
      },
      {
        (object) "82",
        (object) "Ust-Orda Buryat"
      },
      {
        (object) "83",
        (object) "Vladimir"
      },
      {
        (object) "84",
        (object) "Volgograd"
      },
      {
        (object) "85",
        (object) "Vologda"
      },
      {
        (object) "86",
        (object) "Voronezh"
      },
      {
        (object) "87",
        (object) "Yamal-Nenets"
      },
      {
        (object) "88",
        (object) "Yaroslavl'"
      },
      {
        (object) "89",
        (object) "Yevrey"
      },
      {
        (object) "90",
        (object) "Permskiy Kray"
      },
      {
        (object) "91",
        (object) "Krasnoyarskiy Kray"
      },
      {
        (object) "92",
        (object) "Kamchatskiy Kray"
      },
      {
        (object) "93",
        (object) "Zabaykal'skiy Kray"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "RW", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Butare"
      },
      {
        (object) "06",
        (object) "Gitarama"
      },
      {
        (object) "07",
        (object) "Kibungo"
      },
      {
        (object) "09",
        (object) "Kigali"
      },
      {
        (object) "11",
        (object) "Est"
      },
      {
        (object) "12",
        (object) "Kigali"
      },
      {
        (object) "13",
        (object) "Nord"
      },
      {
        (object) "14",
        (object) "Ouest"
      },
      {
        (object) "15",
        (object) "Sud"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SA", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Al Bahah"
      },
      {
        (object) "05",
        (object) "Al Madinah"
      },
      {
        (object) "06",
        (object) "Ash Sharqiyah"
      },
      {
        (object) "08",
        (object) "Al Qasim"
      },
      {
        (object) "10",
        (object) "Ar Riyad"
      },
      {
        (object) "11",
        (object) "Asir Province"
      },
      {
        (object) "13",
        (object) "Ha'il"
      },
      {
        (object) "14",
        (object) "Makkah"
      },
      {
        (object) "15",
        (object) "Al Hudud ash Shamaliyah"
      },
      {
        (object) "16",
        (object) "Najran"
      },
      {
        (object) "17",
        (object) "Jizan"
      },
      {
        (object) "19",
        (object) "Tabuk"
      },
      {
        (object) "20",
        (object) "Al Jawf"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SB", (object) new Hashtable()
    {
      {
        (object) "03",
        (object) "Malaita"
      },
      {
        (object) "06",
        (object) "Guadalcanal"
      },
      {
        (object) "07",
        (object) "Isabel"
      },
      {
        (object) "08",
        (object) "Makira"
      },
      {
        (object) "09",
        (object) "Temotu"
      },
      {
        (object) "10",
        (object) "Central"
      },
      {
        (object) "11",
        (object) "Western"
      },
      {
        (object) "12",
        (object) "Choiseul"
      },
      {
        (object) "13",
        (object) "Rennell and Bellona"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SC", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Anse aux Pins"
      },
      {
        (object) "02",
        (object) "Anse Boileau"
      },
      {
        (object) "03",
        (object) "Anse Etoile"
      },
      {
        (object) "04",
        (object) "Anse Louis"
      },
      {
        (object) "05",
        (object) "Anse Royale"
      },
      {
        (object) "06",
        (object) "Baie Lazare"
      },
      {
        (object) "07",
        (object) "Baie Sainte Anne"
      },
      {
        (object) "08",
        (object) "Beau Vallon"
      },
      {
        (object) "09",
        (object) "Bel Air"
      },
      {
        (object) "10",
        (object) "Bel Ombre"
      },
      {
        (object) "11",
        (object) "Cascade"
      },
      {
        (object) "12",
        (object) "Glacis"
      },
      {
        (object) "13",
        (object) "Grand' Anse"
      },
      {
        (object) "14",
        (object) "Grand' Anse"
      },
      {
        (object) "15",
        (object) "La Digue"
      },
      {
        (object) "16",
        (object) "La Riviere Anglaise"
      },
      {
        (object) "17",
        (object) "Mont Buxton"
      },
      {
        (object) "18",
        (object) "Mont Fleuri"
      },
      {
        (object) "19",
        (object) "Plaisance"
      },
      {
        (object) "20",
        (object) "Pointe La Rue"
      },
      {
        (object) "21",
        (object) "Port Glaud"
      },
      {
        (object) "22",
        (object) "Saint Louis"
      },
      {
        (object) "23",
        (object) "Takamaka"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SD", (object) new Hashtable()
    {
      {
        (object) "27",
        (object) "Al Wusta"
      },
      {
        (object) "28",
        (object) "Al Istiwa'iyah"
      },
      {
        (object) "29",
        (object) "Al Khartum"
      },
      {
        (object) "30",
        (object) "Ash Shamaliyah"
      },
      {
        (object) "31",
        (object) "Ash Sharqiyah"
      },
      {
        (object) "32",
        (object) "Bahr al Ghazal"
      },
      {
        (object) "33",
        (object) "Darfur"
      },
      {
        (object) "34",
        (object) "Kurdufan"
      },
      {
        (object) "35",
        (object) "Upper Nile"
      },
      {
        (object) "40",
        (object) "Al Wahadah State"
      },
      {
        (object) "44",
        (object) "Central Equatoria State"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SE", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Blekinge Lan"
      },
      {
        (object) "03",
        (object) "Gavleborgs Lan"
      },
      {
        (object) "05",
        (object) "Gotlands Lan"
      },
      {
        (object) "06",
        (object) "Hallands Lan"
      },
      {
        (object) "07",
        (object) "Jamtlands Lan"
      },
      {
        (object) "08",
        (object) "Jonkopings Lan"
      },
      {
        (object) "09",
        (object) "Kalmar Lan"
      },
      {
        (object) "10",
        (object) "Dalarnas Lan"
      },
      {
        (object) "12",
        (object) "Kronobergs Lan"
      },
      {
        (object) "14",
        (object) "Norrbottens Lan"
      },
      {
        (object) "15",
        (object) "Orebro Lan"
      },
      {
        (object) "16",
        (object) "Ostergotlands Lan"
      },
      {
        (object) "18",
        (object) "Sodermanlands Lan"
      },
      {
        (object) "21",
        (object) "Uppsala Lan"
      },
      {
        (object) "22",
        (object) "Varmlands Lan"
      },
      {
        (object) "23",
        (object) "Vasterbottens Lan"
      },
      {
        (object) "24",
        (object) "Vasternorrlands Lan"
      },
      {
        (object) "25",
        (object) "Vastmanlands Lan"
      },
      {
        (object) "26",
        (object) "Stockholms Lan"
      },
      {
        (object) "27",
        (object) "Skane Lan"
      },
      {
        (object) "28",
        (object) "Vastra Gotaland"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SH", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Ascension"
      },
      {
        (object) "02",
        (object) "Saint Helena"
      },
      {
        (object) "03",
        (object) "Tristan da Cunha"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SI", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Ajdovscina"
      },
      {
        (object) "02",
        (object) "Beltinci"
      },
      {
        (object) "03",
        (object) "Bled"
      },
      {
        (object) "04",
        (object) "Bohinj"
      },
      {
        (object) "05",
        (object) "Borovnica"
      },
      {
        (object) "06",
        (object) "Bovec"
      },
      {
        (object) "07",
        (object) "Brda"
      },
      {
        (object) "08",
        (object) "Brezice"
      },
      {
        (object) "09",
        (object) "Brezovica"
      },
      {
        (object) "11",
        (object) "Celje"
      },
      {
        (object) "12",
        (object) "Cerklje na Gorenjskem"
      },
      {
        (object) "13",
        (object) "Cerknica"
      },
      {
        (object) "14",
        (object) "Cerkno"
      },
      {
        (object) "15",
        (object) "Crensovci"
      },
      {
        (object) "16",
        (object) "Crna na Koroskem"
      },
      {
        (object) "17",
        (object) "Crnomelj"
      },
      {
        (object) "19",
        (object) "Divaca"
      },
      {
        (object) "20",
        (object) "Dobrepolje"
      },
      {
        (object) "22",
        (object) "Dol pri Ljubljani"
      },
      {
        (object) "24",
        (object) "Dornava"
      },
      {
        (object) "25",
        (object) "Dravograd"
      },
      {
        (object) "26",
        (object) "Duplek"
      },
      {
        (object) "27",
        (object) "Gorenja Vas-Poljane"
      },
      {
        (object) "28",
        (object) "Gorisnica"
      },
      {
        (object) "29",
        (object) "Gornja Radgona"
      },
      {
        (object) "30",
        (object) "Gornji Grad"
      },
      {
        (object) "31",
        (object) "Gornji Petrovci"
      },
      {
        (object) "32",
        (object) "Grosuplje"
      },
      {
        (object) "34",
        (object) "Hrastnik"
      },
      {
        (object) "35",
        (object) "Hrpelje-Kozina"
      },
      {
        (object) "36",
        (object) "Idrija"
      },
      {
        (object) "37",
        (object) "Ig"
      },
      {
        (object) "38",
        (object) "Ilirska Bistrica"
      },
      {
        (object) "39",
        (object) "Ivancna Gorica"
      },
      {
        (object) "40",
        (object) "Izola-Isola"
      },
      {
        (object) "42",
        (object) "Jursinci"
      },
      {
        (object) "44",
        (object) "Kanal"
      },
      {
        (object) "45",
        (object) "Kidricevo"
      },
      {
        (object) "46",
        (object) "Kobarid"
      },
      {
        (object) "47",
        (object) "Kobilje"
      },
      {
        (object) "49",
        (object) "Komen"
      },
      {
        (object) "50",
        (object) "Koper-Capodistria"
      },
      {
        (object) "51",
        (object) "Kozje"
      },
      {
        (object) "52",
        (object) "Kranj"
      },
      {
        (object) "53",
        (object) "Kranjska Gora"
      },
      {
        (object) "54",
        (object) "Krsko"
      },
      {
        (object) "55",
        (object) "Kungota"
      },
      {
        (object) "57",
        (object) "Lasko"
      },
      {
        (object) "61",
        (object) "Ljubljana"
      },
      {
        (object) "62",
        (object) "Ljubno"
      },
      {
        (object) "64",
        (object) "Logatec"
      },
      {
        (object) "66",
        (object) "Loski Potok"
      },
      {
        (object) "68",
        (object) "Lukovica"
      },
      {
        (object) "71",
        (object) "Medvode"
      },
      {
        (object) "72",
        (object) "Menges"
      },
      {
        (object) "73",
        (object) "Metlika"
      },
      {
        (object) "74",
        (object) "Mezica"
      },
      {
        (object) "76",
        (object) "Mislinja"
      },
      {
        (object) "77",
        (object) "Moravce"
      },
      {
        (object) "78",
        (object) "Moravske Toplice"
      },
      {
        (object) "79",
        (object) "Mozirje"
      },
      {
        (object) "80",
        (object) "Murska Sobota"
      },
      {
        (object) "81",
        (object) "Muta"
      },
      {
        (object) "82",
        (object) "Naklo"
      },
      {
        (object) "83",
        (object) "Nazarje"
      },
      {
        (object) "84",
        (object) "Nova Gorica"
      },
      {
        (object) "86",
        (object) "Odranci"
      },
      {
        (object) "87",
        (object) "Ormoz"
      },
      {
        (object) "88",
        (object) "Osilnica"
      },
      {
        (object) "89",
        (object) "Pesnica"
      },
      {
        (object) "91",
        (object) "Pivka"
      },
      {
        (object) "92",
        (object) "Podcetrtek"
      },
      {
        (object) "94",
        (object) "Postojna"
      },
      {
        (object) "97",
        (object) "Puconci"
      },
      {
        (object) "98",
        (object) "Racam"
      },
      {
        (object) "99",
        (object) "Radece"
      },
      {
        (object) "A1",
        (object) "Radenci"
      },
      {
        (object) "A2",
        (object) "Radlje ob Dravi"
      },
      {
        (object) "A3",
        (object) "Radovljica"
      },
      {
        (object) "A6",
        (object) "Rogasovci"
      },
      {
        (object) "A7",
        (object) "Rogaska Slatina"
      },
      {
        (object) "A8",
        (object) "Rogatec"
      },
      {
        (object) "B1",
        (object) "Semic"
      },
      {
        (object) "B2",
        (object) "Sencur"
      },
      {
        (object) "B3",
        (object) "Sentilj"
      },
      {
        (object) "B4",
        (object) "Sentjernej"
      },
      {
        (object) "B6",
        (object) "Sevnica"
      },
      {
        (object) "B7",
        (object) "Sezana"
      },
      {
        (object) "B8",
        (object) "Skocjan"
      },
      {
        (object) "B9",
        (object) "Skofja Loka"
      },
      {
        (object) "C1",
        (object) "Skofljica"
      },
      {
        (object) "C2",
        (object) "Slovenj Gradec"
      },
      {
        (object) "C4",
        (object) "Slovenske Konjice"
      },
      {
        (object) "C5",
        (object) "Smarje pri Jelsah"
      },
      {
        (object) "C6",
        (object) "Smartno ob Paki"
      },
      {
        (object) "C7",
        (object) "Sostanj"
      },
      {
        (object) "C8",
        (object) "Starse"
      },
      {
        (object) "C9",
        (object) "Store"
      },
      {
        (object) "D1",
        (object) "Sveti Jurij"
      },
      {
        (object) "D2",
        (object) "Tolmin"
      },
      {
        (object) "D3",
        (object) "Trbovlje"
      },
      {
        (object) "D4",
        (object) "Trebnje"
      },
      {
        (object) "D5",
        (object) "Trzic"
      },
      {
        (object) "D6",
        (object) "Turnisce"
      },
      {
        (object) "D7",
        (object) "Velenje"
      },
      {
        (object) "D8",
        (object) "Velike Lasce"
      },
      {
        (object) "E1",
        (object) "Vipava"
      },
      {
        (object) "E2",
        (object) "Vitanje"
      },
      {
        (object) "E3",
        (object) "Vodice"
      },
      {
        (object) "E5",
        (object) "Vrhnika"
      },
      {
        (object) "E6",
        (object) "Vuzenica"
      },
      {
        (object) "E7",
        (object) "Zagorje ob Savi"
      },
      {
        (object) "E9",
        (object) "Zavrc"
      },
      {
        (object) "F1",
        (object) "Zelezniki"
      },
      {
        (object) "F2",
        (object) "Ziri"
      },
      {
        (object) "F3",
        (object) "Zrece"
      },
      {
        (object) "G4",
        (object) "Dobrova-Horjul-Polhov Gradec"
      },
      {
        (object) "G7",
        (object) "Domzale"
      },
      {
        (object) "H4",
        (object) "Jesenice"
      },
      {
        (object) "H6",
        (object) "Kamnik"
      },
      {
        (object) "H7",
        (object) "Kocevje"
      },
      {
        (object) "I2",
        (object) "Kuzma"
      },
      {
        (object) "I3",
        (object) "Lenart"
      },
      {
        (object) "I5",
        (object) "Litija"
      },
      {
        (object) "I6",
        (object) "Ljutomer"
      },
      {
        (object) "I7",
        (object) "Loska Dolina"
      },
      {
        (object) "I9",
        (object) "Luce"
      },
      {
        (object) "J1",
        (object) "Majsperk"
      },
      {
        (object) "J2",
        (object) "Maribor"
      },
      {
        (object) "J5",
        (object) "Miren-Kostanjevica"
      },
      {
        (object) "J7",
        (object) "Novo Mesto"
      },
      {
        (object) "J9",
        (object) "Piran"
      },
      {
        (object) "K5",
        (object) "Preddvor"
      },
      {
        (object) "K7",
        (object) "Ptuj"
      },
      {
        (object) "L1",
        (object) "Ribnica"
      },
      {
        (object) "L3",
        (object) "Ruse"
      },
      {
        (object) "L7",
        (object) "Sentjur pri Celju"
      },
      {
        (object) "L8",
        (object) "Slovenska Bistrica"
      },
      {
        (object) "N2",
        (object) "Videm"
      },
      {
        (object) "N3",
        (object) "Vojnik"
      },
      {
        (object) "N5",
        (object) "Zalec"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SK", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Banska Bystrica"
      },
      {
        (object) "02",
        (object) "Bratislava"
      },
      {
        (object) "03",
        (object) "Kosice"
      },
      {
        (object) "04",
        (object) "Nitra"
      },
      {
        (object) "05",
        (object) "Presov"
      },
      {
        (object) "06",
        (object) "Trencin"
      },
      {
        (object) "07",
        (object) "Trnava"
      },
      {
        (object) "08",
        (object) "Zilina"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SL", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Eastern"
      },
      {
        (object) "02",
        (object) "Northern"
      },
      {
        (object) "03",
        (object) "Southern"
      },
      {
        (object) "04",
        (object) "Western Area"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Acquaviva"
      },
      {
        (object) "02",
        (object) "Chiesanuova"
      },
      {
        (object) "03",
        (object) "Domagnano"
      },
      {
        (object) "04",
        (object) "Faetano"
      },
      {
        (object) "05",
        (object) "Fiorentino"
      },
      {
        (object) "06",
        (object) "Borgo Maggiore"
      },
      {
        (object) "07",
        (object) "San Marino"
      },
      {
        (object) "08",
        (object) "Monte Giardino"
      },
      {
        (object) "09",
        (object) "Serravalle"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SN", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Dakar"
      },
      {
        (object) "03",
        (object) "Diourbel"
      },
      {
        (object) "05",
        (object) "Tambacounda"
      },
      {
        (object) "07",
        (object) "Thies"
      },
      {
        (object) "09",
        (object) "Fatick"
      },
      {
        (object) "10",
        (object) "Kaolack"
      },
      {
        (object) "11",
        (object) "Kolda"
      },
      {
        (object) "12",
        (object) "Ziguinchor"
      },
      {
        (object) "13",
        (object) "Louga"
      },
      {
        (object) "14",
        (object) "Saint-Louis"
      },
      {
        (object) "15",
        (object) "Matam"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SO", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Bakool"
      },
      {
        (object) "02",
        (object) "Banaadir"
      },
      {
        (object) "03",
        (object) "Bari"
      },
      {
        (object) "04",
        (object) "Bay"
      },
      {
        (object) "05",
        (object) "Galguduud"
      },
      {
        (object) "06",
        (object) "Gedo"
      },
      {
        (object) "07",
        (object) "Hiiraan"
      },
      {
        (object) "08",
        (object) "Jubbada Dhexe"
      },
      {
        (object) "09",
        (object) "Jubbada Hoose"
      },
      {
        (object) "10",
        (object) "Mudug"
      },
      {
        (object) "11",
        (object) "Nugaal"
      },
      {
        (object) "12",
        (object) "Sanaag"
      },
      {
        (object) "13",
        (object) "Shabeellaha Dhexe"
      },
      {
        (object) "14",
        (object) "Shabeellaha Hoose"
      },
      {
        (object) "16",
        (object) "Woqooyi Galbeed"
      },
      {
        (object) "18",
        (object) "Nugaal"
      },
      {
        (object) "19",
        (object) "Togdheer"
      },
      {
        (object) "20",
        (object) "Woqooyi Galbeed"
      },
      {
        (object) "21",
        (object) "Awdal"
      },
      {
        (object) "22",
        (object) "Sool"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SR", (object) new Hashtable()
    {
      {
        (object) "10",
        (object) "Brokopondo"
      },
      {
        (object) "11",
        (object) "Commewijne"
      },
      {
        (object) "12",
        (object) "Coronie"
      },
      {
        (object) "13",
        (object) "Marowijne"
      },
      {
        (object) "14",
        (object) "Nickerie"
      },
      {
        (object) "15",
        (object) "Para"
      },
      {
        (object) "16",
        (object) "Paramaribo"
      },
      {
        (object) "17",
        (object) "Saramacca"
      },
      {
        (object) "18",
        (object) "Sipaliwini"
      },
      {
        (object) "19",
        (object) "Wanica"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "ST", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Principe"
      },
      {
        (object) "02",
        (object) "Sao Tome"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SV", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Ahuachapan"
      },
      {
        (object) "02",
        (object) "Cabanas"
      },
      {
        (object) "03",
        (object) "Chalatenango"
      },
      {
        (object) "04",
        (object) "Cuscatlan"
      },
      {
        (object) "05",
        (object) "La Libertad"
      },
      {
        (object) "06",
        (object) "La Paz"
      },
      {
        (object) "07",
        (object) "La Union"
      },
      {
        (object) "08",
        (object) "Morazan"
      },
      {
        (object) "09",
        (object) "San Miguel"
      },
      {
        (object) "10",
        (object) "San Salvador"
      },
      {
        (object) "11",
        (object) "Santa Ana"
      },
      {
        (object) "12",
        (object) "San Vicente"
      },
      {
        (object) "13",
        (object) "Sonsonate"
      },
      {
        (object) "14",
        (object) "Usulutan"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SY", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Al Hasakah"
      },
      {
        (object) "02",
        (object) "Al Ladhiqiyah"
      },
      {
        (object) "03",
        (object) "Al Qunaytirah"
      },
      {
        (object) "04",
        (object) "Ar Raqqah"
      },
      {
        (object) "05",
        (object) "As Suwayda'"
      },
      {
        (object) "06",
        (object) "Dar"
      },
      {
        (object) "07",
        (object) "Dayr az Zawr"
      },
      {
        (object) "08",
        (object) "Rif Dimashq"
      },
      {
        (object) "09",
        (object) "Halab"
      },
      {
        (object) "10",
        (object) "Hamah"
      },
      {
        (object) "11",
        (object) "Hims"
      },
      {
        (object) "12",
        (object) "Idlib"
      },
      {
        (object) "13",
        (object) "Dimashq"
      },
      {
        (object) "14",
        (object) "Tartus"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "SZ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Hhohho"
      },
      {
        (object) "02",
        (object) "Lubombo"
      },
      {
        (object) "03",
        (object) "Manzini"
      },
      {
        (object) "04",
        (object) "Shiselweni"
      },
      {
        (object) "05",
        (object) "Praslin"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TD", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Batha"
      },
      {
        (object) "02",
        (object) "Biltine"
      },
      {
        (object) "03",
        (object) "Borkou-Ennedi-Tibesti"
      },
      {
        (object) "04",
        (object) "Chari-Baguirmi"
      },
      {
        (object) "05",
        (object) "Guera"
      },
      {
        (object) "06",
        (object) "Kanem"
      },
      {
        (object) "07",
        (object) "Lac"
      },
      {
        (object) "08",
        (object) "Logone Occidental"
      },
      {
        (object) "09",
        (object) "Logone Oriental"
      },
      {
        (object) "10",
        (object) "Mayo-Kebbi"
      },
      {
        (object) "11",
        (object) "Moyen-Chari"
      },
      {
        (object) "12",
        (object) "Ouaddai"
      },
      {
        (object) "13",
        (object) "Salamat"
      },
      {
        (object) "14",
        (object) "Tandjile"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TG", (object) new Hashtable()
    {
      {
        (object) "22",
        (object) "Centrale"
      },
      {
        (object) "23",
        (object) "Kara"
      },
      {
        (object) "24",
        (object) "Maritime"
      },
      {
        (object) "25",
        (object) "Plateaux"
      },
      {
        (object) "26",
        (object) "Savanes"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TH", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Mae Hong Son"
      },
      {
        (object) "02",
        (object) "Chiang Mai"
      },
      {
        (object) "03",
        (object) "Chiang Rai"
      },
      {
        (object) "04",
        (object) "Nan"
      },
      {
        (object) "05",
        (object) "Lamphun"
      },
      {
        (object) "06",
        (object) "Lampang"
      },
      {
        (object) "07",
        (object) "Phrae"
      },
      {
        (object) "08",
        (object) "Tak"
      },
      {
        (object) "09",
        (object) "Sukhothai"
      },
      {
        (object) "10",
        (object) "Uttaradit"
      },
      {
        (object) "11",
        (object) "Kamphaeng Phet"
      },
      {
        (object) "12",
        (object) "Phitsanulok"
      },
      {
        (object) "13",
        (object) "Phichit"
      },
      {
        (object) "14",
        (object) "Phetchabun"
      },
      {
        (object) "15",
        (object) "Uthai Thani"
      },
      {
        (object) "16",
        (object) "Nakhon Sawan"
      },
      {
        (object) "17",
        (object) "Nong Khai"
      },
      {
        (object) "18",
        (object) "Loei"
      },
      {
        (object) "20",
        (object) "Sakon Nakhon"
      },
      {
        (object) "21",
        (object) "Nakhon Phanom"
      },
      {
        (object) "22",
        (object) "Khon Kaen"
      },
      {
        (object) "23",
        (object) "Kalasin"
      },
      {
        (object) "24",
        (object) "Maha Sarakham"
      },
      {
        (object) "25",
        (object) "Roi Et"
      },
      {
        (object) "26",
        (object) "Chaiyaphum"
      },
      {
        (object) "27",
        (object) "Nakhon Ratchasima"
      },
      {
        (object) "28",
        (object) "Buriram"
      },
      {
        (object) "29",
        (object) "Surin"
      },
      {
        (object) "30",
        (object) "Sisaket"
      },
      {
        (object) "31",
        (object) "Narathiwat"
      },
      {
        (object) "32",
        (object) "Chai Nat"
      },
      {
        (object) "33",
        (object) "Sing Buri"
      },
      {
        (object) "34",
        (object) "Lop Buri"
      },
      {
        (object) "35",
        (object) "Ang Thong"
      },
      {
        (object) "36",
        (object) "Phra Nakhon Si Ayutthaya"
      },
      {
        (object) "37",
        (object) "Saraburi"
      },
      {
        (object) "38",
        (object) "Nonthaburi"
      },
      {
        (object) "39",
        (object) "Pathum Thani"
      },
      {
        (object) "40",
        (object) "Krung Thep"
      },
      {
        (object) "41",
        (object) "Phayao"
      },
      {
        (object) "42",
        (object) "Samut Prakan"
      },
      {
        (object) "43",
        (object) "Nakhon Nayok"
      },
      {
        (object) "44",
        (object) "Chachoengsao"
      },
      {
        (object) "45",
        (object) "Prachin Buri"
      },
      {
        (object) "46",
        (object) "Chon Buri"
      },
      {
        (object) "47",
        (object) "Rayong"
      },
      {
        (object) "48",
        (object) "Chanthaburi"
      },
      {
        (object) "49",
        (object) "Trat"
      },
      {
        (object) "50",
        (object) "Kanchanaburi"
      },
      {
        (object) "51",
        (object) "Suphan Buri"
      },
      {
        (object) "52",
        (object) "Ratchaburi"
      },
      {
        (object) "53",
        (object) "Nakhon Pathom"
      },
      {
        (object) "54",
        (object) "Samut Songkhram"
      },
      {
        (object) "55",
        (object) "Samut Sakhon"
      },
      {
        (object) "56",
        (object) "Phetchaburi"
      },
      {
        (object) "57",
        (object) "Prachuap Khiri Khan"
      },
      {
        (object) "58",
        (object) "Chumphon"
      },
      {
        (object) "59",
        (object) "Ranong"
      },
      {
        (object) "60",
        (object) "Surat Thani"
      },
      {
        (object) "61",
        (object) "Phangnga"
      },
      {
        (object) "62",
        (object) "Phuket"
      },
      {
        (object) "63",
        (object) "Krabi"
      },
      {
        (object) "64",
        (object) "Nakhon Si Thammarat"
      },
      {
        (object) "65",
        (object) "Trang"
      },
      {
        (object) "66",
        (object) "Phatthalung"
      },
      {
        (object) "67",
        (object) "Satun"
      },
      {
        (object) "68",
        (object) "Songkhla"
      },
      {
        (object) "69",
        (object) "Pattani"
      },
      {
        (object) "70",
        (object) "Yala"
      },
      {
        (object) "71",
        (object) "Ubon Ratchathani"
      },
      {
        (object) "72",
        (object) "Yasothon"
      },
      {
        (object) "73",
        (object) "Nakhon Phanom"
      },
      {
        (object) "75",
        (object) "Ubon Ratchathani"
      },
      {
        (object) "76",
        (object) "Udon Thani"
      },
      {
        (object) "77",
        (object) "Amnat Charoen"
      },
      {
        (object) "78",
        (object) "Mukdahan"
      },
      {
        (object) "79",
        (object) "Nong Bua Lamphu"
      },
      {
        (object) "80",
        (object) "Sa Kaeo"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TJ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Kuhistoni Badakhshon"
      },
      {
        (object) "02",
        (object) "Khatlon"
      },
      {
        (object) "03",
        (object) "Sughd"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Ahal"
      },
      {
        (object) "02",
        (object) "Balkan"
      },
      {
        (object) "03",
        (object) "Dashoguz"
      },
      {
        (object) "04",
        (object) "Lebap"
      },
      {
        (object) "05",
        (object) "Mary"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TN", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Kasserine"
      },
      {
        (object) "03",
        (object) "Kairouan"
      },
      {
        (object) "06",
        (object) "Jendouba"
      },
      {
        (object) "10",
        (object) "Qafsah"
      },
      {
        (object) "14",
        (object) "El Kef"
      },
      {
        (object) "15",
        (object) "Al Mahdia"
      },
      {
        (object) "16",
        (object) "Al Munastir"
      },
      {
        (object) "17",
        (object) "Bajah"
      },
      {
        (object) "18",
        (object) "Bizerte"
      },
      {
        (object) "19",
        (object) "Nabeul"
      },
      {
        (object) "22",
        (object) "Siliana"
      },
      {
        (object) "23",
        (object) "Sousse"
      },
      {
        (object) "27",
        (object) "Ben Arous"
      },
      {
        (object) "28",
        (object) "Madanin"
      },
      {
        (object) "29",
        (object) "Gabes"
      },
      {
        (object) "31",
        (object) "Kebili"
      },
      {
        (object) "32",
        (object) "Sfax"
      },
      {
        (object) "33",
        (object) "Sidi Bou Zid"
      },
      {
        (object) "34",
        (object) "Tataouine"
      },
      {
        (object) "35",
        (object) "Tozeur"
      },
      {
        (object) "36",
        (object) "Tunis"
      },
      {
        (object) "37",
        (object) "Zaghouan"
      },
      {
        (object) "38",
        (object) "Aiana"
      },
      {
        (object) "39",
        (object) "Manouba"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TO", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Ha"
      },
      {
        (object) "02",
        (object) "Tongatapu"
      },
      {
        (object) "03",
        (object) "Vava"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TR", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Adiyaman"
      },
      {
        (object) "03",
        (object) "Afyonkarahisar"
      },
      {
        (object) "04",
        (object) "Agri"
      },
      {
        (object) "05",
        (object) "Amasya"
      },
      {
        (object) "07",
        (object) "Antalya"
      },
      {
        (object) "08",
        (object) "Artvin"
      },
      {
        (object) "09",
        (object) "Aydin"
      },
      {
        (object) "10",
        (object) "Balikesir"
      },
      {
        (object) "11",
        (object) "Bilecik"
      },
      {
        (object) "12",
        (object) "Bingol"
      },
      {
        (object) "13",
        (object) "Bitlis"
      },
      {
        (object) "14",
        (object) "Bolu"
      },
      {
        (object) "15",
        (object) "Burdur"
      },
      {
        (object) "16",
        (object) "Bursa"
      },
      {
        (object) "17",
        (object) "Canakkale"
      },
      {
        (object) "19",
        (object) "Corum"
      },
      {
        (object) "20",
        (object) "Denizli"
      },
      {
        (object) "21",
        (object) "Diyarbakir"
      },
      {
        (object) "22",
        (object) "Edirne"
      },
      {
        (object) "23",
        (object) "Elazig"
      },
      {
        (object) "24",
        (object) "Erzincan"
      },
      {
        (object) "25",
        (object) "Erzurum"
      },
      {
        (object) "26",
        (object) "Eskisehir"
      },
      {
        (object) "28",
        (object) "Giresun"
      },
      {
        (object) "31",
        (object) "Hatay"
      },
      {
        (object) "32",
        (object) "Mersin"
      },
      {
        (object) "33",
        (object) "Isparta"
      },
      {
        (object) "34",
        (object) "Istanbul"
      },
      {
        (object) "35",
        (object) "Izmir"
      },
      {
        (object) "37",
        (object) "Kastamonu"
      },
      {
        (object) "38",
        (object) "Kayseri"
      },
      {
        (object) "39",
        (object) "Kirklareli"
      },
      {
        (object) "40",
        (object) "Kirsehir"
      },
      {
        (object) "41",
        (object) "Kocaeli"
      },
      {
        (object) "43",
        (object) "Kutahya"
      },
      {
        (object) "44",
        (object) "Malatya"
      },
      {
        (object) "45",
        (object) "Manisa"
      },
      {
        (object) "46",
        (object) "Kahramanmaras"
      },
      {
        (object) "48",
        (object) "Mugla"
      },
      {
        (object) "49",
        (object) "Mus"
      },
      {
        (object) "50",
        (object) "Nevsehir"
      },
      {
        (object) "52",
        (object) "Ordu"
      },
      {
        (object) "53",
        (object) "Rize"
      },
      {
        (object) "54",
        (object) "Sakarya"
      },
      {
        (object) "55",
        (object) "Samsun"
      },
      {
        (object) "57",
        (object) "Sinop"
      },
      {
        (object) "58",
        (object) "Sivas"
      },
      {
        (object) "59",
        (object) "Tekirdag"
      },
      {
        (object) "60",
        (object) "Tokat"
      },
      {
        (object) "61",
        (object) "Trabzon"
      },
      {
        (object) "62",
        (object) "Tunceli"
      },
      {
        (object) "63",
        (object) "Sanliurfa"
      },
      {
        (object) "64",
        (object) "Usak"
      },
      {
        (object) "65",
        (object) "Van"
      },
      {
        (object) "66",
        (object) "Yozgat"
      },
      {
        (object) "68",
        (object) "Ankara"
      },
      {
        (object) "69",
        (object) "Gumushane"
      },
      {
        (object) "70",
        (object) "Hakkari"
      },
      {
        (object) "71",
        (object) "Konya"
      },
      {
        (object) "72",
        (object) "Mardin"
      },
      {
        (object) "73",
        (object) "Nigde"
      },
      {
        (object) "74",
        (object) "Siirt"
      },
      {
        (object) "75",
        (object) "Aksaray"
      },
      {
        (object) "76",
        (object) "Batman"
      },
      {
        (object) "77",
        (object) "Bayburt"
      },
      {
        (object) "78",
        (object) "Karaman"
      },
      {
        (object) "79",
        (object) "Kirikkale"
      },
      {
        (object) "80",
        (object) "Sirnak"
      },
      {
        (object) "81",
        (object) "Adana"
      },
      {
        (object) "82",
        (object) "Cankiri"
      },
      {
        (object) "83",
        (object) "Gaziantep"
      },
      {
        (object) "84",
        (object) "Kars"
      },
      {
        (object) "85",
        (object) "Zonguldak"
      },
      {
        (object) "86",
        (object) "Ardahan"
      },
      {
        (object) "87",
        (object) "Bartin"
      },
      {
        (object) "88",
        (object) "Igdir"
      },
      {
        (object) "89",
        (object) "Karabuk"
      },
      {
        (object) "90",
        (object) "Kilis"
      },
      {
        (object) "91",
        (object) "Osmaniye"
      },
      {
        (object) "92",
        (object) "Yalova"
      },
      {
        (object) "93",
        (object) "Duzce"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TT", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Arima"
      },
      {
        (object) "02",
        (object) "Caroni"
      },
      {
        (object) "03",
        (object) "Mayaro"
      },
      {
        (object) "04",
        (object) "Nariva"
      },
      {
        (object) "05",
        (object) "Port-of-Spain"
      },
      {
        (object) "06",
        (object) "Saint Andrew"
      },
      {
        (object) "07",
        (object) "Saint David"
      },
      {
        (object) "08",
        (object) "Saint George"
      },
      {
        (object) "09",
        (object) "Saint Patrick"
      },
      {
        (object) "10",
        (object) "San Fernando"
      },
      {
        (object) "11",
        (object) "Tobago"
      },
      {
        (object) "12",
        (object) "Victoria"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TW", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Fu-chien"
      },
      {
        (object) "02",
        (object) "Kao-hsiung"
      },
      {
        (object) "03",
        (object) "T'ai-pei"
      },
      {
        (object) "04",
        (object) "T'ai-wan"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "TZ", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Pwani"
      },
      {
        (object) "03",
        (object) "Dodoma"
      },
      {
        (object) "04",
        (object) "Iringa"
      },
      {
        (object) "05",
        (object) "Kigoma"
      },
      {
        (object) "06",
        (object) "Kilimanjaro"
      },
      {
        (object) "07",
        (object) "Lindi"
      },
      {
        (object) "08",
        (object) "Mara"
      },
      {
        (object) "09",
        (object) "Mbeya"
      },
      {
        (object) "10",
        (object) "Morogoro"
      },
      {
        (object) "11",
        (object) "Mtwara"
      },
      {
        (object) "12",
        (object) "Mwanza"
      },
      {
        (object) "13",
        (object) "Pemba North"
      },
      {
        (object) "14",
        (object) "Ruvuma"
      },
      {
        (object) "15",
        (object) "Shinyanga"
      },
      {
        (object) "16",
        (object) "Singida"
      },
      {
        (object) "17",
        (object) "Tabora"
      },
      {
        (object) "18",
        (object) "Tanga"
      },
      {
        (object) "19",
        (object) "Kagera"
      },
      {
        (object) "20",
        (object) "Pemba South"
      },
      {
        (object) "21",
        (object) "Zanzibar Central"
      },
      {
        (object) "22",
        (object) "Zanzibar North"
      },
      {
        (object) "23",
        (object) "Dar es Salaam"
      },
      {
        (object) "24",
        (object) "Rukwa"
      },
      {
        (object) "25",
        (object) "Zanzibar Urban"
      },
      {
        (object) "26",
        (object) "Arusha"
      },
      {
        (object) "27",
        (object) "Manyara"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "UA", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Cherkas'ka ObLast'"
      },
      {
        (object) "02",
        (object) "Chernihivs'ka ObLast'"
      },
      {
        (object) "03",
        (object) "Chernivets'ka ObLast'"
      },
      {
        (object) "04",
        (object) "Dnipropetrovs'ka ObLast'"
      },
      {
        (object) "05",
        (object) "Donets'ka ObLast'"
      },
      {
        (object) "06",
        (object) "Ivano-Frankivs'ka ObLast'"
      },
      {
        (object) "07",
        (object) "Kharkivs'ka ObLast'"
      },
      {
        (object) "08",
        (object) "Khersons'ka ObLast'"
      },
      {
        (object) "09",
        (object) "Khmel'nyts'ka ObLast'"
      },
      {
        (object) "10",
        (object) "Kirovohrads'ka ObLast'"
      },
      {
        (object) "11",
        (object) "Krym"
      },
      {
        (object) "12",
        (object) "Kyyiv"
      },
      {
        (object) "13",
        (object) "Kyyivs'ka ObLast'"
      },
      {
        (object) "14",
        (object) "Luhans'ka ObLast'"
      },
      {
        (object) "15",
        (object) "L'vivs'ka ObLast'"
      },
      {
        (object) "16",
        (object) "Mykolayivs'ka ObLast'"
      },
      {
        (object) "17",
        (object) "Odes'ka ObLast'"
      },
      {
        (object) "18",
        (object) "Poltavs'ka ObLast'"
      },
      {
        (object) "19",
        (object) "Rivnens'ka ObLast'"
      },
      {
        (object) "20",
        (object) "Sevastopol'"
      },
      {
        (object) "21",
        (object) "Sums'ka ObLast'"
      },
      {
        (object) "22",
        (object) "Ternopil's'ka ObLast'"
      },
      {
        (object) "23",
        (object) "Vinnyts'ka ObLast'"
      },
      {
        (object) "24",
        (object) "Volyns'ka ObLast'"
      },
      {
        (object) "25",
        (object) "Zakarpats'ka ObLast'"
      },
      {
        (object) "26",
        (object) "Zaporiz'ka ObLast'"
      },
      {
        (object) "27",
        (object) "Zhytomyrs'ka ObLast'"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "UG", (object) new Hashtable()
    {
      {
        (object) "26",
        (object) "Apac"
      },
      {
        (object) "28",
        (object) "Bundibugyo"
      },
      {
        (object) "29",
        (object) "Bushenyi"
      },
      {
        (object) "30",
        (object) "Gulu"
      },
      {
        (object) "31",
        (object) "Hoima"
      },
      {
        (object) "33",
        (object) "Jinja"
      },
      {
        (object) "36",
        (object) "Kalangala"
      },
      {
        (object) "37",
        (object) "Kampala"
      },
      {
        (object) "38",
        (object) "Kamuli"
      },
      {
        (object) "39",
        (object) "Kapchorwa"
      },
      {
        (object) "40",
        (object) "Kasese"
      },
      {
        (object) "41",
        (object) "Kibale"
      },
      {
        (object) "42",
        (object) "Kiboga"
      },
      {
        (object) "43",
        (object) "Kisoro"
      },
      {
        (object) "45",
        (object) "Kotido"
      },
      {
        (object) "46",
        (object) "Kumi"
      },
      {
        (object) "47",
        (object) "Lira"
      },
      {
        (object) "50",
        (object) "Masindi"
      },
      {
        (object) "52",
        (object) "Mbarara"
      },
      {
        (object) "56",
        (object) "Mubende"
      },
      {
        (object) "58",
        (object) "Nebbi"
      },
      {
        (object) "59",
        (object) "Ntungamo"
      },
      {
        (object) "60",
        (object) "Pallisa"
      },
      {
        (object) "61",
        (object) "Rakai"
      },
      {
        (object) "65",
        (object) "Adjumani"
      },
      {
        (object) "66",
        (object) "Bugiri"
      },
      {
        (object) "67",
        (object) "Busia"
      },
      {
        (object) "69",
        (object) "Katakwi"
      },
      {
        (object) "70",
        (object) "Luwero"
      },
      {
        (object) "71",
        (object) "Masaka"
      },
      {
        (object) "72",
        (object) "Moyo"
      },
      {
        (object) "73",
        (object) "Nakasongola"
      },
      {
        (object) "74",
        (object) "Sembabule"
      },
      {
        (object) "76",
        (object) "Tororo"
      },
      {
        (object) "77",
        (object) "Arua"
      },
      {
        (object) "78",
        (object) "Iganga"
      },
      {
        (object) "79",
        (object) "Kabarole"
      },
      {
        (object) "80",
        (object) "Kaberamaido"
      },
      {
        (object) "81",
        (object) "Kamwenge"
      },
      {
        (object) "82",
        (object) "Kanungu"
      },
      {
        (object) "83",
        (object) "Kayunga"
      },
      {
        (object) "84",
        (object) "Kitgum"
      },
      {
        (object) "85",
        (object) "Kyenjojo"
      },
      {
        (object) "86",
        (object) "Mayuge"
      },
      {
        (object) "87",
        (object) "Mbale"
      },
      {
        (object) "88",
        (object) "Moroto"
      },
      {
        (object) "89",
        (object) "Mpigi"
      },
      {
        (object) "90",
        (object) "Mukono"
      },
      {
        (object) "91",
        (object) "Nakapiripirit"
      },
      {
        (object) "92",
        (object) "Pader"
      },
      {
        (object) "93",
        (object) "Rukungiri"
      },
      {
        (object) "94",
        (object) "Sironko"
      },
      {
        (object) "95",
        (object) "Soroti"
      },
      {
        (object) "96",
        (object) "Wakiso"
      },
      {
        (object) "97",
        (object) "Yumbe"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "US", (object) new Hashtable()
    {
      {
        (object) "AA",
        (object) "Armed Forces Americas"
      },
      {
        (object) "AE",
        (object) "Armed Forces Europe, Middle East, & Canada"
      },
      {
        (object) "AK",
        (object) "Alaska"
      },
      {
        (object) "AL",
        (object) "Alabama"
      },
      {
        (object) "AP",
        (object) "Armed Forces Pacific"
      },
      {
        (object) "AR",
        (object) "Arkansas"
      },
      {
        (object) "AS",
        (object) "American Samoa"
      },
      {
        (object) "AZ",
        (object) "Arizona"
      },
      {
        (object) "CA",
        (object) "California"
      },
      {
        (object) "CO",
        (object) "Colorado"
      },
      {
        (object) "CT",
        (object) "Connecticut"
      },
      {
        (object) "DC",
        (object) "District of Columbia"
      },
      {
        (object) "DE",
        (object) "Delaware"
      },
      {
        (object) "FL",
        (object) "Florida"
      },
      {
        (object) "FM",
        (object) "Federated States of Micronesia"
      },
      {
        (object) "GA",
        (object) "Georgia"
      },
      {
        (object) "GU",
        (object) "Guam"
      },
      {
        (object) "HI",
        (object) "Hawaii"
      },
      {
        (object) "IA",
        (object) "Iowa"
      },
      {
        (object) "ID",
        (object) "Idaho"
      },
      {
        (object) "IL",
        (object) "Illinois"
      },
      {
        (object) "IN",
        (object) "Indiana"
      },
      {
        (object) "KS",
        (object) "Kansas"
      },
      {
        (object) "KY",
        (object) "Kentucky"
      },
      {
        (object) "LA",
        (object) "Louisiana"
      },
      {
        (object) "MA",
        (object) "Massachusetts"
      },
      {
        (object) "MD",
        (object) "Maryland"
      },
      {
        (object) "ME",
        (object) "Maine"
      },
      {
        (object) "MH",
        (object) "Marshall Islands"
      },
      {
        (object) "MI",
        (object) "Michigan"
      },
      {
        (object) "MN",
        (object) "Minnesota"
      },
      {
        (object) "MO",
        (object) "Missouri"
      },
      {
        (object) "MP",
        (object) "Northern Mariana Islands"
      },
      {
        (object) "MS",
        (object) "Mississippi"
      },
      {
        (object) "MT",
        (object) "Montana"
      },
      {
        (object) "NC",
        (object) "North Carolina"
      },
      {
        (object) "ND",
        (object) "North Dakota"
      },
      {
        (object) "NE",
        (object) "Nebraska"
      },
      {
        (object) "NH",
        (object) "New Hampshire"
      },
      {
        (object) "NJ",
        (object) "New Jersey"
      },
      {
        (object) "NM",
        (object) "New Mexico"
      },
      {
        (object) "NV",
        (object) "Nevada"
      },
      {
        (object) "NY",
        (object) "New York"
      },
      {
        (object) "OH",
        (object) "Ohio"
      },
      {
        (object) "OK",
        (object) "Oklahoma"
      },
      {
        (object) "OR",
        (object) "Oregon"
      },
      {
        (object) "PA",
        (object) "Pennsylvania"
      },
      {
        (object) "PR",
        (object) "Puerto Rico"
      },
      {
        (object) "PW",
        (object) "Palau"
      },
      {
        (object) "RI",
        (object) "Rhode Island"
      },
      {
        (object) "SC",
        (object) "South Carolina"
      },
      {
        (object) "SD",
        (object) "South Dakota"
      },
      {
        (object) "TN",
        (object) "Tennessee"
      },
      {
        (object) "TX",
        (object) "Texas"
      },
      {
        (object) "UT",
        (object) "Utah"
      },
      {
        (object) "VA",
        (object) "Virginia"
      },
      {
        (object) "VI",
        (object) "Virgin Islands"
      },
      {
        (object) "VT",
        (object) "Vermont"
      },
      {
        (object) "WA",
        (object) "Washington"
      },
      {
        (object) "WI",
        (object) "Wisconsin"
      },
      {
        (object) "WV",
        (object) "West Virginia"
      },
      {
        (object) "WY",
        (object) "Wyoming"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "UY", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Artigas"
      },
      {
        (object) "02",
        (object) "Canelones"
      },
      {
        (object) "03",
        (object) "Cerro Largo"
      },
      {
        (object) "04",
        (object) "Colonia"
      },
      {
        (object) "05",
        (object) "Durazno"
      },
      {
        (object) "06",
        (object) "Flores"
      },
      {
        (object) "07",
        (object) "Florida"
      },
      {
        (object) "08",
        (object) "Lavalleja"
      },
      {
        (object) "09",
        (object) "Maldonado"
      },
      {
        (object) "10",
        (object) "Montevideo"
      },
      {
        (object) "11",
        (object) "Paysandu"
      },
      {
        (object) "12",
        (object) "Rio Negro"
      },
      {
        (object) "13",
        (object) "Rivera"
      },
      {
        (object) "14",
        (object) "Rocha"
      },
      {
        (object) "15",
        (object) "Salto"
      },
      {
        (object) "16",
        (object) "San Jose"
      },
      {
        (object) "17",
        (object) "Soriano"
      },
      {
        (object) "18",
        (object) "Tacuarembo"
      },
      {
        (object) "19",
        (object) "Treinta y Tres"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "UZ", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Andijon"
      },
      {
        (object) "02",
        (object) "Bukhoro"
      },
      {
        (object) "03",
        (object) "Farghona"
      },
      {
        (object) "04",
        (object) "Jizzakh"
      },
      {
        (object) "05",
        (object) "Khorazm"
      },
      {
        (object) "06",
        (object) "Namangan"
      },
      {
        (object) "07",
        (object) "Nawoiy"
      },
      {
        (object) "08",
        (object) "Qashqadaryo"
      },
      {
        (object) "09",
        (object) "Qoraqalpoghiston"
      },
      {
        (object) "10",
        (object) "Samarqand"
      },
      {
        (object) "11",
        (object) "Sirdaryo"
      },
      {
        (object) "12",
        (object) "Surkhondaryo"
      },
      {
        (object) "13",
        (object) "Toshkent"
      },
      {
        (object) "14",
        (object) "Toshkent"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "VC", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Charlotte"
      },
      {
        (object) "02",
        (object) "Saint Andrew"
      },
      {
        (object) "03",
        (object) "Saint David"
      },
      {
        (object) "04",
        (object) "Saint George"
      },
      {
        (object) "05",
        (object) "Saint Patrick"
      },
      {
        (object) "06",
        (object) "Grenadines"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "VE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Amazonas"
      },
      {
        (object) "02",
        (object) "Anzoategui"
      },
      {
        (object) "03",
        (object) "Apure"
      },
      {
        (object) "04",
        (object) "Aragua"
      },
      {
        (object) "05",
        (object) "Barinas"
      },
      {
        (object) "06",
        (object) "Bolivar"
      },
      {
        (object) "07",
        (object) "Carabobo"
      },
      {
        (object) "08",
        (object) "Cojedes"
      },
      {
        (object) "09",
        (object) "Delta Amacuro"
      },
      {
        (object) "11",
        (object) "Falcon"
      },
      {
        (object) "12",
        (object) "Guarico"
      },
      {
        (object) "13",
        (object) "Lara"
      },
      {
        (object) "14",
        (object) "Merida"
      },
      {
        (object) "15",
        (object) "Miranda"
      },
      {
        (object) "16",
        (object) "Monagas"
      },
      {
        (object) "17",
        (object) "Nueva Esparta"
      },
      {
        (object) "18",
        (object) "Portuguesa"
      },
      {
        (object) "19",
        (object) "Sucre"
      },
      {
        (object) "20",
        (object) "Tachira"
      },
      {
        (object) "21",
        (object) "Trujillo"
      },
      {
        (object) "22",
        (object) "Yaracuy"
      },
      {
        (object) "23",
        (object) "Zulia"
      },
      {
        (object) "24",
        (object) "Dependencias Federales"
      },
      {
        (object) "25",
        (object) "Distrito Federal"
      },
      {
        (object) "26",
        (object) "Vargas"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "VN", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "An Giang"
      },
      {
        (object) "03",
        (object) "Ben Tre"
      },
      {
        (object) "05",
        (object) "Cao Bang"
      },
      {
        (object) "09",
        (object) "Dong Thap"
      },
      {
        (object) "13",
        (object) "Hai Phong"
      },
      {
        (object) "20",
        (object) "Ho Chi Minh"
      },
      {
        (object) "21",
        (object) "Kien Giang"
      },
      {
        (object) "23",
        (object) "Lam Dong"
      },
      {
        (object) "24",
        (object) "Long An"
      },
      {
        (object) "30",
        (object) "Quang Ninh"
      },
      {
        (object) "32",
        (object) "Son La"
      },
      {
        (object) "33",
        (object) "Tay Ninh"
      },
      {
        (object) "34",
        (object) "Thanh Hoa"
      },
      {
        (object) "35",
        (object) "Thai Binh"
      },
      {
        (object) "37",
        (object) "Tien Giang"
      },
      {
        (object) "39",
        (object) "Lang Son"
      },
      {
        (object) "43",
        (object) "An Giang"
      },
      {
        (object) "44",
        (object) "Dac Lac"
      },
      {
        (object) "45",
        (object) "Dong Nai"
      },
      {
        (object) "46",
        (object) "Dong Thap"
      },
      {
        (object) "47",
        (object) "Kien Giang"
      },
      {
        (object) "49",
        (object) "Song Be"
      },
      {
        (object) "50",
        (object) "Vinh Phu"
      },
      {
        (object) "51",
        (object) "Ha Noi"
      },
      {
        (object) "52",
        (object) "Ho Chi Minh"
      },
      {
        (object) "53",
        (object) "Ba Ria-Vung Tau"
      },
      {
        (object) "54",
        (object) "Binh Dinh"
      },
      {
        (object) "55",
        (object) "Binh Thuan"
      },
      {
        (object) "58",
        (object) "Ha Giang"
      },
      {
        (object) "59",
        (object) "Ha Tay"
      },
      {
        (object) "60",
        (object) "Ha Tinh"
      },
      {
        (object) "61",
        (object) "Hoa Binh"
      },
      {
        (object) "62",
        (object) "Khanh Hoa"
      },
      {
        (object) "63",
        (object) "Kon Tum"
      },
      {
        (object) "64",
        (object) "Quang Tri"
      },
      {
        (object) "65",
        (object) "Nam Ha"
      },
      {
        (object) "66",
        (object) "Nghe An"
      },
      {
        (object) "67",
        (object) "Ninh Binh"
      },
      {
        (object) "68",
        (object) "Ninh Thuan"
      },
      {
        (object) "69",
        (object) "Phu Yen"
      },
      {
        (object) "70",
        (object) "Quang Binh"
      },
      {
        (object) "71",
        (object) "Quang Ngai"
      },
      {
        (object) "72",
        (object) "Quang Tri"
      },
      {
        (object) "73",
        (object) "Soc Trang"
      },
      {
        (object) "74",
        (object) "Thua Thien"
      },
      {
        (object) "75",
        (object) "Tra Vinh"
      },
      {
        (object) "76",
        (object) "Tuyen Quang"
      },
      {
        (object) "77",
        (object) "Vinh Long"
      },
      {
        (object) "78",
        (object) "Da Nang"
      },
      {
        (object) "79",
        (object) "Hai Duong"
      },
      {
        (object) "80",
        (object) "Ha Nam"
      },
      {
        (object) "81",
        (object) "Hung Yen"
      },
      {
        (object) "82",
        (object) "Nam Dinh"
      },
      {
        (object) "83",
        (object) "Phu Tho"
      },
      {
        (object) "84",
        (object) "Quang Nam"
      },
      {
        (object) "85",
        (object) "Thai Nguyen"
      },
      {
        (object) "86",
        (object) "Vinh Puc Province"
      },
      {
        (object) "87",
        (object) "Can Tho"
      },
      {
        (object) "88",
        (object) "Dak Lak"
      },
      {
        (object) "89",
        (object) "Lai Chau"
      },
      {
        (object) "90",
        (object) "Lao Cai"
      },
      {
        (object) "91",
        (object) "Dak Nong"
      },
      {
        (object) "92",
        (object) "Dien Bien"
      },
      {
        (object) "93",
        (object) "Hau Giang"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "VU", (object) new Hashtable()
    {
      {
        (object) "05",
        (object) "Ambrym"
      },
      {
        (object) "06",
        (object) "Aoba"
      },
      {
        (object) "07",
        (object) "Torba"
      },
      {
        (object) "08",
        (object) "Efate"
      },
      {
        (object) "09",
        (object) "Epi"
      },
      {
        (object) "10",
        (object) "Malakula"
      },
      {
        (object) "11",
        (object) "Paama"
      },
      {
        (object) "12",
        (object) "Pentecote"
      },
      {
        (object) "13",
        (object) "Sanma"
      },
      {
        (object) "14",
        (object) "Shepherd"
      },
      {
        (object) "15",
        (object) "Tafea"
      },
      {
        (object) "16",
        (object) "Malampa"
      },
      {
        (object) "17",
        (object) "Penama"
      },
      {
        (object) "18",
        (object) "Shefa"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "WS", (object) new Hashtable()
    {
      {
        (object) "02",
        (object) "Aiga-i-le-Tai"
      },
      {
        (object) "03",
        (object) "Atua"
      },
      {
        (object) "04",
        (object) "Fa"
      },
      {
        (object) "05",
        (object) "Gaga"
      },
      {
        (object) "06",
        (object) "Va"
      },
      {
        (object) "07",
        (object) "Gagaifomauga"
      },
      {
        (object) "08",
        (object) "Palauli"
      },
      {
        (object) "09",
        (object) "Satupa"
      },
      {
        (object) "10",
        (object) "Tuamasaga"
      },
      {
        (object) "11",
        (object) "Vaisigano"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "YE", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Abyan"
      },
      {
        (object) "02",
        (object) "Adan"
      },
      {
        (object) "03",
        (object) "Al Mahrah"
      },
      {
        (object) "04",
        (object) "Hadramawt"
      },
      {
        (object) "05",
        (object) "Shabwah"
      },
      {
        (object) "06",
        (object) "Lahij"
      },
      {
        (object) "07",
        (object) "Al Bayda'"
      },
      {
        (object) "08",
        (object) "Al Hudaydah"
      },
      {
        (object) "09",
        (object) "Al Jawf"
      },
      {
        (object) "10",
        (object) "Al Mahwit"
      },
      {
        (object) "11",
        (object) "Dhamar"
      },
      {
        (object) "12",
        (object) "Hajjah"
      },
      {
        (object) "13",
        (object) "Ibb"
      },
      {
        (object) "14",
        (object) "Ma'rib"
      },
      {
        (object) "15",
        (object) "Sa'dah"
      },
      {
        (object) "16",
        (object) "San'a'"
      },
      {
        (object) "17",
        (object) "Taizz"
      },
      {
        (object) "18",
        (object) "Ad Dali"
      },
      {
        (object) "19",
        (object) "Amran"
      },
      {
        (object) "20",
        (object) "Al Bayda'"
      },
      {
        (object) "21",
        (object) "Al Jawf"
      },
      {
        (object) "22",
        (object) "Hajjah"
      },
      {
        (object) "23",
        (object) "Ibb"
      },
      {
        (object) "24",
        (object) "Lahij"
      },
      {
        (object) "25",
        (object) "Taizz"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "ZA", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "North-Western Province"
      },
      {
        (object) "02",
        (object) "KwaZulu-Natal"
      },
      {
        (object) "03",
        (object) "Free State"
      },
      {
        (object) "05",
        (object) "Eastern Cape"
      },
      {
        (object) "06",
        (object) "Gauteng"
      },
      {
        (object) "07",
        (object) "Mpumalanga"
      },
      {
        (object) "08",
        (object) "Northern Cape"
      },
      {
        (object) "09",
        (object) "Limpopo"
      },
      {
        (object) "10",
        (object) "North-West"
      },
      {
        (object) "11",
        (object) "Western Cape"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "ZM", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Western"
      },
      {
        (object) "02",
        (object) "Central"
      },
      {
        (object) "03",
        (object) "Eastern"
      },
      {
        (object) "04",
        (object) "Luapula"
      },
      {
        (object) "05",
        (object) "Northern"
      },
      {
        (object) "06",
        (object) "North-Western"
      },
      {
        (object) "07",
        (object) "Southern"
      },
      {
        (object) "08",
        (object) "Copperbelt"
      },
      {
        (object) "09",
        (object) "Lusaka"
      }
    });
    RegionName.GEOIP_REGION_NAME.Add((object) "ZW", (object) new Hashtable()
    {
      {
        (object) "01",
        (object) "Manicaland"
      },
      {
        (object) "02",
        (object) "Midlands"
      },
      {
        (object) "03",
        (object) "Mashonaland Central"
      },
      {
        (object) "04",
        (object) "Mashonaland East"
      },
      {
        (object) "05",
        (object) "Mashonaland West"
      },
      {
        (object) "06",
        (object) "Matabeleland North"
      },
      {
        (object) "07",
        (object) "Matabeleland South"
      },
      {
        (object) "08",
        (object) "Masvingo"
      },
      {
        (object) "09",
        (object) "Bulawayo"
      },
      {
        (object) "10",
        (object) "Harare"
      }
    });
  }
}
