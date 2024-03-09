using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DB1
{
    public partial class Attendence : Form
    {
        public SqlDataReader dr;
        public Attendence()
        {
            InitializeComponent();
        }

        private void Attendence_Load(object sender, EventArgs e)
        {

        }




        public void Cmb_student()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("Student");
            SqlCommand cmd = new SqlCommand("Select ID,FirstName,LastName,Contact,Email,RegistrationNumber,Status From Student", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            comboBox1.DisplayMember = "RegistrationNumber";
            comboBox1.ValueMember = "ID";
            comboBox1.DataSource = t;

        }
        public void Cmb_attendance()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("ClassAttendance");
            SqlCommand cmd = new SqlCommand("Select ID,AttendanceDate From ClassAttendance", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            comboBox2.DisplayMember = "AttendanceDate";
            comboBox2.ValueMember = "ID";
            comboBox2.DataSource = t;
            Form2 Form = new Form2();
            Form.Refresh();

        }
        public void Cmb_attenstatus()
        {
            var Con = Configuration.getInstance().getConnection();
            DataTable t = new DataTable("Loookup");
            SqlCommand cmd = new SqlCommand("Select Lookupid,Name,Category From Lookup", Con);
            dr = cmd.ExecuteReader();
            t.Load(dr);
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "Lookupid";
            comboBox3.DataSource = t;

        }
        private void StudentAttendance_Load(object sender, EventArgs e)
        {
            Cmb_attenstatus();
            Cmb_attendance();
            Cmb_student();
            Form2 Form = new Form2();
            Form.Refresh();

        }





        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = Configuration.getInstance().getConnection())
                {
                    string query = "INSERT INTO ClassAttendance (AttendanceDate) VALUES (@AttendanceDate)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AttendanceDate", dateTimePicker1.Value);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Date added successfully.", "ClassAttendance");
                     
                        Form2 form = new Form2();
                        form.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("No rows were affected.", "ClassAttendance");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "ERROR");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = Configuration.getInstance().getConnection())
                {
                    if (!string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrEmpty(comboBox2.Text) && !string.IsNullOrEmpty(comboBox3.Text))
                    {
                        string query = "INSERT INTO StudentAttendance (AttendanceId, StudentId, AttendanceStatus) VALUES (@AttendanceId, @StudentId, @AttendanceStatus)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@AttendanceId", comboBox2.SelectedValue);
                        command.Parameters.AddWithValue("@StudentId", comboBox1.SelectedValue);
                        command.Parameters.AddWithValue("@AttendanceStatus", comboBox3.Text); 

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record added successfully.", "StudentAttendance");
                            data(); // Assuming data() is a method to refresh data after adding a new record
                        }
                        else
                        {
                            MessageBox.Show("No rows were affected.", "StudentAttendance");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please fill in all fields.", "ERROR");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "ERROR");
            }

        }
        public void data()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from StudentAttendance", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dataGridView1.DataSource = dta;
            getstudentID();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = Configuration.getInstance().getConnection())
                {
                    if (!string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrEmpty(comboBox2.Text) && !string.IsNullOrEmpty(comboBox3.Text))
                    {
                        string query = "UPDATE StudentAttendance SET AttendanceId = @AttendanceId, StudentId = @StudentId, AttendanceStatus = @AttendanceStatus WHERE AttendanceId = @OldAttendanceId";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@AttendanceId", comboBox2.SelectedValue);
                        command.Parameters.AddWithValue("@StudentId", comboBox1.SelectedValue);
                        command.Parameters.AddWithValue("@AttendanceStatus", comboBox3.SelectedValue);
                        command.Parameters.AddWithValue("@OldAttendanceId", comboBox2.SelectedValue);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record updated successfully.", "StudentAttendance");
                            data(); // Assuming data() is a method to refresh data after updating a record
                        }
                        else
                        {
                            MessageBox.Show("No rows were affected.", "StudentAttendance");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please fill in all fields.", "ERROR");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "ERROR");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = Configuration.getInstance().getConnection())
                {
                    if (!string.IsNullOrEmpty(comboBox1.Text))
                    {
                        string query = "DELETE FROM StudentAttendance WHERE StudentId = @StudentId";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@StudentId", comboBox1.SelectedValue);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully.", "StudentAttendance");
                            data(); // Assuming data() is a method to refresh data after deleting a record
                        }
                        else
                        {
                            MessageBox.Show("No rows were affected.", "StudentAttendance");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a Student ID.", "ERROR");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "ERROR");
            }

        }
        public static void PDF(string connectionString, string query)
        {
            // Connect to the database and retrieve data
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Create a new PDF document and add a page
                    Document document = new Document();
                    PdfWriter.GetInstance(document, new FileStream("Attendace.pdf", FileMode.Create));
                    document.Open();

                    // Add a table to the PDF document
                    PdfPTable table = new PdfPTable(reader.FieldCount);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 2f, 2f, 2f });

                    // Add headers to the table
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(reader.GetName(i)));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    // Add rows to the table
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            table.AddCell(reader[i].ToString());
                        }
                    }

                    // Add the table to the document
                    document.Add(table);

                    // Close the document and the reader
                    document.Close();
                    reader.Close();
                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {

            String connection = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";
            string query = "Select * from StudentAttendance";
            PDF(connection, query);
            MessageBox.Show("Pdf Successfully Created!!!");
        }
        private void getstudentID()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Student", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
           

            List<int> idList = new List<int>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0); 
                    idList.Add(id); 
                }
            }
            comboBox3.DataSource = idList;

            cmd.ExecuteNonQuery();
        }
        private void getAttendanceID()
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Student", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
         

            List<int> idList = new List<int>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0); 
                    idList.Add(id); 
                }
            }
            comboBox3.DataSource = idList;

            cmd.ExecuteNonQuery();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var Con = Configuration.getInstance().getConnection();

            try
            {

                SqlCommand cmd = new SqlCommand("select StudentAttendance.StudentId,StudentAttendance.AttendanceId,StudentAttendance.AttendanceStatus from StudentAttendance join Student on Student.Id = StudentAttendance.StudentId and StudentAttendance.StudentId = '" + textBox1.Text + "' and '" + comboBox4.Text + "' = StudentAttendance.AttendanceId", Con);
                SqlDataAdapter dta = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dta.Fill(dt);
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR"); };
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.CurrentRow.Index != -1)
            {

                comboBox2.SelectedValue = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                comboBox1.SelectedValue = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                comboBox3.SelectedValue = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form1 form = new Form1();
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            this.Hide();
            LOGIN form = new LOGIN();
            form.Show();
        }
    }
    }
