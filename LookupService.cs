// Decompiled with JetBrains decompiler
// Type: LookupService
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;

public class LookupService
{
  private static Country UNKNOWN_COUNTRY = new Country("--", "N/A");
  private static int COUNTRY_BEGIN = 16776960;
  private static int STRUCTURE_INFO_MAX_SIZE = 20;
  private static int DATABASE_INFO_MAX_SIZE = 100;
  private static int FULL_RECORD_LENGTH = 100;
  private static int SEGMENT_RECORD_LENGTH = 3;
  private static int STANDARD_RECORD_LENGTH = 3;
  private static int ORG_RECORD_LENGTH = 4;
  private static int MAX_RECORD_LENGTH = 4;
  private static int MAX_ORG_RECORD_LENGTH = 1000;
  private static int FIPS_RANGE = 360;
  private static int STATE_BEGIN_REV0 = 16700000;
  private static int STATE_BEGIN_REV1 = 16000000;
  private static int US_OFFSET = 1;
  private static int CANADA_OFFSET = 677;
  private static int WORLD_OFFSET = 1353;
  public static int GEOIP_STANDARD = 0;
  public static int GEOIP_MEMORY_CACHE = 1;
  public static int GEOIP_UNKNOWN_SPEED = 0;
  public static int GEOIP_DIALUP_SPEED = 1;
  public static int GEOIP_CABLEDSL_SPEED = 2;
  public static int GEOIP_CORPORATE_SPEED = 3;
  private static string[] countryCode = new string[254]
  {
    "--",
    "AP",
    "EU",
    "AD",
    "AE",
    "AF",
    "AG",
    "AI",
    "AL",
    "AM",
    "CW",
    "AO",
    "AQ",
    "AR",
    "AS",
    "AT",
    "AU",
    "AW",
    "AZ",
    "BA",
    "BB",
    "BD",
    "BE",
    "BF",
    "BG",
    "BH",
    "BI",
    "BJ",
    "BM",
    "BN",
    "BO",
    "BR",
    "BS",
    "BT",
    "BV",
    "BW",
    "BY",
    "BZ",
    "CA",
    "CC",
    "CD",
    "CF",
    "CG",
    "CH",
    "CI",
    "CK",
    "CL",
    "CM",
    "CN",
    "CO",
    "CR",
    "CU",
    "CV",
    "CX",
    "CY",
    "CZ",
    "DE",
    "DJ",
    "DK",
    "DM",
    "DO",
    "DZ",
    "EC",
    "EE",
    "EG",
    "EH",
    "ER",
    "ES",
    "ET",
    "FI",
    "FJ",
    "FK",
    "FM",
    "FO",
    "FR",
    "SX",
    "GA",
    "GB",
    "GD",
    "GE",
    "GF",
    "GH",
    "GI",
    "GL",
    "GM",
    "GN",
    "GP",
    "GQ",
    "GR",
    "GS",
    "GT",
    "GU",
    "GW",
    "GY",
    "HK",
    "HM",
    "HN",
    "HR",
    "HT",
    "HU",
    "ID",
    "IE",
    "IL",
    "IN",
    "IO",
    "IQ",
    "IR",
    "IS",
    "IT",
    "JM",
    "JO",
    "JP",
    "KE",
    "KG",
    "KH",
    "KI",
    "KM",
    "KN",
    "KP",
    "KR",
    "KW",
    "KY",
    "KZ",
    "LA",
    "LB",
    "LC",
    "LI",
    "LK",
    "LR",
    "LS",
    "LT",
    "LU",
    "LV",
    "LY",
    "MA",
    "MC",
    "MD",
    "MG",
    "MH",
    "MK",
    "ML",
    "MM",
    "MN",
    "MO",
    "MP",
    "MQ",
    "MR",
    "MS",
    "MT",
    "MU",
    "MV",
    "MW",
    "MX",
    "MY",
    "MZ",
    "NA",
    "NC",
    "NE",
    "NF",
    "NG",
    "NI",
    "NL",
    "NO",
    "NP",
    "NR",
    "NU",
    "NZ",
    "OM",
    "PA",
    "PE",
    "PF",
    "PG",
    "PH",
    "PK",
    "PL",
    "PM",
    "PN",
    "PR",
    "PS",
    "PT",
    "PW",
    "PY",
    "QA",
    "RE",
    "RO",
    "RU",
    "RW",
    "SA",
    "SB",
    "SC",
    "SD",
    "SE",
    "SG",
    "SH",
    "SI",
    "SJ",
    "SK",
    "SL",
    "SM",
    "SN",
    "SO",
    "SR",
    "ST",
    "SV",
    "SY",
    "SZ",
    "TC",
    "TD",
    "TF",
    "TG",
    "TH",
    "TJ",
    "TK",
    "TM",
    "TN",
    "TO",
    "TL",
    "TR",
    "TT",
    "TV",
    "TW",
    "TZ",
    "UA",
    "UG",
    "UM",
    "US",
    "UY",
    "UZ",
    "VA",
    "VC",
    "VE",
    "VG",
    "VI",
    "VN",
    "VU",
    "WF",
    "WS",
    "YE",
    "YT",
    "RS",
    "ZA",
    "ZM",
    "ME",
    "ZW",
    "A1",
    "A2",
    "O1",
    "AX",
    "GG",
    "IM",
    "JE",
    "BL",
    "MF",
    "BQ"
  };
  private static string[] countryName = new string[254]
  {
    "N/A",
    "Asia/Pacific Region",
    "Europe",
    "Andorra",
    "United Arab Emirates",
    "Afghanistan",
    "Antigua and Barbuda",
    "Anguilla",
    "Albania",
    "Armenia",
    "Curacao",
    "Angola",
    "Antarctica",
    "Argentina",
    "American Samoa",
    "Austria",
    "Australia",
    "Aruba",
    "Azerbaijan",
    "Bosnia and Herzegovina",
    "Barbados",
    "Bangladesh",
    "Belgium",
    "Burkina Faso",
    "Bulgaria",
    "Bahrain",
    "Burundi",
    "Benin",
    "Bermuda",
    "Brunei Darussalam",
    "Bolivia",
    "Brazil",
    "Bahamas",
    "Bhutan",
    "Bouvet Island",
    "Botswana",
    "Belarus",
    "Belize",
    "Canada",
    "Cocos (Keeling) Islands",
    "Congo, The Democratic Republic of the",
    "Central African Republic",
    "Congo",
    "Switzerland",
    "Cote D'Ivoire",
    "Cook Islands",
    "Chile",
    "Cameroon",
    "China",
    "Colombia",
    "Costa Rica",
    "Cuba",
    "Cape Verde",
    "Christmas Island",
    "Cyprus",
    "Czech Republic",
    "Germany",
    "Djibouti",
    "Denmark",
    "Dominica",
    "Dominican Republic",
    "Algeria",
    "Ecuador",
    "Estonia",
    "Egypt",
    "Western Sahara",
    "Eritrea",
    "Spain",
    "Ethiopia",
    "Finland",
    "Fiji",
    "Falkland Islands (Malvinas)",
    "Micronesia, Federated States of",
    "Faroe Islands",
    "France",
    "Sint Maarten (Dutch part)",
    "Gabon",
    "United Kingdom",
    "Grenada",
    "Georgia",
    "French Guiana",
    "Ghana",
    "Gibraltar",
    "Greenland",
    "Gambia",
    "Guinea",
    "Guadeloupe",
    "Equatorial Guinea",
    "Greece",
    "South Georgia and the South Sandwich Islands",
    "Guatemala",
    "Guam",
    "Guinea-Bissau",
    "Guyana",
    "Hong Kong",
    "Heard Island and McDonald Islands",
    "Honduras",
    "Croatia",
    "Haiti",
    "Hungary",
    "Indonesia",
    "Ireland",
    "Israel",
    "India",
    "British Indian Ocean Territory",
    "Iraq",
    "Iran, Islamic Republic of",
    "Iceland",
    "Italy",
    "Jamaica",
    "Jordan",
    "Japan",
    "Kenya",
    "Kyrgyzstan",
    "Cambodia",
    "Kiribati",
    "Comoros",
    "Saint Kitts and Nevis",
    "Korea, Democratic People's Republic of",
    "Korea, Republic of",
    "Kuwait",
    "Cayman Islands",
    "Kazakhstan",
    "Lao People's Democratic Republic",
    "Lebanon",
    "Saint Lucia",
    "Liechtenstein",
    "Sri Lanka",
    "Liberia",
    "Lesotho",
    "Lithuania",
    "Luxembourg",
    "Latvia",
    "Libya",
    "Morocco",
    "Monaco",
    "Moldova, Republic of",
    "Madagascar",
    "Marshall Islands",
    "Macedonia",
    "Mali",
    "Myanmar",
    "Mongolia",
    "Macau",
    "Northern Mariana Islands",
    "Martinique",
    "Mauritania",
    "Montserrat",
    "Malta",
    "Mauritius",
    "Maldives",
    "Malawi",
    "Mexico",
    "Malaysia",
    "Mozambique",
    "Namibia",
    "New Caledonia",
    "Niger",
    "Norfolk Island",
    "Nigeria",
    "Nicaragua",
    "Netherlands",
    "Norway",
    "Nepal",
    "Nauru",
    "Niue",
    "New Zealand",
    "Oman",
    "Panama",
    "Peru",
    "French Polynesia",
    "Papua New Guinea",
    "Philippines",
    "Pakistan",
    "Poland",
    "Saint Pierre and Miquelon",
    "Pitcairn Islands",
    "Puerto Rico",
    "Palestinian Territory",
    "Portugal",
    "Palau",
    "Paraguay",
    "Qatar",
    "Reunion",
    "Romania",
    "Russian Federation",
    "Rwanda",
    "Saudi Arabia",
    "Solomon Islands",
    "Seychelles",
    "Sudan",
    "Sweden",
    "Singapore",
    "Saint Helena",
    "Slovenia",
    "Svalbard and Jan Mayen",
    "Slovakia",
    "Sierra Leone",
    "San Marino",
    "Senegal",
    "Somalia",
    "Suriname",
    "Sao Tome and Principe",
    "El Salvador",
    "Syrian Arab Republic",
    "Swaziland",
    "Turks and Caicos Islands",
    "Chad",
    "French Southern Territories",
    "Togo",
    "Thailand",
    "Tajikistan",
    "Tokelau",
    "Turkmenistan",
    "Tunisia",
    "Tonga",
    "Timor-Leste",
    "Turkey",
    "Trinidad and Tobago",
    "Tuvalu",
    "Taiwan",
    "Tanzania, United Republic of",
    "Ukraine",
    "Uganda",
    "United States Minor Outlying Islands",
    "United States",
    "Uruguay",
    "Uzbekistan",
    "Holy See (Vatican City State)",
    "Saint Vincent and the Grenadines",
    "Venezuela",
    "Virgin Islands, British",
    "Virgin Islands, U.S.",
    "Vietnam",
    "Vanuatu",
    "Wallis and Futuna",
    "Samoa",
    "Yemen",
    "Mayotte",
    "Serbia",
    "South Africa",
    "Zambia",
    "Montenegro",
    "Zimbabwe",
    "Anonymous Proxy",
    "Satellite Provider",
    "Other",
    "Aland Islands",
    "Guernsey",
    "Isle of Man",
    "Jersey",
    "Saint Barthelemy",
    "Saint Martin",
    "Bonaire, Saint Eustatius and Saba"
  };
  private byte databaseType = byte.Parse(DatabaseInfo.COUNTRY_EDITION.ToString());
  private FileStream file;
  private DatabaseInfo databaseInfo;
  private int[] databaseSegments;
  private int recordLength;
  private int dboptions;
  private byte[] dbbuffer;

  public LookupService(string databaseFile, int options)
  {
    try
    {
      this.file = new FileStream(databaseFile, FileMode.Open, FileAccess.Read);
      this.dboptions = options;
      this.init();
    }
    catch (SystemException ex)
    {
      Console.Write("cannot open file " + databaseFile + "\n");
    }
  }

  public LookupService(string databaseFile)
    : this(databaseFile, LookupService.GEOIP_STANDARD)
  {
  }

  private void init()
  {
    byte[] buffer1 = new byte[3];
    byte[] buffer2 = new byte[LookupService.SEGMENT_RECORD_LENGTH];
    this.databaseType = (byte) DatabaseInfo.COUNTRY_EDITION;
    this.recordLength = LookupService.STANDARD_RECORD_LENGTH;
    this.file.Seek(-3L, SeekOrigin.End);
    for (int index1 = 0; index1 < LookupService.STRUCTURE_INFO_MAX_SIZE; ++index1)
    {
      this.file.Read(buffer1, 0, 3);
      if (buffer1[0] == byte.MaxValue && buffer1[1] == byte.MaxValue && buffer1[2] == byte.MaxValue)
      {
        this.databaseType = byte.Parse(this.file.ReadByte().ToString());
        if (this.databaseType >= (byte) 106)
          this.databaseType -= (byte) 105;
        if ((int) this.databaseType == DatabaseInfo.REGION_EDITION_REV0)
        {
          this.databaseSegments = new int[1];
          this.databaseSegments[0] = LookupService.STATE_BEGIN_REV0;
          this.recordLength = LookupService.STANDARD_RECORD_LENGTH;
          break;
        }
        if ((int) this.databaseType == DatabaseInfo.REGION_EDITION_REV1)
        {
          this.databaseSegments = new int[1];
          this.databaseSegments[0] = LookupService.STATE_BEGIN_REV1;
          this.recordLength = LookupService.STANDARD_RECORD_LENGTH;
          break;
        }
        if ((int) this.databaseType == DatabaseInfo.CITY_EDITION_REV0 || (int) this.databaseType == DatabaseInfo.CITY_EDITION_REV1 || ((int) this.databaseType == DatabaseInfo.ORG_EDITION || (int) this.databaseType == DatabaseInfo.ORG_EDITION_V6) || ((int) this.databaseType == DatabaseInfo.ISP_EDITION || (int) this.databaseType == DatabaseInfo.ISP_EDITION_V6 || ((int) this.databaseType == DatabaseInfo.ASNUM_EDITION || (int) this.databaseType == DatabaseInfo.ASNUM_EDITION_V6)) || ((int) this.databaseType == DatabaseInfo.NETSPEED_EDITION_REV1 || (int) this.databaseType == DatabaseInfo.NETSPEED_EDITION_REV1_V6 || ((int) this.databaseType == DatabaseInfo.CITY_EDITION_REV0_V6 || (int) this.databaseType == DatabaseInfo.CITY_EDITION_REV1_V6)))
        {
          this.databaseSegments = new int[1];
          this.databaseSegments[0] = 0;
          this.recordLength = (int) this.databaseType == DatabaseInfo.CITY_EDITION_REV0 || (int) this.databaseType == DatabaseInfo.CITY_EDITION_REV1 || ((int) this.databaseType == DatabaseInfo.ASNUM_EDITION_V6 || (int) this.databaseType == DatabaseInfo.NETSPEED_EDITION_REV1) || ((int) this.databaseType == DatabaseInfo.NETSPEED_EDITION_REV1_V6 || (int) this.databaseType == DatabaseInfo.CITY_EDITION_REV0_V6 || ((int) this.databaseType == DatabaseInfo.CITY_EDITION_REV1_V6 || (int) this.databaseType == DatabaseInfo.ASNUM_EDITION)) ? LookupService.STANDARD_RECORD_LENGTH : LookupService.ORG_RECORD_LENGTH;
          this.file.Read(buffer2, 0, LookupService.SEGMENT_RECORD_LENGTH);
          for (int index2 = 0; index2 < LookupService.SEGMENT_RECORD_LENGTH; ++index2)
            this.databaseSegments[0] += LookupService.unsignedByteToInt(buffer2[index2]) << index2 * 8;
          break;
        }
        break;
      }
      this.file.Seek(-4L, SeekOrigin.Current);
    }
    if ((int) this.databaseType == DatabaseInfo.COUNTRY_EDITION || (int) this.databaseType == DatabaseInfo.COUNTRY_EDITION_V6 || ((int) this.databaseType == DatabaseInfo.PROXY_EDITION || (int) this.databaseType == DatabaseInfo.NETSPEED_EDITION))
    {
      this.databaseSegments = new int[1];
      this.databaseSegments[0] = LookupService.COUNTRY_BEGIN;
      this.recordLength = LookupService.STANDARD_RECORD_LENGTH;
    }
    if ((this.dboptions & LookupService.GEOIP_MEMORY_CACHE) != 1)
      return;
    int length = (int) this.file.Length;
    this.dbbuffer = new byte[length];
    this.file.Seek(0L, SeekOrigin.Begin);
    this.file.Read(this.dbbuffer, 0, length);
  }

  public void close()
  {
    try
    {
      this.file.Close();
      this.file = (FileStream) null;
    }
    catch (Exception ex)
    {
    }
  }

  public Country getCountry(IPAddress ipAddress)
  {
    return this.getCountry(LookupService.bytestoLong(ipAddress.GetAddressBytes()));
  }

  public Country getCountryV6(string ipAddress)
  {
    IPAddress ipAddress1;
    try
    {
      ipAddress1 = IPAddress.Parse(ipAddress);
    }
    catch (Exception ex)
    {
      Console.Write(ex.Message);
      return LookupService.UNKNOWN_COUNTRY;
    }
    return this.getCountryV6(ipAddress1);
  }

  public Country getCountry(string ipAddress)
  {
    IPAddress ipAddress1;
    try
    {
      ipAddress1 = IPAddress.Parse(ipAddress);
    }
    catch (Exception ex)
    {
      Console.Write(ex.Message);
      return LookupService.UNKNOWN_COUNTRY;
    }
    return this.getCountry(LookupService.bytestoLong(ipAddress1.GetAddressBytes()));
  }

  public Country getCountryV6(IPAddress ipAddress)
  {
    if (this.file == null)
      throw new Exception("Database has been closed.");
    if ((int) this.databaseType == DatabaseInfo.CITY_EDITION_REV1 | (int) this.databaseType == DatabaseInfo.CITY_EDITION_REV0)
    {
      Location location = this.getLocation(ipAddress);
      if (location == null)
        return LookupService.UNKNOWN_COUNTRY;
      return new Country(location.countryCode, location.countryName);
    }
    int index = this.SeekCountryV6(ipAddress) - LookupService.COUNTRY_BEGIN;
    if (index == 0)
      return LookupService.UNKNOWN_COUNTRY;
    return new Country(LookupService.countryCode[index], LookupService.countryName[index]);
  }

  public Country getCountry(long ipAddress)
  {
    if (this.file == null)
      throw new Exception("Database has been closed.");
    if ((int) this.databaseType == DatabaseInfo.CITY_EDITION_REV1 | (int) this.databaseType == DatabaseInfo.CITY_EDITION_REV0)
    {
      Location location = this.getLocation(ipAddress);
      if (location == null)
        return LookupService.UNKNOWN_COUNTRY;
      return new Country(location.countryCode, location.countryName);
    }
    int index = this.SeekCountry(ipAddress) - LookupService.COUNTRY_BEGIN;
    if (index == 0)
      return LookupService.UNKNOWN_COUNTRY;
    return new Country(LookupService.countryCode[index], LookupService.countryName[index]);
  }

  public int getID(string ipAddress)
  {
    IPAddress ipAddress1;
    try
    {
      ipAddress1 = IPAddress.Parse(ipAddress);
    }
    catch (Exception ex)
    {
      Console.Write(ex.Message);
      return 0;
    }
    return this.getID(LookupService.bytestoLong(ipAddress1.GetAddressBytes()));
  }

  public int getID(IPAddress ipAddress)
  {
    return this.getID(LookupService.bytestoLong(ipAddress.GetAddressBytes()));
  }

  public int getID(long ipAddress)
  {
    if (this.file == null)
      throw new Exception("Database has been closed.");
    return this.SeekCountry(ipAddress) - this.databaseSegments[0];
  }

  public DatabaseInfo getDatabaseInfo()
  {
    if (this.databaseInfo != null)
      return this.databaseInfo;
    try
    {
      lock (this)
      {
        bool flag = false;
        byte[] buffer1 = new byte[3];
        this.file.Seek(-3L, SeekOrigin.End);
        for (int index = 0; index < LookupService.STRUCTURE_INFO_MAX_SIZE; ++index)
        {
          this.file.Read(buffer1, 0, 3);
          if (buffer1[0] == byte.MaxValue && buffer1[1] == byte.MaxValue && buffer1[2] == byte.MaxValue)
          {
            flag = true;
            break;
          }
        }
        if (flag)
          this.file.Seek(-3L, SeekOrigin.Current);
        else
          this.file.Seek(-3L, SeekOrigin.End);
        for (int count = 0; count < LookupService.DATABASE_INFO_MAX_SIZE; ++count)
        {
          this.file.Read(buffer1, 0, 3);
          if (buffer1[0] == (byte) 0 && buffer1[1] == (byte) 0 && buffer1[2] == (byte) 0)
          {
            byte[] buffer2 = new byte[count];
            char[] chArray = new char[count];
            this.file.Read(buffer2, 0, count);
            for (int index = 0; index < count; ++index)
              chArray[index] = char.Parse(buffer2[index].ToString());
            this.databaseInfo = new DatabaseInfo(new string(chArray));
            return this.databaseInfo;
          }
          this.file.Seek(-4L, SeekOrigin.Current);
        }
      }
    }
    catch (Exception ex)
    {
      Console.Write(ex.Message);
    }
    return new DatabaseInfo("");
  }

  public Region getRegion(IPAddress ipAddress)
  {
    return this.getRegion(LookupService.bytestoLong(ipAddress.GetAddressBytes()));
  }

  public Region getRegion(string str)
  {
    IPAddress ipAddress;
    try
    {
      ipAddress = IPAddress.Parse(str);
    }
    catch (Exception ex)
    {
      Console.Write(ex.Message);
      return (Region) null;
    }
    return this.getRegion(LookupService.bytestoLong(ipAddress.GetAddressBytes()));
  }

  [MethodImpl(MethodImplOptions.Synchronized)]
  public Region getRegion(long ipnum)
  {
    Region region = new Region();
    if ((int) this.databaseType == DatabaseInfo.REGION_EDITION_REV0)
    {
      int index = this.SeekCountry(ipnum) - LookupService.STATE_BEGIN_REV0;
      char[] chArray = new char[2];
      if (index >= 1000)
      {
        region.countryCode = "US";
        region.countryName = "United States";
        chArray[0] = (char) ((index - 1000) / 26 + 65);
        chArray[1] = (char) ((index - 1000) % 26 + 65);
        region.region = new string(chArray);
      }
      else
      {
        region.countryCode = LookupService.countryCode[index];
        region.countryName = LookupService.countryName[index];
        region.region = "";
      }
    }
    else if ((int) this.databaseType == DatabaseInfo.REGION_EDITION_REV1)
    {
      int num = this.SeekCountry(ipnum) - LookupService.STATE_BEGIN_REV1;
      char[] chArray = new char[2];
      if (num < LookupService.US_OFFSET)
      {
        region.countryCode = "";
        region.countryName = "";
        region.region = "";
      }
      else if (num < LookupService.CANADA_OFFSET)
      {
        region.countryCode = "US";
        region.countryName = "United States";
        chArray[0] = (char) ((num - LookupService.US_OFFSET) / 26 + 65);
        chArray[1] = (char) ((num - LookupService.US_OFFSET) % 26 + 65);
        region.region = new string(chArray);
      }
      else if (num < LookupService.WORLD_OFFSET)
      {
        region.countryCode = "CA";
        region.countryName = "Canada";
        chArray[0] = (char) ((num - LookupService.CANADA_OFFSET) / 26 + 65);
        chArray[1] = (char) ((num - LookupService.CANADA_OFFSET) % 26 + 65);
        region.region = new string(chArray);
      }
      else
      {
        region.countryCode = LookupService.countryCode[(num - LookupService.WORLD_OFFSET) / LookupService.FIPS_RANGE];
        region.countryName = LookupService.countryName[(num - LookupService.WORLD_OFFSET) / LookupService.FIPS_RANGE];
        region.region = "";
      }
    }
    return region;
  }

  public Location getLocation(IPAddress addr)
  {
    return this.getLocation(LookupService.bytestoLong(addr.GetAddressBytes()));
  }

  public Location getLocationV6(string str)
  {
    IPAddress addr;
    try
    {
      addr = IPAddress.Parse(str);
    }
    catch (Exception ex)
    {
      Console.Write(ex.Message);
      return (Location) null;
    }
    return this.getLocationV6(addr);
  }

  public Location getLocation(string str)
  {
    IPAddress ipAddress;
    try
    {
      ipAddress = IPAddress.Parse(str);
    }
    catch (Exception ex)
    {
      Console.Write(ex.Message);
      return (Location) null;
    }
    return this.getLocation(LookupService.bytestoLong(ipAddress.GetAddressBytes()));
  }

  [MethodImpl(MethodImplOptions.Synchronized)]
  public Location getLocationV6(IPAddress addr)
  {
    byte[] buffer = new byte[LookupService.FULL_RECORD_LENGTH];
    char[] chArray = new char[LookupService.FULL_RECORD_LENGTH];
    int num1 = 0;
    Location location = new Location();
    int length1 = 0;
    double num2 = 0.0;
    double num3 = 0.0;
    try
    {
      int num4 = this.SeekCountryV6(addr);
      if (num4 == this.databaseSegments[0])
        return (Location) null;
      int sourceIndex = num4 + (2 * this.recordLength - 1) * this.databaseSegments[0];
      if ((this.dboptions & LookupService.GEOIP_MEMORY_CACHE) == 1)
      {
        Array.Copy((Array) this.dbbuffer, sourceIndex, (Array) buffer, 0, Math.Min(this.dbbuffer.Length - sourceIndex, LookupService.FULL_RECORD_LENGTH));
      }
      else
      {
        this.file.Seek((long) sourceIndex, SeekOrigin.Begin);
        this.file.Read(buffer, 0, LookupService.FULL_RECORD_LENGTH);
      }
      for (int index = 0; index < LookupService.FULL_RECORD_LENGTH; ++index)
        chArray[index] = char.Parse(buffer[index].ToString());
      location.countryCode = LookupService.countryCode[LookupService.unsignedByteToInt(buffer[0])];
      location.countryName = LookupService.countryName[LookupService.unsignedByteToInt(buffer[0])];
      int startIndex1 = num1 + 1;
      while (buffer[startIndex1 + length1] != (byte) 0)
        ++length1;
      if (length1 > 0)
        location.region = new string(chArray, startIndex1, length1);
      int startIndex2 = startIndex1 + (length1 + 1);
      int length2 = 0;
      location.regionName = RegionName.getRegionName(location.countryCode, location.region);
      while (buffer[startIndex2 + length2] != (byte) 0)
        ++length2;
      if (length2 > 0)
        location.city = new string(chArray, startIndex2, length2);
      int startIndex3 = startIndex2 + (length2 + 1);
      int length3 = 0;
      while (buffer[startIndex3 + length3] != (byte) 0)
        ++length3;
      if (length3 > 0)
        location.postalCode = new string(chArray, startIndex3, length3);
      int num5 = startIndex3 + (length3 + 1);
      for (int index = 0; index < 3; ++index)
        num2 += (double) (LookupService.unsignedByteToInt(buffer[num5 + index]) << index * 8);
      location.latitude = num2 / 10000.0 - 180.0;
      int num6 = num5 + 3;
      for (int index = 0; index < 3; ++index)
        num3 += (double) (LookupService.unsignedByteToInt(buffer[num6 + index]) << index * 8);
      location.longitude = num3 / 10000.0 - 180.0;
      location.metro_code = location.dma_code = 0;
      location.area_code = 0;
      if ((int) this.databaseType != DatabaseInfo.CITY_EDITION_REV1)
      {
        if ((int) this.databaseType != DatabaseInfo.CITY_EDITION_REV1_V6)
          goto label_38;
      }
      int num7 = 0;
      if (location.countryCode == "US")
      {
        int num8 = num6 + 3;
        for (int index = 0; index < 3; ++index)
          num7 += LookupService.unsignedByteToInt(buffer[num8 + index]) << index * 8;
        location.metro_code = location.dma_code = num7 / 1000;
        location.area_code = num7 % 1000;
      }
    }
    catch (IOException ex)
    {
      Console.Write("IO Exception while seting up segments");
    }
label_38:
    return location;
  }

  [MethodImpl(MethodImplOptions.Synchronized)]
  public Location getLocation(long ipnum)
  {
    byte[] buffer = new byte[LookupService.FULL_RECORD_LENGTH];
    char[] chArray = new char[LookupService.FULL_RECORD_LENGTH];
    int num1 = 0;
    Location location = new Location();
    int length1 = 0;
    double num2 = 0.0;
    double num3 = 0.0;
    try
    {
      int num4 = this.SeekCountry(ipnum);
      if (num4 == this.databaseSegments[0])
        return (Location) null;
      int sourceIndex = num4 + (2 * this.recordLength - 1) * this.databaseSegments[0];
      if ((this.dboptions & LookupService.GEOIP_MEMORY_CACHE) == 1)
      {
        Array.Copy((Array) this.dbbuffer, sourceIndex, (Array) buffer, 0, Math.Min(this.dbbuffer.Length - sourceIndex, LookupService.FULL_RECORD_LENGTH));
      }
      else
      {
        this.file.Seek((long) sourceIndex, SeekOrigin.Begin);
        this.file.Read(buffer, 0, LookupService.FULL_RECORD_LENGTH);
      }
      for (int index = 0; index < LookupService.FULL_RECORD_LENGTH; ++index)
        chArray[index] = char.Parse(buffer[index].ToString());
      location.countryCode = LookupService.countryCode[LookupService.unsignedByteToInt(buffer[0])];
      location.countryName = LookupService.countryName[LookupService.unsignedByteToInt(buffer[0])];
      int startIndex1 = num1 + 1;
      while (buffer[startIndex1 + length1] != (byte) 0)
        ++length1;
      if (length1 > 0)
        location.region = new string(chArray, startIndex1, length1);
      int startIndex2 = startIndex1 + (length1 + 1);
      int length2 = 0;
      location.regionName = RegionName.getRegionName(location.countryCode, location.region);
      while (buffer[startIndex2 + length2] != (byte) 0)
        ++length2;
      if (length2 > 0)
        location.city = new string(chArray, startIndex2, length2);
      int startIndex3 = startIndex2 + (length2 + 1);
      int length3 = 0;
      while (buffer[startIndex3 + length3] != (byte) 0)
        ++length3;
      if (length3 > 0)
        location.postalCode = new string(chArray, startIndex3, length3);
      int num5 = startIndex3 + (length3 + 1);
      for (int index = 0; index < 3; ++index)
        num2 += (double) (LookupService.unsignedByteToInt(buffer[num5 + index]) << index * 8);
      location.latitude = num2 / 10000.0 - 180.0;
      int num6 = num5 + 3;
      for (int index = 0; index < 3; ++index)
        num3 += (double) (LookupService.unsignedByteToInt(buffer[num6 + index]) << index * 8);
      location.longitude = num3 / 10000.0 - 180.0;
      location.metro_code = location.dma_code = 0;
      location.area_code = 0;
      if ((int) this.databaseType == DatabaseInfo.CITY_EDITION_REV1)
      {
        int num7 = 0;
        if (location.countryCode == "US")
        {
          int num8 = num6 + 3;
          for (int index = 0; index < 3; ++index)
            num7 += LookupService.unsignedByteToInt(buffer[num8 + index]) << index * 8;
          location.metro_code = location.dma_code = num7 / 1000;
          location.area_code = num7 % 1000;
        }
      }
    }
    catch (IOException ex)
    {
      Console.Write("IO Exception while seting up segments");
    }
    return location;
  }

  public string getOrg(IPAddress addr)
  {
    return this.getOrg(LookupService.bytestoLong(addr.GetAddressBytes()));
  }

  public string getOrgV6(string str)
  {
    IPAddress addr;
    try
    {
      addr = IPAddress.Parse(str);
    }
    catch (Exception ex)
    {
      Console.Write(ex.Message);
      return (string) null;
    }
    return this.getOrgV6(addr);
  }

  public string getOrg(string str)
  {
    IPAddress ipAddress;
    try
    {
      ipAddress = IPAddress.Parse(str);
    }
    catch (Exception ex)
    {
      Console.Write(ex.Message);
      return (string) null;
    }
    return this.getOrg(LookupService.bytestoLong(ipAddress.GetAddressBytes()));
  }

  [MethodImpl(MethodImplOptions.Synchronized)]
  public string getOrgV6(IPAddress addr)
  {
    int length = 0;
    byte[] buffer = new byte[LookupService.MAX_ORG_RECORD_LENGTH];
    char[] chArray = new char[LookupService.MAX_ORG_RECORD_LENGTH];
    try
    {
      int num = this.SeekCountryV6(addr);
      if (num == this.databaseSegments[0])
        return (string) null;
      int sourceIndex = num + (2 * this.recordLength - 1) * this.databaseSegments[0];
      if ((this.dboptions & LookupService.GEOIP_MEMORY_CACHE) == 1)
      {
        Array.Copy((Array) this.dbbuffer, sourceIndex, (Array) buffer, 0, Math.Min(this.dbbuffer.Length - sourceIndex, LookupService.MAX_ORG_RECORD_LENGTH));
      }
      else
      {
        this.file.Seek((long) sourceIndex, SeekOrigin.Begin);
        this.file.Read(buffer, 0, LookupService.MAX_ORG_RECORD_LENGTH);
      }
      for (; buffer[length] != (byte) 0; ++length)
        chArray[length] = char.Parse(buffer[length].ToString());
      chArray[length] = char.MinValue;
      return new string(chArray, 0, length);
    }
    catch (IOException ex)
    {
      Console.Write("IO Exception");
      return (string) null;
    }
  }

  [MethodImpl(MethodImplOptions.Synchronized)]
  public string getOrg(long ipnum)
  {
    int length = 0;
    byte[] buffer = new byte[LookupService.MAX_ORG_RECORD_LENGTH];
    char[] chArray = new char[LookupService.MAX_ORG_RECORD_LENGTH];
    try
    {
      int num = this.SeekCountry(ipnum);
      if (num == this.databaseSegments[0])
        return (string) null;
      int sourceIndex = num + (2 * this.recordLength - 1) * this.databaseSegments[0];
      if ((this.dboptions & LookupService.GEOIP_MEMORY_CACHE) == 1)
      {
        Array.Copy((Array) this.dbbuffer, sourceIndex, (Array) buffer, 0, Math.Min(this.dbbuffer.Length - sourceIndex, LookupService.MAX_ORG_RECORD_LENGTH));
      }
      else
      {
        this.file.Seek((long) sourceIndex, SeekOrigin.Begin);
        this.file.Read(buffer, 0, LookupService.MAX_ORG_RECORD_LENGTH);
      }
      for (; buffer[length] != (byte) 0; ++length)
        chArray[length] = char.Parse(buffer[length].ToString());
      chArray[length] = char.MinValue;
      return new string(chArray, 0, length);
    }
    catch (IOException ex)
    {
      Console.Write("IO Exception");
      return (string) null;
    }
  }

  [MethodImpl(MethodImplOptions.Synchronized)]
  private int SeekCountryV6(IPAddress ipAddress)
  {
    byte[] addressBytes = ipAddress.GetAddressBytes();
    byte[] buffer = new byte[2 * LookupService.MAX_RECORD_LENGTH];
    int[] numArray = new int[2];
    int num1 = 0;
    for (int maxValue = (int) sbyte.MaxValue; maxValue >= 0; --maxValue)
    {
      try
      {
        if ((this.dboptions & LookupService.GEOIP_MEMORY_CACHE) == 1)
        {
          for (int index = 0; index < 2 * LookupService.MAX_RECORD_LENGTH; ++index)
            buffer[index] = this.dbbuffer[index + 2 * this.recordLength * num1];
        }
        else
        {
          this.file.Seek((long) (2 * this.recordLength * num1), SeekOrigin.Begin);
          this.file.Read(buffer, 0, 2 * LookupService.MAX_RECORD_LENGTH);
        }
      }
      catch (IOException ex)
      {
        Console.Write("IO Exception");
      }
      for (int index1 = 0; index1 < 2; ++index1)
      {
        numArray[index1] = 0;
        for (int index2 = 0; index2 < this.recordLength; ++index2)
        {
          int num2 = (int) buffer[index1 * this.recordLength + index2];
          if (num2 < 0)
            num2 += 256;
          numArray[index1] += num2 << index2 * 8;
        }
      }
      int num3 = (int) sbyte.MaxValue - maxValue;
      int index3 = num3 >> 3;
      int num4 = 1 << (num3 & 7 ^ 7);
      if (((int) addressBytes[index3] & num4) > 0)
      {
        if (numArray[1] >= this.databaseSegments[0])
          return numArray[1];
        num1 = numArray[1];
      }
      else
      {
        if (numArray[0] >= this.databaseSegments[0])
          return numArray[0];
        num1 = numArray[0];
      }
    }
    Console.Write("Error Seeking country while Seeking " + (object) ipAddress);
    return 0;
  }

  [MethodImpl(MethodImplOptions.Synchronized)]
  private int SeekCountry(long ipAddress)
  {
    try
    {
      byte[] buffer = new byte[2 * LookupService.MAX_RECORD_LENGTH];
      int[] numArray = new int[2];
      int num1 = 0;
      for (int index1 = 31; index1 >= 0; --index1)
      {
        try
        {
          if ((this.dboptions & LookupService.GEOIP_MEMORY_CACHE) == 1)
          {
            for (int index2 = 0; index2 < 2 * LookupService.MAX_RECORD_LENGTH; ++index2)
              buffer[index2] = this.dbbuffer[index2 + 2 * this.recordLength * num1];
          }
          else
          {
            this.file.Seek((long) (2 * this.recordLength * num1), SeekOrigin.Begin);
            this.file.Read(buffer, 0, 2 * LookupService.MAX_RECORD_LENGTH);
          }
        }
        catch (IOException ex)
        {
          Console.Write("IO Exception");
        }
        for (int index2 = 0; index2 < 2; ++index2)
        {
          numArray[index2] = 0;
          for (int index3 = 0; index3 < this.recordLength; ++index3)
          {
            int num2 = (int) buffer[index2 * this.recordLength + index3];
            if (num2 < 0)
              num2 += 256;
            numArray[index2] += num2 << index3 * 8;
          }
        }
        if ((ipAddress & (long) (1 << index1)) > 0L)
        {
          if (numArray[1] >= this.databaseSegments[0])
            return numArray[1];
          num1 = numArray[1];
        }
        else
        {
          if (numArray[0] >= this.databaseSegments[0])
            return numArray[0];
          num1 = numArray[0];
        }
      }
      Console.Write("Error Seeking country while Seeking " + (object) ipAddress);
      return 0;
    }
    catch
    {
      return 0;
    }
  }

  private static long swapbytes(long ipAddress)
  {
    return (ipAddress & (long) byte.MaxValue) << 24 | (ipAddress >> 8 & (long) byte.MaxValue) << 16 | (ipAddress >> 16 & (long) byte.MaxValue) << 8 | ipAddress >> 24 & (long) byte.MaxValue;
  }

  private static long bytestoLong(byte[] address)
  {
    long num1 = 0;
    for (int index = 0; index < 4; ++index)
    {
      long num2 = (long) address[index];
      if (num2 < 0L)
        num2 += 256L;
      num1 += num2 << (3 - index) * 8;
    }
    return num1;
  }

  private static int unsignedByteToInt(byte b)
  {
    return (int) b & (int) byte.MaxValue;
  }
}
