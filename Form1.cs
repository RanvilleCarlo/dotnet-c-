using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace dotnet
{
    public partial class Form1 : Form
    {
        private DataGridView bankAccountsGrid;
        private TextBox txtOwnerName;
        private TextBox txtAmount;
        private Button btnCreateAccount;
        private Button btnDeposit;
        private Button btnWithdraw;
        private Label lblStatus;
        private List<BankAccount> bankAccounts;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
            bankAccounts = new List<BankAccount>(); // Initialize list
        }

        private void SetupUI()
        {
            // Form Styling
            this.Text = "Bank Account Manager";
            this.Size = new Size(550, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240); // Light Gray Background

            // Label for Owner Name
            Label lblOwnerName = new Label { Text = "OwnerName:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            Controls.Add(lblOwnerName);

            // TextBox for Owner Name
            txtOwnerName = new TextBox { Location = new Point(130, 18), Width = 300, Font = new Font("Arial", 10) };
            Controls.Add(txtOwnerName);

            // Label for Amount
            Label lblAmount = new Label { Text = "Amount:", Location = new Point(20, 50), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            Controls.Add(lblAmount);

            // TextBox for Amount
            txtAmount = new TextBox { Location = new Point(130, 48), Width = 100, Font = new Font("Arial", 10) };
            Controls.Add(txtAmount);

            // Create Account Button
            btnCreateAccount = CreateButton("Create Account", new Point(20, 80), new Size(150, 40));
            btnCreateAccount.Click += BtnCreateAccount_Click;
            Controls.Add(btnCreateAccount);

            // Deposit Money Button
            btnDeposit = CreateButton("Deposit", new Point(180, 80), new Size(100, 40));
            btnDeposit.Click += BtnDeposit_Click;
            Controls.Add(btnDeposit);

            // Withdraw Money Button
            btnWithdraw = CreateButton("Withdraw", new Point(290, 80), new Size(100, 40));
            btnWithdraw.Click += BtnWithdraw_Click;
            Controls.Add(btnWithdraw);

            // DataGridView to Display Accounts
            bankAccountsGrid = new DataGridView
            {
                Location = new Point(20, 130),
                Size = new Size(500, 180),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                BackgroundColor = Color.White,
                GridColor = Color.LightGray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, // Allow row selection
                MultiSelect = false // Only one row can be selected at a time
            };
            Controls.Add(bankAccountsGrid);

            // Status Label
            lblStatus = new Label { Location = new Point(20, 320), Width = 500, ForeColor = Color.DarkRed, Font = new Font("Arial", 10, FontStyle.Italic) };
            Controls.Add(lblStatus);
        }

        private Button CreateButton(string text, Point location, Size size)
        {
            return new Button
            {
                Text = text,
                Location = location,
                Size = size,
                BackColor = Color.FromArgb(60, 120, 215), // Blue Button
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        private void BtnCreateAccount_Click(object sender, EventArgs e)
        {
            string owner = txtOwnerName.Text.Trim();
            if (string.IsNullOrEmpty(owner))
            {
                lblStatus.Text = "Owner name cannot be empty!";
                return;
            }

            BankAccount newAccount = new BankAccount
            {
                Owner = owner,
                AccountNumber = Guid.NewGuid(),
                Balance = 0
            };

            bankAccounts.Add(newAccount);
            UpdateGrid();
            lblStatus.Text = $"Account created for {owner}.";
        }

        private void BtnDeposit_Click(object sender, EventArgs e)
        {
            if (bankAccountsGrid.SelectedRows.Count == 0)
            {
                lblStatus.Text = "Select an account to deposit money.";
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                lblStatus.Text = "Enter a valid deposit amount!";
                return;
            }

            int index = bankAccountsGrid.SelectedRows[0].Index;
            bankAccounts[index].Balance += amount;
            UpdateGrid();
            lblStatus.Text = $"Deposited {amount:C} to {bankAccounts[index].Owner}'s account.";
        }

        private void BtnWithdraw_Click(object sender, EventArgs e)
        {
            if (bankAccountsGrid.SelectedRows.Count == 0)
            {
                lblStatus.Text = "Select an account to withdraw money.";
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                lblStatus.Text = "Enter a valid withdrawal amount!";
                return;
            }

            int index = bankAccountsGrid.SelectedRows[0].Index;
            if (bankAccounts[index].Balance < amount)
            {
                lblStatus.Text = "Insufficient balance!";
                return;
            }

            bankAccounts[index].Balance -= amount;
            UpdateGrid();
            lblStatus.Text = $"Withdrew {amount:C} from {bankAccounts[index].Owner}'s account.";
        }

        private void UpdateGrid()
        {
            bankAccountsGrid.DataSource = null;
            bankAccountsGrid.DataSource = bankAccounts;
        }
    }

    public class BankAccount
    {
        public string Owner { get; set; }
        public Guid AccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
