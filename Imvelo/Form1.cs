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

namespace Imvelo
{
    public partial class AdminDashboardForm : Form
    {
        private String connectionString = "Server=tcp:imvelo.database.windows.net,1433;Initial Catalog=Imvelo Database;Persist Security Info=False;User ID=imvelo;Password=Group14@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public AdminDashboardForm()
        {
            InitializeComponent();
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            lblWelcomeAdmin.Text = "Hi, Admin";
            LoadDashboardData();
            LoadRecentBookings();

        }
        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Total Tourists
                    string touristQuery = "SELECT COUNT(*) FROM TOURIST";

                    using (SqlCommand cmd = new SqlCommand(touristQuery, conn))
                    {
                        int totalTourists = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTotalTouristsNum.Text = totalTourists.ToString();
                    }

                    // Total Bookings
                    string bookingQuery = "SELECT COUNT(*) FROM BOOKING";

                    using (SqlCommand cmd = new SqlCommand(bookingQuery, conn))
                    {
                        int totalBookings = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTotalBookingsNum.Text = totalBookings.ToString();
                    }
                    // Total Accommodations
                    string accommodationQuery =
                        "SELECT COUNT(*) FROM ACCOMMODATION";

                    using (SqlCommand cmd =
                        new SqlCommand(accommodationQuery, conn))
                    {
                        int totalAccommodations =
                            Convert.ToInt32(cmd.ExecuteScalar());

                        lblTotalAccommodationsNum.Text =
                            totalAccommodations.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading dashboard data:\n" + ex.Message,
                    "Dashboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadRecentBookings()
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT TOP 5
                            B.booking_id AS [Booking ID],
                            T.full_name AS [Tourist],
                            A.name AS [Accommodation],
                            B.check_in_date AS [Check-in Date],
                            B.check_out_date AS [Check-out Date],
                            B.status AS [Status]
                        FROM BOOKING B
                        INNER JOIN TOURIST T
                            ON B.tourist_id = T.tourist_id
                        INNER JOIN ACCOMMODATION A
                            ON B.accommodation_id = A.accommodation_id
                        ORDER BY B.booking_date DESC";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(query, conn))
                    {
                        DataTable table = new DataTable();

                        adapter.Fill(table);

                        dgvRecentBookingsAdminform.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading recent bookings:\n" + ex.Message,
                    "Dashboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void lblWelcomeAdmin_Click(object sender, EventArgs e)
        {

        }
    }
}

