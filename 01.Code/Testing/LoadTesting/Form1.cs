using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SOAFramework.Library;

namespace LoadTesting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private BindingList<TestingData> _list = new BindingList<TestingData>();
        private int i = 0;
        private int x = 0;
        private Status _status = Status.Wait;
        private string result = null;

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressbar.Visible = progressBar1.Visible = false;
            TestingData data = e.Result as TestingData;
            data.Result = this.result;
            _list.Add(data);
            btnRun.Enabled = true;
            timer1.Stop();
            _status = Status.Finished;
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _status = Status.Executing;
            TestingData data = e.Argument as TestingData;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<Task> list = new List<Task>();
            string errmsg = null;

            for (int i = 0; i < data.Count; i++)
            {
                Task t = new Task(RunTest, new Arg { i = i + 1, Data = data });
                list.Add(t);
                t.Start();
                x = i;
            }
            try
            {
                Task.WaitAll(list.ToArray());
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            watch.Stop();
            data.Time = watch.ElapsedMilliseconds;
            e.Result = data;
            _status = Status.Finished;
        }

        private void RunTest(object param)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Arg arg = param as Arg;
            var data = arg.Data;
            string postData = arg.Data.PostData;
            string result = null;
            if (data.Method.Equals("post")) result = HttpHelper.Post(data.Url, Encoding.UTF8.GetBytes(postData));
            else result = HttpHelper.Get(data.Url);
            this.result = result;
            i++;
            watch.Stop();
            if (watch.ElapsedMilliseconds > arg.Data.MaxRequestTime) arg.Data.MaxRequestTime = watch.ElapsedMilliseconds;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            int taskcount = (int)numCount.Value;
            bool post = rabPost.Checked;
            string[] urls = txbUrl.Text.Split(';');
            string postdata = textBox1.Text;
            progressbar.Maximum = progressBar1.Maximum = taskcount;
            progressbar.Value = progressBar1.Value = 0;
            i = 0;
            timer1.Start();
            progressbar.Visible = progressBar1.Visible = true;
            btnRun.Enabled = false;
            foreach (var url in urls)
            {
                if (string.IsNullOrEmpty(url)) continue;
                BackgroundWorker _worker = new BackgroundWorker();
                _worker.DoWork += _worker_DoWork;
                _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
                TestingData data = new TestingData { Url = url, Method = post ? "post" : "get", Count = taskcount, PostData = postdata };
                _worker.RunWorkerAsync(data);
                //while (_status != Status.Finished)
                //{
                //    Thread.Sleep(100);
                //}
            }
            //dgvData.DataSource = _list;
            //dgvData.Invalidate();
            //dgvData.FirstDisplayedScrollingRowIndex = dgvData.Rows.Count - 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvData.DataSource = _list;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressbar.Value = i;
            progressBar1.Value = x;
        }
    }

    public class Arg
    {
        public TestingData Data { get; set; }

        public long i { get; set; }
    }

    public enum Status
    {
        Wait,
        Executing,
        Finished,
    }
}
