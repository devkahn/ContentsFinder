using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace CF.Helpers
{
    public static class FileHelper
    {
        public static FileInfo OpenFile(string caption )
        {
            FileInfo output = null;

            OpenFileDialog odf = new OpenFileDialog();
            if(odf.ShowDialog() == DialogResult.OK)
            {
                output = new FileInfo(odf.FileName);
            }

            return output;
        }
        public static bool SaveTextFile(string caption, string stringValue)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            sfd.Title = caption;
            sfd.OverwritePrompt = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = sfd.FileName;
                    if (File.Exists(filePath))
                    {
                        if (sfd.OverwritePrompt)
                        {
                            File.Delete(filePath);
                        }
                        else
                        {
                            string eMsg = "동일한 명칭의 파일이 존재합니다.\n다시 확인하세요.";
                            MessageHelper.ShowErrorMessage(caption, eMsg);
                            return false;
                        }
                    }

                    File.WriteAllText(filePath, stringValue);


                    return true;
                }
                catch (Exception)
                {
                    string eMsg = "파일 저장에 실패하였습니다.\n다시 시도 하세요.";
                    MessageHelper.ShowErrorMessage(caption, eMsg);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool SaveTextFile(string caption, string stringValue, FileInfo fInfo)
        {
            try
            {
                string filePath = fInfo.FullName;
                if (File.Exists(filePath)) File.Delete(filePath);

                File.CreateText(filePath).Close();
                File.WriteAllText(filePath, stringValue);
                string sMsg = "데이터 저장에 성공하였습니다.";
                MessageHelper.ShowSuccessMessage(caption, sMsg);

                return true;
            }
            catch (Exception ee)
            {
                string eMsg = "파일 저장에 실패하였습니다.\n다시 시도 하세요.";
                eMsg += "\n\n";
                eMsg += ee.Message;

                MessageHelper.ShowErrorMessage(caption, eMsg);
                return false;
            }
        }

        public static FileInfo GetSavePath(string caption, string fileName = "")
        {
            FileInfo output = null;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = fileName;
            sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            sfd.Title = caption;
            sfd.OverwritePrompt = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                output = new FileInfo(sfd.FileName);
            }

            return output;
        }
    }
}
