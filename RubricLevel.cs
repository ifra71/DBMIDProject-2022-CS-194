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
    public partial class RubricLevel : Form
    {
        public RubricLevel()
        {
            InitializeComponent();
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
        public void data()
        {
            var Con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from RubricLevel", Con);
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            DataTable dta = new DataTable();
            dt.Fill(dta);
            dataGridView1.DataSource = dta;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if textboxes are empty
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Please enter all details.");
                }
                else
                {
                    // Get database connection
                    using (var con = Configuration.getInstance().getConnection())
                    {
                        // Create SQL command to insert data
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO RubricLevel (RubricId, Details, MeasurementLevel) VALUES (@RubricId, @Details, @MeasurementLevel)", con))
                        {
                            // Add parameters
                            cmd.Parameters.AddWithValue("@RubricId", int.Parse(comboBox1.Text));
                            cmd.Parameters.AddWithValue("@Details", textBox1.Text);
                            cmd.Parameters.AddWithValue("@MeasurementLevel", textBox2.Text);

                            // Open connection and execute insert command
                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Data saved successfully.");
                                data(); // Refresh data
                            }
                            else
                            {
                                MessageBox.Show("Failed to save data.");
                            }
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid Rubric ID.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if ID is provided
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Please provide an ID to be deleted.");
                    return;
                }

                // Get database connection
                using (var con = Configuration.getInstance().getConnection())
                {
                    // Create SQL command to delete record
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM RubricLevel WHERE Id=@id";

                        // Add parameter for ID
                        cmd.Parameters.AddWithValue("@id", textBox2.Text);

                        // Open connection and execute delete command
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully.");
                            data(); // Refresh data
                        }
                        else
                        {
                            MessageBox.Show("No record found with the provided ID.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Please provide an ID to edit.");
                    return;
                }

                if (!int.TryParse(comboBox1.Text, out int rubricId))
                {
                    MessageBox.Show("Invalid Rubric ID.");
                    return;
                }

                using (var con = Configuration.getInstance().getConnection())
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "UPDATE RubricLevel SET Details=@Details, MeasurementLevel=@MeasurementLevel WHERE RubricId=@RubricId";

                        // Add parameters
                        cmd.Parameters.AddWithValue("@Details", textBox1.Text);
                        cmd.Parameters.AddWithValue("@MeasurementLevel", textBox2.Text);
                        cmd.Parameters.AddWithValue("@RubricId", rubricId);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record has been successfully edited.");
                            data(); // Refresh data
                        }
                        else
                        {
                            MessageBox.Show("No record found with the provided ID.");
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid Rubric ID.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            
                String connection = @"Data Source=IFRA-FAZAL;Initial Catalog=ProjectB;Integrated Security=True";
                string query = "Select * from RubricLevel";
                PdfReport.PDF(connection, query);
                MessageBox.Show("Pdf Successfully Created!!!");
            }

        private void RubricLevel_Load(object sender, EventArgs e)
        {

        }
    }
    }
