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
using eShiftCompany.Business.Interface;
using eShiftCompany.Business.Services;
using eShiftCompany.Repository.Services;
using eShiftCompany.Models;
using eShiftCompany.Repository.Interface;

namespace eShiftCompany
{
    public partial class EmployeeReg: Form
    {
        private IEmployeeRegService _employeeService;

        public EmployeeReg()
        {
            InitializeComponent();
            _employeeService = new EmployeeRegService(new EmployeeRegRepository());
        }

        private void LoadEmployeeIDs()
        {
            cbEmployeeID.Items.Clear();
            var employees = _employeeService.GetAllEmployees();
            foreach (var emp in employees)
            {
                cbEmployeeID.Items.Add(emp.EmployeeID.ToString());
            }
        }
        private void LoadEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            dgvEmployees.DataSource = employees;
        }

        private void EmployeeReg_Load(object sender, EventArgs e)
        {
            cbRole.Items.AddRange(new string[] { "Driver", "Assistant" });
            LoadEmployees();
            LoadEmployeeIDs();
        }

        private void lblAdd_Click(object sender, EventArgs e)
        {
            var gender = rbMale.Checked ? "Male" : "Female";

            var employee = new EmployeeRegModel
            {
                Name = txtName.Text,
                NIC = txtNic.Text,
                Contact = txtContact.Text,
                Age = int.Parse(txtAge.Text),
                Gender = gender,
                Role = cbRole.Text,
                Email = txtEmail.Text
            };

            _employeeService.AddEmployee(employee);
            MessageBox.Show("Employee Added Successfully");

            LoadEmployees();
            LoadEmployeeIDs();
            ClearFields();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbEmployeeID.Text))
            {
                MessageBox.Show("Select Employee ID to Update.");
                return;
            }

            var gender = rbMale.Checked ? "Male" : "Female";

            var employee = new EmployeeRegModel
            {
                EmployeeID = int.Parse(cbEmployeeID.Text),
                Name = txtName.Text,
                NIC = txtNic.Text,
                Contact = txtContact.Text,
                Age = int.Parse(txtAge.Text),
                Gender = gender,
                Role = cbRole.Text,
                Email = txtEmail.Text
            };

            _employeeService.UpdateEmployee(employee);
            MessageBox.Show("Employee Updated Successfully");

            LoadEmployees();
            ClearFields();
        }
        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbEmployeeID.Text))
            {
                MessageBox.Show("Select Employee ID to Delete.");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                int id = int.Parse(cbEmployeeID.Text);
                _employeeService.DeleteEmployee(id);
                MessageBox.Show("Employee Deleted Successfully");

                LoadEmployees();
                LoadEmployeeIDs();
                ClearFields();
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {
            cbEmployeeID.Text = "";
            txtName.Clear();
            txtNic.Clear();
            txtContact.Clear();
            txtAge.Clear();
            txtEmail.Clear();
            cbRole.SelectedIndex = -1;
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbEmployeeID.Text))
            {
                MessageBox.Show("Select Employee ID to Search.");
                return;
            }

            int id = int.Parse(cbEmployeeID.Text);
            var emp = _employeeService.GetEmployeeById(id);

            if (emp != null)
            {
                txtName.Text = emp.Name;
                txtNic.Text = emp.NIC;
                txtContact.Text = emp.Contact;
                txtAge.Text = emp.Age.ToString();
                cbRole.Text = emp.Role;
                txtEmail.Text = emp.Email;
                if (emp.Gender == "Male")
                    rbMale.Checked = true;
                else
                    rbFemale.Checked = true;
            }
            else
            {
                MessageBox.Show("Employee Not Found.");
            }
        }

        private void employeeRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeeReg employeeReg = new EmployeeReg();
            employeeReg.Show();
            
        }

        private void transportUnitsManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ITransportUnitManagementRepository repo = new TransportUnitManagementRepository();
            ITransportUnitManagementService service = new TransportUnitManagementService(repo);

            // Inject into form
            TransportUnitManagement form = new TransportUnitManagement(service);
            form.Show();
            this.Hide();
        }

        private void loadManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILoadManagementService service = new LoadManagementService();
            LoadManagement loadForm = new LoadManagement(service);
            loadForm.Show();
        }

        private void adminManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
            this.Hide();
        }

        private void manageStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IManageStatusRepository repository = new ManageStatusRepository();
            IManageStatusService service = new ManageStatusService(repository);
            ManageStatus manageStatusForm = new ManageStatus(service);
            manageStatusForm.Show();
            this.Hide();
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IReportsRepository repo = new ReportsRepository();
            IReportsService service = new ReportsService(repo);
            Reports reportsForm = new Reports(service);
            reportsForm.Show();
            this.Hide();
        }

        private void productManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IProductManagementRepository repository = new ProductManagementRepository();
            IProductManagementService service = new ProductManagementService(repository);
            ProductManagement productForm = new ProductManagement(service);
            productForm.Show();
            this.Hide();
        }

        private void customerManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SignUp adminSignup = new SignUp("admin");
            adminSignup.Show();
            this.Hide();
        }

        private void linkLabellogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }
    }
}
