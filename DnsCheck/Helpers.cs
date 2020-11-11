﻿using Nager.PublicSuffix;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace DnsCheck
{
    public static class Helpers
    {
        public static void ListProviders(Dictionary<string, string> a, List<Provider> provider)
        {
            Console.WriteLine(provider.Count +" provider");
            Console.WriteLine(a.Count + " domain(s)");


            if (a.Count > 0)
            {
                    foreach (Provider p in provider)
                    {
                        bool firstForTitle = false;
                        foreach (var item in from item in a
                                             where item.Value == p.Name
                                             select item)
                        {
                            if (firstForTitle == false)
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write(p.Name + "\n");
                                Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.White;
                                firstForTitle = true;
                            }
                            Console.WriteLine(item.Key);
                        }
                        Console.ResetColor();
                    }


            }

        }

        public static void ListWarnings(List<string> t, string desc, ConsoleColor bgColor, ConsoleColor fgColor, string toFile = "")
        {
            if (t.Count > 0)
            {
                Console.WriteLine();
                Console.BackgroundColor = bgColor;
                Console.ForegroundColor = fgColor;
                Console.Write(desc);
                Console.ResetColor();
                Console.WriteLine();
                foreach (var ne in t)
                {
                    Console.WriteLine(ne);
                }

                if (toFile.Length > 0)
                {
                    try
                    {
                        String path = @"" + toFile;
                        if (File.Exists(path)) File.Delete(path);
                        using StreamWriter sr = File.AppendText(path);
                        foreach (var ne in t)
                        {
                            sr.WriteLine(ne);
                        }
                        sr.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    Console.WriteLine("\n + Saved to \"" + toFile + "\"");
                }
            }
        }

        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public static void ExecuteCommand(string Command)
        {
            ProcessStartInfo ProcessInfo;
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + Command)
            {
                CreateNoWindow = true,
                UseShellExecute = true
            };
            _ = Process.Start(ProcessInfo);
        }

        public static string ParseDomain(string domain)
        {
            try
            {
                var domainParser = new DomainParser(new WebTldRuleProvider());
                var domainName = domainParser.Get(domain);
                return domainName.Domain + "." + domainName.TLD;
            }
            catch (Exception e)
            {
                _ = e.Message;
                return "badrequest";
                throw;
            }
        }

        public static void AlertApiLimit(string desc)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(desc);
            Console.ResetColor();
        }

        public static void Banner()
        {
            var switch_on = DateTime.Now.Second % 4;
            var cColor = switch_on switch
            {
                0 => ConsoleColor.DarkRed,
                1 => ConsoleColor.DarkBlue,
                2 => ConsoleColor.DarkGreen,
                3 => ConsoleColor.DarkMagenta,
                _ => ConsoleColor.DarkYellow,
            };

            Console.WriteLine();
            Console.Write("[");
            Console.BackgroundColor = cColor;
            Console.ForegroundColor = ConsoleColor.White;
            string title = "Bulk DNS checking tool with DNS Lookup API - github.com/ersinkoc/ot.DnsCheck";
            Console.Write(title);
            Console.Title = title;

            Console.ResetColor();
            Console.Write("]");

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("   .-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-.");
            Console.WriteLine("   | You can purchase a subscription via PromptApi/RapidApi to use the API |");
            Console.WriteLine("   |                                                                       |");
            Console.WriteLine("   |    www.promptapi.com (recommended)                                    |");
            Console.WriteLine("   !    https://promptapi.com/marketplace/description/dns_lookup-api       !");
            Console.WriteLine("   :                                                                       :");
            Console.WriteLine("   :    www.rapidapi.com                                                   :");
            Console.WriteLine("   .    https://rapidapi.com/ersinkoc/api/dns-lookup2                      .");
            Console.WriteLine("   .                                                                       .");
            Console.WriteLine("   :    - 20 requests/day       - Free                                     :");
            Console.WriteLine("   :    - 30.000 requests/month - 9.99$                                    :");
            Console.WriteLine("   !    - 90.000 requests/month - 19.99$                                   !");
            Console.WriteLine("   |                                                                       |");
            Console.WriteLine("   | This console application is just an example of bulk NS & MX checking. |");
            Console.WriteLine("   | You can also access A, AAAA, SOA, MX, TXT and NS records with the     |");
            Console.WriteLine("   | DNS Lookup API. You can develop your own applications with API.       |");
            Console.WriteLine("   .-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-.");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}