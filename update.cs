using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetFwTypeLib;
namespace FireWall
{
    public partial class update : Form
    {
        public INetFwRule mainrule = null;
        public update()
        {
            InitializeComponent();
            
        }
        public update(INetFwRule rule)
        {
            InitializeComponent();
            mainrule = rule;
            textBox1.Text = rule.Name.Split(':')[1];
            if (rule.Action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW)
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            else
            {
                radioButton2.Checked = false;
                radioButton1.Checked = true;
            }
            if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN)
                inc.Checked = true;
            else
                outg.Checked = true;
        }

        private void bl_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Rules.FirewallRules fw = new Rules.FirewallRules();
           INetFwRule newrule = mainrule;
           if (mainrule.Name.StartsWith("FWHAPP"))
           {
               newrule.Name = "FWHAPP:"+textBox1.Text;
               if (inc.Checked)
                   newrule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
               else if(outg.Checked)
                   newrule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
               if (radioButton1.Checked)
                   newrule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
               else if (radioButton2.Checked)
                   newrule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
               if(fw.FWUpdateRule(mainrule, newrule, true))
                   MessageBox.Show(newrule.Name + " Updated Sucessfully.", "Update Rule", MessageBoxButtons.OK, MessageBoxIcon.Information);
               else
                   MessageBox.Show(newrule.Name + " could Not Be Deleted.", "Update Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
           else if(mainrule.Name.StartsWith("FWHIP"))
           {
               newrule.Name = "FWHIP:" + textBox1.Text;
               if (inc.Checked)
                   newrule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
               else if (outg.Checked)
                   newrule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
               if (radioButton1.Checked)
                   newrule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
               else if (radioButton2.Checked)
                   newrule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
               fw.FWUpdateRule(mainrule, newrule, false);
               if (fw.FWUpdateRule(mainrule, newrule, true))
                   MessageBox.Show(newrule.Name + " Updated Sucessfully.", "Update Rule", MessageBoxButtons.OK, MessageBoxIcon.Information);
               else
                   MessageBox.Show(newrule.Name + " could Not Be Deleted.", "Update Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
           this.Hide();
        }
    }
}
