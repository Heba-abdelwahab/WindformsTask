using DevExpress.Pdf.Native.BouncyCastle.Asn1.Ocsp;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Models;
using WinFormsApp1.Repositories;

namespace WinFormsApp1
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        DepartmentRepository departmentRepository;
        EmployeeRepository employeeRepository;
        TreeNodeData CurrentTreeListNode;
        public XtraForm1()
        {
            InitializeComponent();
            CompanyContext companyContext = new CompanyContext();
            departmentRepository = new DepartmentRepository(companyContext);
            employeeRepository = new EmployeeRepository(companyContext);

        }

        private void LoadDataToCobmoBox(System.Windows.Forms.ComboBox comboBox)
        {
            comboBox.DataSource = departmentRepository.GetAllDepartments();
            comboBox.DisplayMember = "Name";
            comboBox.ValueMember = "Id";
        }
        private void CnfigTreelist()
        {
            treeList1.KeyFieldName = "Id";
            treeList1.ParentFieldName = "ParentId";
            treeList1.Columns.Add().Caption = "Name";
            treeList1.Columns[0].Visible = true;
            treeList1.Columns[1].Visible = false;
            treeList1.Columns[2].Visible = false;
            treeList1.Columns[3].Visible = false;
        }
        private void LoadDataToTreelist()
        {
            List<Department> depts = departmentRepository.GetAllDepartmentsWithEmp();
            List<TreeNodeData> treeListData = new List<TreeNodeData>();

            foreach (Department dept in depts)
            {
                treeListData.Add(new TreeNodeData()
                {
                    Id = 'D' + dept.Id.ToString(),
                    IsEmployee = false,
                    Name = dept.Name,
                    ParentId = null,
                    DepartmentParentDetails = dept,

                });

                foreach (Employee emp in dept.Employees)
                {
                    treeListData.Add(new TreeNodeData()
                    {
                        Id = 'E' + emp.Id.ToString(),
                        IsEmployee = true,
                        Name = emp.Name,
                        ParentId = 'D' + dept.Id.ToString(),
                        EmployeeDetails = emp

                    });

                }

            }
            treeList1.DataSource = treeListData;
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

            TreeNodeData selectedNode = (TreeNodeData)treeList1.GetDataRecordByNode(e.Node);
            CurrentTreeListNode = selectedNode;
            if (selectedNode != null && selectedNode.IsEmployee)
            {
                DisplayEmployeeDetails(selectedNode.EmployeeDetails);
                Department department = departmentRepository.GetById(int.Parse(selectedNode.ParentId.Substring(1)));
                DisplayDepartmentDetails(department);
            }
            if (selectedNode != null && selectedNode.IsEmployee == false)
            {
                EmptyEmployeeDetails();
                DisplayDepartmentDetails(selectedNode.DepartmentParentDetails);
            }
        }
        private void DisplayEmployeeDetails(Employee emp)
        {
            EmpNameTxtbox2.Text = emp.Name;
            EmpPhoneTxtbox2.Text = emp.Phone;
            EmpAddTxtbox2.Text = emp.Address;
            var dept = EmpDeptcomboBox2.Items.Cast<Department>().FirstOrDefault(dept => dept.Id == emp.DepartmentId);
            EmpDeptcomboBox2.SelectedIndex = EmpDeptcomboBox2.Items.IndexOf(dept);


        }
        private void EmptyEmployeeDetails()
        {
            EmpNameTxtbox2.Text = "";
            EmpPhoneTxtbox2.Text = "";
            EmpAddTxtbox2.Text = "";
            EmpDeptcomboBox2.SelectedIndex = -1;


        }
        private void DisplayDepartmentDetails(Department dept)
        {
            DeptTxtBox2.Text = dept.Name;

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage1;

            LoadDataToCobmoBox(EmpDeptComboBox);


        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage2;

            LoadDataToTreelist();
            CnfigTreelist();
            LoadDataToCobmoBox(EmpDeptcomboBox2);
        }



        private void XtraForm1_Load(object sender, EventArgs e)
        {
            LoadDataToCobmoBox(EmpDeptComboBox);
        }

        private void EmpSaveBtn_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee()
            {
                Address = EmpAddressTxtbox.Text,
                DepartmentId = (int)EmpDeptComboBox.SelectedValue,
                Name = EmpNameTxtbox.Text,
                Phone = EmpPhoneTxtBox.Text,
            };
            employeeRepository.Add(employee);
            employeeRepository.SaveChanges();
            EmpSaveMeg.Text = "Save Done";
            EmpAddressTxtbox.Text = "";
            EmpNameTxtbox.Text = "";
            EmpPhoneTxtBox.Text = "";
            EmpDeptComboBox.SelectedIndex = 0;

        }

        private void DeptSaveBtn_Click(object sender, EventArgs e)
        {
            Department department = new Department()
            {
                Name = DeptNameTxtbox.Text,
            };
            departmentRepository.Add(department);
            departmentRepository.SaveChanges();
            DeptSaveMsg.Text = "save done";
            DeptNameTxtbox.Text = "";
        }

        private void updateNameInTreelist(string id, string name)
        {
            TreeListNode node = treeList1.FindNodeByKeyID(id);
            node.SetValue(0, name);
        }
        private void DeleteNodeFromTreelist(string id)
        {
            var node = treeList1.FindNodeByKeyID(id);
            treeList1.DeleteNode(node);
        }
        private void DeptUpdateBtn_Click(object sender, EventArgs e)
        {
            int deptid;
            if (CurrentTreeListNode == null)
            {
                DeptUpdateDeleteMsh.Text = "No Dept Was Selected";
                return;
            }

            if (DeptTxtBox2.Text == "")
            {
                DeptUpdateDeleteMsh.Text = "you must enter name";
                return;
            }

            if (CurrentTreeListNode.IsEmployee == true)

                deptid = int.Parse(CurrentTreeListNode.ParentId.Substring(1));
            else
                deptid = CurrentTreeListNode.DepartmentParentDetails.Id;

            departmentRepository.Update(deptid, new Department() { Name = DeptTxtBox2.Text });
            departmentRepository.SaveChanges();

            string id = 'D' + deptid.ToString();
            updateNameInTreelist(id, DeptTxtBox2.Text);

        }

        private void DeteleBtn_Click(object sender, EventArgs e)
        {
            int deptid;
            if (CurrentTreeListNode == null)
            {
                DeptUpdateDeleteMsh.Text = "No Dept Was Selected";
                return;
            }
            if (CurrentTreeListNode.IsEmployee == true)

                deptid = int.Parse(CurrentTreeListNode.ParentId.Substring(1));
            else
                deptid = CurrentTreeListNode.DepartmentParentDetails.Id;

            departmentRepository.Delete(deptid);
            departmentRepository.SaveChanges();


            string id = 'D' + deptid.ToString();
            DeleteNodeFromTreelist(id);
        }

        private void MoveChildToAnotherParent(string parentid, string childid)
        {
            TreeListNode ParentNode = treeList1.FindNodeByKeyID(parentid);
            TreeListNode ChildNode = treeList1.FindNodeByKeyID(childid);
            ParentNode.TreeList.MoveNode(ChildNode, ParentNode);
        }
        private void EmpUpdate_Click(object sender, EventArgs e)
        {
            if (CurrentTreeListNode.IsEmployee == false)
            {
                EmpUpdateDeleteMsg.Text = "No Employee was selected";
                return;
            }

            int employeeid = CurrentTreeListNode.EmployeeDetails.Id;
            int oldEmpDeptId = CurrentTreeListNode.EmployeeDetails.DepartmentId;
            Employee UpdateEmp = new Employee()
            {
                Id = employeeid,
                Address = EmpAddTxtbox2.Text,
                Name = EmpNameTxtbox2.Text,
                Phone = EmpPhoneTxtbox2.Text,
                DepartmentId = (int)EmpDeptcomboBox2.SelectedValue,
            };
            employeeRepository.Update(employeeid, UpdateEmp);
            employeeRepository.SaveChanges();

            string id = 'E' + employeeid.ToString();
            updateNameInTreelist(id, EmpNameTxtbox2.Text);


            if (oldEmpDeptId != UpdateEmp.DepartmentId)
                MoveChildToAnotherParent('D' + UpdateEmp.DepartmentId.ToString(), 'E' + employeeid.ToString());

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (CurrentTreeListNode.IsEmployee == false)
            {
                EmpUpdateDeleteMsg.Text = "No Employee was selected";
                return;
            }

            int employeeid = CurrentTreeListNode.EmployeeDetails.Id;
            employeeRepository.Delete(employeeid);
            employeeRepository.SaveChanges();

            string id = 'E' + employeeid.ToString();
            DeleteNodeFromTreelist(id);

            EmptyEmployeeDetails();
        }

        private void treeList1_AfterExpand(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            TreeNodeData selectedNode = (TreeNodeData)treeList1.GetDataRecordByNode(e.Node);
            CurrentTreeListNode = selectedNode;
            if (selectedNode != null && selectedNode.IsEmployee == false)
            {
                EmptyEmployeeDetails();
                DisplayDepartmentDetails(selectedNode.DepartmentParentDetails);
            }
        }
    }
}