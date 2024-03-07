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

namespace DB1
{
    public partial class STUDENT : Form
    {
        public STUDENT()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var connection = Configuration.getInstance().getConnection();

            try
            {
                if (IsValidInput())
                {
                    string query = "INSERT INTO Student (FirstName, LastName, Contact, Email, RegistrationNumber, Status) " +
                                   "VALUES (@FirstName, @LastName, @Contact, @Email, @RegistrationNumber, @Status)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //command.Parameters.AddWithValue("@FirstName", textBox1.Text);
                        //command.Parameters.AddWithValue("@LastName", textBox3.Text);
                        //command.Parameters.AddWithValue("@Contact", textBox5.Text);
                        //command.Parameters.AddWithValue("@Email", textBox4.Text);
                        //command.Parameters.AddWithValue("@RegistrationNumber", textBox1.Text);
                        //command.Parameters.AddWithValue("@Status", comboBox1.SelectedValue);
                        command.Parameters.AddWithValue("@FirstName", textBox1.Text);
                        command.Parameters.AddWithValue("@LastName", textBox2.Text);
                        command.Parameters.AddWithValue("@Contact", textBox4.Text);
                        command.Parameters.AddWithValue("@Email", textBox5.Text);
                        command.Parameters.AddWithValue("@RegistrationNumber", textBox3.Text);
                        command.Parameters.AddWithValue("@Status", comboBox1.SelectedValue);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student added successfully", "Success");
                            RefreshData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add student", "Error");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter valid data", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
            finally
            {
                connection.Close();
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
                    string selectQuery = "SELECT * FROM Student";
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
        private bool IsValidInput()
        {
            return textBox1.Text.Length >= 5 && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                   !string.IsNullOrWhiteSpace(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                   textBox4.Text.Contains("@gmail.com");
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
            string query = "Select * from Student";
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

        private void button2_Click(object sender, EventArgs e)
        {

            var connection = Configuration.getInstance().getConnection();
            try
            {
                if (IsValidRegistrationNumber(textBox3.Text))
                {
                    string query = "DELETE FROM Student WHERE RegistrationNumber = @RegistrationNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RegistrationNumber", textBox3.Text);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully", "Success");
                           data(); 
                        }
                        else
                        {
                            MessageBox.Show("No record found with the provided registration number", "Error");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid registration number", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
            finally
            {
                connection.Close();
            }
        }

        private bool IsValidRegistrationNumber(string registrationNumber)
        {
            return registrationNumber.Length >= 9 && !string.IsNullOrWhiteSpace(registrationNumber);
        }
        public void data()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Student", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dataGridView1.DataSource = dta;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var connection = Configuration.getInstance().getConnection();

            try
            {
                if (IsValidInputmail())
                {
                    string query = "UPDATE Student SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, " +
                                   "Email = @Email, RegistrationNumber = @RegistrationNumber, Status = @Status " +
                                   "WHERE RegistrationNumber = @RegistrationNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", textBox1.Text);
                        command.Parameters.AddWithValue("@LastName", textBox2.Text);
                        command.Parameters.AddWithValue("@Contact", textBox4.Text);
                        command.Parameters.AddWithValue("@Email", textBox5.Text);
                        command.Parameters.AddWithValue("@RegistrationNumber", textBox3.Text);
                        command.Parameters.AddWithValue("@Status", comboBox1.SelectedValue);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student updated successfully", "Success");
                            data(); // Assuming this method refreshes the data display
                        }
                        else
                        {
                            MessageBox.Show("Failed to update student", "Error");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter valid data", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
            finally
            {
                connection.Close();
            }
        }

        private bool IsValidInputmail()
        {
            return textBox1.Text.Length >= 9 && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                   !string.IsNullOrWhiteSpace(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                   textBox4.Text.Contains("@gmail.com");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var connection = Configuration.getInstance().getConnection();

            try
            {
                if (!string.IsNullOrWhiteSpace(textBox6.Text))
                {
                    string query = "SELECT * FROM Student WHERE RegistrationNumber LIKE @RegistrationNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RegistrationNumber", textBox3.Text + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dataTable;
                        }
                        else
                        {
                            MessageBox.Show("No records found", "Information");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a registration number", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
            finally
            {
                connection.Close();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void STUDENT_Load(object sender, EventArgs e)
        {

        }
    }
    }

   