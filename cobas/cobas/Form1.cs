using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace cobas
{

    public partial class Form1 : Form
    {
        MySqlDataAdapter mySqlDataAdapter;
        MySqlConnection mySqlConnection;
        MySqlDataReader mySqlDataReader;
        MySqlCommand mySqlCommand;

        string connection = "server=localhost;uid=root;pwd=;database=premier_league";
        string query = "";
        DataTable dtPlayer = new DataTable();

        public Form1()
        {
            InitializeComponent();
            query = "";
            mySqlConnection = new MySqlConnection(connection);
            mySqlCommand = new MySqlCommand(query,mySqlConnection);
        }

        private void forDGV()
        {
            query = "select * from player";
            mySqlConnection = new MySqlConnection(connection);
            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            mySqlDataAdapter.Fill(dtPlayer);
            dataGridView3.DataSource = dtPlayer;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                mySqlConnection.Open();
                MySqlCommand sc = new MySqlCommand("SELECT nationality_id, nation FROM nationality n;", mySqlConnection);
                mySqlDataReader=sc.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("nation", typeof(string));
                dt.Load(mySqlDataReader);
                comboBox_Nationality.DisplayMember = "nation";
                comboBox_Nationality.ValueMember = "nationality_id";

                comboBox_Nationality.DataSource = dt;
                mySqlConnection.Close();

                mySqlConnection.Open();
                MySqlCommand sc1 = new MySqlCommand("SELECT team_id, team_name FROM team t;", mySqlConnection);
                mySqlDataReader = sc1.ExecuteReader();
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("team_name", typeof(string));
                dt1.Load(mySqlDataReader);
                comboBox_TeamName.DisplayMember = "team_name";
                comboBox_TeamName.ValueMember = "team_id";
                comboBox_TeamName.DataSource = dt1;
                mySqlConnection.Close();

                mySqlConnection.Open();
                MySqlCommand sc2 = new MySqlCommand("SELECT team_name FROM team t;", mySqlConnection);
                mySqlDataReader = sc2.ExecuteReader();
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("team_name", typeof(string));
                dt2.Load(mySqlDataReader);
                comboBox_TeamManager.DisplayMember = "team_name";
                comboBox_TeamManager.DataSource = dt2;
                mySqlConnection.Close();
            }
            catch (Exception)
            {

                
            }
            forDGV();
        }

        private void button_AddPlayer_Click(object sender, EventArgs e)
        {
            try
            {
                mySqlConnection.Open();
                MySqlCommand sc = new MySqlCommand("insert into PLAYER values('" + textBox_PlayerID.Text + "','" + textBox_TeamNumber.Text + "','" + textBox_Name.Text + "','" + comboBox_Nationality.SelectedValue.ToString() + "','" + textBox_Position.Text + "','" + textBox_Height.Text + "','" + textBox_Weight.Text + "','" + dateTimePicker1.Text + "','" + comboBox_TeamName.SelectedValue.ToString() +"','"+ '1' + "','" + '0' + "')", mySqlConnection);
                sc.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                forDGV();
                mySqlConnection.Close();
            }



        }

        private void comboBox_TeamManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtManager = new DataTable();
            mySqlConnection = new MySqlConnection(connection);
            query = $"SELECT m.manager_name, t.team_name,birthdate,nation\r\nFrom team t\r\nJoin manager m\r\non t.manager_id=m.manager_id\r\njoin nationality n\r\non m.nationality_id=n.nationality_id\r\nwhere t.team_name = '{comboBox_TeamManager.Text}';";
            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            mySqlDataAdapter.Fill(dtManager);
            dataGridView1.DataSource = dtManager;

            DataTable dtManagerNotWorking = new DataTable();
            mySqlConnection = new MySqlConnection(connection);
            query = $"SELECT manager_name,nation,birthdate\r\nFROM manager m\r\njoin nationality n\r\non m.nationality_id=n.nationality_id\r\nwhere working = 0;";
            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
            mySqlDataAdapter.Fill(dtManagerNotWorking);
            dataGridView2.DataSource = dtManagerNotWorking;
        }
    }
}
