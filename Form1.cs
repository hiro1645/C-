using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Npgsql;

namespace WindowsFormsApp0
{
    public partial class Form1 : Form 
    {
        
        private const string V = "vbtab";
        private DataTable dt;

        Class1 select = new Class1();
        Class1 tran = new Class1();

        public Form1()
        {
            InitializeComponent();
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dt = null;

            GetGrid();

            dataGridView3.DataSource = dt;

            Column();
        }


        private StreamReader Getfile()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.FilterIndex = 2;

            ofd.Title = "開くファイルを選択してください";

            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(ofd.FileName);  
            }

            if (ofd.FileName == null)
            {
                return null;
            }

            StreamReader SR = new StreamReader(ofd.FileName, System.Text.Encoding.GetEncoding("shift_jis"));
            //if (ofd.FileName.EndsWith(".csv"))
            //{

            //}
            //else
            //{
            //    MessageBox.Show("エラー");
            //}
            return SR;
        }

        void GetGrid()
        {
            StreamReader SR = Getfile();
            if (SR is null)
            {
                return;
            }
            dt = new DataTable();

            string line = string.Empty;
            int i = 0;
            DataRow dr;

            if (i == 0)
            {
                line = SR.ReadLine();

                string[] item = line.Split('\t');

                int s = 0;

                foreach (String v in item)
                {
                    //dt.Columns.Add(item[s]);
                    dt.Columns.Add(v);
                    s ++;
                }
            }

            while (true)

            {
                line = SR.ReadLine();

                if (line == null)
                {
                    break;
                }

                String[] item = line.Split('\t');

                dr = dt.NewRow();
                dr.ItemArray = item;
                dt.Rows.Add(dr);
            }
      

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dt is null)
            {
                MessageBox.Show("ファイルを読み込んでください");
                return;
            }

            string a = "登録します。よろしいですか？";

             DialogResult result = MessageBox.Show(a,
                                             "質問",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Exclamation,
                                             MessageBoxDefaultButton.Button2);

            //何が選択されたか調べる 
            if (result == DialogResult.No)
            {
                return;
            }

            //dt = (DataTable)dataGridView1.DataSource;

            string sql = string.Empty;
            string sql2 = string.Empty;
            int count = dt.Rows.Count;
            int check = 0;
            int aa = 0;
            DataTable dt2 = new DataTable(); 

            sql = "";
            sql += " insert into mst_inserts3 ";
            sql += " (a, b, c, d, f,g,h,i,j,k,l) values ";

            foreach(DataRow row in dt.Rows)
            {
                sql2 = "";
                sql2 += "select a from mst_inserts3 ";
                sql2 += " where a = '" + row + "' ";

                dt2 = select.SelectSpl(sql2);
                check++;

                if (dt2.Rows.Count == 0)
                {
                    aa++;
                    if (check == count)
                    {
                        sql += " ('" + row[0] + "','" + row[1] + "', '" + row[2] + "','"+ row[3] + "','" + row[4] + "','" 
                            + row[5] + "','" + row[6] + "', '" + row[7] + "','" + row[8] + "','" + row[9] + "','" + row[10] + "')";
                    }
                    else
                    {
                        sql += " ('" + row[0] + "','" + row[1] + "', '" + row[2] + "','" + row[3] + "','" + row[4] + "','"
                            + row[5] + "','" + row[6] + "', '" + row[7] + "','" + row[8] + "','" + row[9] + "','" + row[10] + "'),";
                    }
                }
                if (aa == 0)
                {
                    MessageBox.Show("すべて登録済みです");
                    return;
                }

            }
 
            tran.TranSpl(sql);

            MessageBox.Show("完了！");
        }
        void Column()
        {
            dataGridView4.DataSource = null;
            dataGridView4.Columns.Clear();

            int dtcolumns = dt.Columns.Count;

            DataTable dt3 = new DataTable();

            DataRow dr;

            dt3.Columns.Add("タイトル");

            for (int a = 0; a <= dtcolumns - 1; a++)
            {
                dr = dt3.NewRow();
                dr["タイトル"] = dt.Columns[a];
                dt3.Rows.Add(dr);
            }

            dataGridView4.DataSource = dt3;
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            dataGridView4.Columns.Add(column);
        }

        void Csv(DataTable dt2)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {

                sfd.FileName = "out.csv";

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                    using (StreamWriter writer = new StreamWriter(sfd.FileName, false, Encoding.GetEncoding("shift_jis")))
                    {

                        int rowCount = dt2.Rows.Count;
                            int ColumnCount = dt2.Columns.Count;

                            var strList1 = new List<string>();
                            for (int i = 0; i<= ColumnCount - 1; i++)
                            {
                                strList1.Add(dt2.Columns[i].Caption);
                            }

                            string[] strary1 = strList1.ToArray();
                            string strCsvData1 = String.Join(",", strary1);

                            writer.WriteLine(strCsvData1);

                            for (int i = 0; i<= rowCount - 1; i++)
                            {
                                var strList = new List<string>();

                                for (int j=0; j <= ColumnCount - 1; j++)
                                {
                                    strList.Add((string)dt2.Rows[i][j]);
                                }
                                string[] strArray = strList.ToArray();
                                string strCsvData = String.Join(",", strArray);

                                writer.WriteLine(strCsvData);
                            }
                            MessageBox.Show("完了しました");
                         }


                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Csv(dt);
        }
    }
}
