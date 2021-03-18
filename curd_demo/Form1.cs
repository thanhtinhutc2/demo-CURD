using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace curd_demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=F51N30SU\SQLEXPRESS;Initial Catalog=DemoCURD;Integrated Security=True");

        private void GetStudentsRecord()
        {

            SqlCommand cmd = new SqlCommand("Select * from StudentsTb", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            conn.Close();
            dataGridView1.DataSource = dt;
        }

        private bool IsValidData()
        {
            if (txtTenSV.Text == string.Empty
                || txtHoSV.Text == string.Empty
                || txtDiaChi.Text == string.Empty
                || string.IsNullOrEmpty(txtDienThoai.Text)
                || string.IsNullOrEmpty(txtSBD.Text))
            {
                MessageBox.Show("Có chỗ chưa nhập dữ liệu!", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (IsValidData())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO StudentsTb values " + "(@Name, @FatherName, @RollNumber, @Address, @Mobile)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", txtHoSV.Text);
                cmd.Parameters.AddWithValue("@FatherName", txtTenSV.Text);
                cmd.Parameters.AddWithValue("@RollNumber", txtSBD.Text);
                cmd.Parameters.AddWithValue("@Address", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtDienThoai.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                GetStudentsRecord();
            }
        }

        public int StudentID;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentID = Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value);
            txtTenSV.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtHoSV.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtSBD.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtDiaChi.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtDienThoai.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("update StudentsTb set Name = @Name" +
                    "FatherName = @FatherName, RollNumber = @RollNumber, Address = @Address, " +
                    "Mobile = @Mobile where StudentID = @id", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", txtTenSV.Text);
                cmd.Parameters.AddWithValue("@FatherName", txtHoSV.Text);
                cmd.Parameters.AddWithValue("@RollNumber", txtSBD.Text);
                cmd.Parameters.AddWithValue("@Address", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtDienThoai.Text);
                cmd.Parameters.AddWithValue("@id", this.StudentID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                GetStudentsRecord();
                ResetData();
            }
            else
            {
                MessageBox.Show("Cập nhật bị lỗi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ResetData()
        {
            txtDiaChi.Text = "";
            txtDienThoai.Text = "";
            txtHoSV.Text = "";
            txtSBD.Text = "";
            txtTenSV.Text = "";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("delete from StudentsTb where StudentID = @id", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", this.StudentID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                GetStudentsRecord();
                ResetData();
            }
            else
            {
                MessageBox.Show("Xóa bị lỗi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
