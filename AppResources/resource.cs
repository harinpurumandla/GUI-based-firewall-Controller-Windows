using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using NetFwTypeLib;
using System.Data;
using System.IO;

namespace FireWall.AppResource
{
    class resource
    {
        public System.Net.IPAddress[] getIPAddress(String website)
        {
            try
            {
                if (website.StartsWith("http"))
                {
                    website += "/";
                    Console.WriteLine(website.Split('/')[2]);
                    website = website.Split('/')[2];
                }
                if (website.StartsWith("www."))
                    return System.Net.Dns.GetHostAddresses(website).Distinct().ToArray();

                else
                    return System.Net.Dns.GetHostAddresses("www." + website).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
       
        public bool isAmbiguous(List<INetFwRule> rules, INetFwRule rule)
        {
            foreach (INetFwRule rul in rules)
            {
                if (rul.ApplicationName == rule.ApplicationName && rul.Direction == rule.Direction)
                    if((rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW && rul.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK) && (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK && rul.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW))
                        return true;
            }
            return false;
        }
        public List<INetFwRule> findDuplicates(List<INetFwRule> rules)
        {
            List<INetFwRule> commonrules = new List<INetFwRule>();
            for(int i=0;i<rules.Count;i++)
                for(int j=0;j<rules.Count;j++)
                {
                    if(j != i)
                    {
                        INetFwRule temp1 = rules[i];
                        INetFwRule temp2 = rules[j];
                        temp1.Name = temp2.Name;
                        if(temp1.Equals(temp2))
                        {
                            commonrules.Add(rules[i]);
                        }
                    }
                }
            return commonrules;
        }
        public DataTable getRuleNames(List<INetFwRule> rules)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Rule Name", typeof(String));
            table.Columns.Add("Type", typeof(String));
            table.Columns.Add("Direction", typeof(String));
            try
            {
                
                if (rules != null)
                {
                    foreach (INetFwRule rule in rules)
                    {
                        {
                            string action = "";
                            string direction = "";
                            if (rule.Name.StartsWith("FWH"))
                            {
                                if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW)
                                    action = "Allow";
                                else if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK)
                                    action = "Block";
                                else
                                    action = "All";
                                if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN)
                                    direction = "Incoming";
                                else if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT)
                                    direction = "Outgoing";
                                else
                                    direction = "Both";
                                table.Rows.Add(rule.Name, action, direction);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
                {}
            return table;

        }
        public String getAnalysis(List<INetFwRule> rules)
        {
            String str = "\n";
            int count = rules.Count;
            int inbound = 0;
            int outbound = 0;
            int blocked = 0;
            int allowed = 0;
            List<INetFwRule> helperrules = new List<INetFwRule>();
            foreach(INetFwRule rule in rules)
            {
                if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW)
                    allowed++;
                if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK)
                    blocked++;
                if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN)
                    inbound++;
                if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT)
                    outbound++;
                if (rule.Name.StartsWith("FWH"))
                    helperrules.Add(rule);
            }
            str += "Total number of Rules " + count.ToString() + ".\n";
            str+= "\t\tTotal Inbound Rules " + inbound.ToString()+".\n";
            str += "\t\tTotal Outbound Rules " + outbound.ToString()+".\n";
            str+= "\t\tTotal Blocked Rules "+blocked.ToString()+".\n";
            str+="\t\tTotal Allowed Rules "+allowed.ToString()+".\n";
            str += "";
            int in1 = 0, in2 = 0, out1 = 0, out2 = 0, b1 = 0, b2 = 0, b3 = 0, b4 = 0, a1 = 0, a2 = 0,a3=0, a4 = 0;
            foreach(INetFwRule rule in helperrules)
            {
                if(rule.Name.StartsWith("FWHAPP"))
                {
                    if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN)
                    {
                        in1++;
                        if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW)
                            a1++;
                        if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK)
                            b1++;
                    }
                    if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT)
                    {
                        out1++;
                        if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW)
                            a2++;
                        if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK)
                            b2++;
                    }
                }
                else
                {
                    if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN)
                    {
                        in2++;
                        if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW)
                            a3++;
                        if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK)
                            b3++;
                    }
                    if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT)
                    {
                        out2++;
                        if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW)
                            a4++;
                        if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK)
                            b4++;
                    }
                }
                
            }
            str += "Total number of rules created by Firewall Helper " + helperrules.Count + ".\n";
            str += "Applicaiton rules: " + (in1 + out1).ToString() + ".\n";
            str += "		Inbound rules: " + in1.ToString() + ".\n";
            str += "		Outbound Rules: " + out1.ToString() + ".\n";
            str += "IP rules: " + (in2 + out2).ToString() + ".\n";
            str += "		Inbound rules: " + in2.ToString() + ".\n";
            str += "		Outbound Rules: " + out2.ToString() + ".\n";
            return str;
        }
        public DataTable getDefaultRules(List<INetFwRule> rules)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Rule Name", typeof(String));
            table.Columns.Add("Type", typeof(String));
            table.Columns.Add("Direction", typeof(String));
            try
            {

                if (rules != null)
                {
                    foreach (INetFwRule rule in rules)
                    {
                        {
                            string action = "";
                            string direction = "";
                            if (!rule.Name.StartsWith("FWH"))
                            {
                                if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW)
                                    action = "Allow";
                                else if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK)
                                    action = "Block";
                                else
                                    action = "All";
                                if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN)
                                    direction = "Incoming";
                                else if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT)
                                    direction = "Outgoing";
                                else
                                    direction = "Both";
                                table.Rows.Add(rule.Name, action, direction);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return table;

        }
        public INetFwRule getRule(List<INetFwRule> rules,string action,string direction,string name)
        {
            NET_FW_RULE_DIRECTION_ dir;
            NET_FW_ACTION_ act;
            if (action.Equals("Allow"))
                act = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            else if (action.Equals("Block"))
                act = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            else
                act = NET_FW_ACTION_.NET_FW_ACTION_MAX;



            if (direction.Equals("Incoming"))
                dir = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            else if (direction.Equals("Outgoing"))
                dir = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            else
                dir = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_MAX;
            try
            {

                if (rules != null)
                {
                    foreach (INetFwRule rule in rules)
                    {
                        {
                            
                            if (rule.Name.StartsWith("FWH"))
                            {
                                if (rule.Name.Equals(name) && rule.Action.Equals(act) && rule.Direction.Equals(dir))
                                    return rule;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return null;
        }
        public bool WriteLog(String log)
        {
            try
            {
                TextWriter newWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "log.txt", true);
                newWriter.WriteLine("*********************************** " + DateTime.Now + "***********************************");
                newWriter.WriteLine(log);
                newWriter.WriteLine("*******************************************************************************************\n");
                newWriter.Flush();
                newWriter.Close();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public String BestPractices()
        {
            string practices = "\n\nIndustries Best Practices used ";
            practices += "\n\tNo Duplicate Rules.";
            practices += "\n\tNo Rules Which clashes with other rules (No ambigious Rules).";
            practices += "\n\tAll Rule changes are updated.";
            practices += "\n\t Apply States based on Changes.";
            return practices;
        }
    }
}
