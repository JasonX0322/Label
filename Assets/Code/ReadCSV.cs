using Boo.Lang;
using System;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class ReadCSV : MonoBehaviour
{
    DataTable dtPersonalityPool;
    DataTable dtAppearancePool;
    DataTable dtInternalPool;
    DataTable dtLabelDetail;

    DataTable dtField;
    DataTable dtEnemy;
    public static ReadCSV I;

    void Awake()
    {
        I = this;
    }
    void Start()
    {
        LoadGameCSV();
    }

    void LoadGameCSV()
    {
        string personalityPath = Application.streamingAssetsPath + "/personalityPool.csv";
        string appearancePath = Application.streamingAssetsPath + "/appearancePool.csv";
        string internalPath = Application.streamingAssetsPath + "/internalPool.csv";
        string labelDetailPath = Application.streamingAssetsPath + "/labelDetail.csv";
        dtPersonalityPool = OpenCSV(personalityPath);
        dtAppearancePool = OpenCSV(appearancePath);
        dtInternalPool = OpenCSV(internalPath);
        dtLabelDetail = OpenCSV(labelDetailPath);
    }

    public void LoadBattleCSV(string battleName)
    {
        string fieldPath = Application.streamingAssetsPath + "/" + battleName + "_field" + ".csv";
        string enemyPath = Application.streamingAssetsPath + "/" + battleName + "_enemy" + ".csv";
        dtField = OpenCSV(fieldPath);
        dtEnemy = OpenCSV(enemyPath);

    }

    public string GetEnemyElement(int row,int col)
    {
        return dtEnemy.Rows[row][col].ToString();
    }

    public string GetFieldElement(int row,int col)
    {
        return dtField.Rows[row][col].ToString();
    }
    /// <summary>
    /// 从csv读取数据返回table
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    DataTable OpenCSV(string filePath)
    {
        DataTable dt = new DataTable();
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                //记录每次读取的一行记录
                string strLine = "";
                //记录每行记录中的各字段内容
                string[] aryLine = null;
                string[] tableHead = null;
                //标示列数
                int columnCount = 0;
                //标示是否是读取的第一行
                bool IsFirst = true;
                //逐行读取CSV中的数据
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (IsFirst == true)
                    {
                        tableHead = strLine.Split(',');
                        IsFirst = false;
                        columnCount = tableHead.Length;
                        //创建列
                        for (int i = 0; i < columnCount; i++)
                        {
                            DataColumn dc = new DataColumn(tableHead[i]);
                            dt.Columns.Add(dc);
                        }
                    }
                    else
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (aryLine != null && aryLine.Length > 0)
                {
                    dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                }
                sr.Close();
                fs.Close();
                return dt;
            }
        }
    }
}
