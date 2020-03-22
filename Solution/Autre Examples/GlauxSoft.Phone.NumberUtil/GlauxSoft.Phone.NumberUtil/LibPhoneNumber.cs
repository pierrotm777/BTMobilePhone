using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace GlauxSoft.Phone.NumberUtil
{
    /// <summary>
    /// Google's common library for parsing, formatting, storing and validating international phone numbers. 
    /// This version is build with Microsoft .NET and uses Google's metadata 'PhoneNumberMetaData.xml' as its base
    /// Current implementation does only format the phone number based on the regional settings and sets the corresponding 
    /// Country Code (ISO two letter code). Other functionalities such as vantiy numbers or line type (fixed line, mobile, etc) are 
    /// currently not supported by this port.
    /// </summary>
    public static class LibPhoneNumber
    {
        private static XDocument doc = null;
        private static Dictionary<string, string> isoCountryNameMapping;
        private static object _sync = new object();

        /// <summary>
        /// Formats 
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static PhoneNumberInfo DoFormatAndFindRegion(string phoneNumber)
        {
            PhoneNumberInfo retValue = new PhoneNumberInfo();

            try
            {
                // check input and pre-conditions
                if (phoneNumber == null)
                    return null;

                if (!phoneNumber.StartsWith("+"))
                    return null;


                //Remove the following: 
                //   -- any numbers enclosed with brackets . Bsp: +41 (0)31 388 10 10
                //   -- white space characters
                //   -- none-digits
                Regex regEx = new Regex("((\\[|\\()(\\d)*(\\]|\\)))|\\s|\\D");
                Match m = regEx.Match(phoneNumber);
                while (m.Success)
                {
                    phoneNumber = phoneNumber.Remove(m.Index, m.Length);
                    //m = m.NextMatch(); // will not work, because NextMatch throw exception, because we change it during iteration.
                    // so let us do Metch again (not optimal for peroformance, but enough for now).
                    m = regEx.Match(phoneNumber);
                }
                //...leading '+'-sign was removed above. Correct it.
                phoneNumber = "+" + phoneNumber;



                // load the xml
                lock (_sync)
                {
                    if (doc == null)
                    {
                        // load xml from zipped embedded resource
                        Stream stream = typeof(LibPhoneNumber).Assembly.GetManifestResourceStream("GlauxSoft.Phone.NumberUtil.MetaData.PhoneNumberMetaData.compressed");
                        GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress);
                        doc = XDocument.Load(new StreamReader(zipStream));
                        stream.Close();
                    }
                }

                // note about thread safety: XDocument class is not thread-safe, but we only use it in read mode


                // #### get territoris related to itu calling code (country code).
                //      There are some countries sharing the same country calling code, like USA, Canada, GB, JE, etc. ####
                int numbersOfDigitsForCountryCode = 0;
                XElement[] matchingTerritories = null;
                do
                {

                    numbersOfDigitsForCountryCode++;

                    // prevent ArgumentOutOfRangeException in Substring method
                    if (numbersOfDigitsForCountryCode + 1 >= phoneNumber.Length)
                        break;

                    // ...starting with 1, because of leading plus sign
                    string possibleCountryCode = phoneNumber.Substring(1, numbersOfDigitsForCountryCode);

                    var query = from territory in doc.Root.Elements("territories").Elements("territory")
                                where territory.Attribute("countryCode").Value == possibleCountryCode
                                select territory;
                    matchingTerritories = query.ToArray();

                } while (matchingTerritories == null || matchingTerritories.Count() == 0);

                // set ITU calling code (country code in google meta data)
                if (matchingTerritories != null && matchingTerritories.Count() > 0)
                {
                    retValue.ITUCallingCode = matchingTerritories[0].Attribute("countryCode").Value;
                }

                // #### if single territory -> get country code ####
                if (matchingTerritories != null && matchingTerritories.Count() == 1)
                {
                    retValue.TwoLetterISOCountryCode = matchingTerritories.First().Attribute("id").Value;
                }
                // #### if multiple territories found ( case for NANPA and Russia ) 
                //      -> iterate over all territories check if any leading digits 
                //         pattern matches for any type (mobile, land line, etc..) ####
                else if (matchingTerritories != null && matchingTerritories.Count() > 1)
                {

                    // set the main country to the top of the list...
                    // The field 'mainCountryForCode' is set when a country is considered to be the main country
                    // for a calling code. It may not be set by more than one country with the
                    // same calling code, and it should not be set by countries with a unique
                    // calling code. This can be used to indicate that "GB" is the main country
                    // for the calling code "44" for example, rather than Jersey or the Isle of Man.
                    {
                        var mainCountryForCode = matchingTerritories.SingleOrDefault(item => item.Attribute("mainCountryForCode") != null && item.Attribute("mainCountryForCode").Value.ToLower() == "true");
                        List<XElement> tmp = matchingTerritories.ToList();
                        tmp.Remove(mainCountryForCode);
                        tmp.Insert(0, mainCountryForCode);
                        matchingTerritories = tmp.ToArray();
                    }

                    // flag for further method control
                    bool doesMatch = false;

                    // check each territory...
                    //////var matchingTerritoriesForNumberPattern = matchingTerritories.ToList();
                    //////matchingTerritoriesForNumberPattern.Reverse();
                    foreach (var territory in matchingTerritories)
                    {
                        string currentCountryCode = "+" + territory.Attribute("countryCode").Value;
                        string possibleNationalNummer = phoneNumber.Substring(currentCountryCode.Length);

                        // #### check whether the territory defines a leading digits pattern.
                        //      If so, we do not need to check the inner number patterns ####
                        XAttribute leadingDigits = territory.Attribute("leadingDigits");
                        if (leadingDigits != null)
                        {
                            doesMatch = Regex.IsMatch(possibleNationalNummer, "^(" + leadingDigits + ").*");
                            if (doesMatch)
                            {
                                retValue.TwoLetterISOCountryCode = territory.Attribute("id").Value;
                                break;
                            }
                        }
                        // note: if leadingDigit does not match, procced search with number pattern (not matching leading pattern does not break the rule)


                        if (phoneNumber.Length > currentCountryCode.Length)
                        {

                            // define the possible number type explicit (extracted from phonemetadata.proto), because 
                            // there are section like 'areaCodeOptional' which must be ingored, otherwise unit tests will fail.
                            List<string> numberTypes = new List<string>();
                            //numberTypes.Add("general_desc"); // we don't need that, right? All unit tests are green so far.
                            numberTypes.Add("fixedLine");
                            numberTypes.Add("mobile");
                            numberTypes.Add("tollFree");
                            numberTypes.Add("premiumRate");
                            numberTypes.Add("sharedCost");
                            numberTypes.Add("personalNumber");
                            numberTypes.Add("voip");
                            numberTypes.Add("pager");
                            numberTypes.Add("uan");

                            // define query to get all number patterns within this territory
                            var numberPatternQuery = from numberPattern in territory.Descendants("nationalNumberPattern")
                                                     where numberTypes.Contains(numberPattern.Parent.Name.ToString())
                                                     select numberPattern;

                            // #### check if any number pattern matches with the 
                            //      current possibleNationalNummer (mobile, land line, etc..) ####
                            foreach (var numberPatternElement in numberPatternQuery)
                            {
                                string numberPattern = numberPatternElement.Value.Replace("\n", "").Replace("\r", "").Replace(" ", "");

                                // note: it is not defined as leading pattern -> google meta data contains full pattern to match with a full number
                                //       add start- and end-pattern iwthin brackets
                                doesMatch = Regex.IsMatch(possibleNationalNummer, "^(" + numberPattern + ")$");
                                if (doesMatch)
                                {
                                    retValue.TwoLetterISOCountryCode = territory.Attribute("id").Value;
                                    break;
                                }
                            }

                            // break outer loop...
                            if (doesMatch)
                                break;
                        }

                    }

                    // set default if not found
                    if (doesMatch == false)
                    {
                        ////// set 'ZZ' as unknown
                        ////retValue.TwoLetterISOCountryCode = "ZZ";
                        // no: we don't set it to 'ZZ', because we already know the territory
                        retValue.TwoLetterISOCountryCode = matchingTerritories[0].Attribute("id").Value;
                    }

                }
                // #### if not found we set region code to 'ZZ' which means 'unknown'  ####
                else
                {
                    // set 'ZZ' as unknown
                    retValue.TwoLetterISOCountryCode = "ZZ";
                }


                // #### format phone number ####
                // #### get all number formats from the matching territories ####
                {
                    var numberFormatQuery = from e in matchingTerritories.Elements("availableFormats").Elements("numberFormat")
                                            select e;

                    // go through list and check pattern 
                    bool patternMatches = false;

                    // special case: if no number format is set for a given region, then it will be formatted as a block (but keep the space between country code and national number)
                    if (matchingTerritories.Count() > 0 && numberFormatQuery.Count() == 0)
                    {
                        // set correct flag-value for further processing
                        patternMatches = true;

                        string countryCode = "+" + matchingTerritories[0].Attribute("countryCode").Value;
                        string nationalNummer = phoneNumber.Substring(countryCode.Length);
                        retValue.FormattedInternationalNumber = countryCode + " " + nationalNummer;
                    }
                    else
                    {

                        foreach (var numberFormat in numberFormatQuery)
                        {
                            string countryCode = "+" + numberFormat.Parent.Parent.Attribute("countryCode").Value;
                            string nationalNummer = phoneNumber.Substring(countryCode.Length);

                            patternMatches = Regex.IsMatch(nationalNummer, "^(" + numberFormat.Attribute("pattern").Value.Replace("\n", "").Replace("\r", "").Replace(" ", "") + ")$");

                            if (patternMatches)
                            {
                                // check if leading digits pattern is defined
                                var leadingDigits = numberFormat.Elements("leadingDigits").ToArray();

                                if (leadingDigits.Length > 0)
                                {
                                    // any of the leading digits pattern matches?
                                    ////foreach (var leadingDigitsPattern in numberFormat.Elements("leadingDigits"))
                                    // note: we only need to check the latest element, because it is the most detailed (see comments in phonemetadata.proto).
                                    var leadingDigitsPattern = numberFormat.Elements("leadingDigits").Last();
                                    {
                                        // only add the start regex pattern `^`, because of its 'leading' kind...
                                        string leadingPattern = "^(" + leadingDigitsPattern.Value.Replace("\n", "").Replace("\r", "").Replace(" ", "") + ")";
                                        patternMatches = Regex.IsMatch(nationalNummer, leadingPattern);
                                    }
                                }
                                else
                                {
                                    // no leading pattern is defined, that means the top pattern makes the rule...
                                    patternMatches = true;
                                }

                                if (patternMatches)
                                {
                                    // format phone number
                                    string format = numberFormat.Element("format").Value;
                                    // if an international format is defined, use this (except is is defined as 'NA') 
                                    var intlFormatElement = numberFormat.Element("intlFormat");
                                    if (intlFormatElement != null && intlFormatElement.Value != "NA")
                                        format = intlFormatElement.Value;

                                    string formattedNumber = Regex.Replace(nationalNummer, numberFormat.Attribute("pattern").Value.Replace("\n", "").Replace("\r", "").Replace(" ", ""), format);
                                    // add country code
                                    formattedNumber = countryCode + " " + formattedNumber;
                                    retValue.FormattedInternationalNumber = formattedNumber;
                                    break;
                                }

                            }
                        }
                    }

                    if (patternMatches == false)
                    {
                        // if no number format matches, then it will be formatted as a block (but keep the space between country code and national number)
                        if (matchingTerritories.Count() > 0)
                        {
                            string countryCode = "+" + matchingTerritories[0].Attribute("countryCode").Value;
                            string nationalNummer = phoneNumber.Substring(countryCode.Length);
                            retValue.FormattedInternationalNumber = countryCode + " " + nationalNummer;
                        }
                        else
                        {
                            // default of default...
                            retValue.FormattedInternationalNumber = phoneNumber;
                        }
                    }

                }


                // #### Set ISO country name ####
                // see: http://www.iso.org/iso/country_codes/iso_3166_code_lists.htm
                lock (_sync)
                {
                    if (isoCountryNameMapping == null)
                    {
                        // load data from embedded resource
                        Stream streamIsoCountryNames = typeof(LibPhoneNumber).Assembly.GetManifestResourceStream("GlauxSoft.Phone.NumberUtil.MetaData.EnglishCountryNamesISO3166.txt");
                        StringReader sr = new StringReader(new StreamReader(streamIsoCountryNames).ReadToEnd());
                        streamIsoCountryNames.Close();
                        isoCountryNameMapping = new Dictionary<string, string>();
                        string line = null;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(';');

                            if (parts.Length == 2)
                            {
                                isoCountryNameMapping.Add(parts[1], parts[0]);
                            }
                        }

                        // add two countries which are not in the list of ISO country codes so far
                        // note: if they get members of this list in the future, it will throw an exception by calling the Add method, because the element will aready be in the list.
                        isoCountryNameMapping.Add("AC", "ASCENSION ISLAND");
                        isoCountryNameMapping.Add("AN", "NETHERLANDS ANTILLES");
                    }
                }


                string isoCountryName = "";
                bool b = isoCountryNameMapping.TryGetValue(retValue.TwoLetterISOCountryCode, out isoCountryName);
                retValue.EnglishCountryNameISO3166 = isoCountryName;
                if (string.IsNullOrEmpty(isoCountryName) && retValue.TwoLetterISOCountryCode != "ZZ")
                {
                    throw new NotImplementedException(string.Format("No country name found for country code {0}", retValue.TwoLetterISOCountryCode));
                }


                return retValue;

            }
            catch (Exception ex)
            {
                // return defaults in case of an error
                retValue.FormattedInternationalNumber = phoneNumber;
                retValue.ITUCallingCode = "";
                retValue.TwoLetterISOCountryCode = "ZZ";
                retValue.EnglishCountryNameISO3166 = "";
                return retValue;
            }
        }
    }
}

