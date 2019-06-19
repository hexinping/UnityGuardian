using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class DeleteAssetBundle  {

	/// <summary>
        /// 批量删除AB包文件
        /// </summary>
        [MenuItem("AssetBundleTools/DeleteAllAssetBundles")]
        public static void DelAssetBundle()
        {
            //删除AB包输出目录
            string strNeedDeleteDIR = string.Empty;

            strNeedDeleteDIR = PathTool.GetABOutPath();
            if (!string.IsNullOrEmpty(strNeedDeleteDIR))
            {
                //注意： 这里参数"true"表示可以删除非空目录
                Directory.Delete(strNeedDeleteDIR,true);
                //去除删除警告
                File.Delete(strNeedDeleteDIR + ".meta");
                //刷新
                AssetDatabase.Refresh();
            }
        }
}
