using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DnsCheck
{
    class SetProviders
    {
        public static List<Provider> dnsProviders;
        public static List<Provider> mailProviders;

        public static void Mail()
        {
            try
            {
                var fileName = @"" + "MailProviders.json";
                if (File.Exists(fileName))
                {
                    string jsonstring = File.ReadAllText(fileName);
                    if (!String.IsNullOrEmpty(jsonstring))
                        mailProviders = JsonConvert.DeserializeObject<List<Provider>>(jsonstring);

                }

                if (mailProviders == null)
                {
                    mailProviders = new List<Provider>() {
                        new Provider(){
                            Name = "Google",
                            SearchPhrases = new List<SearchPhrase>() {
                                new SearchPhrase() {Phrase = "aspmx.l.google.com", FindAt = CheckAlgorith.Full },
                                new SearchPhrase(){ Phrase = "alt1.aspmx.l.google.com", FindAt = CheckAlgorith.Full },
                                new SearchPhrase() {Phrase = "alt2.aspmx.l.google.com", FindAt = CheckAlgorith.Full },
                                new SearchPhrase() {Phrase = "alt3.aspmx.l.google.com", FindAt = CheckAlgorith.Full },
                                new SearchPhrase() {Phrase = "alt4.aspmx.l.google.com", FindAt = CheckAlgorith.Full },
                                new SearchPhrase() {Phrase = ".l.google.com", FindAt = CheckAlgorith.EndWidth },
                            }
                        },

                        new Provider(){
                            Name = "BizimHost",
                            SearchPhrases = new List<SearchPhrase>() {
                                new SearchPhrase() { Phrase = ".bizimdns.com",FindAt = CheckAlgorith.EndWidth },
                                new SearchPhrase() { Phrase = ".bizimhost.com.tr",FindAt = CheckAlgorith.EndWidth }

                            }
                        },

                        new Provider(){
                            Name = "Yandex",
                            SearchPhrases = new List<SearchPhrase>() {
                                new SearchPhrase() { Phrase = "mx.yandex.net",FindAt = CheckAlgorith.Full }
                            }
                        },

                        new Provider(){
                            Name = "Office365",
                            SearchPhrases = new List<SearchPhrase>() {
                                new SearchPhrase() { Phrase = ".protection.outlook.com",FindAt = CheckAlgorith.EndWidth },
                                new SearchPhrase() { Phrase = ".outlook.com",FindAt = CheckAlgorith.EndWidth },

                            }
                        },

                        new Provider(){
                            Name = "Zoho",
                            SearchPhrases = new List<SearchPhrase>() {
                                new SearchPhrase() { Phrase = ".zoho.com",FindAt = CheckAlgorith.EndWidth },
                            }
                        },
                    };

                    try
                    {
                        string json = JsonConvert.SerializeObject(mailProviders, Formatting.Indented);

                        String path = @"" + "MailProviders.json";
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void DNS()
        {
            try
            {
                var fileName = @"" + "DnsProviders.json";
                if (File.Exists(fileName))
                {
                    string jsonstring = File.ReadAllText(fileName);
                    if (!String.IsNullOrEmpty(jsonstring))
                        dnsProviders = JsonConvert.DeserializeObject<List<Provider>>(jsonstring);
                }


                if (dnsProviders == null)
                {
                    dnsProviders = new List<Provider>() {
                            new Provider(){
                                Name = "Google",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() {Phrase = "ns1.google.com", FindAt = CheckAlgorith.Full },
                                    new SearchPhrase(){ Phrase = "ns2.google.com", FindAt = CheckAlgorith.Full },
                                    new SearchPhrase() {Phrase = ".google.com", FindAt = CheckAlgorith.EndWidth },
                                }
                            },

                            new Provider(){
                                Name = "GoogleDomains",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() { Phrase = ".googledomains.com",FindAt = CheckAlgorith.EndWidth }
                                }
                            },

                            new Provider(){
                                Name = "BizimDns",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() { Phrase = ".bizimdns.com",FindAt = CheckAlgorith.EndWidth }
                                }
                            },

                            new Provider(){
                                Name = "Yandex",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() { Phrase = ".yandex.net",FindAt = CheckAlgorith.EndWidth }
                                }
                            },

                            new Provider(){
                                Name = "CloudFlare",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() { Phrase = ".cloudflare.com",FindAt = CheckAlgorith.EndWidth }
                                }
                            },

                            new Provider(){
                                Name = "Azure",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() { Phrase = ".azure-dns.com",FindAt = CheckAlgorith.EndWidth },
                                    new SearchPhrase() { Phrase = ".azure-dns.net",FindAt = CheckAlgorith.EndWidth },
                                    new SearchPhrase() { Phrase = ".azure-dns.org",FindAt = CheckAlgorith.EndWidth },
                                }
                            },

                            new Provider(){
                                Name = "AmazonWS",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() { Phrase = ".awsdns-",FindAt = CheckAlgorith.Contains },
                                }
                            },

                            new Provider(){
                                Name = "BlueHost",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() { Phrase = ".bluehost.com",FindAt = CheckAlgorith.EndWidth },
                                }
                            },

                            new Provider(){
                                Name = "HostGator",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() { Phrase = ".hostgator.com",FindAt = CheckAlgorith.EndWidth },
                                }
                            },

                            new Provider(){
                                Name = "UltraDNS",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() { Phrase = ".ultradns.net",FindAt = CheckAlgorith.EndWidth },
                                }
                            },

                            new Provider(){
                                Name = "DomainControl",
                                SearchPhrases = new List<SearchPhrase>() {
                                    new SearchPhrase() { Phrase = ".domaincontrol.com",FindAt = CheckAlgorith.EndWidth },
                                }
                            },
                    };

                    try
                    {
                        string json = JsonConvert.SerializeObject(dnsProviders, Formatting.Indented);

                        String path = @"" + "DnsProviders.json";
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }

        }
    }
}
