using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NetFwTypeLib;
using System.IO;
namespace FireWall
{
    public partial class Form1 : Form
    {
        RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",true);
        public Form1()
        {
            reg.SetValue("Windows FireWall Controller", Application.ExecutablePath.ToString());
            InitializeComponent();
            button1.Select();
            tabControl1.Visible = false;
            panel9.Visible = false;
            tabControl2.Visible = false;
            tabControl3.Visible = false;
            button1.FlatAppearance.BorderColor = Color.White;
            button2.FlatAppearance.BorderColor = Color.White;
            button3.FlatAppearance.BorderColor = Color.White;
            button4.FlatAppearance.BorderColor = Color.White;
            AppResource.resource res = new AppResource.resource();
            Rules.FirewallRules fwrule = new Rules.FirewallRules();
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.MultiSelect = false;
            List<INetFwRule> rules = fwrule.ruleList();
            DataTable table = res.getDefaultRules(rules);
            dataGridView2.DataSource = table;
            richTextBox1.Text = res.getAnalysis(rules);
            int profile = fwrule.FWGetProfile();
            switch (profile)
            {
                case 1:
                    checkBox4.Checked = true;
                    fwrule.FWProfileChange(4);
                    break;
                case 2:
                    checkBox3.Checked = true;
                    fwrule.FWProfileChange(3);
                    break;
                case 3:
                    checkBox2.Checked = true;
                    fwrule.FWProfileChange(2);
                    break;
                case 4:
                    checkBox1.Checked = true;
                    fwrule.FWProfileChange(1);
                    break;
            }
            richTextBox2.Text = res.BestPractices();
        }
       

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AppResource.resource res = new AppResource.resource();
            Rules.FirewallRules fwrule = new Rules.FirewallRules();
            if (checkBox1.Checked == true)
            {
                fwrule.FWProfileChange(4);
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            AppResource.resource res = new AppResource.resource();
            Rules.FirewallRules fwrule = new Rules.FirewallRules();
            if (checkBox2.Checked == true)
            {
                fwrule.FWProfileChange(3);
                checkBox1.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            AppResource.resource res = new AppResource.resource();
            Rules.FirewallRules fwrule = new Rules.FirewallRules();
            if (checkBox3.Checked == true)
            {
                fwrule.FWProfileChange(2);
                checkBox2.Checked = false;
                checkBox1.Checked = false;
                checkBox4.Checked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            AppResource.resource res = new AppResource.resource();
            Rules.FirewallRules fwrule = new Rules.FirewallRules();
            if (checkBox4.Checked == true)
            {
                fwrule.FWProfileChange(1);
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            tabControl3.Visible = false;
            panel4.Visible = true;
            panel9.Visible = false;
            tabControl2.Visible = false;
            tabControl1.Visible = false;
            button1.BackColor = Color.LightGray;
            button2.BackColor = Color.White;
            button3.BackColor = Color.White;
            button4.BackColor = Color.White;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            tabControl3.Visible = false;
            tabControl2.Visible = false;
            tabControl1.Visible = true;
            panel4.Visible = false;
            panel9.Visible = true;
            tabControl1.Visible = true;
            button2.BackColor = Color.LightGray;
            button1.BackColor = Color.White;
            button3.BackColor = Color.White;
            button4.BackColor = Color.White;
        }

        private void button3_Click(object sender, EventArgs e)
        {


            
            tabControl3.Visible = true;
            tabControl1.Visible = false;
            tabControl2.Visible = false;
            panel4.Visible = false;
            panel9.Visible = true;
            button3.BackColor = Color.LightGray;
            button2.BackColor = Color.White;
            button1.BackColor = Color.White;
            button4.BackColor = Color.White;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            tabControl3.Visible = false;
            tabControl1.Visible = false;
            tabControl2.Visible = true;
            panel4.Visible = false;
            panel9.Visible = true;
            button4.BackColor = Color.LightGray;
            button2.BackColor = Color.White;
            button1.BackColor = Color.White;
            button3.BackColor = Color.White;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label2.Visible = false;
        }
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Rules.FirewallRules fwrule = new Rules.FirewallRules();
            List<INetFwRule> rules = fwrule.ruleList();
            AppResource.resource res = new AppResource.resource();
            richTextBox1.Text = res.getAnalysis(rules);
            richTextBox2.Text = res.BestPractices();
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            AppResource.resource res = new AppResource.resource();
            Rules.FirewallRules fwrule = new Rules.FirewallRules();
            if(tabControl1.SelectedIndex ==1)
            {
                List<INetFwRule> rules = fwrule.ruleList();
                DataTable table = res.getRuleNames(rules);
                dataGridView1.DataSource = table;
                
                
            }
            label2.Visible = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Executable|*.exe";
            fd.Multiselect = true;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = String.Join(";", fd.FileNames);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            FireWall.Rules.FirewallRules rules = new Rules.FirewallRules();
            AppResource.resource res = new AppResource.resource();
            bool result = false;
            if (textBox2.Text != "")
            { 
            foreach (string path in textBox2.Text.Split(';'))
                if (File.Exists(path))
                {
                    if (inc.Checked || bo.Checked)
                    {
                        result = rules.FWApplicationRule(path, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN, (bl.Checked) ? NET_FW_ACTION_.NET_FW_ACTION_BLOCK : NET_FW_ACTION_.NET_FW_ACTION_ALLOW);
                    }
                    if (outg.Checked || bo.Checked)
                    {
                        result = rules.FWApplicationRule(path, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT, (bl.Checked) ? NET_FW_ACTION_.NET_FW_ACTION_BLOCK : NET_FW_ACTION_.NET_FW_ACTION_ALLOW);
                    }
                }
            }
            if(textBox1.Text !="")
            {
                System.Net.IPAddress[] Ipaddress = res.getIPAddress(textBox1.Text);
                if(Ipaddress != null)
                {
                    int count = 1;
                    foreach (System.Net.IPAddress ip in Ipaddress)
                    {
                        if (inc.Checked || bo.Checked)
                        {
                            result = rules.FWIpRule(textBox1.Text, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN, (bl.Checked) ? NET_FW_ACTION_.NET_FW_ACTION_BLOCK : NET_FW_ACTION_.NET_FW_ACTION_ALLOW, ip.ToString(), count);
                        }
                        if (outg.Checked || bo.Checked)
                        {
                            result = rules.FWIpRule(textBox1.Text, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT, (bl.Checked) ? NET_FW_ACTION_.NET_FW_ACTION_BLOCK : NET_FW_ACTION_.NET_FW_ACTION_ALLOW, ip.ToString(), count);
                        }
                        count++;
                    }
                }
            }
            if(result == true)
            {
                MessageBox.Show("New Rule/s added succesfully.");
            }
            else
            {
                MessageBox.Show("one or more Rule/s failed to add.");
            }
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

            string rulename = Convert.ToString(selectedRow.Cells[0].Value);
            string type = Convert.ToString(selectedRow.Cells[1].Value);
            string direction = Convert.ToString(selectedRow.Cells[2].Value);
            AppResource.resource rs = new AppResource.resource();
            Rules.FirewallRules fw = new Rules.FirewallRules();
            INetFwRule rule = rs.getRule(fw.ruleList(), type, direction, rulename);
            if (rule != null)
            {
                if (fw.FWDeleteRule(rule))
                {
                    MessageBox.Show(rule.Name + " Deleted Sucessfully.", "Delte Rule", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    List<INetFwRule> rules = fw.ruleList();
                    DataTable table = rs.getRuleNames(rules);
                    dataGridView1.DataSource = table;
                }
                else
                    MessageBox.Show(rule.Name + " could Not Be Deleted.", "Delte Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show(rule.Name + " could Not Be Deleted.", "Delte Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button8_Click(object sender, EventArgs e)
        {
          int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

            string rulename = Convert.ToString(selectedRow.Cells[0].Value);
            string type = Convert.ToString(selectedRow.Cells[1].Value);
            string direction = Convert.ToString(selectedRow.Cells[2].Value);
            AppResource.resource rs = new AppResource.resource();
            Rules.FirewallRules fw = new Rules.FirewallRules();
            INetFwRule rule = rs.getRule(fw.ruleList(), type, direction, rulename);
            if (rule != null)
            {
                tabControl1.SelectedIndex = 0;
                update up = new update(rule);
                up.Show();
                
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("wf.msc");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("firewall.cpl");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("taskmgr");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("devmgmt.msc");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("resmon");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("wscui.cpl");
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
                    Application.Exit();
        }
        private void Form1_Move(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
            else
                e.Cancel = false;

            // Confirm user wants to close
           
            this.Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }
    }
   
}
