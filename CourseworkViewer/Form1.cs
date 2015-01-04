using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseworkViewer
{
	using System.IO;

	public partial class Form1 : Form
	{
		public class CourseworkRecord
		{
			public int Sn { get; set; }
			public string Sno { get; set; }
			public string Sname { get; set; }
			public string CName { get; set; }
			public string SubmitTime { get; set; }
			public string FileName { get; set; }
		}


		private string scanDirectoryPath = ""; //用户选择的上传文件夹（upload文件夹）

		private List<string> courseworkNameList = new List<string>();

		private List<string> studentNameList = new List<string>();

		private List<CourseworkRecord> records = new List<CourseworkRecord>();

		//private List<CourseworkRecord> resultView = new List<CourseworkRecord>(); 

		public Form1()
		{
			InitializeComponent();
		}

		private void 选择文件夹DToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				dataGridView1.DataSource = 0;
				scanDirectoryPath = folderBrowserDialog1.SelectedPath;

				//scanDirectoryPath = @"E:\HTTP\CourseworkUploadSystem4ASP\";

				this.ReadConfigFile(scanDirectoryPath + "\\config.txt");
				ShowAllFiles(scanDirectoryPath + "\\upload\\");
				this.ChangeColor(1);
			}
		}

		private void ShowAllFiles(string directoryPath)
		{
			List<DataRow> rows = new List<DataRow>();
			List<CourseworkRecord> sameRows = new List<CourseworkRecord>();


			try
			{
				//DataTable filesDataTable = new DataTable("filesDataTable");

				//filesDataTable.Columns.Add("序号");
				//filesDataTable.Columns.Add("学号");
				//filesDataTable.Columns.Add("姓名");
				//filesDataTable.Columns.Add("该文件所属的作业名称");
				//filesDataTable.Columns.Add("文件提交时间");
				//filesDataTable.Columns.Add("提交的文件名");

				foreach (string studentName in studentNameList)
				{
					foreach (string courseworkName in courseworkNameList)
					{
						//DataRow datarow = filesDataTable.NewRow();
						CourseworkRecord record = new CourseworkRecord();
						//datarow[1] = studentName.Split('+')[0];
						record.Sno = studentName.Split('+')[0];
						//datarow[2] = studentName.Split('+')[1];
						record.Sname = studentName.Split('+')[1];
						//datarow[3] = courseworkName;
						record.CName = courseworkName;

						//filesDataTable.Rows.Add(datarow);
						records.Add(record);
					}
				}

				//filesDataTable.DefaultView.Sort = "学号 ASC, 该文件所属的作业名称 ASC";

				//directoryPath 就是那个选中的“upload”文件夹
				string[] courseworkPathList = Directory.GetDirectories(directoryPath); //每项就是结尾是每次作业的绝对路径

				for (int i = 0; i < courseworkPathList.Length; i++) //遍历upload下每个作业文件夹
				{
					string currentCourseworkPath = courseworkPathList[i];
					string currentCourseworkName = currentCourseworkPath.Substring(currentCourseworkPath.LastIndexOf("\\") + 1);
					//获取当前作业名称

					string[] studentPathList = Directory.GetDirectories(currentCourseworkPath); //得到当前作业文件夹下所有学生的文件夹绝对路径（学号+姓名）

					for (int j = 0; j < studentPathList.Length; j++) //遍历某个作业文件夹中每个学生文件夹
					{
						string currentStudentPath = studentPathList[j];
						string currentStudentName = currentStudentPath.Substring(currentStudentPath.LastIndexOf("\\") + 1);

						//foreach (DataRow currentRow in filesDataTable.Rows)
						foreach (CourseworkRecord record in records)
						//for(int l = 0; l < records.Count; l++)
						{
							//CourseworkRecord record = records[l];
							//if ((currentRow[1] + "+" + currentRow[2]) == currentStudentName && currentRow[3].ToString() == currentCourseworkName)
							if ((record.Sno + "+" + record.Sname) == currentStudentName && record.CName == currentCourseworkName)
							{
								string[] courseworkFileList = Directory.GetFiles(currentStudentPath);

								for (int k = 0; k < courseworkFileList.Length; k++)
								{
									string currentCourseworkFilePath = courseworkFileList[k];
									string currentCourseworkFileName =
										currentCourseworkFilePath.Substring(currentCourseworkFilePath.LastIndexOf("\\") + 1);

									if (k == 0)
									{
										//currentRow[0] = Convert.ToString(filesDataTable.Rows. + 1);//序号
										//currentRow[3] = currentCourseworkName; //所属作业名称
										//currentRow[5] = currentCourseworkFileName; //提交的文件名
										record.FileName = currentCourseworkFileName;
										//currentRow[1] = currentCourseworkFileName.Split('+')[0]; //学号
										//currentRow[2] = currentCourseworkFileName.Split('+')[1]; //姓名
										//currentRow[4] = currentCourseworkFileName.Split('+')[2]; //提交时间
										record.SubmitTime = currentCourseworkFileName.Split('+')[2];
									}
									else
									{
										//DataRow sameRow = filesDataTable.NewRow(); //同一个作业超过提交一个文件
										CourseworkRecord sameRecored = new CourseworkRecord();
										sameRecored.Sno = record.Sno;
										sameRecored.Sname = record.Sname;
										sameRecored.CName = record.CName;
										sameRecored.SubmitTime = currentCourseworkFileName.Split('+')[2];
										sameRecored.FileName = currentCourseworkFileName;


										//sameRow[0] = currentRow[0];
										//sameRow[1] = currentRow[1];
										//sameRow[2] = currentRow[2];
										//sameRow[3] = currentRow[3];
										//sameRow[4] = currentCourseworkFileName.Split('+')[2];
										//sameRow[5] = currentCourseworkFileName;

										//filesDataTable.Rows.Add(sameRow);
										//rows.Add(sameRow);
										sameRows.Add(sameRecored);
									}
									//filesDataTable.Rows.Add(currentRow);
								}
							}
						}
					}
				}

				//foreach (DataRow row in rows)
				//{
				//	filesDataTable.Rows.Add(row);
				//}
				foreach (CourseworkRecord sameRow in sameRows)
				{
					records.Add(sameRow);
				}

				//filesDataTable.DefaultView.Sort = "学号 ASC, 该文件所属的作业名称 ASC, 文件提交时间 ASC";
				//dataGridView1.DataSource = filesDataTable.DefaultView;

				IEnumerable<CourseworkRecord> resultView = from r in records
														   orderby r.Sno,r.CName,r.SubmitTime
														   select
															   new CourseworkRecord()
															   {
																   Sn = r.Sn,
																   Sno = r.Sno,
																   Sname = r.Sname,
																   CName = r.CName,
																   SubmitTime = r.SubmitTime,
																   FileName = r.FileName
															   };



				dataGridView1.DataSource = resultView.ToList();


			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private void ChangeColor(int keyIndex)
		{
			List<Color> colorList = new List<Color>();
			colorList.Add(Color.LightBlue);
			colorList.Add(Color.LightCoral);
			colorList.Add(Color.LightPink);
			colorList.Add(Color.LightGreen);


			int colorIndex = 0;
			dataGridView1.Rows[0].DefaultCellStyle.BackColor = colorList[colorIndex];
			int no = 1;

			dataGridView1.Rows[0].Cells[0].Value = no;

			for (int i = 1; i < dataGridView1.Rows.Count; i++)
			{
				no++;
				if (dataGridView1.Rows[i].Cells[keyIndex].Value.ToString() != dataGridView1.Rows[i - 1].Cells[keyIndex].Value.ToString())
				{
					no = 1;
					colorIndex++;

					if (colorIndex > 3)
					{
						colorIndex = 0;
					}




					dataGridView1.Rows[i].Cells[0].Value = no;
				}
				else
				{
					//dataGridView1.Rows[i].DefaultCellStyle.BackColor = rowColor;
					dataGridView1.Rows[i].Cells[0].Value = no;
				}
				dataGridView1.Rows[i].DefaultCellStyle.BackColor = colorList[colorIndex]; ;
			}
		}

		//public Color GetRandomColor()
		//{
		//	Random RandomNum_First = new Random(Guid.NewGuid().GetHashCode());
		//	//  对于C#的随机数，没什么好说的
		//	//System.Threading.Thread.Sleep(RandomNum_First.Next(50));
		//	Random RandomNum_Sencond = new Random(Guid.NewGuid().GetHashCode());

		//	//System.Threading.Thread.Sleep(RandomNum_First.Next(50));
		//	Random RandomNum_Third = new Random(Guid.NewGuid().GetHashCode());

		//	//  为了在白色背景上显示，尽量生成深色
		//	int int_Red = RandomNum_First.Next(256);
		//	int int_Green = RandomNum_Sencond.Next(256);
		//	//int int_Blue = (int_Red + int_Green < 400) ? 0 : 400 - int_Red - int_Green;
		//	int int_Blue = RandomNum_Sencond.Next(256);

		//	//int_Blue = (int_Blue > 255) ? 255 : int_Blue;

		//	return Color.FromArgb(int_Red, int_Green, int_Blue);
		//}



		private void ReadConfigFile(string configFilePath)
		{
			StreamReader sr = null;

			try
			{
				sr = new StreamReader(configFilePath, Encoding.Default); //得到文件路径并读取

				string currentLine = "";

				while ((currentLine = sr.ReadLine()) != null)
				{
					if (currentLine == "[Student List]")
					{
						string studentRecordStr = "";

						while (studentRecordStr != "[Coursework List]")
						{
							studentRecordStr = sr.ReadLine().Trim();
							string[] studentRecord = new string[2];

							if (studentRecordStr != "[Coursework List]")
							{
								studentRecord = studentRecordStr.Split(',');
								studentNameList.Add(studentRecord[0] + "+" + studentRecord[1]);
							}
						}

						while (sr.Peek() >= 0)
						{
							studentRecordStr = sr.ReadLine().Trim();
							courseworkNameList.Insert(0, studentRecordStr);
						}
					}
					else
					{
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				if (sr != null)
				{
					sr.Close();
				}
			}
		}

		private void 按学号排序NToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IEnumerable<CourseworkRecord> resultView = from r in records
													   orderby r.Sno, r.CName, r.SubmitTime
													   select
														   new CourseworkRecord()
														   {
															   Sn = r.Sn,
															   Sno = r.Sno,
															   Sname = r.Sname,
															   CName = r.CName,
															   SubmitTime = r.SubmitTime,
															   FileName = r.FileName
														   };



			dataGridView1.DataSource = resultView.ToList();
			this.ChangeColor(1);
		}

		private void 按每次作业时间先后排序TToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IEnumerable<CourseworkRecord> resultView = from r in records
													   orderby r.CName, r.SubmitTime,r.Sno
													   select
														   new CourseworkRecord()
														   {
															   Sn = r.Sn,
															   Sno = r.Sno,
															   Sname = r.Sname,
															   CName = r.CName,
															   SubmitTime = r.SubmitTime,
															   FileName = r.FileName
														   };



			dataGridView1.DataSource = resultView.ToList();
			this.ChangeColor(3);
		}
	}
}