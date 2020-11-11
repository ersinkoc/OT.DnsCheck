﻿using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DnsCheck
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            Console.Clear();
            SetProviders.DNS();
            SetProviders.Mail();

            Console.WriteLine("DNS Providers: " + SetProviders.dnsProviders.Count + " Mail Providers: " + SetProviders.mailProviders.Count);

            int FreeCheckLimit = 5;
            int MaxCheckLimit = 1000;
            int ApiCheckLimit = 0;
            string fileName = "";
            ConsoleKey key;

            while (fileName != "e")
            {

                Premium.CheckPremium("", true);


                ApiCheckLimit = Premium.RateLimit;

                // Banner
                Helpers.Banner();
                Console.ResetColor();

                if (Premium.CheckType == "PREMIUM")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  *You already have access for Premium DNS Lookup API - (U)se Premium Rights\n");
                    Console.ResetColor();
                }
                Console.WriteLine($"[ API Provider: (P)romptapi.com | (R)apidapi.com | (F)ree {FreeCheckLimit} domains | (E)xit ]");

                //string apiProvider = Console.ReadLine().ToLower
                string apiProvider = string.Empty;

                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && apiProvider.Length > 0)
                    {
                        Console.Write("\b \b");
                        apiProvider = apiProvider[0..^1];
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        apiProvider += keyInfo.KeyChar;

                        if (apiProvider.Length == 1)
                            Console.Write(key);
                        else
                            Console.Write("*");

                    }
                } while (key != ConsoleKey.Enter);


                apiProvider = apiProvider.ToLower();

                if (apiProvider == "e" || apiProvider == "exit")
                {
                    break;
                }

                string apiKey = string.Empty;
                string apiProviderFullName;


                if (apiProvider == "p" || apiProvider == "promptapi" || apiProvider == "promptapi.com" || apiProvider == "r" || apiProvider == "rapidapi" || apiProvider == "rapidapi.com")
                {
                askAPIKey:

                    if (apiProvider == "p" || apiProvider == "promptapi" || apiProvider == "promptapi.com")
                    {
                        apiProviderFullName = "promptapi.com";
                        apiProvider = "p";
                    }
                    else
                    {
                        apiProviderFullName = "rapidapi.com";
                        apiProvider = "r";
                    }

                    Console.WriteLine("\n" + $"Enter {apiProviderFullName} API Key:");

                    //apiKey = Console.ReadLine();
                    do
                    {
                        var keyInfo = Console.ReadKey(intercept: true);
                        key = keyInfo.Key;

                        if (key == ConsoleKey.Backspace && apiKey.Length > 0)
                        {
                            Console.Write("\b \b");
                            apiKey = apiKey[0..^1];
                        }
                        else if (!char.IsControl(keyInfo.KeyChar))
                        {

                            Console.Write("*");

                            apiKey += keyInfo.KeyChar;
                        }
                    } while (key != ConsoleKey.Enter);

                    if (apiKey.Length == 0) goto askAPIKey; // apikey lenght rapidapi 50 - promptapi 30
                }
                else if (apiProvider == "f" || apiProvider == "free" || apiProvider.Length == 0)
                {
                    Premium.CheckPremium("", false);
                    apiKey = "free";
                    apiProvider = "f";
                    apiProviderFullName = "Free Test";
                    ApiCheckLimit = FreeCheckLimit;
                }
                else
                {

                    if (apiProvider.ToLower() == "u")
                        Premium.CheckPremium("", true);
                    else
                        Premium.CheckPremium(apiProvider, false);


                    apiKey = apiProvider;
                    if (Premium.CheckType == "PREMIUM")
                    {
                        apiProviderFullName = "Premium Provider";
                        ApiCheckLimit = Premium.RateLimit;
                    }
                    else
                    {
                        apiKey = "free";
                        apiProvider = "f";
                        apiProviderFullName = "Free Test";
                        ApiCheckLimit = FreeCheckLimit;
                    }
                }

                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine();
                switch (apiProvider)
                {
                    case "p":
                        Console.WriteLine(" You're using DNS Lookup API via PromptApi.com ");
                        apiProviderFullName = "promptapi.com";
                        ApiCheckLimit = MaxCheckLimit;
                        break;

                    case "r":
                        Console.WriteLine(" You're using DNS Lookup API via RapidApi.com ");
                        apiProviderFullName = "rapidapi.com";
                        ApiCheckLimit = MaxCheckLimit;
                        break;

                    case "f":
                        Console.WriteLine($" You're using Free DNS Lookup API for {FreeCheckLimit} domains ");
                        apiProviderFullName = "Free";

                        break;

                    default:
                        if (Premium.CheckType == "PREMIUM")
                        {
                            Console.WriteLine($" You can {ApiCheckLimit} domains checking with our Premium DNS Lookup API Servers ");
                            apiProviderFullName = "Premium Servers";
                        }
                        else
                        {
                            Console.WriteLine($" You're using Free DNS Lookup API for {FreeCheckLimit} domains ");
                            apiProviderFullName = "Free";
                        }
                        break;
                }
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
                var strProvider = " API Provider : " + apiProviderFullName;
                var strMaxDomain = " Check Limit  : " + ApiCheckLimit + " domains";
                Console.WriteLine(strProvider + "\n" + strMaxDomain);
                string path = Directory.GetCurrentDirectory();

            askFile:

                Console.WriteLine();
                Console.WriteLine(@"[ Filename (c:\domain.txt) - (L)ist - (PLESK) for get domain list - (E)xit ]");
                Console.ResetColor();

                fileName = Console.ReadLine();

                if (fileName.ToLower() == "l" || fileName.ToLower() == "list")
                {
                    var allFilesInAllFolders = Directory.EnumerateFiles(path, "*.txt");

                    foreach (var file in allFilesInAllFolders)
                    {
                        Console.WriteLine(file);
                    }
                    goto askFile;
                }

                Console.WriteLine();

                if (fileName.ToLower() == "e" || fileName.ToLower() == "exit" || fileName.Length == 0)
                {
                    //Console.Clear();
                    continue;
                }

                if (fileName.ToLower() == "plesk")
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        try
                        {
                            Helpers.Bash("for i in `mysql -uadmin -p\\`cat /etc/psa/.psa.shadow\\` psa -Ns -e \"select name from domains\"`; do echo $i; done > domain.txt");
                            fileName = "domain.txt";
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                    }

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        try
                        {
                            Helpers.ExecuteCommand("plesk bin subscription.exe --list > domain.txt");
                            fileName = "domain.txt";
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                    }
                }

                fileName = @"" + fileName;
                if (!File.Exists(fileName)) continue;

                var domains = File.ReadAllLines(fileName);
                var domainList = new ArrayList();
                int maxDomainLenght = 0;

                Console.Write("\nChecking domain list in file... ");
                int totalDomain = domains.Count();
                if (totalDomain == 0)
                {
                    Console.WriteLine("Empty File!");
                    continue;
                }
                int fives = totalDomain / 20 + 1;
                foreach (string d in domains)
                {
                    string parsedDomain = Helpers.ParseDomain(d);
                    if (parsedDomain == "badrequest") continue;
                    if (!domainList.Contains(parsedDomain))
                        domainList.Add(parsedDomain);

                    maxDomainLenght = Math.Max(parsedDomain.Length, maxDomainLenght);
                    int x = domainList.Count;
                    Console.Title = "Checking domains...";

                    if (x % fives == 0) Console.Write("o");
                    if (x == ApiCheckLimit) break;
                }

                domains = (string[])domainList.ToArray(typeof(string));
                totalDomain = domains.Count();
                if (totalDomain == 0)
                {
                    Console.WriteLine("There is no domain in the file!");
                    continue;
                }

                var errorNonExistDomain = new List<string>();
                var errorNoMxRecord = new List<string>();
                var errorNotSpecific = new List<string>();
                Dictionary<string, string> HostedMailServers = new Dictionary<string, string>();
                Dictionary<string, string> KnownDNSProvider = new Dictionary<string, string>();



                string CheckFor = "";
            AskCheckFor:
                Console.WriteLine();
                Console.WriteLine();


                Console.WriteLine("[ (NS) Check | (MX) Check | (E)xit ]");
                CheckFor = Console.ReadLine().ToLower();

                if (CheckFor.Length == 0 || CheckFor == "e" || CheckFor == "exit")
                    break;

                if (CheckFor == "mx" || CheckFor == "ns")
                {

                    Console.WriteLine($"\n{CheckFor.ToUpper()} checking for {domains.Count()} domains...\n");

                    foreach (string domain in domains)
                    {
                        if (apiKey == "free")
                        {
                            System.Threading.Thread.Sleep(1000);
                        }

                        try
                        {
                            Console.ResetColor();
                            Console.Title = CheckFor.ToUpper() + " Checking ... " + domain;

                            bool domainscreen = false;
                            string domainstr = domain;
                            string url = "";

                            url = Premium.DNSAPIServer + "/api/" + CheckFor + "/" + domain;

                            if (apiProvider == "p")
                                url = "https://api.promptapi.com/dns_lookup/api/" + CheckFor + "/" + domain;

                            if (apiProvider == "r")
                                url = "https://dns-lookup2.p.rapidapi.com/Api/" + CheckFor + "/" + domain;

                            RestRequest request = new RestRequest(Method.GET);

                            // PromptAPI apikey
                            if (apiProvider == "p")
                                request.AddHeader("apikey", apiKey);

                            // RapidApi apikey
                            if (apiProvider == "r")
                            {
                                request.AddHeader("x-rapidapi-host", "dns-lookup2.p.rapidapi.com");
                                request.AddHeader("x-rapidapi-key", apiKey);
                            }

                            var client = new RestClient(url)
                            {
                                Timeout = -1
                            };

                            IRestResponse response = client.Get(request);
                            int rstatus = (int)response.StatusCode;

                            // rate limit for RapidApi
                            if (apiProvider == "r" && response.Content.Contains("rate limit"))
                            {
                                Helpers.AlertApiLimit("RAPIDAPI.COM: You have exceeded the rate limit");
                                break;
                            }

                            if (apiProvider == "r" && (int)response.StatusCode == 403)
                            {
                                Helpers.AlertApiLimit("RAPIDAPI.COM: You are not subscribed to this API.");
                                break;
                            }
                            // rate limit check for PromptApi
                            if (apiProvider == "p" && response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                            {
                                Helpers.AlertApiLimit("PROMPTAPI.COM: You have exceeded your daily/monthly API rate limit.");
                                break;
                            }

                            if (apiProvider == "p" && (int)response.StatusCode == 401)
                            {
                                Helpers.AlertApiLimit("PROMPTAPI.COM: Invalid authentication credentials");
                                break;
                            }

                            if (response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                            {
                                Console.WriteLine("Timeout!!!");
                                break;
                            }

                            if (rstatus == 429 && apiKey == "free")
                            {
                                Console.WriteLine("Houston, we have a problem.! You're using Free DNS Lookup API with Rate Limits");
                                Console.WriteLine(rstatus + "\n" + response.StatusCode + " " + response.StatusDescription);
                                break;
                            }

                            if (rstatus != 200)
                            {
                                Console.WriteLine("Houston, we have a problem.!");
                                Console.WriteLine(rstatus + "\n" + response.StatusCode + " " + response.StatusDescription);
                                break;
                            }

                            if (rstatus == 200)
                            {
                                if (CheckFor == "mx")
                                {
                                    ReturnJsonMX rJson = new ReturnJsonMX();
                                    rJson = JsonConvert.DeserializeObject<ReturnJsonMX>(response.Content);

                                    if (rJson.Warnings.Count > 0)
                                    {
                                        var warnings = "";
                                        foreach (string ii in rJson.Warnings)
                                        {
                                            warnings += ii.ToString();
                                            switch (ii)
                                            {
                                                case "Non-Existent Domain":
                                                    errorNonExistDomain.Add(domain);
                                                    break;

                                                case "No MX Records found":
                                                    errorNoMxRecord.Add(domain);
                                                    break;

                                                default:
                                                    errorNotSpecific.Add(domain);
                                                    break;
                                            }
                                        }

                                        Console.Write("[");
                                        Console.BackgroundColor = ConsoleColor.DarkRed;
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("W");
                                        Console.ResetColor();

                                        domainstr = domainscreen ? "  +" + new String('-', maxDomainLenght - 2) : domain + new String(' ', maxDomainLenght - domain.Length + 1);

                                        Console.Write($"] {domainstr} : ");
                                        domainscreen = true;
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine(warnings);
                                    }

                                    Console.ResetColor();

                                    foreach (var item in rJson.Results)
                                    {
                                        if (item.Exchange == rJson.Domain || item.Exchange.EndsWith("." + rJson.Domain))
                                        {
                                            Console.Write("[L]");
                                            if (domainscreen)
                                                domainstr = new String(' ', maxDomainLenght + 1);
                                            else
                                                domainstr = domain + new String(' ', maxDomainLenght - domain.Length + 1);

                                            Console.Write($" {domainstr} : {item.Exchange} ({item.Reference})" + "\n");
                                            domainscreen = true;
                                        }
                                        else
                                        {
                                            Console.Write("[");
                                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.Write("R");
                                            Console.ResetColor();
                                            if (domainscreen)
                                                domainstr = new String(' ', maxDomainLenght + 1);
                                            else
                                                domainstr = domain + new String(' ', maxDomainLenght - domain.Length + 1);

                                            Console.Write($"] {domainstr} : {item.Exchange} ({item.Reference})" + "\n");
                                            domainscreen = true;

                                            foreach (var mailProvider in SetProviders.mailProviders)
                                            {
                                                foreach (var nsSearch in mailProvider.SearchPhrases)
                                                {
                                                    switch (nsSearch.FindAt)
                                                    {
                                                        case CheckAlgorith.Full:
                                                            if (item.Exchange == nsSearch.Phrase)
                                                            {
                                                                if (!HostedMailServers.ContainsKey(domain))
                                                                    HostedMailServers.Add(domain, mailProvider.Name);
                                                            }
                                                            break;
                                                        case CheckAlgorith.StartWidth:
                                                            if (item.Exchange.StartsWith(nsSearch.Phrase))
                                                            {
                                                                if (!HostedMailServers.ContainsKey(domain))
                                                                    HostedMailServers.Add(domain, mailProvider.Name);
                                                            }
                                                            break;
                                                        case CheckAlgorith.Contains:
                                                            if (item.Exchange.Contains(nsSearch.Phrase))
                                                            {
                                                                if (!HostedMailServers.ContainsKey(domain))
                                                                    HostedMailServers.Add(domain, mailProvider.Name);
                                                            }
                                                            break;
                                                        case CheckAlgorith.EndWidth:
                                                            if (item.Exchange.EndsWith(nsSearch.Phrase))
                                                            {
                                                                if (!HostedMailServers.ContainsKey(domain))
                                                                    HostedMailServers.Add(domain, mailProvider.Name);
                                                            }
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }


                                if (CheckFor == "ns")
                                {
                                    ReturnJsonNS rJson = new ReturnJsonNS();
                                    rJson = JsonConvert.DeserializeObject<ReturnJsonNS>(response.Content);

                                    if (rJson.Warnings.Count > 0)
                                    {
                                        var warnings = "";
                                        foreach (string ii in rJson.Warnings)
                                        {
                                            warnings += ii.ToString();
                                            switch (ii)
                                            {
                                                case "Non-Existent Domain":
                                                    errorNonExistDomain.Add(domain);
                                                    break;
                                                default:
                                                    errorNotSpecific.Add(domain);
                                                    break;
                                            }
                                        }

                                        Console.Write("[");
                                        Console.BackgroundColor = ConsoleColor.DarkRed;
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("W");
                                        Console.ResetColor();

                                        domainstr = domainscreen ? "  +" + new String('-', maxDomainLenght - 2) : domain + new String(' ', maxDomainLenght - domain.Length + 1);

                                        Console.Write($"] {domainstr} : ");
                                        domainscreen = true;
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine(warnings);
                                    }

                                    Console.ResetColor();

                                    foreach (var item in rJson.Results)
                                    {
                                        Console.Write("[");
                                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write(" ");
                                        Console.ResetColor();
                                        if (domainscreen)
                                            domainstr = new String(' ', maxDomainLenght + 1);
                                        else
                                            domainstr = domain + new String(' ', maxDomainLenght - domain.Length + 1);

                                        Console.Write($"] {domainstr} : {item.nameServer}" + "\n");
                                        domainscreen = true;


                                        foreach (var dnsProvider in SetProviders.dnsProviders)
                                        {
                                            foreach (var nsSearch in dnsProvider.SearchPhrases)
                                            {
                                                switch (nsSearch.FindAt)
                                                {
                                                    case CheckAlgorith.Full:
                                                        if (item.nameServer == nsSearch.Phrase)
                                                        {
                                                            if (!KnownDNSProvider.ContainsKey(domain))
                                                                KnownDNSProvider.Add(domain, dnsProvider.Name);
                                                        }
                                                        break;
                                                    case CheckAlgorith.StartWidth:
                                                        if (item.nameServer.StartsWith(nsSearch.Phrase))
                                                        {
                                                            if (!KnownDNSProvider.ContainsKey(domain))
                                                                KnownDNSProvider.Add(domain, dnsProvider.Name);
                                                        }
                                                        break;
                                                    case CheckAlgorith.Contains:
                                                        if (item.nameServer.Contains(nsSearch.Phrase))
                                                        {
                                                            if (!KnownDNSProvider.ContainsKey(domain))
                                                                KnownDNSProvider.Add(domain, dnsProvider.Name);
                                                        }
                                                        break;
                                                    case CheckAlgorith.EndWidth:
                                                        if (item.nameServer.EndsWith(nsSearch.Phrase))
                                                        {
                                                            if (!KnownDNSProvider.ContainsKey(domain))
                                                                KnownDNSProvider.Add(domain, dnsProvider.Name);
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    Console.Title = "Check Completed";
                    Console.WriteLine();


                    if (CheckFor == "mx")
                    {
                        Helpers.ListProviders(HostedMailServers, SetProviders.mailProviders);
                    }

                    if (CheckFor == "ns")
                    {
                        Helpers.ListProviders(KnownDNSProvider, SetProviders.dnsProviders);

                    }

                    Console.WriteLine();
                    // Errors
                    var rFile = Path.GetFileName(fileName).Replace("noexistent_", "").Replace("nomx_", "").Replace("error_", "");
                    Helpers.ListWarnings(errorNonExistDomain, "Non-Existent Domains:", ConsoleColor.Red, ConsoleColor.White, "noexistent_" + rFile);
                    if (CheckFor == "mx") Helpers.ListWarnings(errorNoMxRecord, "No MX Record Found:", ConsoleColor.Red, ConsoleColor.White, "nomx_" + rFile);
                    Helpers.ListWarnings(errorNotSpecific, "Errors:", ConsoleColor.DarkRed, ConsoleColor.White, "error_" + rFile);
                    Console.ResetColor();
                    Console.WriteLine("\n\nEnd of checking...");
                    Console.ReadKey();

                }
                else
                {
                    goto AskCheckFor;
                }


            }
        }
    }
}