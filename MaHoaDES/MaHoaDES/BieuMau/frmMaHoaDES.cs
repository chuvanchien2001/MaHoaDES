using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MaHoaDES.DoiTuong;
using AOC.ThuVien;
 
using System.Threading;
using System.Numerics;

namespace MaHoaDES.BieuMau
{
    public partial class frmMaHoaDES : Form
    {
        int MaHoaHayGiaiMa = 1;
        int dong = -1;
        bool FileHayChuoi = false;
        DES64Bit MaHoaDES64;
        Khoa Khoa;
        Thread thread = null;
        DataTable dsK;
        DataTable dsA;
        DataTable dsKP;
        DataTable dsDC;
        public static string TenTienTrinh = "";
        public static int GiaiDoan = -1;
        private static int Dem = 0;
        private static List<long> ds = new List<long>();
        private static List<long> dsg = new List<long>();
        private static List<DuocChon> dsdc = new List<DuocChon>();

        public frmMaHoaDES()
        {
            InitializeComponent();
        }
        private void EnableHoacDisableNut(bool b)
        {
            btnChonFile.Enabled = b;
            btnGiaiMaFile.Enabled = b;
            btnGiaiMaVanBan.Enabled = b;
            btnMaHoaFile.Enabled = b;
            btnMaHoaVanBan.Enabled = b;
        }
        private void Chay()
        {
            thread = new Thread(new ThreadStart(MaHoa));
            thread.IsBackground = true;
            timer1.Enabled = true;
            thread.Start();
        }

        private void MaHoa()
        {

            MaHoaDES64 = new DES64Bit();
            

            GiaiDoan = 0;
            Dem = 0;

            if (FileHayChuoi)
            {
                Khoa = new Khoa(txtKhoaFile.Text);
                if (MaHoaHayGiaiMa == 1)
                {
                    GiaiDoan = 0;
                    ChuoiNhiPhan chuoi = DocFileTxt.FileReadToBinary(txtFileNguon.Text);
                    GiaiDoan = 1;
                    ChuoiNhiPhan KQ = MaHoaDES64.ThucHienDES(Khoa, chuoi, 1);
                    GiaiDoan = 2;
                    DocFileTxt.WriteBinaryToFile(txtFileDich.Text, KQ);
                    GiaiDoan = 3;
                    MessageBox.Show("Mã hóa file thành công!", "Thành công");
                }
                else
                {
                    GiaiDoan = 0;
                    ChuoiNhiPhan chuoi = DocFileTxt.FileReadToBinary(txtFileNguon.Text);
                    GiaiDoan = 1;
                    ChuoiNhiPhan KQ = MaHoaDES64.ThucHienDES(Khoa, chuoi, -1);
                    if (KQ == null)
                    {
                        MessageBox.Show("Lỗi giải mã . kiểm tra khóa!", "Lỗi");
                        timer1.Enabled = false;
                        return;
                    }
                    GiaiDoan = 2;
                    DocFileTxt.WriteBinaryToFile(txtFileDich.Text, KQ);
                    GiaiDoan = 3;
                    MessageBox.Show("Giải mã file thành công!", "Thành công");
                }
            }
            else
            {
                Khoa = new Khoa(txtKhoaVanBan.Text);
                if (MaHoaHayGiaiMa == 1)
                {

                    MaHoaDES64 = new DES64Bit();
                    GiaiDoan = 0;
                    GiaiDoan = 1;
                    string kq = MaHoaDES64.ThucHienDESText(Khoa, txtVanBanNguon.Text, 1);
                    txtVanBanDich.Text = kq;
                    GiaiDoan = 2;
                    GiaiDoan = 3;
                    MessageBox.Show("Mã hóa chuỗi thành công!", "Thành công");
                }
                else
                {
                    MaHoaDES64 = new DES64Bit();
                    GiaiDoan = 0;
                    GiaiDoan = 1;
                    string kq = MaHoaDES64.ThucHienDESText(Khoa, txtVanBanNguon.Text, -1);
                    txtVanBanDich.Text = kq;
                    if (kq == "")
                    {
                        return;
                    }
                    GiaiDoan = 2;
                    GiaiDoan = 3;
                    MessageBox.Show("Giải mã chuỗi thành công!", "Thành công");
                }
            }
            
            timer1.Enabled = false;
        }

        private void txtMaHoaVanBan_Click(object sender, EventArgs e)
        {
            FileHayChuoi = false;
            MaHoaHayGiaiMa = 1;
            ClearProgressBar();
            MaHoa();
            EnableHoacDisableNut(true);
        }

        private void txtGiaiMaVanBan_Click(object sender, EventArgs e)
        {

            FileHayChuoi = false;
            MaHoaHayGiaiMa = -1;
            ClearProgressBar();
            MaHoa();
            EnableHoacDisableNut(true);
        }

        private void btnMaHoaFile_Click(object sender, EventArgs e)
        {
            FileHayChuoi = true;
            MaHoaHayGiaiMa = 1;
            ClearProgressBar();
            Chay();
            EnableHoacDisableNut(true);
        }

        private void btnGiaiMaFile_Click(object sender, EventArgs e)
        {
            FileHayChuoi = true;
            MaHoaHayGiaiMa = -1;
            ClearProgressBar();
            Chay();
            EnableHoacDisableNut(true);
        }
        private void btnChonFile_Click(object sender, EventArgs e)
        {
            txtFileNguon.Clear();
            txtFileDich.Clear();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileNguon.Text = openFileDialog1.FileName;
                txtFileDich.Text = openFileDialog1.FileName.Replace(".", "_2.");
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {


            if (GiaiDoan != -1)
                Dem++;
            if (GiaiDoan == 0)
            {

                if (Dem > 200)
                    Dem = 200;
            }
            else if (GiaiDoan == 1)
            {
                if (Dem < 200)
                    Dem = 200;
                if (Dem > 600)
                    Dem = 600;

            }
            else if (GiaiDoan == 2)
            {
                if (Dem < 600)
                    Dem = 600;
                if (Dem >= 900)
                    Dem = 900;

            }
            else if (GiaiDoan == 3)
            {
                Dem = 1000;

            }
            progressBar1.Value = Dem;
        }

        private void frmMaHoaDES_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null && thread.ThreadState == ThreadState.Running)
                thread.Abort();
        }

        private void txtKhoaFile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !(ChuoiHexa.BoHexa.Contains(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void frmMaHoaDES_Load(object sender, EventArgs e)
        {
 
            MaHoa();
            dsK = createTable();
            dsA = createTable1();
            dsKP = createTable2();
            dsDC = createTable3();
        }
        private Boolean kt(int x)
        {
            foreach (var it in dsdc)
            {
                if (x == it.getXi())
                    return false;
            }
            return true;
        }
        private void ClearProgressBar()
        {
            progressBar1.Value = 0;
            EnableHoacDisableNut(false);
        }

        // Các hàm gộp khóa 
       
        public DataTable createTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Xi");
            dt.Columns.Add("Pi");
            return dt;
        }
        public DataTable createTable1()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("i");
            dt.Columns.Add("ai");
            return dt;
        }
        public DataTable createTable2()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Xi");
            dt.Columns.Add("Pi");
            return dt;
        }
        public DataTable createTable3()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Xi");
            dt.Columns.Add("Pi");
            return dt;
        }
        public static Boolean KtSNT(long n)
        {
            // so nguyen n < 2 khong phai la so nguyen to
            if (n < 2)
            {
                return false;
            }
            // check so nguyen to khi n >= 2
            long squareRoot = (long)Math.Sqrt(n);
            for (int i = 2; i <= squareRoot; i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
        private void btnChiaKhoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (KtSNT(long.Parse(txtP.Text)))
                {
                    if (Int32.Parse(txtP.Text) <= Int32.Parse(txtM.Text))
                    {
                        MessageBox.Show(this, "Số Thành Viên Dữ Khóa Phải nhỏ Hơn P.");
                    }
                    else
                    {
                        if (Int32.Parse(txtP.Text) > Int32.Parse(txtK.Text))
                        {
                            if (long.Parse(txtT.Text) <= long.Parse(txtM.Text))
                            {
                                ChiaSeKhoa chiase = new ChiaSeKhoa();
                                ds = chiase.chiakhoa(Int32.Parse(txtK.Text), Int32.Parse(txtP.Text),
                                         Int32.Parse(txtM.Text), Int32.Parse(txtT.Text));
                                List<long> sa = chiase.dA;
                                for (var i = 0; i < sa.Count; i++)
                                    dsA.Rows.Add(i + 1, sa[i]);
                                tableA.DataSource = dsA;
                                for (var i = 0; i < ds.Count; i++)
                                    dsK.Rows.Add(i + 1, ds[i]);
                                tableK.DataSource = dsK;
                            }
                            else
                            {
                                MessageBox.Show(this, "Số thành viến mở khóa không được lớn hơn số thành viên giữ khóa");
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Khóa Phải nhỏ Hơn P.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "P Không Phải Số Nguyên Tố");
                }
            
            }
            catch (Exception )
            {
                MessageBox.Show(this, "Nhập Sai kiểu dữ liệu (Tất cả đều là số nguyên)");
            }
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dsg = ds;
            for (var i = 0; i < dsg.Count; i++)
                dsKP.Rows.Add(i + 1, dsg[i]);
            tableKP.DataSource = dsKP;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtK.Text="";
            txtP.Text="";
            txtM.Text="";
            txtT.Text="";
            ds.Clear();
            dsK.Clear();
            dsA.Clear();
            tableK.DataSource = ds;
            tableA.DataSource = ds;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dong != -1)
            {
               
                if (kt(dong + 1))
                {

                    DuocChon dc = new DuocChon(dong + 1, ds[dong]);
                    dsdc.Add(dc);
                    dsDC.Rows.Add(dong + 1, ds[dong]);
                    tableDC.DataSource = dsDC;

                }
            else
            {
                MessageBox.Show(this, "Mảnh đã tồn tại !!!");
            }
                
            }
            else
            {
                MessageBox.Show(this, "Lựa chọn không hợp lệ !!!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChiaSeKhoa chiase = new ChiaSeKhoa();
            long kq = chiase.khoiphuckhoa(dsdc, Int32.Parse(txtP.Text));
            txtKP.Text = kq.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtKP.Text = "";
            dsdc.Clear();
            dsDC.Clear();
            dsKP.Clear();
            tableDC.DataSource=null;
            tableKP.DataSource=null;

      
        }
        private void tableKP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dong = e.RowIndex;
            MessageBox.Show(this, "Dong =" +dong);

        }

       
    }
      
}
