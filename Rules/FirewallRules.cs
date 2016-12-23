using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFwTypeLib;
using System.Windows.Forms;

namespace FireWall.Rules
{
    class FirewallRules
    {
        public AppResource.resource resource = new AppResource.resource();
        public bool FWApplicationRule(string path, NET_FW_RULE_DIRECTION_ d, NET_FW_ACTION_ fwaction)
        {
            try
            {
                INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule.Action = fwaction;
                firewallRule.Enabled = true;
                firewallRule.InterfaceTypes = "All";
                firewallRule.ApplicationName = path;
                firewallRule.Name = "FWHAPP: " + System.IO.Path.GetFileName(path);
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallRule.Direction = d;
                if (!this.isDuplicate(firewallRule.ApplicationName, d,fwaction) && !this.isAmbiguous(firewallRule.ApplicationName, d, fwaction))
                {
                    firewallPolicy.Rules.Add(firewallRule);
                    String act = (fwaction == NET_FW_ACTION_.NET_FW_ACTION_ALLOW) ? "Allow" : "Block";
                    String dire = (d == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN) ? "Incoming" : "Outgoing";
                    resource.WriteLog("Application rule added in the path " + path + " Action: " + act + "Direction: " + dire);
                }
                else
                {
                    return false;
                }
                return true;
                

            }
            catch (Exception ex) { Console.WriteLine(ex.GetType()); MessageBox.Show(ex.ToString()); return false; }
        }
        public bool FWApplicationRule(string path,String name, NET_FW_RULE_DIRECTION_ d, NET_FW_ACTION_ fwaction)
        {
            try
            {
                INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule.Action = fwaction;
                firewallRule.Enabled = true;
                firewallRule.InterfaceTypes = "All";
                firewallRule.ApplicationName = path;
                firewallRule.Name = name;
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallRule.Direction = d;
                firewallPolicy.Rules.Add(firewallRule);
                if (!this.isDuplicate(firewallRule.ApplicationName, d, fwaction) && !this.isAmbiguous(firewallRule.ApplicationName, d, fwaction))
                {
                    firewallPolicy.Rules.Add(firewallRule);
                    String act = (fwaction == NET_FW_ACTION_.NET_FW_ACTION_ALLOW) ? "Allow" : "Block";
                    String dire = (d == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN) ? "Incoming" : "Outgoing";
                    resource.WriteLog("Application rule added in the path " + path + " Action: " + act + "Direction: " + dire);
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.GetType()); return false; }
        }
        public bool FWState()
        {
            Type netFwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 mgr = (INetFwPolicy2)Activator.CreateInstance(netFwPolicy2Type);
            if (mgr.get_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE) && mgr.get_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC) && mgr.get_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN))
            {
                return true;
            }
            else
                return false;

        }

        public bool FWEnable(bool result)
        {
            try
            {
                Type netFwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 mgr = (INetFwPolicy2)Activator.CreateInstance(netFwPolicy2Type);

                // Gets the current firewall profile (domain, public, private, etc.)
                NET_FW_PROFILE_TYPE2_ fwCurrentProfileTypes = (NET_FW_PROFILE_TYPE2_)mgr.CurrentProfileTypes;
                mgr.set_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE, result);
                mgr.set_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN, result);
                mgr.set_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC, result);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<INetFwRule> ruleList()
        {
            Type netFwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(netFwPolicy2Type);

            List<INetFwRule> RuleList = new List<INetFwRule>();

            foreach (INetFwRule rule in fwPolicy2.Rules)
            {
                RuleList.Add(rule);
                //Console.WriteLine(rule.LocalPorts);
            }
            return RuleList;
        }
        public bool isFirewallEnabled()
        {
            Type NetFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
            INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(NetFwMgrType);
            bool Firewallenabled = mgr.LocalPolicy.CurrentProfile.FirewallEnabled;
            return Firewallenabled;
        }
        public bool FWDeleteRule(INetFwRule rule)
        {
            try
            {
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Remove(rule.Name);

                String act = (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW) ? "Allow" : "Block";
                String dire = (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN) ? "Incoming" : "Outgoing";
                resource.WriteLog("rule deleted in the path " + rule.ApplicationName + " Action: " + act + "Direction: " + dire);
                return true;
            }
            catch (Exception ee)
            {
                Console.WriteLine("Can not REMOVE rule.\r\n" + ee.ToString());
                return false;
            }
        }
        public bool FWIpRule(string website, NET_FW_RULE_DIRECTION_ d, NET_FW_ACTION_ fwaction, string Ip,int count)
        {
            try
            {
                INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule.Action = fwaction;
                firewallRule.Enabled = true;
                firewallRule.InterfaceTypes = "All";
                firewallRule.ApplicationName = website;
                firewallRule.Name = "FWHIP: " + website+"- "+count;
                firewallRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_ANY;
                firewallRule.RemoteAddresses = Ip;
                firewallRule.LocalAddresses = "*";
                firewallRule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallRule.Direction = d;

                if (!this.isDuplicate(firewallRule.ApplicationName, d, fwaction) && !this.isAmbiguous(firewallRule.ApplicationName, d, fwaction))
                {
                    firewallPolicy.Rules.Add(firewallRule);
                    String act = (fwaction == NET_FW_ACTION_.NET_FW_ACTION_ALLOW) ? "Allow" : "Block";
                    String dire = (d == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN) ? "Incoming" : "Outgoing";
                    resource.WriteLog("Ip rule added to the website " + website + " Action: " + act + "Direction: " + dire);
                }
                else
                {
                    return false;
                }
               
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); return false; }
        }
        public bool FWIpRule(string website,string name, NET_FW_RULE_DIRECTION_ d, NET_FW_ACTION_ fwaction, string Ip)
        {
            try
            {
                INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule.Action = fwaction;
                firewallRule.Enabled = true;
                firewallRule.InterfaceTypes = "All";
                firewallRule.ApplicationName = website;
                firewallRule.Name = name;
                firewallRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_ANY;
                firewallRule.RemoteAddresses = Ip;
                firewallRule.LocalAddresses = "*";
                firewallRule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallRule.Direction = d;
                if (!this.isDuplicate(firewallRule.ApplicationName, d, fwaction) && !this.isAmbiguous(firewallRule.ApplicationName, d, fwaction))
                {
                    firewallPolicy.Rules.Add(firewallRule);
                    String act = (fwaction == NET_FW_ACTION_.NET_FW_ACTION_ALLOW) ? "Allow" : "Block";
                    String dire = (d == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN) ? "Incoming" : "Outgoing";
                    resource.WriteLog("Ip rule added to the website " + website + " Action: " + act + "Direction: " + dire);
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); return false; }
        }
        public bool FWUpdateRule(INetFwRule oldrule, INetFwRule newrule, bool app)
        {
            try
            {
                if (FWDeleteRule(oldrule))
                    if (app)
                        return FWApplicationRule(newrule.ApplicationName,newrule.Name, newrule.Direction, newrule.Action);
                    else
                        return FWIpRule(newrule.ApplicationName,newrule.Name, newrule.Direction, newrule.Action, newrule.RemoteAddresses);
                return false;
            }
            catch (Exception ee)
            {
                return false;
            }
        }
        public bool FWProfileChange(int action)
        {
            Type netFwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 mgr = (INetFwPolicy2)Activator.CreateInstance(netFwPolicy2Type);
            
            try
            {
                if (action == 1)
                {
                    resource.WriteLog("profile changed to No filtering");
                    return FWEnable(false);
                }
                else if (action == 2)
                {
                    if (FWEnable(true))
                    {
                        mgr.set_BlockAllInboundTraffic(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL, false);
                        mgr.set_DefaultOutboundAction(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL, NET_FW_ACTION_.NET_FW_ACTION_MAX);
                        mgr.set_DefaultInboundAction(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL, NET_FW_ACTION_.NET_FW_ACTION_BLOCK);
                        resource.WriteLog("profile changed to low filtering");
                        return true;
                    }
                    else
                        return false;
                }
                else if (action == 3)
                {
                    if (FWEnable(true))
                    {
                        mgr.set_BlockAllInboundTraffic(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL, false);
                        mgr.set_DefaultOutboundAction(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL, NET_FW_ACTION_.NET_FW_ACTION_BLOCK);
                        mgr.set_DefaultInboundAction(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL, NET_FW_ACTION_.NET_FW_ACTION_ALLOW);
                        resource.WriteLog("profile changed to high filtering");
                        return true;
                    }
                    else
                        return false;
                }
                else if (action == 4)
                {

                    if (FWEnable(true))
                    {
                        mgr.set_BlockAllInboundTraffic(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL, true);
                        resource.WriteLog("profile changed to high filtering");
                        return true;
                    }
                    else
                        return false;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public int FWGetProfile()
        {
            Type netFwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 mgr = (INetFwPolicy2)Activator.CreateInstance(netFwPolicy2Type);
            try
            {
                if (!isFirewallEnabled())
                    return 1;
                else
                {
                    if (mgr.get_BlockAllInboundTraffic(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL))
                    {
                        
                        return 4;
                    }
                    else if (mgr.get_DefaultInboundAction(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL) == NET_FW_ACTION_.NET_FW_ACTION_BLOCK && mgr.get_DefaultOutboundAction(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL) == NET_FW_ACTION_.NET_FW_ACTION_MAX)
                    {
                        return 2;
                    }
                    else if (mgr.get_DefaultInboundAction(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL) == NET_FW_ACTION_.NET_FW_ACTION_ALLOW && mgr.get_DefaultOutboundAction(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL) == NET_FW_ACTION_.NET_FW_ACTION_BLOCK)
                    {
                        return 3;
                    }
                    else
                    {
                        return 3;
                    }
                }
            }
            catch (Exception e)
            {
                return 3;
            }
        }
        public bool isDuplicate(string appname, NET_FW_RULE_DIRECTION_ d, NET_FW_ACTION_ fwaction)
        {
            Type netFwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(netFwPolicy2Type);

            foreach (INetFwRule rul in fwPolicy2.Rules)
            {
                if (rul.ApplicationName == appname && rul.Direction == d && rul.Action == fwaction)
                    return true;
            }
           
            return false;
        }
        public bool isAmbiguous(string appname, NET_FW_RULE_DIRECTION_ d, NET_FW_ACTION_ fwaction)
        {
            Type netFwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(netFwPolicy2Type);

            foreach (INetFwRule rul in fwPolicy2.Rules)
            {
                if (rul.ApplicationName == appname && rul.Direction == d)
                    if ((fwaction == NET_FW_ACTION_.NET_FW_ACTION_ALLOW && rul.Action == NET_FW_ACTION_.NET_FW_ACTION_BLOCK) && (fwaction == NET_FW_ACTION_.NET_FW_ACTION_BLOCK && rul.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW))
                        return true;
            }

            return false;
        }
    }
}
