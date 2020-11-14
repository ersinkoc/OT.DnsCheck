using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DnsCheck
{
    internal class SetProviders
    {
        public static List<Provider> dnsProviders;
        public static List<Provider> mailProviders;

        public static void Mail()
        {
            try
            {
                var fileName = @"" + "Providers_Mail.txt";
                if (File.Exists(fileName))
                {
                    string jsonstring = File.ReadAllText(fileName);
                    if (!String.IsNullOrEmpty(jsonstring))
                        mailProviders = JsonConvert.DeserializeObject<List<Provider>>(jsonstring);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (mailProviders == null)
                LoadMailProviders();
        }

        public static void DNS()
        {
            try
            {
                var fileName = @"" + "Providers_Dns.txt";
                if (File.Exists(fileName))
                {
                    string jsonstring = File.ReadAllText(fileName);
                    if (!String.IsNullOrEmpty(jsonstring))
                        dnsProviders = JsonConvert.DeserializeObject<List<Provider>>(jsonstring);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (dnsProviders == null)
                LoadDnsProviders();
        }

        internal static void LoadMailProviders()
        {
            mailProviders = new List<Provider>() {
                new Provider(){
                    Name = "Google",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() {Phrase = "aspmx.l.google.com", FindAt = WhereIs.Full },
                        new SearchPhrase(){ Phrase = "alt1.aspmx.l.google.com", FindAt = WhereIs.Full },
                        new SearchPhrase() {Phrase = "alt2.aspmx.l.google.com", FindAt = WhereIs.Full },
                        new SearchPhrase() {Phrase = "alt3.aspmx.l.google.com", FindAt = WhereIs.Full },
                        new SearchPhrase() {Phrase = "alt4.aspmx.l.google.com", FindAt = WhereIs.Full },
                        new SearchPhrase() {Phrase = ".l.google.com", FindAt = WhereIs.EndWidth },
                    }
                },
                new Provider(){
                    Name = "BizimHost",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".bizimdns.com",FindAt = WhereIs.EndWidth },
                        new SearchPhrase() { Phrase = ".bizimhost.com.tr",FindAt = WhereIs.EndWidth }
                    }
                },
                new Provider(){
                    Name = "Yandex",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = "mx.yandex.net",FindAt = WhereIs.Full }
                    }
                },
                new Provider(){
                    Name = "Office365",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".protection.outlook.com",FindAt = WhereIs.EndWidth },
                        new SearchPhrase() { Phrase = ".outlook.com",FindAt = WhereIs.EndWidth },
                    }
                },
                new Provider(){
                    Name = "Zoho",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".zoho.com",FindAt = WhereIs.EndWidth },
                    }
                },
            };

            try
            {
                string json = JsonConvert.SerializeObject(mailProviders, Formatting.Indented);

                String path = @"" + "Providers_Mail.txt";
                if (File.Exists(path)) File.Delete(path);
                using StreamWriter sr = File.AppendText(path);
                sr.WriteLine(json);
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        internal static void LoadDnsProviders()
        {
            dnsProviders = new List<Provider>() {
                new Provider(){
                    Name = "Google",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() {Phrase = "ns1.google.com", FindAt = WhereIs.Full },
                        new SearchPhrase(){ Phrase = "ns2.google.com", FindAt = WhereIs.Full },
                        new SearchPhrase() {Phrase = ".google.com", FindAt = WhereIs.EndWidth },
                    }
                },

                new Provider(){
                    Name = "GoogleDomains",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".googledomains.com",FindAt = WhereIs.EndWidth }
                    }
                },

                new Provider(){
                    Name = "BizimDns",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".bizimdns.com",FindAt = WhereIs.EndWidth }
                    }
                },

                new Provider(){
                    Name = "Yandex",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".yandex.net",FindAt = WhereIs.EndWidth }
                    }
                },

                new Provider(){
                    Name = "CloudFlare",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".cloudflare.com",FindAt = WhereIs.EndWidth }
                    }
                },

                new Provider(){
                    Name = "Azure",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".azure-dns.com",FindAt = WhereIs.EndWidth },
                        new SearchPhrase() { Phrase = ".azure-dns.net",FindAt = WhereIs.EndWidth },
                        new SearchPhrase() { Phrase = ".azure-dns.org",FindAt = WhereIs.EndWidth },
                    }
                },

                new Provider(){
                    Name = "AmazonWS",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".awsdns-",FindAt = WhereIs.Contains },
                    }
                },

                new Provider(){
                    Name = "BlueHost",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".bluehost.com",FindAt = WhereIs.EndWidth },
                    }
                },

                new Provider(){
                    Name = "HostGator",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".hostgator.com",FindAt = WhereIs.EndWidth },
                    }
                },

                new Provider(){
                    Name = "UltraDNS",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".ultradns.net",FindAt = WhereIs.EndWidth },
                    }
                },

                new Provider(){
                    Name = "DomainControl",
                    SearchPhrases = new List<SearchPhrase>() {
                        new SearchPhrase() { Phrase = ".domaincontrol.com",FindAt = WhereIs.EndWidth },
                    }
                },
            };

            try
            {
                string json = JsonConvert.SerializeObject(dnsProviders, Formatting.Indented);

                String path = @"" + "Providers_Dns.txt";
                if (File.Exists(path)) File.Delete(path);
                using StreamWriter sr = File.AppendText(path);
                sr.WriteLine(json);
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}