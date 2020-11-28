using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace budoco
{
    /*

    
    Budoco doesn't use a standard ASPNET appsettings.json file.
    Instead, it uses a linux style config file.

    The following is a rant about why I'm not using the appsettings.json file.
 
    * The MS JSON parser doesn't allow file comments. Users need comments
    to understand the settings, and it can be useful to comment out a line
    and try a different line.

    * The code generated by "dotnet" new has the config file being loaded 
    in Startup.cs, but Serilog wants to start logging in Program.cs. I want
    to configure the log file location early, in Program.cs. Maybe there's
    a way to move things around, but it's not obvious.

    * The syntax here is so easy. With json file, much easier to miss
    a quote, brace, comma.

    In my lifetime Microsoft has had four different config styles.
        1) .ini, like in windows 3.1 Pretty much like this.
        2) The registry. That was scary to edit, unfriendly to backup,
        no comments, no way to version control it.
        3) XML files. They were okay. Verbose and ugly, but they worked.
        4) JSON. 
    In that same time period unix/linux has had pretty much one format 
    and it has worked fine.

    */


    public static class bd_config
    {
        static Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

        // This reads "budoco_config.txt" and loads it into a key/value pair
        // collection.
        // Valid lines are either:
        // key:value
        // or
        // # this is a comment
        // or
        // blank

        public static void load_config()
        {
            bd_util.console_write_line("bd_config.load_config()");
            dict.Clear();

            var lines = File.ReadLines("budoco_config_active.txt");

            int line_count = 0;

            foreach (var line in lines)
            {
                line_count++;
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("#"))
                    continue;

                string[] pair = line.Split(":");

                if (pair.Length < 2)
                    throw new Exception(
                        "Line " + line_count.ToString() + " is bad:\n" + line);

                // handle ":" in the value, like http://localhost
                if (pair.Length > 2)
                {
                    for (int i = 2; i < pair.Length; i++)
                    {
                        pair[1] += ":" + pair[i];
                    }
                }

                // We have a valid key=value line.
                string key = pair[0].Trim();

                if (key == "")
                    throw new Exception(
                        "Line " + line_count.ToString() + " is bad:\n" + line);

                string string_value = pair[1].Trim();
                int int_value;

                if (Int32.TryParse(string_value, out int_value))
                {
                    dict[key] = int_value;
                }
                else
                {
                    dict[key] = string_value;
                }
            }

            bd_util.console_write_line("config:");
            foreach (var k in dict.Keys)
            {
                bd_util.console_write_line(k + "=" + dict[k].ToString());
            }
        }

        public static dynamic get(string key)
        {
            return dict[key];
        }


    }
}
