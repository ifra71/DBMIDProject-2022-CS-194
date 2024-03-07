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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Xml.Linq;

namespace DB1
{
    public partial class CLO : Form
    {
        public CLO()
        {
            InitializeComponent();
        }

        private void CLO_Load(object sender, EventArgs e)
        {

        }



        
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Please enter the details.");
                    return;
                }

                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    string insertQuery = "INSERT INTO CLO (Name, DateCreated, DateUpdated) VALUES (@Name, @DateCreated, @DateUpdated)";
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@DateUpdated", dateTimePicker2.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data saved successfully.");
                        RefreshData();
                        data();

                    }
                    else
                    {
                        MessageBox.Show("Failed to save data.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }
      
        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Please provide a name to edit.");
                    return;
                }

                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE CLO SET DateCreated=@DateCreated, DateUpdated=@DateUpdated WHERE Name = @name", con);

                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@DateUpdated", dateTimePicker2.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record has been successfully edited.");
                        RefreshData();
                        data();

                    }
                    else
                    {
                        MessageBox.Show("No record found for the provided name.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Please provide a CLO to be deleted.");
                    return;
                }

                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM CLO WHERE Name=@name", con);
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Deleted successfully.");
                        RefreshData();
                        data();
                    }
                    else
                    {
                        MessageBox.Show("No record found for the provided CLO.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void RefreshData()
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    con.Open();

                    // Assuming you have a DataGridView named dataGridView1
                    string selectQuery = "SELECT * FROM CLO";
                    SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, con);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while refreshing data: {ex.Message}");
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


        private void button6_Click(object sender, EventArgs e)
        {

            String connection = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";
            string query = "Select * from Clo";
            PDF(connection, query);
            MessageBox.Show("Pdf Successfully Created!!!");
        }
        public static void PDF(string connectionString, string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            throw new Exception("No data returned from the query.");
                        }

                        // Create a new PDF document and add a page
                        Document document = new Document();
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("Student.pdf", FileMode.Create));
                        document.Open();

                        // Add a table to the PDF document
                        PdfPTable table = new PdfPTable(reader.FieldCount);
                        table.WidthPercentage = 100;

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

                        // Close the document and the writer
                        document.Close();
                        writer.Close();
                    }
                }

                MessageBox.Show("PDF successfully created: Student.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF: {ex.Message}");
            }
        }
        public void data()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from CLO", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dataGridView1.DataSource = dta;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    

