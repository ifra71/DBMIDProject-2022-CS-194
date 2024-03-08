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

namespace DB1
{
    public partial class Assessment : Form
    {
        public Assessment()
        {
            InitializeComponent();
            data();
            this.Refresh();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Please enter all details!");
                }
                else
                {
                   
                    using (var con = Configuration.getInstance().getConnection())
                    {
                        
                        string insertQuery = "INSERT INTO Assessment (Title, DateCreated, TotalMarks, TotalWeightage) " +
                                             "VALUES (@Title, @DateCreated, @TotalMarks, @TotalWeightage)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                         
                            cmd.Parameters.AddWithValue("@Title", textBox1.Text);
                            cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);

                          
                            if (int.TryParse(textBox2.Text, out int totalMarks))
                            {
                                cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
                            }
                            else
                            {
                                MessageBox.Show("Invalid value for TotalMarks. Please enter a valid integer.");
                                return;
                            }

                            if (int.TryParse(textBox3.Text, out int totalWeightage))
                            {
                                cmd.Parameters.AddWithValue("@TotalWeightage", totalWeightage);
                            }
                            else
                            {
                                MessageBox.Show("Invalid value for TotalWeightage. Please enter a valid integer.");
                                return;
                            }

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully saved");

                            this.Refresh();
                            data();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }
        public void data()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Assessment", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dataGridView1.DataSource = dta;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if Title field is provided
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Please provide a title to be edited.");
                    return;
                }

                // Get a database connection
                using (var con = Configuration.getInstance().getConnection())
                {
                    // Prepare SQL command for updating record
                    string updateQuery = "UPDATE Assessment SET DateCreated=@DateCreated, TotalMarks=@TotalMarks, TotalWeightage=@TotalWeightage WHERE Title = @Title";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                    {
                        // Set parameters for the SQL command
                        cmd.Parameters.AddWithValue("@Title", textBox1.Text);
                        cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);

                        // Parse and set TotalMarks, handle parse errors gracefully
                        if (int.TryParse(textBox2.Text, out int totalMarks))
                        {
                            cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
                        }
                        else
                        {
                            MessageBox.Show("Invalid value for TotalMarks. Please enter a valid integer.");
                            return;
                        }

                        // Parse and set TotalWeightage, handle parse errors gracefully
                        if (int.TryParse(textBox3.Text, out int totalWeightage))
                        {
                            cmd.Parameters.AddWithValue("@TotalWeightage", totalWeightage);
                        }
                        else
                        {
                            MessageBox.Show("Invalid value for TotalWeightage. Please enter a valid integer.");
                            return;
                        }

                        // Execute SQL command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record has been successfully edited.");
                            // Refresh UI and update data
                            this.Refresh();
                            data();
                        }
                        else
                        {
                            MessageBox.Show("No record found with the provided title.");
                        }
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
                    MessageBox.Show("Please provide a title to be deleted.");
                    return;
                }

                
                using (var con = Configuration.getInstance().getConnection())
                {
                   
                    string deleteQuery = "DELETE FROM Assessment WHERE Title = @Title";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                    {
                        
                        cmd.Parameters.AddWithValue("@Title", textBox1.Text);

                     
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record has been deleted successfully.");
                           
                            this.Refresh();
                            data();
                        }
                        else
                        {
                            MessageBox.Show("No record found with the provided title.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String connection = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";
            string query = "Select * from Assessment";
            PdfReport.PDF(connection, query);
            MessageBox.Show("Pdf Successfully Created!!!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            LOGIN form = new LOGIN();
            form.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form = new Form1();
            form.Show();
        }
    }
}
