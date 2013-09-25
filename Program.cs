using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProPlan.BusinessLogic;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using System.Data;


namespace ProPlan.ImageResizer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("This Command Execute Following tasks:");
                Console.WriteLine("Create OrigImage fold");
                Console.WriteLine("MOVE all image from ContestImages fold to OrigImage");
                Console.WriteLine("Resize images and Save to ContestImages fold.");
                Console.WriteLine("Put Wish Image width.");
                return;
            }
            int wth = 400;	//set default
            try{
				wth = Convert.ToInt32(args[0]);
			}
			catch(Exception ex){}
			finally{ wth = 400;}			
                       

            //Get all photos
            //string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Input\";
            //string Current = Directory.GetCurrentDirectory() + @"\ContestImages\";
            string parentDir = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;            
            string Current = parentDir + @"\ContestImages\";

            //Create folder structure, to avoid permissions issues
            CreateFolderStructure(parentDir);

            string[] filePaths = Directory.GetFiles(Current, "*.*", SearchOption.AllDirectories);
            Collection<string> allFiles = new Collection<string>();
            

            //DataSet ds = ProductBusinessLogic.GetAllPetProducts();

            foreach (string s in filePaths)
            {
                //Check for photo extensions manually, since "Directory.GetFiles" does not allow compound filters
                if (s.ToLower().Contains("jpg") || s.ToLower().Contains("jpeg") || s.ToLower().Contains("gif") || s.ToLower().Contains("png") || s.ToLower().Contains("bmp"))
                {
                    Console.WriteLine("$>Move Original Photo: ");

                    string fname = Path.GetFileName(s);
                    
                    if (fname.Equals("blankDog.jpg")) continue;

                    string newfile = parentDir + @"\OrigImage\" + fname;
                    //copy original to OrigImage fold
                    //File.Copy(s, newfile,true);
                    //Move Original Image
                    File.Move(s, newfile);
                    Console.WriteLine("$>Moved Photo: " + fname);

                    //Resizing original photo
                    Console.WriteLine("$Resizing and Save to ContestImages fold: " + fname);
                    ProPlan.BusinessLogic.ImageResizingBusinessLogic.DrawThumbnail(newfile, wth, Current);
                }
            }

           

            Console.WriteLine("$>End; " + filePaths.Count() + " files processed");
        }

        //Initial creation of folder structure
        private static void CreateFolderStructure(string rootpath)
        {
            string path = rootpath + @"\OrigImage\";
            if(!Directory.Exists(path))            
                Directory.CreateDirectory(path);
        }


        /// <summary>
        /// Debugging
        /// </summary>
        /// <param name="dv">the DataView</param>
        public static void TraceDataView(DataView dv)
        {
            if (dv != null)
            {
                System.Text.StringBuilder buffer = new System.Text.StringBuilder();
                //first write column names
                for (int i = 0; i < dv.Table.Columns.Count; i++)
                {
                    buffer.Append(dv.Table.Columns[i].ColumnName);
                    buffer.Append("|");
                }
                Console.WriteLine("DataView Columns", buffer.ToString());
                //write data
                for (int i = 0; i < dv.Count; i++)
                {
                    buffer = new System.Text.StringBuilder();
                    for (int j = 0; j < dv.Table.Columns.Count; j++)
                    {
                        buffer.Append(dv[i][j]);
                        buffer.Append("|");
                    }
                    Console.WriteLine("DataRow", buffer.ToString());
                }
            }
        }

    }
}
