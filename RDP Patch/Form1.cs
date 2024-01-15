using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceProcess;
using System.IO;
using System.Security.AccessControl;

namespace RDP_Patch
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public async Task<ServiceControllerStatus> getServicesStatus(string serviceName)
        {
            //string termService = "TermService";
            //string sessionEnv = "SessionEnv";
            //string umrdpService = "UmRdpService";
            ServiceController sc = new ServiceController(serviceName);
            try
            {
                await Task.Run(() => { sc.Refresh(); });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return sc.Status;
        }

        public void getFilePermission(string filePath)
        {
            try
            {
                FileSecurity fileSec = File.GetAccessControl(filePath);
                AuthorizationRuleCollection rules = fileSec.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
                foreach (AuthorizationRule rule in rules)
                {
                    FileSystemAccessRule fsRule = rule as FileSystemAccessRule;
                    if (fsRule != null)
                    {
                        Console.WriteLine($"Identity: {fsRule.IdentityReference}");
                        Console.WriteLine($"Rights: {fsRule.FileSystemRights}");
                        Console.WriteLine($"Access Control Type: {fsRule.AccessControlType}");
                        Console.WriteLine($"Inheritance Flags: {fsRule.InheritanceFlags}");
                        Console.WriteLine($"Propagation Flags: {fsRule.PropagationFlags}");
                        Console.WriteLine();
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex);
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            ServiceControllerStatus termStatus = await getServicesStatus(termService);
            ServiceControllerStatus sessionStatus = await getServicesStatus(sessionEnv);
            ServiceControllerStatus umrdpStatus = await getServicesStatus(umrdpService);
            textBox1.AppendText("Term Service - " + termStatus.ToString());
            textBox1.AppendText(Environment.NewLine);
            textBox1.AppendText("Session Env - " + sessionStatus.ToString());
            textBox1.AppendText(Environment.NewLine);
            textBox1.AppendText("UmRdp Service - " + umrdpStatus.ToString());
            filePath = Environment.SystemDirectory + $@"\termsrv.dll";
            textBox2.AppendText(filePath);
            getFilePermission(filePath);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
        }

    }
}
