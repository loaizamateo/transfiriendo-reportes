using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.Shared;
using System.IO;

namespace reportesTransfiriendo
{
    public partial class Form2 : Form
    {
        SqlConnection Conn;
        SqlCommand comandRpt;
        SqlDataAdapter dbAdapter;
        ConnectionInfo rptConnInfo;
        DataSet dbRecord;

        string strSQLCommand;
        string strSQLConn;
        string reporte;
        private string valueCufe;
        private string valueReport;


        public Form2(string cufe, string report)
        {
            InitializeComponent();
            this.valueCufe = cufe;
            this.valueReport = report;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            strSQLConn = "Data source= MATEOLOAIZA3D59\\SQLEXPRESS; Initial catalog = transfiriendo_base; User id = mateo; Password= mateo123";
            strSQLCommand = "DocumentoTributario_g_CufeMejorado";
            crystalReportViewer1.ToolPanelView = ToolPanelViewType.None;

            var files = Directory.GetFiles(@"reportes", this.valueReport + ".rpt", SearchOption.AllDirectories).Where(s => s.EndsWith(this.valueReport + ".rpt"));
            int b = 0;
            foreach (string file in files)
            {
                b = 1;
                reporte = file;
            }

            if(b == 1)
            {
                Conn = new SqlConnection(strSQLConn);
                comandRpt = new SqlCommand(strSQLCommand, Conn);
                comandRpt.CommandType = CommandType.StoredProcedure;
                comandRpt.Parameters.Add("@Cufe",SqlDbType.VarChar, 50).Value = this.valueCufe;

                dbAdapter = new SqlDataAdapter(comandRpt);
                dbRecord = new DataSet();

                dbAdapter.Fill(dbRecord);

                using (ReportDocument report = new ReportDocument())
                {
                    report.Load(reporte);
                    report.SetDataSource(dbRecord);
                    rptConnInfo = new ConnectionInfo();
                    rptConnInfo.ServerName = "MATEOLOAIZA3D59\\SQLEXPRESS";
                    rptConnInfo.DatabaseName = "transfiriendo_base";
                    rptConnInfo.UserID = "mateo";
                    rptConnInfo.Password = "mateo123";
                    rptConnInfo.Type = ConnectionInfoType.SQL;
                    foreach (Table t in report.Database.Tables)
                    {
                        t.LogOnInfo.ConnectionInfo = rptConnInfo;
                        t.ApplyLogOnInfo(t.LogOnInfo);
                    }

                    crystalReportViewer1.ReportSource = report;
                    crystalReportViewer1.Refresh();
                }    
            }
            else
            {
                MessageBox.Show("El reporte digitado no existe");
            }
        }
    }
}
