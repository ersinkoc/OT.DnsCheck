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
                    Parsers = new List<Parser>() {
                        new Parser() {Word = "aspmx.l.google.com", Algorithm = ParsingAlgorithm.Full },
                        new Parser(){ Word = "alt1.aspmx.l.google.com", Algorithm = ParsingAlgorithm.Full },
                        new Parser() {Word = "alt2.aspmx.l.google.com", Algorithm = ParsingAlgorithm.Full },
                        new Parser() {Word = "alt3.aspmx.l.google.com", Algorithm = ParsingAlgorithm.Full },
                        new Parser() {Word = "alt4.aspmx.l.google.com", Algorithm = ParsingAlgorithm.Full },
                        new Parser() {Word = ".l.google.com", Algorithm = ParsingAlgorithm.EndWidth },
                    }
                },
                new Provider(){
                    Name = "BizimHost",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".bizimdns.com",Algorithm = ParsingAlgorithm.EndWidth },
                        new Parser() { Word = ".bizimhost.com.tr",Algorithm = ParsingAlgorithm.EndWidth }
                    }
                },
                new Provider(){
                    Name = "Yandex",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = "mx.yandex.net",Algorithm = ParsingAlgorithm.Full }
                    }
                },
                new Provider(){
                    Name = "Office365",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".protection.outlook.com",Algorithm = ParsingAlgorithm.EndWidth },
                        new Parser() { Word = ".outlook.com",Algorithm = ParsingAlgorithm.EndWidth },
                    }
                },
                new Provider(){
                    Name = "Zoho",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".zoho.com",Algorithm = ParsingAlgorithm.EndWidth },
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
                    Parsers = new List<Parser>() {
                        new Parser() {Word = "ns1.google.com", Algorithm = ParsingAlgorithm.Full },
                        new Parser(){ Word = "ns2.google.com", Algorithm = ParsingAlgorithm.Full },
                        new Parser() {Word = ".google.com", Algorithm = ParsingAlgorithm.EndWidth },
                    }
                },

                new Provider(){
                    Name = "GoogleDomains",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".googledomains.com",Algorithm = ParsingAlgorithm.EndWidth }
                    }
                },

                new Provider(){
                    Name = "BizimDns",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".bizimdns.com",Algorithm = ParsingAlgorithm.EndWidth }
                    }
                },

                new Provider(){
                    Name = "Yandex",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".yandex.net",Algorithm = ParsingAlgorithm.EndWidth }
                    }
                },

                new Provider(){
                    Name = "CloudFlare",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".cloudflare.com",Algorithm = ParsingAlgorithm.EndWidth }
                    }
                },

                new Provider(){
                    Name = "Azure",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".azure-dns.com",Algorithm = ParsingAlgorithm.EndWidth },
                        new Parser() { Word = ".azure-dns.net",Algorithm = ParsingAlgorithm.EndWidth },
                        new Parser() { Word = ".azure-dns.org",Algorithm = ParsingAlgorithm.EndWidth },
                    }
                },

                new Provider(){
                    Name = "AmazonWS",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".awsdns-",Algorithm = ParsingAlgorithm.Contains },
                    }
                },

                new Provider(){
                    Name = "BlueHost",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".bluehost.com",Algorithm = ParsingAlgorithm.EndWidth },
                    }
                },

                new Provider(){
                    Name = "HostGator",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".hostgator.com",Algorithm = ParsingAlgorithm.EndWidth },
                    }
                },

                new Provider(){
                    Name = "UltraDNS",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".ultradns.net",Algorithm = ParsingAlgorithm.EndWidth },
                    }
                },

                new Provider(){
                    Name = "DomainControl",
                    Parsers = new List<Parser>() {
                        new Parser() { Word = ".domaincontrol.com",Algorithm = ParsingAlgorithm.EndWidth },
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